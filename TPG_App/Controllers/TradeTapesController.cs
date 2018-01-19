using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using TPG_App.Common;
using TPG_App.Models;

namespace TPG_App.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/TradeTapes")]
    public class TradeTapesController : ApiController
    {
        const string APP_SETTINGS_CREATED_TAPES_PATH = "CreatedTapesPath";
        const string APP_SETTINGS_IMPORTED_TAPES_PATH = "ImportedTapesPath";
        const string APP_SETTINGS_FAILED_TAPES_PATH = "FailedTapesPath";

        private TradeModels db = new TradeModels();

        // GET: api/TradeTapes
        public IQueryable<TradeTape> GetTradeTapes()
        {
            return db.TradeTapes;
        }

        // GET: api/TradeTapes/5
        [ResponseType(typeof(TradeTape))]
        public async Task<IHttpActionResult> GetTradeTape(int id)
        {
            TradeTape tradeTape = await db.TradeTapes.FindAsync(id);
            if (tradeTape == null)
            {
                return NotFound();
            }

            return Ok(tradeTape);
        }

        // PUT: api/TradeTapes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeTape(int id, TradeTape tradeTape)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradeTape.TapeID)
            {
                return BadRequest();
            }

            db.Entry(tradeTape).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeTapeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// This method will upload the TradeTape and set the created date in the DB.  This does not do the importing of the loan information which will be done in the PostTradeTape method
        /// in the 'import' route
        /// </summary>
        /// <returns></returns>
        // POST: api/TradeTapes
        [ResponseType(typeof(TradeTape))]
        [MimeMultipart]
        public async Task<IHttpActionResult> PostTradeTape()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uploadPath = ConfigurationManager.AppSettings[APP_SETTINGS_CREATED_TAPES_PATH];

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            string localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

            //Get the trade tape information from the multipart form data in the body
            TradeTape tradeTape = new TradeTape()
            {
                TradeID = Convert.ToInt32(multipartFormDataStreamProvider.FormData["TradeID"]),
                Name = multipartFormDataStreamProvider.FormData["Name"],
                Description = multipartFormDataStreamProvider.FormData["Description"],
                StoragePath = localFileName,
                CreatedDate = DateTime.UtcNow
            };

            db.TradeTapes.Add(tradeTape);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradeTape.TapeID }, tradeTape);
        }

        /// <summary>
        /// This method will import the tradeTape that is already uploaded
        /// </summary>
        /// <param name="tradeTape"></param>
        /// <returns></returns>
        [ResponseType(typeof(TradeTape))]
        [HttpPost]
        [Route("{id:int}/import", Name= "TapeImportApi")]
        //[ActionName("import")]
        public async Task<IHttpActionResult> PostTradeTapeImport(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TradeTapeExists(id))
            {
                ModelState.AddModelError("TapeID", "invalid tapeId received");
                return BadRequest(ModelState);
            }

            TradeTape tradeTape = await db.TradeTapes.FindAsync(id);
            List<long> assetIds = await ParseAndImportAssetsAsync(tradeTape);

            if((assetIds == null) || (assetIds.Count == 0))
            {
                ModelState.AddModelError("TapeErrors", "tape has invalid data.  Please refer to the tape logs for more information");
                return BadRequest(ModelState);
            }

            db.Entry(tradeTape).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeTapeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("TapeImportApi", new { id = tradeTape.TapeID }, tradeTape);
        }

        private async Task<List<long>> ParseAndImportAssetsAsync(TradeTape tradeTape)
        {
            int skipRowCount = 1; //Start with skipping the first row as the first row contains the column headings
            int rowBatchSize = 100;
            var assetCtrl = new TradeAssetsController();
            List<string> parseErrors = new List<string>();
            parseErrors.Add("LoanID, Line#, ParserError");
            do
            {
                List<TradeAsset> tradeAssets = new List<TradeAsset>();
                ParseHeader(parseErrors, File.ReadLines(tradeTape.StoragePath).Take(1));
                var loanBatch = File.ReadLines(tradeTape.StoragePath).Skip(skipRowCount).Take(rowBatchSize);
                var counterPartyID = db.TradePools.Where(p => p.TradeID == tradeTape.TradeID).First().CounterPartyID;

                int lineCountInBatch = 0;
                foreach (var line in loanBatch)
                {
                    lineCountInBatch++;
                    string[] loanAttributes = line.Split(',');
                    try
                    {
                        var asset = new TradeAsset
                        {
                            TradeID = tradeTape.TradeID,
                            TapeID = tradeTape.TapeID,
                            Seller_CounterPartyID = (short)counterPartyID,
                            SellerAssetID = loanAttributes[2].Trim(),
                            OriginalBalance = String.IsNullOrEmpty(loanAttributes[4].Trim()) ? 0 : Convert.ToDecimal(loanAttributes[4]),
                            CurrentBalance = String.IsNullOrEmpty(loanAttributes[5].Trim()) ? 0 : Convert.ToDecimal(loanAttributes[5]),
                            ForebearBalance = String.IsNullOrEmpty(loanAttributes[6].Trim()) ? 0 : Convert.ToDecimal(loanAttributes[6]),
                            Bpo = String.IsNullOrEmpty(loanAttributes[12].Trim()) ? 0 : Convert.ToDecimal(loanAttributes[12]),
                            BpoDate = Convert.ToDateTime(loanAttributes[11]),
                            OriginalPmt = String.IsNullOrEmpty(loanAttributes[21].Trim())? 0 : Convert.ToDecimal(loanAttributes[21]),
                            OriginalDate = Convert.ToDateTime(loanAttributes[23]),
                            CurrentPmt = String.IsNullOrEmpty(loanAttributes[22].Trim()) ? 0 : Convert.ToDecimal(loanAttributes[22]),
                            PaidToDate = Convert.ToDateTime(loanAttributes[25]),
                            NextDueDate = Convert.ToDateTime(loanAttributes[26]),
                            MaturityDate = Convert.ToDateTime(loanAttributes[27]),
                            StreetAddress1 = loanAttributes[36].Trim(),
                            City = loanAttributes[37].Trim(),
                            State = loanAttributes[38].Trim(),
                            Zip = loanAttributes[39].Trim(),
                            Cbsa = loanAttributes[40].Trim(),
                            CbsaName = loanAttributes[41].Trim(),
                            ProdType = loanAttributes[55].Trim(),
                            LoanPurp = loanAttributes[56].Trim(),
                            PropType = loanAttributes[57].Trim(),
                            OrigFico = loanAttributes[69].Trim(),
                            CurrFico = loanAttributes[70].Trim(),
                            CurrFicoDate = Convert.ToDateTime(loanAttributes[71]),
                            PayString = loanAttributes[80].Trim()
                        };

                        tradeAssets.Add(asset);
                    }
                    catch(Exception ex)
                    {
                        //If there are any
                        parseErrors.Add(loanAttributes[2].Trim() + "," + (skipRowCount + lineCountInBatch).ToString() + "," + ex.Message);

                    }
                }

                if(parseErrors.Count() <= 1)
                {
                    //only post the assets batch if there are no previous parser errors.  Note there could be paser errors in subsequent batches, in which case these assets will be deleted later
                    await assetCtrl.PostTradeAssetsInBatch(tradeAssets);
                }
               

                if(loanBatch.Count() < rowBatchSize )
                {
                    break;
                }
                skipRowCount += rowBatchSize;

            } while (true);

            var dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_IMPORTED_TAPES_PATH];
            if (parseErrors.Count > 1)
            {
                
                dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_FAILED_TAPES_PATH];
                //If there were assets created in loops prior to finding parser errors, remove those assets from db
                if(skipRowCount > rowBatchSize)
                {
                    db.TradeAssets.RemoveRange(db.TradeAssets.Where(a => a.TapeID == tradeTape.TapeID));
                }
            }
            
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if(parseErrors.Count > 1)
            {
                var errorFileName = dirPath + "\\" + tradeTape.TapeID + ".csv";
                File.WriteAllLines(errorFileName, parseErrors.ToArray());
            }
            else
            {
                tradeTape.ImportedDate = DateTime.UtcNow;
            }
            
            var moveFullFileName = dirPath + "\\" + tradeTape.TapeID + "_" + DateTime.UtcNow.ToFileTimeUtc() + "_" + tradeTape.StoragePath.Split('\\').Last();
            File.Move(tradeTape.StoragePath, moveFullFileName);

            List<long> returnedAssetIds = assetCtrl.GetTradeAssets().Where(a => a.TapeID == tradeTape.TapeID).OrderBy(o => o.AssetID).Select(s => s.AssetID).ToList();
            return returnedAssetIds;
        }

        private void ParseHeader(List<string> parseErrors, IEnumerable<string> header)
        {
            var headerLocations = new[] { new {headerName= "LOANID",  } }
            List<string> columnNames = header.First().Split(',').ToList();
            columnNames.f
        }


        // DELETE: api/TradeTapes/5
        [ResponseType(typeof(TradeTape))]
        public async Task<IHttpActionResult> DeleteTradeTape(int id)
        {
            TradeTape tradeTape = await db.TradeTapes.FindAsync(id);
            if (tradeTape == null)
            {
                return NotFound();
            }

            db.TradeTapes.Remove(tradeTape);
            await db.SaveChangesAsync();

            return Ok(tradeTape);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeTapeExists(int id)
        {
            return db.TradeTapes.Count(e => e.TapeID == id) > 0;
        }
    }
}
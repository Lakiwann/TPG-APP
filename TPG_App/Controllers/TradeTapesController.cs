﻿using LinqToExcel;
using Newtonsoft.Json;
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

            var uploadPath = ConfigurationManager.AppSettings[TapeParser.APP_SETTINGS_CREATED_TAPES_PATH];

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
        /// This method will validate the tradeTape that is already uploaded
        /// </summary>
        /// <param name="tradeTape"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/validate", Name = "TapeValidateApi")]
        //[ActionName("import")]
        public async Task<IHttpActionResult> PostTradeTapeValidate(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TradeTapeExists(id))
            {
                ModelState.AddModelError("TapeID", "invalid tapeId received");
                return BadRequest(ModelState);
            }

            TradeTape tradeTape = await db.TradeTapes.FindAsync(id);
            var parser = new TapeParser(tradeTape);

            //TapeErrorInfo errors = await ParseAndValidateTradeTapeAsync(tradeTape);
            TapeErrorInfo errorInfo = await parser.ParseTapeAsync();
            if (errorInfo.HaveErrors)
            {
                ModelState.AddModelError("TapeParserError", JsonConvert.SerializeObject(errorInfo));
                return BadRequest(ModelState);
            }

            return CreatedAtRoute("TapeValidateApi", new { id = tradeTape.TapeID }, tradeTape);
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
            tradeTape.ImportedDate = DateTime.UtcNow;
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

            var parser = new TapeParser(tradeTape);
            
            List<long> returnedAssetIds = null;

            switch(tradeTape.Name)
            {
                case TapeParser.SELLER_BID_TAPE_NAME:
                    returnedAssetIds = await ImportSellerBidTape(tradeTape, parser);
                    break;
                case TapeParser.SELLER_PRICING_TAPE:
                    //returnedAssetIds = await ImportSellerPricingTape(tradeTape, parser);
                    returnedAssetIds = await ImportSellerPricingTape(tradeTape, parser);
                    break;
            }

            TapeParser.BackUpTape(tradeTape, false);

            return returnedAssetIds;
        }

        //private Task<List<long>> ImportSellerPricingTape(TradeTape tradeTape, TapeParser parser)
        private async Task<List<long>> ImportSellerPricingTape(TradeTape tradeTape, TapeParser parser)
        {
            var pricingSourceName = tradeTape.Name + ":" + tradeTape.TapeID;
            List<string> columnsInTape;
            List<TradeAssetPricing> tapsFromTape = parser.GetTradeAssetPricesFromTape(out columnsInTape);
            
            foreach (var tap in tapsFromTape)
            {
                var tapFromDb = db.TradeAssetPricings.Where(ap => ap.TradeID == tradeTape.TradeID && ap.SellerAssetID == tap.SellerAssetID).First();
                foreach (var col in columnsInTape)
                {
                    switch (col)
                    {
                        case "CurrentBalance":
                            tapFromDb.CurrentBalance = tap.CurrentBalance;
                            break;
                        case "ForebearanceBalance":
                            tapFromDb.ForebearanceBalance = tap.ForebearanceBalance;
                            break;
                        case "CurrentPrice":
                            tapFromDb.CurrentPrice = tap.CurrentPrice;
                            break;
                        case "BidPercentage":
                            //If there is a CurrentPrice and a BidPrice, then the CurrentPrice will be taken to calculate the bid%
                            if(!columnsInTape.Contains("CurrentPrice"))
                            {
                                tapFromDb.BidPercentage = tap.BidPercentage;
                            }
                            break;
                    }
                }

                //Calculate the new Bid%
                if (columnsInTape.Contains("CurrentPrice"))
                {
                    tapFromDb.BidPercentage = tapFromDb.CurrentPrice / (tapFromDb.CurrentBalance + tapFromDb.ForebearanceBalance);
                }

                tapFromDb.Source = pricingSourceName;
                db.Entry(tapFromDb).State = EntityState.Modified;
                try
                {
                    //await db.SaveChangesAsync();
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
               
            }

            List<long> changedPricingAssetIDs = await db.TradeAssetPricings.Where(tap => tap.Source == pricingSourceName).Select(tap => tap.AssetID).ToListAsync();

            return changedPricingAssetIDs;
        }

        private async Task<List<long>> ImportSellerBidTape(TradeTape tradeTape, TapeParser parser)
        {
            int skipCount = 0;
            int batchSize = 100;

            var assetCtrl = new TradeAssetsController(); //TODO:  Change this to BL call later
            List<TradeAsset> tradeAssets = parser.GetTradeAssets();
            while (tradeAssets.Count > skipCount)
            {

                try
                {
                    List<TradeAsset> assetBatch = tradeAssets.Skip(skipCount).Take(batchSize).ToList();

                    db.TradeAssets.AddRange(assetBatch);
                    await db.SaveChangesAsync();

                    List<TradeAssetPricing> assetPricingBatch = assetBatch.Select(a => new TradeAssetPricing()
                    {
                        TradeID = a.TradeID,
                        AssetID = a.AssetID,
                        CurrentBalance = a.CurrentBalance,
                        ForebearanceBalance = a.ForebearanceBalance,
                        SellerAssetID = a.SellerAssetID,
                        BidPercentage = 0,
                        CurrentPrice = 0,
                        OriginalDebt = a.CurrentBalance + a.ForebearanceBalance,
                        OriginalPrice = 0,
                        Source = tradeTape.Name + ":" + tradeTape.TapeID
                    }).ToList();

                    db.TradeAssetPricings.AddRange(assetPricingBatch);
                    await db.SaveChangesAsync();

                    skipCount += batchSize;
                }
                catch (Exception ex)
                {
                    if (skipCount > 0)
                    {
                        db.TradeAssets.RemoveRange(db.TradeAssets.Where(a => a.TapeID == tradeTape.TapeID));
                    }
                    TapeParser.BackUpTape(tradeTape, true);
                    throw;
                }
            }

            List<long> returnedAssetIds = assetCtrl.GetTradeAssets().Where(a => a.TapeID == tradeTape.TapeID).OrderBy(o => o.AssetID).Select(s => s.AssetID).ToList();
            return returnedAssetIds;
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
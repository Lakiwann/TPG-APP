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
using System.Web.Http.Description;
using TPG_App.Common;
using TPG_App.Models;

namespace TPG_App.Controllers
{
    [RoutePrefix("api/TradeTapes")]
    public class TradeTapesController : ApiController
    {
        const string APP_SETTINGS_CREATED_TAPES_PATH = "CreatedTapesPath";
        const string APP_SETTINGS_IMPORTED_TAPES_PATH = "ImportedTapesPath";

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
            var loanLines = File.ReadAllLines(tradeTape.StoragePath).ToList();

            var dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_IMPORTED_TAPES_PATH];
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            tradeTape.ImportedDate = DateTime.UtcNow;

            var newFullFileName = dirPath + "\\"  + tradeTape.TapeID + "_" + DateTime.UtcNow.ToFileTimeUtc() + "_" + tradeTape.StoragePath.Split('\\').Last();
            File.Move(tradeTape.StoragePath, newFullFileName);

            

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
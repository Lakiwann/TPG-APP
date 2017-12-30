using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using TPG_App.Models;

namespace TPG_App.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/TradeAssets")]
    public class TradeAssetsController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradeAssets
        public IQueryable<TradeAsset> GetTradeAssets()
        {
            return db.TradeAssets;
        }

        // GET: api/TradeAssets/5
        [ResponseType(typeof(TradeAsset))]
        public async Task<IHttpActionResult> GetTradeAsset(long id)
        {
            TradeAsset tradeAsset = await db.TradeAssets.FindAsync(id);
            if (tradeAsset == null)
            {
                return NotFound();
            }

            return Ok(tradeAsset);
        }

        // PUT: api/TradeAssets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeAsset(long id, TradeAsset tradeAsset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradeAsset.AssetID)
            {
                return BadRequest();
            }

            db.Entry(tradeAsset).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeAssetExists(id))
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

        // POST: api/TradeAssets
        [ResponseType(typeof(TradeAsset))]
        public async Task<IHttpActionResult> PostTradeAsset(TradeAsset tradeAsset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeAssets.Add(tradeAsset);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradeAsset.AssetID }, tradeAsset);
        }

        [HttpPost]
        [Route("batch", Name = "AssetBatchImport")]
        public async Task<IHttpActionResult> PostTradeAssetsInBatch(IEnumerable<TradeAsset> tradeAssets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeAssets.AddRange(tradeAssets);
            await db.SaveChangesAsync();
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    var msg = ex.Message;
            //}


            return CreatedAtRoute("AssetBatchImport", null, tradeAssets);
        }

        // DELETE: api/TradeAssets/5
        [ResponseType(typeof(TradeAsset))]
        public async Task<IHttpActionResult> DeleteTradeAsset(long id)
        {
            TradeAsset tradeAsset = await db.TradeAssets.FindAsync(id);
            if (tradeAsset == null)
            {
                return NotFound();
            }

            db.TradeAssets.Remove(tradeAsset);
            await db.SaveChangesAsync();
            

            return Ok(tradeAsset);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeAssetExists(long id)
        {
            return db.TradeAssets.Count(e => e.AssetID == id) > 0;
        }
    }
}
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
    public class TradeAssetDiligencesController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradeAssets/{assetId:long}/Diligences
        [Route("{assetId:long}/diligences")]
        public IQueryable<TradeAssetDiligence> GetTradeAssetDiligences(long assetId)
        {
            return db.TradeAssetDiligences.Where(d => d.AssetID == assetId);
        }

        // GET: api/TradeAssetDiligences/5
        [ResponseType(typeof(TradeAssetDiligence))]
        public async Task<IHttpActionResult> GetTradeAssetDiligence(long id)
        {
            TradeAssetDiligence tradeAssetDiligence = await db.TradeAssetDiligences.FindAsync(id);
            if (tradeAssetDiligence == null)
            {
                return NotFound();
            }

            return Ok(tradeAssetDiligence);
        }

        // PUT: api/TradeAssetDiligences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradeAssetDiligence(long id, TradeAssetDiligence tradeAssetDiligence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradeAssetDiligence.DeligenceID)
            {
                return BadRequest();
            }

            db.Entry(tradeAssetDiligence).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeAssetDiligenceExists(id))
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

        // POST: api/TradeAssetDiligences
        [ResponseType(typeof(TradeAssetDiligence))]
        public async Task<IHttpActionResult> PostTradeAssetDiligence(TradeAssetDiligence tradeAssetDiligence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeAssetDiligences.Add(tradeAssetDiligence);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradeAssetDiligence.DeligenceID }, tradeAssetDiligence);
        }

        // DELETE: api/TradeAssetDiligences/5
        [ResponseType(typeof(TradeAssetDiligence))]
        public async Task<IHttpActionResult> DeleteTradeAssetDiligence(long id)
        {
            TradeAssetDiligence tradeAssetDiligence = await db.TradeAssetDiligences.FindAsync(id);
            if (tradeAssetDiligence == null)
            {
                return NotFound();
            }

            db.TradeAssetDiligences.Remove(tradeAssetDiligence);
            await db.SaveChangesAsync();

            return Ok(tradeAssetDiligence);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeAssetDiligenceExists(long id)
        {
            return db.TradeAssetDiligences.Count(e => e.DeligenceID == id) > 0;
        }
    }
}
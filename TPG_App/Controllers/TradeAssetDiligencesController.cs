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

        // GET: api/TradeAssets/diligences/5
        [Route("diligences")]
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
        // GET: api/TradeAssets/diligencetypes
        [Route("diligencetypes")]
        [ResponseType(typeof(List<TradeAssetDiligenceType>))]
        public async Task<IHttpActionResult> GetTradeAssetDiligenceTypes()
        {
            List<TradeAssetDiligenceType> diligenceTypes = await db.TradeAssetDiligenceTypes.OrderBy(o=>o.ID).ToListAsync();

            return Ok(diligenceTypes);
        }

        // GET: api/TradeAssets/diligencecategories
        [Route("diligencecategories")]
        [ResponseType(typeof(List<TradeAssetDiligenceCategory>))]
        public async Task<IHttpActionResult> GetTradeAssetDiligenceCategories()
        {
            List<TradeAssetDiligenceCategory> diligenceCategories = await db.TradeAssetDiligenceCategories.OrderBy(o=>o.ID).ToListAsync();

            return Ok(diligenceCategories);
        }

        // GET: api/TradeAssets/diligencecategories/{categoryId}/descriptions
        [Route("diligencecategories/{categoryId:int}/descriptions")]
        [ResponseType(typeof(List<TradeAssetDiligenceDesc>))]
        public async Task<IHttpActionResult> GetTradeAssetDiligenceDescriptions(int categoryId)
        {
            List<TradeAssetDiligenceDesc> diligenceDescs = await db.TradeAssetDiligenceDescriptions.Where(d => d.CategoryID == categoryId).OrderBy(o=>o.ID).ToListAsync();

            return Ok(diligenceDescs);
        }

        // PUT:  api/TradeAssets/diligences/5
        [Route("diligences")]
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

        // POST:  api/TradeAssets/diligences
        [Route("diligences")]
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

        // DELETE:  api/TradeAssets/diligences/5
        [Route("diligences")]
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
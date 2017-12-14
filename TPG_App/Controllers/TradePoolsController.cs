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
using System.Web.Http.Description;
using TPG_App.Models;

namespace TPG_App.Controllers
{
    public class TradePoolsController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradePools
        public IQueryable<TradePool> GetTradePools()
        {
            return db.TradePools;
        }

        // GET: api/TradePools/5
        [ResponseType(typeof(TradePool))]
        public async Task<IHttpActionResult> GetTradePool(int id)
        {
            TradePool tradePool = await db.TradePools.FindAsync(id);
            if (tradePool == null)
            {
                return NotFound();
            }

            return Ok(tradePool);
        }

        // PUT: api/TradePools/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradePool(int id, TradePool tradePool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradePool.TradeID)
            {
                return BadRequest();
            }

            db.Entry(tradePool).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradePoolExists(id))
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

        // POST: api/TradePools
        [ResponseType(typeof(TradePool))]
        public async Task<IHttpActionResult> PostTradePool(TradePool tradePool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradePools.Add(tradePool);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradePool.TradeID }, tradePool);
        }

        // DELETE: api/TradePools/5
        [ResponseType(typeof(TradePool))]
        public async Task<IHttpActionResult> DeleteTradePool(int id)
        {
            TradePool tradePool = await db.TradePools.FindAsync(id);
            if (tradePool == null)
            {
                return NotFound();
            }

            db.TradePools.Remove(tradePool);
            await db.SaveChangesAsync();

            return Ok(tradePool);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradePoolExists(int id)
        {
            return db.TradePools.Count(e => e.TradeID == id) > 0;
        }
    }
}
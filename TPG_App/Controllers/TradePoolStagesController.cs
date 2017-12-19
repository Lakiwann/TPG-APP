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
    public class TradePoolStagesController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradePoolStages
        public IQueryable<TradePoolStage> GetTradePoolStages()
        {
            return db.TradePoolStages;
        }

        // GET: api/TradePoolStages/5
        [ResponseType(typeof(TradePoolStage))]
        public async Task<IHttpActionResult> GetTradePoolStage(int id)
        {
            TradePoolStage tradePoolStage = await db.TradePoolStages.FindAsync(id);
            if (tradePoolStage == null)
            {
                return NotFound();
            }

            return Ok(tradePoolStage);
        }

        // PUT: api/TradePoolStages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTradePoolStage(int id, TradePoolStage tradePoolStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tradePoolStage.ID)
            {
                return BadRequest();
            }

            db.Entry(tradePoolStage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradePoolStageExists(id))
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

        // POST: api/TradePoolStages
        [ResponseType(typeof(TradePoolStage))]
        public async Task<IHttpActionResult> PostTradePoolStage(TradePoolStage tradePoolStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradePoolStages.Add(tradePoolStage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradePoolStage.ID }, tradePoolStage);
        }

        // DELETE: api/TradePoolStages/5
        [ResponseType(typeof(TradePoolStage))]
        public async Task<IHttpActionResult> DeleteTradePoolStage(int id)
        {
            TradePoolStage tradePoolStage = await db.TradePoolStages.FindAsync(id);
            if (tradePoolStage == null)
            {
                return NotFound();
            }

            db.TradePoolStages.Remove(tradePoolStage);
            await db.SaveChangesAsync();

            return Ok(tradePoolStage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradePoolStageExists(int id)
        {
            return db.TradePoolStages.Count(e => e.ID == id) > 0;
        }
    }
}
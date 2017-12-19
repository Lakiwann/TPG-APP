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
    public class TradeStagesController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradeStages

        public IQueryable<LU_TradeStage> GetLU_TradeStages()
        {
            return db.LU_TradeStages;
        }

        // GET: api/TradeStage/5
        [ResponseType(typeof(LU_TradeStage))]
        public async Task<IHttpActionResult> GetLU_TradeStage(byte id)
        {
            LU_TradeStage lU_TradeStage = await db.LU_TradeStages.FindAsync(id);
            if (lU_TradeStage == null)
            {
                return NotFound();
            }

            return Ok(lU_TradeStage);
        }

        // PUT: api/TradeStage/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLU_TradeStage(byte id, LU_TradeStage lU_TradeStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lU_TradeStage.StageID)
            {
                return BadRequest();
            }

            db.Entry(lU_TradeStage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LU_TradeStageExists(id))
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

        // POST: api/TradeStage
        [ResponseType(typeof(LU_TradeStage))]
        public async Task<IHttpActionResult> PostLU_TradeStage(LU_TradeStage lU_TradeStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LU_TradeStages.Add(lU_TradeStage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lU_TradeStage.StageID }, lU_TradeStage);
        }

        // DELETE: api/TradeStage/5
        [ResponseType(typeof(LU_TradeStage))]
        public async Task<IHttpActionResult> DeleteLU_TradeStage(byte id)
        {
            LU_TradeStage lU_TradeStage = await db.LU_TradeStages.FindAsync(id);
            if (lU_TradeStage == null)
            {
                return NotFound();
            }

            db.LU_TradeStages.Remove(lU_TradeStage);
            await db.SaveChangesAsync();

            return Ok(lU_TradeStage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LU_TradeStageExists(byte id)
        {
            return db.LU_TradeStages.Count(e => e.StageID == id) > 0;
        }
    }
}
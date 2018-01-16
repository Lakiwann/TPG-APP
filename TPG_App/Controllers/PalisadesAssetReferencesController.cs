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
    public class PalisadesAssetReferencesController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/PalisadesAssetReferences
        public IQueryable<PalisadesAssetReference> GetPalisadesAssetReferences()
        {
            return db.PalisadesAssetReferences;
        }

        // GET: api/PalisadesAssetReferences/5
        [ResponseType(typeof(PalisadesAssetReference))]
        public async Task<IHttpActionResult> GetPalisadesAssetReference(long id)
        {
            PalisadesAssetReference palisadesAssetReference = await db.PalisadesAssetReferences.FindAsync(id);
            if (palisadesAssetReference == null)
            {
                return NotFound();
            }

            return Ok(palisadesAssetReference);
        }

        // PUT: api/PalisadesAssetReferences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPalisadesAssetReference(long id, PalisadesAssetReference palisadesAssetReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != palisadesAssetReference.PalID)
            {
                return BadRequest();
            }

            db.Entry(palisadesAssetReference).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalisadesAssetReferenceExists(id))
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

        // POST: api/PalisadesAssetReferences
        [ResponseType(typeof(PalisadesAssetReference))]
        public async Task<IHttpActionResult> PostPalisadesAssetReference(PalisadesAssetReference palisadesAssetReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PalisadesAssetReferences.Add(palisadesAssetReference);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = palisadesAssetReference.PalID }, palisadesAssetReference);
        }

        // DELETE: api/PalisadesAssetReferences/5
        [ResponseType(typeof(PalisadesAssetReference))]
        public async Task<IHttpActionResult> DeletePalisadesAssetReference(long id)
        {
            PalisadesAssetReference palisadesAssetReference = await db.PalisadesAssetReferences.FindAsync(id);
            if (palisadesAssetReference == null)
            {
                return NotFound();
            }

            db.PalisadesAssetReferences.Remove(palisadesAssetReference);
            await db.SaveChangesAsync();

            return Ok(palisadesAssetReference);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PalisadesAssetReferenceExists(long id)
        {
            return db.PalisadesAssetReferences.Count(e => e.PalID == id) > 0;
        }
    }
}
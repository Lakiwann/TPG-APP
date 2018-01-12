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
using System.Web.Http.OData;
using System.Web.Http.Results;
using TPG_App.Models;

namespace TPG_App.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/TradePools")]
    public class TradePoolsController : ApiController
    {
        private TradeModels db = new TradeModels();

        // GET: api/TradePools
        [EnableQuery]
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

        //GET: api/TradePools/5/tradePoolStages/1
        [Route("{id:int}/stages")]
        [EnableQuery]
        public IQueryable<TradePoolStage> GetTradePoolStages(int id)
        {
            var tpsCtrl = new TradePoolStagesController();
            return tpsCtrl.GetTradePoolStages().Where(s => s.TradeID == id);
        }

        //GET: api/TradePools/5/Summary
        [Route("{id:int}/summary")]
        [ResponseType(typeof(TradePoolHighLevelSummary))]
        public async Task<IHttpActionResult> GetTradePoolSummary(int id)
        {
            TradePool tradePool = await db.TradePools.FindAsync(id);
            if (tradePool == null)
            {
                return NotFound();
            }

            List<TradeAsset> assets = await db.TradeAssets.Where(a => a.TradeID == id).ToListAsync();
            
            TradePoolHighLevelSummary poolSummary = assets == null || assets.Count == 0 ?
                new TradePoolHighLevelSummary(id) { TotalCount = 0, TotalDebt = 0, TotalPurchasePrice = 0, TotalReprices = 0, TotalKicks = 0, TotalPIFs = 0, TotalTrailingDocs = 0 } :
                new TradePoolHighLevelSummary(id)
                {
                    TotalCount = assets.Count,
                    TotalDebt = assets.Sum(a => a.CurrentBalance),
                    TotalPurchasePrice = 0,
                    TotalReprices = 0,
                    TotalKicks = 0,
                    TotalPIFs = 0,
                    TotalTrailingDocs = 0
                    
                    //TODO: Add the other 'YEAR/Type' information here
                };

            return Ok(poolSummary);
        }
        //GET: api/TradePools/5/Summary
        //[Route("{id:int}/assetsummary")]
        //[ResponseType(typeof(TradePoolAssetsSummary))]
        //public async Task<IHttpActionResult> GetTradePoolAssetSummary(int id)
        //{
        //    TradePool tradePool = await db.TradePools.FindAsync(id);
        //    if (tradePool == null)
        //    {
        //        return NotFound();
        //    }

        //    TradePoolAssetsSummary poolAssetsSummary = await GetTradePoolAssetsSummaryAsynch(id);

        //    return Ok(poolAssetsSummary);
        //}

        [Route("yearlysummary/{year:int?}")]
        [ResponseType(typeof(List<TradesYearlySummary>))]
        public async Task<IHttpActionResult> GetTradesYearlySummary(int year = 0)
        {
            List<TradesYearlySummary> yearlySummaries = new List<TradesYearlySummary>();
            List<int> years = new List<int>();

            if (year > 0)
            {
                years.Add(year);
            }
            else
            {
                foreach (var y in db.TradePools.GroupBy(p => p.EstSettlementDate.Value.Year).OrderBy(g => g.Key).Select(s => s.Key))
                {
                    years.Add(y);
                }
            };

            foreach (var y in years)
            {
                TradesYearlySummary yearlySummary = new TradesYearlySummary();
                var pools = await db.TradePools.Where(p => p.EstSettlementDate.Value.Year == y).ToListAsync();
                yearlySummary.Year = y;
                yearlySummary.Trades = pools.Count();
                yearlySummary.CounterParties = pools.GroupBy(p => p.TradeName).Count();

                List<AssetSummary> assetSummaries = new List<AssetSummary>();
                foreach (var p in pools)
                {
                    List<AssetSummary> poolAssetSummaries = await GetAssetSummariesAsynch(p.TradeID);
                    if (poolAssetSummaries.Count() > 0)
                    {
                        assetSummaries.AddRange(poolAssetSummaries);
                    }
                }

                yearlySummary.RPLoans = 0;
                yearlySummary.NPLoans = 0;
                yearlySummary.MixedLoans = assetSummaries.Count();
                yearlySummary.BidAmount = 0;
                yearlySummary.TradeAmount = assetSummaries.Sum(a => a.CurrentPrice);
                yearlySummary.RepriceAmount = assetSummaries.Sum(a => a.TotalRepriceAmount);
                yearlySummary.AverageCloseTime = assetSummaries.Average(a => a.CloseTime);
                yearlySummary.AverageFallOut = (assetSummaries.Where(a => a.InStatus == false).Count() / assetSummaries.Count()) * 100;
                yearlySummary.PurchasesAmount = assetSummaries.Sum(a => a.CurrentPrice);
                yearlySummary.SalesAmount = 0;

                yearlySummaries.Add(yearlySummary);
            }

            return Ok(yearlySummaries);
        }

        //GET: api/TradePools/{tradeId}/assetSummary
        [Route("{id:int}/assetsummary")]
        [EnableQuery]
        public async Task<IQueryable<AssetSummary>> GetTradePoolAssetSummary(int id)
        {
            
            List<AssetSummary> assetSumarries = await GetAssetSummariesAsynch(id);

            return assetSumarries.AsQueryable();
        }

        //GET: api/TradePools/counterparties
        [Route("counterparties")]
        [ResponseType(typeof(List<TradeCounterParty>))]
        public async Task<IHttpActionResult> GetTradePoolCounterParties()
        {
            List<TradeCounterParty> lstCounterParties = await db.TradeCounterParties.ToListAsync();
            return Ok(lstCounterParties);
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

            //Update the TradePoolStages. 
            //Only update the trade's last stage, and/or the sequentially subsequent ones
            //For example if the last stage for the trade is 2 and the request sends the stages 1,2,3,5 (- note the stage 4 is missing), 
            //stage 2 will be updated and stage 3 will be added.  
            //There will be no entries added for stage 4 & 5

            var tpsCtrl = new TradePoolStagesController();

            var tpCurrentStages = tpsCtrl.GetTradePoolStages().Where(s => s.TradeID == tradePool.TradeID).OrderByDescending(o => o.StageID);
            
            TradePoolStage tpCurrentStage = tpCurrentStages.Count() == 0 ? null : tpCurrentStages.First();
            int nextStageId = tpCurrentStage != null ? tpCurrentStage.StageID + 1 : 1;
            foreach (var newTradePoolStage in tradePool.TradePoolStages.OrderBy(s => s.StageID))
            {
                if ((tpCurrentStage != null)&&(newTradePoolStage.StageID < tpCurrentStage.StageID))
                    continue;

                newTradePoolStage.TradeID = tradePool.TradeID;
                if ((tpCurrentStage != null) && (newTradePoolStage.StageID == tpCurrentStage.StageID))
                {
                    
                    tpCurrentStage.TradeStageDate = newTradePoolStage.TradeStageDate;
                    var result = await tpsCtrl.PutTradePoolStage(tpCurrentStage.ID, tpCurrentStage);
                    continue;
                }

                
                if(newTradePoolStage.StageID == nextStageId)
                {
                    var result = await tpsCtrl.PostTradePoolStage(newTradePoolStage);
                    nextStageId++;
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

            if (tradePool.TradeID > 1 && db.TradePools.Where(p => (p.TradeID == tradePool.TradeID && p.TradeName == tradePool.TradeName)).Count() > 0)
            {
                //If the TradeId of the incoming entry matches an existing TradePool ID and name then update
                db.Entry(tradePool).State = EntityState.Modified;
            }
            else
            {
                //Otherwise add the new
                db.TradePools.Add(tradePool);
            }

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tradePool.TradeID }, tradePool);
        }

        // POST: api/TradePools
        [Route("counterparties", Name = "CounterPartyApi")]
        [ResponseType(typeof(TradeCounterParty))]
        public async Task<IHttpActionResult> PostTradeCounterParty(TradeCounterParty counterParty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if((counterParty == null) || db.TradeCounterParties.Where(cp => cp.CounterPartyName.ToLower() == counterParty.CounterPartyName.ToLower()).Count() > 0)
            {
                return BadRequest("The counter party name is invalid or already taken");
            }
               
            db.TradeCounterParties.Add(counterParty);
            
            await db.SaveChangesAsync();

            return CreatedAtRoute("CounterPartyApi", new { id = counterParty.CounterPartyID }, counterParty);
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

        private async Task<TradePoolAssetsSummary> GetTradePoolAssetsSummaryAsynch(int tradeId)
        {
            TradePoolAssetsSummary poolAssetsSummary = new TradePoolAssetsSummary(tradeId);
            List<AssetSummary> assetSummaries = await GetAssetSummariesAsynch(tradeId);
            poolAssetsSummary.AssetSummaries = assetSummaries;
            return poolAssetsSummary;
        }

        private async Task<List<AssetSummary>> GetAssetSummariesAsynch(int tradeId)
        {
            List<TradeAsset> assets = await db.TradeAssets.Where(a => a.TradeID == tradeId).OrderBy(o => o.AssetID).ToListAsync();
            List<AssetSummary> assetSummaries = new List<AssetSummary>();
            foreach (var asset in assets)
            {
                AssetSummary assetSummary = new AssetSummary()
                {
                    TradeID = asset.TradeID,
                    AssetID = asset.AssetID,
                    InOutStatus = "IN",
                    NumberOfIssues = 0,
                    TotalRepriceAmount = 0,
                    OriginalPrice = asset.OriginalBalance,
                    CurrentPrice = asset.CurrentBalance,
                    Zip = asset.Zip
                };

                assetSummaries.Add(assetSummary);
            }

            return assetSummaries;
        }


    }
}
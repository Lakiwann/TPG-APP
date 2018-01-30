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

            // List<TradeAsset> assets = await db.TradeAssets.Where(a => a.TradeID == id).ToListAsync();

            List<TradeAssetPricing> assetPricing = await db.TradeAssetPricings.Where(p => p.TradeID == id).ToListAsync();

            TradePoolHighLevelSummary poolSummary = assetPricing == null || assetPricing.Count == 0 ?
                new TradePoolHighLevelSummary(id) { TotalCount = 0, TotalDebt = 0, TotalPurchasePrice = 0, TotalReprices = 0, TotalKicks = 0, TotalPIFs = 0, TotalTrailingDocs = 0 } :
                new TradePoolHighLevelSummary(id)
                {
                    TotalCount = assetPricing.Count(),
                    TotalDebt = assetPricing.Sum(a => a.CurrentBalance + a.ForebearanceBalance),
                    TotalPurchasePrice = assetPricing.Sum(a => a.CurrentPrice),
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
                yearlySummary.CounterParties = pools.GroupBy(p => p.CounterPartyID).Count();

                List<AssetSummary> assetSummaries = new List<AssetSummary>();
                //yearlySummary.RPLoans = 0;
                //yearlySummary.NPLoans = 0;
                //yearlySummary.MixedLoans = assetSummaries.Count();
                //yearlySummary.BidAmount = 0;
                //yearlySummary.TradeAmount = assetSummaries.Count() > 0 ? assetSummaries.Sum(a => a.CurrentPrice) : 0;
                //yearlySummary.RepriceAmount = assetSummaries.Count() > 0 ? assetSummaries.Sum(a => a.TotalRepriceAmount) : 0;
                //yearlySummary.AverageCloseTime = assetSummaries.Count() > 0 ? assetSummaries.Average(a => a.CloseTime) : 0;
                //yearlySummary.AverageFallOut = assetSummaries.Count() > 0 ? ((assetSummaries.Where(a => a.InStatus == false).Count() / assetSummaries.Count()) * 100) : 0;
                //yearlySummary.PurchasesAmount = assetSummaries.Count() > 0 ? assetSummaries.Sum(a => a.CurrentPrice) : 0;
                //yearlySummary.SalesAmount = 0;
                foreach (var p in pools)
                {
                    List<AssetSummary> poolAssetSummaries = await GetAssetSummariesAsynch(p.TradeID);
                    if(poolAssetSummaries.Count > 0)
                    {
                        yearlySummary.Mixed += poolAssetSummaries.Count();
                        yearlySummary.TradeAmount += poolAssetSummaries.Sum(a => a.CurrentDebt);
                        yearlySummary.BidAmount += poolAssetSummaries.Sum(a => a.CurrentPrice);
                        yearlySummary.PurchasesAmount = poolAssetSummaries.Sum(a => a.CurrentPrice);
                    }
                    //if (poolAssetSummaries.Count() > 0)
                    //{
                    //    assetSummaries.AddRange(poolAssetSummaries);
                    //}
                }
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
            //int nextStageId = tpCurrentStage != null ? tpCurrentStage.StageID + 1 : 1;
            int currentStageId = tpCurrentStage != null ? tpCurrentStage.StageID : 0;
            foreach (var tradePoolStage in tradePool.TradePoolStages.OrderBy(s => s.StageID))
            {
                if ((tpCurrentStage != null)&&(tradePoolStage.StageID < currentStageId))
                    continue;

                //var newTradePoolStage = tradePoolStage;
                //newTradePoolStage.TradeID = tradePool.TradeID;
                if ((tpCurrentStage != null) && (tradePoolStage.StageID == currentStageId))
                {
                    
                    tpCurrentStage.TradeStageDate = tradePoolStage.TradeStageDate;
                    //Update currentstage -  (PUT) 
                    var result = await tpsCtrl.PutTradePoolStage(tpCurrentStage.ID, tpCurrentStage);
                    continue;
                }

                if (tradePoolStage.StageID > currentStageId)
                {
                    db.TradePoolStages.Add(tradePoolStage);
                    await db.SaveChangesAsync();

                    //TODO:  This needs to be migrated to a service later.  The above will set the stage to 'Pool Awarded'.  Now generate the PAL IDs for the assets.  
                    if (currentStageId == 2) //If the current stage is Out for Bid before setting it to 'Pool Awarded' assign PAL IDs
                    {
                        //If the new stage is 'Pool Awarded' then Apply the PALID
                        await ApplyPalIDsToTradeAssets(tradePool);
                    }
                    //ENDTODO
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task ApplyPalIDsToTradeAssets(TradePool tradePool)
        {
            List<TradeAsset> assets = await db.TradeAssets.Where(a => a.TradeID == tradePool.TradeID).ToListAsync();
            foreach (var asset in assets)
            {
                string standardizedAssetSearchString = GenerateStandardizedAssetSearchString(asset);

                //Check if there is a PalID matching the AssetID for the seller
                List<PalisadesAssetReference> palEntities = await db.PalisadesAssetReferences.Where(p => (p.SellerCounterPartyId == asset.Seller_CounterPartyID) && (p.SellerAssetId == asset.SellerAssetId)).ToListAsync();
                PalisadesAssetReference pal = null;
                if (palEntities.Count() > 0)
                {
                    pal = palEntities.Where(pe => pe.StandardizedAssetSearchCriteria == standardizedAssetSearchString).OrderByDescending(o => o.CreatedDate).First();
                }
                else
                {
                    try
                    {
                        pal = await db.PalisadesAssetReferences.Where(p => (p.SellerCounterPartyId == asset.Seller_CounterPartyID) && (p.StandardizedAssetSearchCriteria == standardizedAssetSearchString)).OrderByDescending(o => o.CreatedDate).FirstAsync();
                    }
                    catch(Exception)
                    {
                        //TODO:  if there are no results an exception will be thrown - ignoring for now as that would allow to continue the flow.  Make some improvements later to check the results before trying to access the 'First' above
                    }
                }

                if (pal == null)
                {
                    //We couldn't find a matching palid.  Generate a new one
                    pal = new PalisadesAssetReference();
                    pal.SellerCounterPartyId = asset.Seller_CounterPartyID;
                    pal.SellerAssetId = asset.SellerAssetId;
                    pal.StandardizedAssetSearchCriteria = standardizedAssetSearchString;
                    pal.CreatedDate = DateTime.UtcNow;

                    //Save in the DB and once save is successful the object will have a PalID created
                    try
                    {
                        var newPal = db.PalisadesAssetReferences.Add(pal);
                        await db.SaveChangesAsync();
                        asset.PalId = newPal.PalId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                    //asset.PalID = assetPal.PalID;
                }
                else
                {
                    asset.PalId = pal.PalId;
                }
                db.Entry(asset).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        private static string GenerateStandardizedAssetSearchString(TradeAsset asset)
        {

            //TODO:  Defer this for now.  Build the asset serach criteria later.  For now just set it to the origination date
            string standardizedAssetSearchString = asset.OriginalDate.GetValueOrDefault().ToString();
            //string standardizedAddress = asset.StreetAddress1.Trim() + "|" + asset.City.Trim() + "|" + asset.State.Trim() + "|" + asset.Zip.Trim();
            //standardizedAssetSearchString = asset.MaturityDate.Value.ToFileTimeUtc() + "|" + standardizedAddress;
            //if (standardizedAssetSearchString.Length > 250)
            //{
            //    standardizedAssetSearchString = standardizedAssetSearchString.Take(250).ToString();
            //}

            return standardizedAssetSearchString;
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
            IQueryable<TradeAsset> assets = db.TradeAssets.Where(a => a.TradeID == tradeId);
            IQueryable <TradeAssetPricing> pricing = db.TradeAssetPricings.Where(p => p.TradeID == tradeId);

            List<AssetSummary> assetSummaries = await assets.Join(pricing, a => a.AssetID, p => p.AssetID,
            (a, p) => new AssetSummary()
            {
                TradeID = a.TradeID,
                AssetID = a.AssetID,
                SellerAssetId = p.SellerAssetID,
                Status = "IN",
                NumberOfIssues = 0,
                TotalRepriceAmount = 0,
                OriginalDebt = p.OriginalDebt,
                CurrentDebt = p.CurrentBalance + p.ForebearanceBalance,
                OriginalPrice = p.OriginalPrice,
                CurrentPrice = p.CurrentPrice,
                Zip = a.Zip
            })
            .ToListAsync();
            
            return assetSummaries;
        }


    }
}
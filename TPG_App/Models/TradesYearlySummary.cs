using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPG_App.Models
{
    /// <summary>
    /// TradesAcrossYearsSummary contains the high level summary of the totaled-attributes for all the trades, aggregated by year.
    /// </summary>
    public class TradesYearlySummary
    {
        public int Year { get; set; }
        public int Trades { get; set; }
        public int CounterParties { get; set; }
        public int RPLoans { get; set; }
        public int NPLoans { get; set; }
        public int MixedLoans { get; set; }
        public decimal BidAmount {get; set;}
        public decimal TradeAmount { get; set; }
        public decimal RepriceAmount { get; set; }
        public decimal AverageCloseTime { get; set; }
        public decimal AverageFallOut { get; set; }
        public decimal PurchasesAmount { get; set; }
        public decimal SalesAmount { get; set; }
    }
}
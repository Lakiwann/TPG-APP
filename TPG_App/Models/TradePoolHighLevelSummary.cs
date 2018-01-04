using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPG_App.Models
{
    public class TradePoolHighLevelSummary
    {
        int tradeId;
        public TradePoolHighLevelSummary(int tradeID)
        {
            tradeId = tradeID;
        }
        public int TradeID { get { return tradeId; } }
        public decimal TotalDebt { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public int TotalCount { get; set; }
        public int TotalReprices { get; set; }
        public int TotalKicks { get; set; }
        public int TotalPIFs { get; set; }
        public int TotalTrailingDocs { get; set; }
    }
}
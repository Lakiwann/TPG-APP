using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPG_App.Models
{
    public class TradePoolAssetsSummary
    {
        private int tradeId = 0;
       
        public TradePoolAssetsSummary(int tradeID)
        {
            tradeId = tradeID;
        }

        public int TradeId { get { return tradeId; } }
        public List<AssetSummary> AssetSummaries { get; set; }
    }

    public class AssetSummary
    {
        public int TradeID { get; set; }
        public long AssetID { get; set; }

        public string SellerAssetId { get; set; }
        public string Status { get; set; }

        public int NumberOfIssues { get; set; }

        public decimal TotalRepriceAmount { get; set; }

        public decimal OriginalDebt { get; set; }
        public decimal CurrentDebt { get; set; }

        public decimal OriginalPrice { get; set; }
        
        public decimal CurrentPrice { get; set; }

        public string Zip { get; set; }
        public decimal CloseTime { get; set; }
    }
}
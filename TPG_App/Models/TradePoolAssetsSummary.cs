using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPG_App.Models
{
    public class TradePoolAssetsSummary
    {
        private int tradeId = 0;
        List<AssetSummary> assetSummaries = new List<AssetSummary>();

        public TradePoolAssetsSummary(int tradeID)
        {
            tradeId = tradeID;
        }

        public int TradeId { get { return tradeId; } }
        public List<AssetSummary> AssetSummaries { get { return assetSummaries; } }
    }

    public class AssetSummary
    {
        public long AssetID { get; set; }
        public string InOutStatus { get; set; }

        public int NumberOfIssues { get; set; }

        public decimal TotalRepriceAmount { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}
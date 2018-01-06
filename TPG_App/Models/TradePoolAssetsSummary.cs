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
        private bool inStatus = true;
        public long AssetID { get; set; }
        public string InOutStatus { get; set; }

        public int NumberOfIssues { get; set; }

        public decimal TotalRepriceAmount { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal CloseTime { get; set; }
        public bool InStatus {
            get { return inStatus; }
            set { inStatus = value; }
        }
    }
}
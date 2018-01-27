namespace TPG_App.Models
{
    using Audit.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradeAssetPricing")]
    [AuditInclude]
    public partial class TradeAssetPricing : TradeAssetPricingBase
    {
        [Key]
        public long AssetPricingID { get; set; }
    }

    public class TradeAssetPricingBase
    {
        public int TradeID { get; set; }

        public long AssetID { get; set; }
        public short Seller_CounterPartyID { get; set; }
        public string SellerAssetID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal OriginalDebt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CurrentBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ForebearanceBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal OriginalPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CurrentPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal BidPercentage { get; set; }

        [StringLength(50)]
        public string Source { get; set; }

    }
}

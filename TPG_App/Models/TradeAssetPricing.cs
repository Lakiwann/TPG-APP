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
    public partial class TradeAssetPricing
    {
        [Key]
        public long AssetPricingID { get; set; }

        public long AssetID { get; set; }

        [NotMapped]
        public string SellerAssetID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UnpaidBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal BidPercentage { get; set; }

        [StringLength(20)]
        public string Source { get; set; }
    }
}

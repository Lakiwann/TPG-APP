namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradeAssetPricingHistory")]
    public partial class TradeAssetPricingHistory : TradeAssetPricingBase
    {
        [Key]
        public long ID { get; set; }

        public long AssetPricingID { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(20)]
        public string Action { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
    }
}

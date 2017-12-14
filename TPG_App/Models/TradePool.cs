namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradePool")]
    public partial class TradePool
    {
        [Key]
        public int TradeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TradeType { get; set; }

        [Required]
        [StringLength(100)]
        public string TradeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EstSettlementDate { get; set; }

        [StringLength(100)]
        public string ManagerName { get; set; }

        [StringLength(3)]
        public string ManagerInitials { get; set; }
    }
}

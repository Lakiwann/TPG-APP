namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.LU_DiligenceDesc")]
    public partial class TradeAssetDiligenceDesc
    {
        public int ID { get; set; }

        public byte CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}

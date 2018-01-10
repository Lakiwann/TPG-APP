namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradeAssetDiligence")]
    public partial class TradeAssetDiligence
    {
        [Key]
        public long DeligenceID { get; set; }

        public int TradeID { get; set; }

        public long AssetID { get; set; }

        public byte TypeID { get; set; }

        public byte CategoryID { get; set; }

        public int DescriptionID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }
    }
}

namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.PalisadesAssetReference")]
    public partial class PalisadesAssetReference
    {
        [Key]
        public long PalID { get; set; }

        public short Seller_CounterPartyID { get; set; }

        [StringLength(250)]
        public string StandardizedAssetAddress { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }
    }
}

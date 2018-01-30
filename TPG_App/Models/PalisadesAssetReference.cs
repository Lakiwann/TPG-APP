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
        [Column("PalID")]
        public long PalId { get; set; }

        [Column("Seller_CounterPartyID")]
        public short SellerCounterPartyId { get; set; }

        [StringLength(50)]
        [Column("Seller_AssetID")]
        public string SellerAssetId { get; set; }

        [StringLength(250)]
        public string StandardizedAssetSearchCriteria { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }
    }
}

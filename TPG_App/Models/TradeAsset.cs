namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradeAsset")]
    public partial class TradeAsset
    {
        [Key]
        public long AssetID { get; set; }

        public int TradeID { get; set; }

        public int TapeID { get; set; }

        public short Seller_CounterPartyID { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerAssetID { get; set; }

        public long? PalID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal OriginalBalance { get; set; }

        [NotMapped]
        public decimal CurrentBalance { get; set; }

        [NotMapped]
        public decimal ForebearanceBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Bpo { get; set; }

        public DateTime? BpoDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OriginalPmt { get; set; }

        public DateTime? OriginalDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CurrentPmt { get; set; }

        public DateTime? PaidToDate { get; set; }

        public DateTime? NextDueDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        [StringLength(100)]
        public string StreetAddress1 { get; set; }

        [StringLength(100)]
        public string StreetAddress2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(100)]
        public string StreetAddress1_Standardized { get; set; }

        [StringLength(100)]
        public string StreetAddress2_Standardized { get; set; }

        [StringLength(50)]
        public string City_Standardized { get; set; }

        [StringLength(10)]
        public string State_Standardized { get; set; }

        [StringLength(10)]
        public string Zip_Standardized { get; set; }

        [StringLength(100)]
        public string Cbsa { get; set; }

        [StringLength(100)]
        public string CbsaName { get; set; }

        [StringLength(100)]
        public string ProdType { get; set; }

        [StringLength(100)]
        public string LoanPurp { get; set; }

        [StringLength(100)]
        public string PropType { get; set; }

        [StringLength(5)]
        public string OrigFico { get; set; }

        [StringLength(5)]
        public string CurrFico { get; set; }

        public DateTime? CurrFicoDate { get; set; }

        [StringLength(1000)]
        public string PayString { get; set; }
    }
}

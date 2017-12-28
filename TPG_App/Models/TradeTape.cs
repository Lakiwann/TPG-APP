namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradeTape")]
    public partial class TradeTape
    {
        [Key]
        public int TapeID { get; set; }

        public int TradeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [StringLength(1000)]
        public string StoragePath { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ImportedDate { get; set; }
    }
}

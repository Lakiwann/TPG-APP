namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.LU_TapeColumnDefs")]
    public partial class TradeTapeColumnDef
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string TapeName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string PalFieldName { get; set; }

        [Required]
        [StringLength(50)]
        public string ColumnName { get; set; }

        [Required]
        [StringLength(10)]
        public string ColumnType { get; set; }
    }
}

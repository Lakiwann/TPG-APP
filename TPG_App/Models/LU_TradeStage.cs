namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.LU_TradeStage")]
    public partial class LU_TradeStage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LU_TradeStage()
        {
            //TradePoolStages = new HashSet<TradePoolStage>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte StageID { get; set; }

        [Required]
        [StringLength(50)]
        public string StageName { get; set; }

        [Required]
        [StringLength(1)]
        public string Status { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TradePoolStage> TradePoolStages { get; set; }
    }
}

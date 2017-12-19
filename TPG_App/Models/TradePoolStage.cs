namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.TradePoolStage")]
    public partial class TradePoolStage
    {
        public int ID { get; set; }

        public int TradeID { get; set; }

        public byte StageID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TradeStageDate { get; set; }

        public virtual LU_TradeStage LU_TradeStage { get; set; }

       // public virtual TradePool TradePool { get; set; }
    }
}

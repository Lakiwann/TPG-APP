namespace TPG_App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trd.CounterParty")]
    public partial class TradeCounterParty
    {
        [Key]
        public short CounterPartyID { get; set; }

        [Required]
        [StringLength(50)]
        public string CounterPartyName { get; set; }
    }
}

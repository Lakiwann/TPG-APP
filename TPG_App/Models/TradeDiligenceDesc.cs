namespace TPG_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TradeDiligenceDesc : DbContext
    {
        public TradeDiligenceDesc()
            : base("name=TradeDiligenceDesc")
        {
        }

        public virtual DbSet<LU_DiligenceDesc> LU_DiligenceDesc { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LU_DiligenceDesc>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}

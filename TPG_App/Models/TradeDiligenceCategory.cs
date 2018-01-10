namespace TPG_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TradeDiligenceCategory : DbContext
    {
        public TradeDiligenceCategory()
            : base("name=TradeDiligenceCategory")
        {
        }

        public virtual DbSet<LU_DiligenceCategory> LU_DiligenceCategory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LU_DiligenceCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);
        }
    }
}

namespace TPG_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TradeDiligenceType : DbContext
    {
        public TradeDiligenceType()
            : base("name=TradeDiligenceType")
        {
        }

        public virtual DbSet<LU_DiligenceType> LU_DiligenceType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LU_DiligenceType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);
        }
    }
}

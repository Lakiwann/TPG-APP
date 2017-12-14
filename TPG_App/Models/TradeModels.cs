namespace TPG_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TradeModels : DbContext
    {
        public TradeModels()
            : base("name=TradeModels")
        {
        }

        public virtual DbSet<TradePool> TradePools { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TradePool>()
                .Property(e => e.TradeType)
                .IsUnicode(false);

            modelBuilder.Entity<TradePool>()
                .Property(e => e.TradeName)
                .IsUnicode(false);

            modelBuilder.Entity<TradePool>()
                .Property(e => e.ManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<TradePool>()
                .Property(e => e.ManagerInitials)
                .IsUnicode(false);
        }
    }
}

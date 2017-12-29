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

        public System.Data.Entity.DbSet<TPG_App.Models.TradePoolStage> TradePoolStages { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.LU_TradeStage> LU_TradeStages { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeTape> TradeTapes { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeAsset> TradeAssets { get; set; }
    }
}

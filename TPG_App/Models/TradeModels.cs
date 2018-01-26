namespace TPG_App.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Audit.EntityFramework;
    using Audit.EntityFramework.Providers;
    using Audit.Core;

    [AuditDbContext(Mode = AuditOptionMode.OptIn)]
    public partial class TradeModels : AuditDbContext
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

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.SellerAssetID)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.StreetAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.StreetAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.StreetAddress1_Standardized)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.StreetAddress2_Standardized)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.City_Standardized)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.State_Standardized)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.Zip_Standardized)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.Cbsa)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.CbsaName)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.ProdType)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.LoanPurp)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.PropType)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.OrigFico)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.CurrFico)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAsset>()
                .Property(e => e.PayString)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAssetDiligenceType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<TradeAssetDiligenceCategory>()
               .Property(e => e.CategoryName)
               .IsUnicode(false);

            modelBuilder.Entity<TradeAssetDiligenceDesc>()
               .Property(e => e.Description)
               .IsUnicode(false);

            modelBuilder.Entity<TradeCounterParty>()
               .Property(e => e.CounterPartyName)
               .IsUnicode(false);

            modelBuilder.Entity<TradeTapeColumnDef>()
               .Property(e => e.TapeName)
               .IsUnicode(false);

            modelBuilder.Entity<TradeTapeColumnDef>()
                .Property(e => e.PalFieldName)
                .IsUnicode(false);

            modelBuilder.Entity<TradeTapeColumnDef>()
                .Property(e => e.ColumnName)
                .IsUnicode(false);

            modelBuilder.Entity<TradeTapeColumnDef>()
                .Property(e => e.ColumnType)
                .IsUnicode(false);

        }

        public System.Data.Entity.DbSet<TPG_App.Models.TradePoolStage> TradePoolStages { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.LU_TradeStage> LU_TradeStages { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeTape> TradeTapes { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeAsset> TradeAssets { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeAssetDiligence> TradeAssetDiligences { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.TradeAssetDiligenceType> TradeAssetDiligenceTypes { get; set; }
        public System.Data.Entity.DbSet<TPG_App.Models.TradeAssetDiligenceCategory> TradeAssetDiligenceCategories { get; set; }
        public System.Data.Entity.DbSet<TradeAssetDiligenceDesc> TradeAssetDiligenceDescriptions { get; set; }

        public System.Data.Entity.DbSet<TradeCounterParty> TradeCounterParties { get; set; }

        public System.Data.Entity.DbSet<TPG_App.Models.PalisadesAssetReference> PalisadesAssetReferences { get; set; }

        public System.Data.Entity.DbSet<TradeTapeColumnDef> TradeTapeColumnDefs { get; set; }

        public System.Data.Entity.DbSet<TradeAssetPricing> TradeAssetPricings { get; set; }

        public System.Data.Entity.DbSet<TradeAssetPricingHistory> TradeAssetPricingHistories { get; set; }
    }
}

USE [APP]
GO

/****** Object:  Table [Trd].[TradeAsset]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[TradeAsset](
	[AssetID] [bigint] IDENTITY(1,1) NOT NULL,
	[TradeID] [int] NOT NULL,
	[TapeID] [int] NOT NULL,
	[Seller_CounterPartyID] smallint NOT NULL,
	[SellerAssetID] varchar(50) NOT NULL,
	[PalID] [bigint] NULL,
	[OriginalBalance] numeric (18, 2) NOT NULL,
	[Bpo] numeric (18, 2) NULL,
	[BpoDate] datetime NULL,
	[OriginalPmt] numeric (18, 2) NULL,
	[OriginalDate] datetime NULL,
	[CurrentPmt] numeric (18, 2) NULL,
	[PaidToDate] datetime NULL,
	[NextDueDate] datetime NULL,
	[MaturityDate] datetime NULL,
	[StreetAddress1] varchar(100) NULL,
	[StreetAddress2] varchar(100) NULL,
	[City] varchar(50) NULL,
	[State] varchar(10) NULL,
	[Zip] varchar(10) NULL,
	[StreetAddress1_Standardized] varchar(100) NULL,
	[StreetAddress2_Standardized] varchar(100) NULL,
	[City_Standardized] varchar(50) NULL,
	[State_Standardized] varchar(10) NULL,
	[Zip_Standardized] varchar(10) NULL,
	[Cbsa] varchar(100) NULL,
	[CbsaName] varchar(100) NULL,
	[ProdType] varchar(100) NULL,
	[LoanPurp] varchar(100) NULL,
	[PropType] varchar(100) NULL,
	[OrigFico] varchar(5) NULL,
	[CurrFico] varchar(5) NULL,
	[CurrFicoDate] datetime NULL,
	[PayString] varchar(1000) NULL
PRIMARY KEY CLUSTERED 
(
	[AssetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[TradeAsset]  WITH CHECK ADD  CONSTRAINT [FK_TradeAsset_TradeID] FOREIGN KEY([TradeID])
REFERENCES [Trd].[TradePool] ([TradeID])
GO

ALTER TABLE [Trd].[TradeAsset]  WITH CHECK ADD  CONSTRAINT [FK_TradeAsset_TapeID] FOREIGN KEY([TapeID])
REFERENCES [Trd].[TradeTape] ([TapeID])
ON DELETE CASCADE
GO

ALTER TABLE [Trd].[TradeAsset]  WITH CHECK ADD  CONSTRAINT [FK_TradeAsset_Seller_CounterPartyID] FOREIGN KEY([Seller_CounterPartyID])
REFERENCES [Trd].[CounterParty] ([CounterPartyID])
GO

ALTER TABLE [Trd].[TradeAsset]  WITH CHECK ADD  CONSTRAINT [FK_TradeAsset_PalID] FOREIGN KEY([PalID])
REFERENCES [Trd].[PalisadesAssetReference] ([PalID])
GO

ALTER TABLE [Trd].[TradeAsset]  WITH CHECK ADD  CONSTRAINT [UC_TradeAsset] UNIQUE([TradeID], [Seller_CounterPartyID], [SellerAssetID])
GO

CREATE NONCLUSTERED INDEX IX_TradeAsset_TradeID   
    ON [Trd].[TradeAsset] (TradeID);   
GO  




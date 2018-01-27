USE [APP]
GO

/****** Object:  Table [Trd].[TradeAssetPricing]   Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[TradeAssetPricing](
    [AssetPricingID] [bigint] IDENTITY(1,1) NOT NULL,
	[TradeID] int NOT NULL,
	[AssetID] [bigint] NOT NULL,
	[Seller_CounterPartyID] smallint NOT NULL,
	[SellerAssetID] varchar(50) NOT NULL,
	[OriginalDebt] numeric (18,2) NOT NULL,
	[CurrentBalance] numeric (18, 2) NOT NULL,
	[ForebearanceBalance] numeric (18, 2) NOT NULL,
	[OriginalPrice] numeric (18,2) NOT NULL,
	[CurrentPrice] numeric (18,2) NOT NULL,
	[BidPercentage] numeric (3, 2) NOT NULL,
	[Source] varchar (50)
PRIMARY KEY CLUSTERED 
(
	[AssetPricingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[TradeAssetPricing]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetPricing_TradeID] FOREIGN KEY([TradeID])
REFERENCES [Trd].[TradePool] ([TradeID])
GO

ALTER TABLE [Trd].[TradeAssetPricing]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetPricing_AssetID] FOREIGN KEY([AssetID])
REFERENCES [Trd].[TradeAsset] ([AssetID])
ON DELETE CASCADE
GO

CREATE NONCLUSTERED INDEX IX_TradeAssetPricing_TradeID   
    ON [Trd].[TradeAssetPricing] (TradeID);   
GO  

CREATE NONCLUSTERED INDEX IX_TradeAssetPricing_AssetID   
    ON [Trd].[TradeAssetPricing] (AssetID);   
GO  

CREATE NONCLUSTERED INDEX IX_TradeAssetPricing_TradeID_SellerID
    ON [Trd].[TradeAssetPricing] (TradeID, SellerAssetID);   
GO  




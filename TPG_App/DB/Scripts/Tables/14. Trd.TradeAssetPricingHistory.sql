USE [APP]
GO

/****** Object:  Table [Trd].[[TradeAssetPricingHistory]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[TradeAssetPricingHistory](
    [ID] [bigint] IDENTITY(1,1) NOT NULL,
    [AssetPricingID] [bigint] NOT NULL,
	[AssetID] [bigint] NOT NULL,
	[TradeID] int NOT NULL,
	[Seller_CounterPartyID] smallint NOT NULL,
	[SellerAssetID] varchar(50) NOT NULL,
	[OriginalDebt] numeric (18,2) NOT NULL,
	[CurrentBalance] numeric (18, 2) NOT NULL,
	[ForebearanceBalance] numeric (18, 2) NOT NULL,
	[OriginalPrice] numeric (18,2) NOT NULL,
	[CurrentPrice] numeric (18,2) NOT NULL,
	[BidPercentage] numeric (3, 2) NOT NULL,
	[Source] varchar(50),
	[Date] datetime NOT NULL,
	[Action] varchar(20) NOT NULL,
	[UserName] varchar(50) NOT NULL

PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [Trd].[TradeAssetPricingHistory]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetPricingHistory_AssetPricingID] FOREIGN KEY([AssetPricingID])
REFERENCES [Trd].[TradeAssetPricing] ([AssetPricingID])
ON DELETE CASCADE
GO

CREATE NONCLUSTERED INDEX IX_TradeAssetPricingHistory_AssetID   
    ON [Trd].[TradeAssetPricingHistory] (AssetID);   
GO  



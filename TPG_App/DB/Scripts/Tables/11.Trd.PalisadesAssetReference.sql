USE [APP]
GO

/****** Object:  Table [Trd].[PalisadesAssetReference]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[PalisadesAssetReference](
    [PalID] [bigint] IDENTITY(1000000,1) NOT NULL,
	[Seller_CounterPartyID] smallint NOT NULL,
	[Seller_AssetID] varchar(50) NULL,  -- The PalID is loosely coupled with Seller_assetID and standardized property address so business logic can be built to
	[StandardizedAssetSearchCriteria] varchar(250) NULL,  --... lookup the PalID from a matching seller ID and StandardizedAssetSearchCriteria combination to associate the asset to the same PALID if the asset is received later from the same seller
	[CreatedDate] date NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[PalID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[PalisadesAssetReference]  WITH CHECK ADD  CONSTRAINT [FK_PalisadesAssetReference_SellerID] FOREIGN KEY([Seller_CounterPartyID])
REFERENCES [Trd].[CounterParty] ([CounterPartyID])
GO

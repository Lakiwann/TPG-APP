USE [APP]
GO

/****** Object:  Table [Trd].[TradePoolStage]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[TradeAssetDeligence](
    [DeligenceID] [bigint] IDENTITY(1,1) NOT NULL,
	[TradeID] [int] NOT NULL,
	[AssetID] [bigint] NOT NULL,
	[TypeID] [tinyint] NOT NULL,
	[CategoryID] [tinyint] NOT NULL,
	[DescriptionID] [int] NOT NULL,
	[CreatedDate] date NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[DeligenceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[TradeAssetDeligence]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetDel_TradeID] FOREIGN KEY([TradeID])
REFERENCES [Trd].[TradePool] ([TradeID])
GO

ALTER TABLE [Trd].[TradeAssetDeligence] WITH CHECK ADD  CONSTRAINT [FK_TradeAssetDel_AssetID] FOREIGN KEY([AssetID])
REFERENCES [Trd].[TradeAsset] ([AssetID])
ON DELETE CASCADE
GO

ALTER TABLE [Trd].[TradeAssetDeligence]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetDel_TypeID] FOREIGN KEY([TypeID])
REFERENCES [Trd].[lu_DiligenceType] ([ID])
GO

ALTER TABLE [Trd].[TradeAssetDeligence]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetDel_CatID] FOREIGN KEY([CategoryID])
REFERENCES [Trd].[lu_DiligenceCategory] ([ID])
GO

ALTER TABLE [Trd].[TradeAssetDeligence]  WITH CHECK ADD  CONSTRAINT [FK_TradeAssetDel_DescID] FOREIGN KEY([DescriptionID])
REFERENCES [Trd].[lu_DiligenceDesc] ([ID])
GO



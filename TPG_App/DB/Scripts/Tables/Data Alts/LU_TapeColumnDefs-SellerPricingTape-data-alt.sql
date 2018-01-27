USE [APP]
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'SellerAssetID', 'SELLER_ID', 'varchar')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'CurrentBalance', 'Current Balance', 'decimal')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'ForebearanceBalance', 'Forebearance Balance', 'decimal')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'CurrentPrice', 'PRICE$', 'decimal')
GO


INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'BidPercentage', 'PRICE%', 'percent')
GO
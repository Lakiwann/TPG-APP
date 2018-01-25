USE [APP]
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'SellerAssetID', 'SELLER_ID', 'varchar')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'BidPercentage', 'Bid_Px (%)', 'percent')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'UnpaidBalance', 'UPB(T)', 'decimal')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerPricingTape', 'BidPercentage', 'Bid_Px (%)', 'percent')
GO
USE [APP]
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'SellerAssetID', 'LOANID', 'varchar')
GO

INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'OriginalBalance', 'ORIG BAL', 'decimal')
GO
INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'CurrentBalance', 'CURR BAL', 'decimal')
GO
INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'Bpo', 'BPO', 'decimal')
GO
INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'BpoDate', 'BPO DATE', 'datetime')
GO
INSERT INTO [Trd].[LU_TapeColumnDefs] ([TapeName],  [PalFieldName], [ColumnName], [ColumnType])
     VALUES('SellerBidTape', 'OriginalPmt', 'ORIG PMT', 'decimal')
GO

TODO:
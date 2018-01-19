USE [APP]
GO

/****** Object:  Table [Trd].[lu_TradeBidTapeColumnDefs]    Script Date: 12/12/2017 3:29:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


CREATE TABLE [Trd].[LU_TapeColumnDefs](
    [TapeName] varchar(20) NOT NULL,
	[PalFieldName] varchar(20) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[ColumnType] [varchar](10) NOT NULL,
	CONSTRAINT PK_TapeName_PalFieldName PRIMARY KEY(TapeName, PalFieldName)
) ON [PRIMARY]

GO


ALTER TABLE [Trd].[lu_TapeColumnDefs] ADD CONSTRAINT U_TapeName_ColNm UNIQUE(TapeName, ColumnName)
GO

SET ANSI_PADDING OFF
GO


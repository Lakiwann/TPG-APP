USE [APP]
GO

/****** Object:  Table [dbo].[TRD_TradePool]    Script Date: 12/12/2017 3:29:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


CREATE SCHEMA Trd


CREATE TABLE [Trd].[TradePool](
	[TradeID] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TradeType] [varchar](50) NOT NULL,
	[TradeName] [varchar](100) NOT NULL,
	[EstSettlementDate] [date] NULL,
	[ManagerName] [varchar](100) NULL,
	[ManagerInitials] [varchar](3) NULL
) ON [PRIMARY]

GO

ADD CONSTRAINT UC_TradePool_TradeType_TradeName UNIQUE (TradeType,TradeName)


SET ANSI_PADDING OFF
GO


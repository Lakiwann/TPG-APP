USE [APP]
GO

/****** Object:  Table [dbo].[TRD_TradePool]    Script Date: 12/12/2017 3:29:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


--CREATE SCHEMA Trd --Trd schema should be created by running the schema scripts


CREATE TABLE [Trd].[TradePool](
	[TradeID] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TradeType] [varchar](50) NOT NULL,
	[TradeName] [varchar](100) NOT NULL,
	[EstSettlementDate] [date] NULL,
	[ManagerName] [varchar](100) NULL,
	[ManagerInitials] [varchar](3) NULL
) ON [PRIMARY]

GO


SET ANSI_PADDING OFF
GO

--added the CounterParty field 01/11/2018 and the foreign key constraint

ALTER TABLE [Trd].[TradePool]  
ADD CounterPartyID smallint NULL
GO

ALTER TABLE [Trd].[TradePool] WITH  CHECK ADD CONSTRAINT [FK_TradePool_CounterPartyID] FOREIGN KEY([CounterPartyID])
REFERENCES [Trd].[CounterParty] ([CounterPartyID])
GO

ALTER TABLE [Trd].[TradePool] WITH  CHECK ADD CONSTRAINT UC_TradePool_TradeType_TradeName UNIQUE (TradeType,TradeName)
GO
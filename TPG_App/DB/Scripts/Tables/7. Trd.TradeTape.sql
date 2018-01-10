USE [APP]
GO

/****** Object:  Table [Trd].[TradePoolStage]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[TradeTape](
	[TapeID] [int] IDENTITY(1,1) NOT NULL,
	[TradeID] [int] NOT NULL,
	[Name] varchar(50) NOT NULL,
	[Description] varchar(250) NULL,
	[StoragePath] varchar(1000) NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[ImportedDate] datetime NULL
PRIMARY KEY CLUSTERED 
(
	[TapeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[TradeTape]  WITH CHECK ADD  CONSTRAINT [FK_TradeTape_TradeID] FOREIGN KEY([TradeID])
REFERENCES [Trd].[TradePool] ([TradeID])
ON DELETE CASCADE
GO

ALTER TABLE [Trd].[TradeTape] CHECK CONSTRAINT [FK_TradeTape_TradeID]
GO


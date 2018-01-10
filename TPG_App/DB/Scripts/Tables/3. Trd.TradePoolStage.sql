USE [APP]
GO

/****** Object:  Table [dbo].[TRD_TradePool]    Script Date: 12/12/2017 3:29:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Trd].[TradePoolStage](
	[ID] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TradeID] [int] NOT NULL,
	[StageID] [tinyint] NOT NULL,
	[TradeStageDate] [date] NULL,

CONSTRAINT FK_TradePoolStage_TradeId FOREIGN KEY (TradeID)
	REFERENCES [Trd].[TradePool] (TradeID)
	ON DELETE CASCADE,

CONSTRAINT FK_TradePoolStage_StageId FOREIGN KEY (StageID)
	REFERENCES [Trd].[LU_TradeStage] (StageID)
	ON DELETE CASCADE

) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [APP]
GO

/****** Object:  Table [Trd].[TradeStageDef]    Script Date: 12/18/2017 3:31:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Trd].[LU_TradeStage](
	[StageID] [tinyint] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[StageName] [varchar](50) NOT NULL,
	[Status] [varchar](1) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

GO

SET ANSI_PADDING OFF
GO


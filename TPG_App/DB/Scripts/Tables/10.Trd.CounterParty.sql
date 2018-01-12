USE [APP]
GO

/****** Object:  Table [Trd].[TradePoolStage]    Script Date: 12/27/2017 4:31:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Trd].[CounterParty](
    [CounterPartyID] [smallint] IDENTITY(1,1) NOT NULL,
	[CounterPartyName] varchar(50) NOT NULL,

	PRIMARY KEY CLUSTERED 
	(
		[CounterPartyID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



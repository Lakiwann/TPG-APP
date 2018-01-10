USE [APP]
GO

/****** Object:  Table [dbo].[TRD_TradePool]    Script Date: 12/12/2017 3:29:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Trd].[LU_DiligenceDesc](
	[ID] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[CategoryID] [tinyint] NOT NULL,
	[Description] varchar(50) NOT NULL

CONSTRAINT FK_DiligenceDesc_CategoryID FOREIGN KEY (CategoryID)
	REFERENCES [Trd].[LU_DiligenceCategory] (ID)
	ON DELETE CASCADE,
) ON [PRIMARY]

GO

ALTER TABLE [Trd].[LU_DiligenceDesc] ADD CONSTRAINT U_CategoryDescription UNIQUE(CategoryID, Description)

SET ANSI_PADDING OFF
GO


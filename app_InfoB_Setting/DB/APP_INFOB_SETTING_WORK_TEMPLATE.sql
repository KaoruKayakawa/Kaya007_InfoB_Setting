
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[APP_INFOB_SETTING_WORK_TEMPLATE]') AND type in (N'U'))
DROP TABLE [dbo].[APP_INFOB_SETTING_WORK_TEMPLATE]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[APP_INFOB_SETTING_WORK_TEMPLATE](
	[FOLDERNM] [varchar](10) NOT NULL,
	[KAIINCD_FROM] [int] NOT NULL,
	[KAIINCD_TO] [int] NOT NULL,
	[FLAG] [tinyint] NOT NULL
) ON [PRIMARY]
GO

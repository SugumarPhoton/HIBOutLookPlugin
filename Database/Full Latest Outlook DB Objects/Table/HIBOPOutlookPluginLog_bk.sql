/****** Object:  Table [dbo].[HIBOPOutlookPluginLog_bk]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPOutlookPluginLog_bk]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPOutlookPluginLog_bk](
	[LogId] [int] IDENTITY(100001,1) NOT NULL,
	[UniqId] [varchar](125) NULL,
	[UniqEmployee] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqActivity] [int] NOT NULL,
	[PolicyYear] [varchar](10) NULL,
	[PolicyType] [varchar](50) NULL,
	[DescriptionType] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[FolderId] [int] NULL,
	[SubFolder1Id] [int] NULL,
	[SubFolder2Id] [int] NULL,
	[ClientAccessibleDate] [datetime] NULL,
	[EmailAction] [varchar](100) NULL,
	[Version] [int] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[ClientLookupCode] [varchar](10) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

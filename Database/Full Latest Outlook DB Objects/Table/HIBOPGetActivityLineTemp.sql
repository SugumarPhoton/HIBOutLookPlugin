/****** Object:  Table [dbo].[HIBOPGetActivityLineTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityLineTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPGetActivityLineTemp](
	[UploadGuid] [varchar](128) NULL,
	[UniqLine] [int] NULL,
	[UniqPolicy] [int] NULL,
	[UniqEntity] [int] NULL,
	[PolicyType] [varchar](4) NULL,
	[PolicyDesc] [varchar](125) NULL,
	[LineCode] [varchar](4) NULL,
	[LineOfBusiness] [varchar](500) NULL,
	[LineStatus] [varchar](500) NULL,
	[PolicyNumber] [varchar](25) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[UniqCdLineStatus] [int] NULL,
	[IOC] [varchar](6) NULL,
	[BillModeCode] [varchar](1) NULL,
	[ExpirationDate] [datetime] NULL,
	[EffectiveDate] [datetime] NULL,
	[Status] [varchar](20) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Flags] [int] NULL,
	[EntryDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Activity_HIBOPGetActivityLineTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityLineTemp]') AND name = N'IX_Activity_HIBOPGetActivityLineTemp')
CREATE NONCLUSTERED INDEX [IX_Activity_HIBOPGetActivityLineTemp] ON [dbo].[HIBOPGetActivityLineTemp]
(
	[UploadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_HIBOPGetActivityLineTEMP_EntryDate]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityLineTemp]') AND name = N'IX_HIBOPGetActivityLineTEMP_EntryDate')
CREATE NONCLUSTERED INDEX [IX_HIBOPGetActivityLineTEMP_EntryDate] ON [dbo].[HIBOPGetActivityLineTemp]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__HIBOPGetA__Entry__339FAB6E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HIBOPGetActivityLineTemp] ADD  DEFAULT (getdate()) FOR [EntryDate]
END

GO

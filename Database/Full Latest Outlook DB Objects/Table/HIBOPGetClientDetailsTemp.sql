/****** Object:  Table [dbo].[HIBOPGetClientDetailsTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientDetailsTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPGetClientDetailsTemp](
	[UploadGuid] [varchar](128) NULL,
	[UniqEntity] [int] NULL,
	[LookupCode] [varchar](10) NULL,
	[NameOf] [varchar](100) NULL,
	[Address] [varchar](500) NULL,
	[City] [varchar](50) NULL,
	[StateCode] [varchar](4) NULL,
	[StateName] [varchar](50) NULL,
	[PostalCode] [varchar](10) NULL,
	[CountryCode] [varchar](4) NULL,
	[Country] [varchar](125) NULL,
	[UniqAgency] [int] NULL,
	[AgencyCode] [varchar](8) NULL,
	[AgencyName] [varchar](100) NULL,
	[PrimaryContactName] [varchar](100) NULL,
	[Status] [varchar](10) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[EntryDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Activity_HIBOPGetActivityDetailsTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientDetailsTemp]') AND name = N'IX_Activity_HIBOPGetActivityDetailsTemp')
CREATE NONCLUSTERED INDEX [IX_Activity_HIBOPGetActivityDetailsTemp] ON [dbo].[HIBOPGetClientDetailsTemp]
(
	[UploadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_HIBOPGetClientDetailsTemp_EntryDate]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientDetailsTemp]') AND name = N'IX_HIBOPGetClientDetailsTemp_EntryDate')
CREATE NONCLUSTERED INDEX [IX_HIBOPGetClientDetailsTemp_EntryDate] ON [dbo].[HIBOPGetClientDetailsTemp]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__HIBOPGetC__Entry__3493CFA7]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HIBOPGetClientDetailsTemp] ADD  DEFAULT (getdate()) FOR [EntryDate]
END

GO

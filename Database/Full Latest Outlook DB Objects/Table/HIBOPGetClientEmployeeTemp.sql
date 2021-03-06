/****** Object:  Table [dbo].[HIBOPGetClientEmployeeTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientEmployeeTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPGetClientEmployeeTemp](
	[UploadGuid] [varchar](128) NULL,
	[ClientId] [int] NULL,
	[EntryDate] [datetime] NULL,
	[Client_UpdatedDate] [datetime] NULL,
	[Client_InsertedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Activity_HIBOPGetClientEmployeeTemp1]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientEmployeeTemp]') AND name = N'IX_Activity_HIBOPGetClientEmployeeTemp1')
CREATE NONCLUSTERED INDEX [IX_Activity_HIBOPGetClientEmployeeTemp1] ON [dbo].[HIBOPGetClientEmployeeTemp]
(
	[UploadGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_HIBOPGetClientEmployeeTemp_EntryDate]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientEmployeeTemp]') AND name = N'IX_HIBOPGetClientEmployeeTemp_EntryDate')
CREATE NONCLUSTERED INDEX [IX_HIBOPGetClientEmployeeTemp_EntryDate] ON [dbo].[HIBOPGetClientEmployeeTemp]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__HIBOPGetC__Entry__7EF6D905]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HIBOPGetClientEmployeeTemp] ADD  DEFAULT (getdate()) FOR [EntryDate]
END

GO

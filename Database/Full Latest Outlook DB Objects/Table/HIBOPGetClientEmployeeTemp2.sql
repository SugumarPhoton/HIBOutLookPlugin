/****** Object:  Table [dbo].[HIBOPGetClientEmployeeTemp2]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientEmployeeTemp2]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPGetClientEmployeeTemp2](
	[UploadGuid] [varchar](128) NULL,
	[ClientId] [int] NULL,
	[EntryDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__HIBOPGetC__Entry__1A9EF37A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[HIBOPGetClientEmployeeTemp2] ADD  DEFAULT (getdate()) FOR [EntryDate]
END

GO

/****** Object:  Table [dbo].[Monitor_Drive]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Monitor_Drive]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Monitor_Drive](
	[Drive] [varchar](10) NULL,
	[MbFree] [int] NULL,
	[IN_GB] [float] NULL,
	[IN_TB] [float] NULL,
	[EntryDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Monitor_D__Entry__3864608B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Monitor_Drive] ADD  DEFAULT (getdate()) FOR [EntryDate]
END

GO

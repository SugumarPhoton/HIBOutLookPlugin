/****** Object:  Table [dbo].[Monitor_Disk]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Monitor_Disk]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Monitor_Disk](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DBName] [varchar](250) NULL,
	[DBType] [varchar](50) NULL,
	[SpaceOccupiedinGB] [float] NULL,
	[Entrydate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF__Monitor_D__Entry__367C1819]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Monitor_Disk] ADD  DEFAULT (getdate()) FOR [Entrydate]
END

GO

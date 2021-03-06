/****** Object:  Table [dbo].[HIBOPActivityServices]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityServices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityServices](
	[UniqServiceHead] [int] NULL,
	[UniqEntity] [int] NULL,
	[ServiceNumber] [smallint] NULL,
	[UniqCdServiceCode] [char](4) NULL,
	[Description] [varchar](max) NULL,
	[ContractNumber] [varchar](125) NULL,
	[InceptionDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[UniqDepartment] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Services_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityServices]') AND name = N'IX_Services_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Services_UniqEntity] ON [dbo].[HIBOPActivityServices]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

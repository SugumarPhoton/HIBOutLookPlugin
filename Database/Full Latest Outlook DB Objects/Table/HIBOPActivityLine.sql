/****** Object:  Table [dbo].[HIBOPActivityLine]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityLine]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityLine](
	[UniqLine] [int] NOT NULL,
	[UniqPolicy] [int] NULL,
	[UniqEntity] [int] NULL,
	[PolicyDesc] [varchar](125) NULL,
	[LineCode] [char](4) NULL,
	[LineOfBusiness] [varchar](500) NULL,
	[LineStatus] [varchar](500) NULL,
	[PolicyNumber] [varchar](25) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[UniqCdLineStatus] [int] NULL,
	[IOC] [char](6) NULL,
	[BillModeCode] [char](1) NULL,
	[ExpirationDate] [datetime] NULL,
	[EffectiveDate] [datetime] NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [HIBOPActivityLine_PK] PRIMARY KEY CLUSTERED 
(
	[UniqLine] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IDX_HIBOPActivityLine_ExpirationDate]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityLine]') AND name = N'IDX_HIBOPActivityLine_ExpirationDate')
CREATE NONCLUSTERED INDEX [IDX_HIBOPActivityLine_ExpirationDate] ON [dbo].[HIBOPActivityLine]
(
	[ExpirationDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Line_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityLine]') AND name = N'IX_Line_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Line_UniqEntity] ON [dbo].[HIBOPActivityLine]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

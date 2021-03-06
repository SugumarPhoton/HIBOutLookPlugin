/****** Object:  Table [dbo].[HIBOPClient]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPClient](
	[OPCId] [int] IDENTITY(1,1) NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[LookupCode] [varchar](10) NOT NULL,
	[NameOf] [varchar](100) NOT NULL,
	[PrimaryContactName] [varchar](100) NOT NULL,
	[Address] [varchar](500) NULL,
	[City] [varchar](50) NULL,
	[StateCode] [varchar](4) NULL,
	[StateName] [varchar](50) NULL,
	[PostalCode] [varchar](10) NULL,
	[CountryCode] [varchar](4) NULL,
	[Country] [varchar](125) NULL,
	[Status] [int] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[InactiveDate] [datetime] NULL,
 CONSTRAINT [HIBOPClient_PK] PRIMARY KEY CLUSTERED 
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Client_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClient]') AND name = N'IX_Client_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Client_UniqEntity] ON [dbo].[HIBOPClient]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

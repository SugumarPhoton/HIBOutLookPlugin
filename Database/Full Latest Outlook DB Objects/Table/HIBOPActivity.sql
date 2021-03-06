/****** Object:  Table [dbo].[HIBOPActivity]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivity](
	[OPAId] [int] IDENTITY(1,1) NOT NULL,
	[UniqActivity] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqActivityCode] [int] NULL,
	[ActivityCode] [char](4) NULL,
	[DescriptionOf] [varchar](125) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[PolicyNumber] [varchar](50) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[Status] [int] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[ClosedDate] [datetime] NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[UniqDepartment] [int] NULL,
	[UniqProfitCenter] [int] NULL,
	[UniqAssociatedItem] [int] NULL,
	[AssociationType] [varchar](20) NULL,
	[UniqEmployee] [int] NULL,
	[UniqPolicy] [int] NULL,
	[UniqLine] [int] NULL,
	[UniqClaim] [int] NULL,
	[LossDate] [datetime] NULL,
	[PolicyDescription] [varchar](125) NULL,
	[LineCode] [varchar](4) NULL,
	[LineDescription] [varchar](max) NULL,
	[ICO] [varchar](6) NULL,
	[LineEffectiveDate] [datetime] NULL,
	[LineExpirationDate] [datetime] NULL,
	[UniqEntityCompanyIssuing] [int] NULL,
	[UniqEntityCompanyBilling] [int] NULL,
 CONSTRAINT [HIBOPActivity_PK] PRIMARY KEY CLUSTERED 
(
	[UniqEntity] ASC,
	[UniqActivity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Activity_UniqActivity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND name = N'IX_Activity_UniqActivity')
CREATE NONCLUSTERED INDEX [IX_Activity_UniqActivity] ON [dbo].[HIBOPActivity]
(
	[UniqActivity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Activity_UniqAssociatedItem]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND name = N'IX_Activity_UniqAssociatedItem')
CREATE NONCLUSTERED INDEX [IX_Activity_UniqAssociatedItem] ON [dbo].[HIBOPActivity]
(
	[UniqAssociatedItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Activity_UniqCdPolicyLineType]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND name = N'IX_Activity_UniqCdPolicyLineType')
CREATE NONCLUSTERED INDEX [IX_Activity_UniqCdPolicyLineType] ON [dbo].[HIBOPActivity]
(
	[UniqCdPolicyLineType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Activity_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND name = N'IX_Activity_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Activity_UniqEntity] ON [dbo].[HIBOPActivity]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_HIBOPActivity_Clsdt_agency_branch]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivity]') AND name = N'IX_HIBOPActivity_Clsdt_agency_branch')
CREATE NONCLUSTERED INDEX [IX_HIBOPActivity_Clsdt_agency_branch] ON [dbo].[HIBOPActivity]
(
	[ClosedDate] ASC,
	[UniqAgency] ASC,
	[UniqBranch] ASC
)
INCLUDE ( 	[UniqActivity],
	[UniqEntity],
	[UniqDepartment],
	[UniqEmployee]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

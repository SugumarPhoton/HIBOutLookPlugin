/****** Object:  Table [dbo].[HIBOPActivityOpportunity]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityOpportunity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityOpportunity](
	[UniqOpportunity] [int] NOT NULL,
	[UniqEntity] [int] NULL,
	[OppDesc] [varchar](125) NULL,
	[TargetedDate] [datetime] NULL,
	[ActualDate] [datetime] NULL,
	[UniqSalesTeam] [int] NULL,
	[UniqEmployeeOwner] [int] NULL,
	[UniqOpportunityStage] [int] NULL,
	[OwnerName] [varchar](125) NULL,
	[SalesTeam] [varchar](125) NULL,
	[SalesManager] [varchar](125) NULL,
	[Stage] [varchar](125) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[UniqDepartment] [int] NULL,
 CONSTRAINT [HIBOPActivityOpportunity_PK] PRIMARY KEY CLUSTERED 
(
	[UniqOpportunity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Opportunity_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityOpportunity]') AND name = N'IX_Opportunity_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Opportunity_UniqEntity] ON [dbo].[HIBOPActivityOpportunity]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

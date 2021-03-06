/****** Object:  Table [dbo].[HIBOPPolicy]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPPolicy](
	[UniqPolicy] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqAgency] [int] NULL,
	[UniqBranch] [int] NULL,
	[DescriptionOf] [varchar](125) NULL,
	[UniqCdPolicyLineType] [int] NULL,
	[PolicyNumber] [varchar](25) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[PolicyStatus] [varchar](20) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqDepartment] [int] NULL,
 CONSTRAINT [HIBOPPolicy_PK] PRIMARY KEY CLUSTERED 
(
	[UniqPolicy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IDX_HIBOPPolicy_ExpirationDate]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicy]') AND name = N'IDX_HIBOPPolicy_ExpirationDate')
CREATE NONCLUSTERED INDEX [IDX_HIBOPPolicy_ExpirationDate] ON [dbo].[HIBOPPolicy]
(
	[ExpirationDate] ASC
)
INCLUDE ( 	[UniqEntity],
	[UniqAgency],
	[UniqBranch],
	[UniqCdPolicyLineType],
	[InsertedDate],
	[UpdatedDate],
	[UniqDepartment]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Policy_UniqCdPolicyLineType]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicy]') AND name = N'IX_Policy_UniqCdPolicyLineType')
CREATE NONCLUSTERED INDEX [IX_Policy_UniqCdPolicyLineType] ON [dbo].[HIBOPPolicy]
(
	[UniqCdPolicyLineType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Policy_UniqEntity]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicy]') AND name = N'IX_Policy_UniqEntity')
CREATE NONCLUSTERED INDEX [IX_Policy_UniqEntity] ON [dbo].[HIBOPPolicy]
(
	[UniqEntity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[HIBOPPolicyLineType]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicyLineType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPPolicyLineType](
	[UniqCdPolicyLineType] [int] NOT NULL,
	[CdPolicyLineTypeCode] [varchar](4) NOT NULL,
	[PolicyLineTypeDesc] [varchar](125) NULL,
	[Status] [int] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqDepartment] [int] NULL,
	[UniqProfitCenter] [int] NULL,
 CONSTRAINT [UniqCdPolicyLineType_PK] PRIMARY KEY CLUSTERED 
(
	[UniqCdPolicyLineType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_HIBOPPolicyLineType_uniqcdpolicylinetype]    Script Date: 2/22/2019 3:06:22 AM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPPolicyLineType]') AND name = N'IX_HIBOPPolicyLineType_uniqcdpolicylinetype')
CREATE NONCLUSTERED INDEX [IX_HIBOPPolicyLineType_uniqcdpolicylinetype] ON [dbo].[HIBOPPolicyLineType]
(
	[UniqCdPolicyLineType] ASC
)
INCLUDE ( 	[CdPolicyLineTypeCode]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[HIBOPActivityLookupDetails]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityLookupDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityLookupDetails](
	[ALDId] [int] IDENTITY(1,1) NOT NULL,
	[UniqLine] [int] NOT NULL,
	[UniqPolicy] [int] NOT NULL,
	[UniqEntity] [int] NOT NULL,
	[UniqClaim] [int] NULL,
	[LineCode] [char](4) NOT NULL,
	[PolicyType] [char](4) NOT NULL,
	[Linedescription] [nvarchar](max) NOT NULL,
	[PolicyNumber] [varchar](25) NOT NULL,
	[PolicyDesc] [varchar](125) NOT NULL,
	[UniqCdPolicyLineType] [int] NOT NULL,
	[UniqCdLineStatus] [int] NOT NULL,
	[LineExpDate] [datetime] NOT NULL,
	[LineEffDate] [datetime] NOT NULL,
	[PolicyExpDate] [datetime] NOT NULL,
	[PolicyEffDate] [datetime] NOT NULL,
	[ClaimNumber] [int] NULL,
	[CompanyClaimNumber] [varchar](25) NULL,
	[DateLoss] [datetime] NULL,
	[ClosedDate] [datetime] NULL,
	[LookupCode] [char](10) NOT NULL,
	[AccountName] [varchar](100) NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UniqClaimAssociation] [int] NULL,
	[IOC] [varchar](125) NULL,
	[IOCCode] [varchar](20) NULL,
	[AttachmentDesc] [varchar](125) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

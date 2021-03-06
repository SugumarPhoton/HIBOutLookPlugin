/****** Object:  Table [dbo].[HIBOPActivityPolicyTemp]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityPolicyTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityPolicyTemp](
	[UploadGuid] [varchar](128) NULL,
	[UniqEntity] [int] NULL,
	[CdPolicyLineTypeCode] [varchar](128) NULL,
	[UniqPolicy] [int] NULL,
	[PolicyNumber] [varchar](25) NULL,
	[DescriptionOf] [varchar](120) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[PolicyStatus] [varchar](20) NULL,
	[Status] [varchar](20) NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

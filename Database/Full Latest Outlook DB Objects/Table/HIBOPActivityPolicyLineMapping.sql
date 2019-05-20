/****** Object:  Table [dbo].[HIBOPActivityPolicyLineMapping]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPActivityPolicyLineMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPActivityPolicyLineMapping](
	[UniqActivity] [int] NULL,
	[UniqPolicy] [int] NULL,
	[UniqLine] [int] NULL,
	[UniqMarketingLine] [int] NULL,
	[UniqCarrierSubmission] [int] NULL
) ON [PRIMARY]
END
GO

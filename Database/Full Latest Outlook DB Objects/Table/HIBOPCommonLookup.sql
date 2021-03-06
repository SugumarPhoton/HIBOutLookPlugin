/****** Object:  Table [dbo].[HIBOPCommonLookup]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPCommonLookup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPCommonLookup](
	[CommonLkpId] [int] NOT NULL,
	[CommonLkpTypeCode] [varchar](50) NOT NULL,
	[CommonLkpCode] [varchar](10) NOT NULL,
	[CommonLkpName] [varchar](500) NOT NULL,
	[CommonLkpDescription] [varchar](1000) NULL,
	[SortOrder] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

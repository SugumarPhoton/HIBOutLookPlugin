/****** Object:  Table [dbo].[HIBOPBranch]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPBranch]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPBranch](
	[UniqBranch] [int] NOT NULL,
	[BranchCode] [varchar](10) NOT NULL,
	[BranchName] [varchar](100) NULL,
	[LicenceNumber] [varchar](15) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [HIBOPBranch_PK] PRIMARY KEY CLUSTERED 
(
	[UniqBranch] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

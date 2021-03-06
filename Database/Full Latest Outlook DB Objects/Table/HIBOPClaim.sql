/****** Object:  Table [dbo].[HIBOPClaim]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClaim]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPClaim](
	[UniqEntity] [int] NOT NULL,
	[UniqClaim] [int] NOT NULL,
	[ClaimCode] [varchar](5) NULL,
	[ClaimName] [varchar](125) NULL,
	[LossDate] [datetime] NULL,
	[ReportedDate] [datetime] NULL,
	[ClaimNumber] [int] NULL,
	[CompanyClaimNumber] [varchar](25) NULL,
	[ClosedDate] [datetime] NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [HIBOPClaim_PK] PRIMARY KEY CLUSTERED 
(
	[UniqClaim] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

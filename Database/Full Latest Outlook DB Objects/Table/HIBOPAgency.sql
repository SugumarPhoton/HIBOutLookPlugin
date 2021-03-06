/****** Object:  Table [dbo].[HIBOPAgency]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPAgency]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPAgency](
	[UniqAgency] [int] NOT NULL,
	[AgencyCode] [char](3) NULL,
	[AgencyName] [varchar](100) NULL,
	[LicenseNumber] [varchar](15) NULL,
	[Flags] [int] NULL,
	[InsertedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [HIBOPAgency_PK] PRIMARY KEY CLUSTERED 
(
	[UniqAgency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[HIBOPUserDeltaSyncInfo_backup01222019]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSyncInfo_backup01222019]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPUserDeltaSyncInfo_backup01222019](
	[UserDeltaSyncInfo] [int] IDENTITY(1,1) NOT NULL,
	[IPAddress] [varchar](100) NULL,
	[EmployeeId] [int] NULL,
	[UserLookupCode] [varchar](10) NULL,
	[SpName] [varchar](256) NULL,
	[LastSyncDate] [datetime] NULL,
	[IsDeltaFlag] [bit] NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

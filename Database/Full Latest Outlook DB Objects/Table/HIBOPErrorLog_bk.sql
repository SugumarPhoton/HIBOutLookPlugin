/****** Object:  Table [dbo].[HIBOPErrorLog_bk]    Script Date: 2/22/2019 3:06:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPErrorLog_bk]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HIBOPErrorLog_bk](
	[LogID] [bigint] IDENTITY(1000001,1) NOT NULL,
	[Source] [varchar](256) NULL,
	[Thread] [int] NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](max) NULL,
	[Exception] [varchar](4000) NULL,
	[LogDate] [datetime] NULL,
	[LoggedBy] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

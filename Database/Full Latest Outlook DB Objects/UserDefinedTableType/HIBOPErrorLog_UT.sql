/****** Object:  UserDefinedTableType [dbo].[HIBOPErrorLog_UT]    Script Date: 2/7/2019 6:42:13 AM ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'HIBOPErrorLog_UT' AND ss.name = N'dbo')
CREATE TYPE [dbo].[HIBOPErrorLog_UT] AS TABLE(
	[Source] [varchar](256) NULL,
	[Thread] [int] NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](max) NULL,
	[Exception] [varchar](4000) NULL,
	[LoggedBy] [varchar](100) NULL,
	[LogDate] [datetime] NULL
)
GO

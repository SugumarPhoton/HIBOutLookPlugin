IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPErrorLog')
BEGIN
CREATE TABLE HIBOPErrorLog
(
	[LogID] [bigint] IDENTITY(1000001,1) NOT NULL,
	[Source] [varchar](256) NULL,
	[Thread] [int] NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](max) NULL,
	[Exception] [varchar](4000) NULL,
	[LogDate] [datetime] NULL,
	[LoggedBy] [varchar](100) NULL
) 
END
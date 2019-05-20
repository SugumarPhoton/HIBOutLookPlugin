IF NOT EXISTS ( SELECT 'X' FROM SYS.table_types WHERE NAME = 'HIBOPErrorLog_UT')
BEGIN
CREATE TYPE [dbo].[HIBOPErrorLog_UT] AS TABLE(
	[Source]				VARCHAR(256),
	[Thread]				INT ,
	[Level]					VARCHAR(50),
	[Logger]			    VARCHAR(255),
	[Message]				VARCHAR(MAX),
	[Exception]				VARCHAR(4000),
	[LoggedBy]				VARCHAR(100),
	[LogDate]				DATETIME
	
	
)
END
GO

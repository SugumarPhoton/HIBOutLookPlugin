IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='HIBOPActivityCode_IDX1')
BEGIN
	DROP INDEX HIBOPActivityCode.HIBOPActivityCode_IDX1;
END
GO

CREATE UNIQUE CLUSTERED INDEX HIBOPActivityCode_IDX1 on HIBOPActivityCode (UniqActivityCode);
GO
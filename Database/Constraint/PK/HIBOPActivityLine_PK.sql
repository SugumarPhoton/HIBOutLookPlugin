

IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='HIBOPActivityLine_PK')
BEGIN
	ALTER TABLE HIBOPActivityLine DROP CONSTRAINT HIBOPActivityLine_PK;
END
GO
ALTER TABLE HIBOPActivityLine
ADD CONSTRAINT HIBOPActivityLine_PK PRIMARY KEY CLUSTERED  (UniqLine);
GO






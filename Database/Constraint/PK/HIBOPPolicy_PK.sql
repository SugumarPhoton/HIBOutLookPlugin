

IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='HIBOPPolicy_PK')
BEGIN
	ALTER TABLE HIBOPPolicy DROP CONSTRAINT HIBOPPolicy_PK;
END
GO
ALTER TABLE HIBOPPolicy
ADD CONSTRAINT HIBOPPolicy_PK PRIMARY KEY CLUSTERED  (UniqPolicy);
GO



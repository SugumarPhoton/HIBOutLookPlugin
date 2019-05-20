

IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='HIBOPActivity_PK')
BEGIN
	ALTER TABLE HIBOPActivity DROP CONSTRAINT HIBOPActivity_PK;
END
GO
ALTER TABLE HIBOPActivity
ADD CONSTRAINT HIBOPActivity_PK PRIMARY KEY CLUSTERED  (UniqEntity,UniqActivity);
GO

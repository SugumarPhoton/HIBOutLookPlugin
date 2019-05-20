IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='HIBOPAId_PK')
BEGIN
	ALTER TABLE HIBOPActivity DROP CONSTRAINT HIBOPAId_PK;
END
GO
ALTER TABLE HIBOPActivity
ADD CONSTRAINT HIBOPAId_PK PRIMARY KEY CLUSTERED  ([OPAid] ASC);
GO

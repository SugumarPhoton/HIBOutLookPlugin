

IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='HIBOPAgency_PK')
BEGIN
	ALTER TABLE HIBOPAgency DROP CONSTRAINT HIBOPAgency_PK;
END
GO
ALTER TABLE HIBOPAgency
ADD CONSTRAINT HIBOPAgency_PK PRIMARY KEY CLUSTERED  (UniqAgency);
GO


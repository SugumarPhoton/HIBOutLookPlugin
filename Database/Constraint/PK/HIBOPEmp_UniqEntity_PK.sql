
IF EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='PK' AND NAME ='Emp_UniqEntity_PK')
BEGIN
	ALTER TABLE HIBOPEmployee DROP CONSTRAINT Emp_UniqEntity_PK;
END
GO
ALTER TABLE HIBOPEmployee
ADD CONSTRAINT Emp_UniqEntity_PK PRIMARY KEY CLUSTERED  ([UniqEntity] ASC);
GO


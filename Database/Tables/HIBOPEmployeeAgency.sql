IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPEmployeeAgency')
BEGIN
CREATE TABLE HIBOPEmployeeAgency
(
UniqEntity			INT NOT NULL,
UniqAgency			INT NOT NULL,
UniqBranch			INT NOT NULL,
UniqDepartment		INT NOT NULL,
UniqProfitCenter	INT NOT NULL,
InsertedDate		DATETIME,
UpdatedDate			DATETIME
)END
GO






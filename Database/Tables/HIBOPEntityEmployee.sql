IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME ='HIBOPEntityEmployee')
BEGIN
CREATE TABLE [dbo].[HIBOPEntityEmployee]
(
[UniqEntity]      				INT NOT NULL,
UniqEmployee					INT	NOT NULL,
Lookupcode						CHAR(6) NOT NULL,
EmployeeName					VARCHAR(125)
)
END

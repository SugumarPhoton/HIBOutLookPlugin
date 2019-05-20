use EpicDMZSub

IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='securityuser_NDX1')
BEGIN
DROP INDEX securityuser.securityuser_NDX1;
END
GO
CREATE NONCLUSTERED INDEX securityuser_NDX1 ON securityuser(UniqEmployee);
GO
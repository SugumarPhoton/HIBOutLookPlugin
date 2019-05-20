IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_ActivityAccount_HIBOPGetActivityAccountTemp')   
   DROP INDEX IX_ActivityAccount_HIBOPGetActivityAccountTemp ON HIBOPGetActivityAccountTemp;   
GO  
CREATE NONCLUSTERED INDEX IX_ActivityAccount_HIBOPGetActivityAccountTemp   
   ON HIBOPGetActivityAccountTemp (Lookupcode,UniqAgency,UniqBranch,UniqEntity);   
GO  
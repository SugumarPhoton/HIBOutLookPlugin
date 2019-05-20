IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Transaction_UniqEntity')   
    DROP INDEX IX_Transaction_UniqEntity ON HIBOPActivityTransaction;   
GO  
CREATE NONCLUSTERED INDEX IX_Transaction_UniqEntity  
    ON HIBOPActivityTransaction (UniqEntity);   
GO  

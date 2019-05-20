IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Client_UniqEntity')   
    DROP INDEX IX_Client_UniqEntity ON HIBOPClient;   
GO  
CREATE NONCLUSTERED INDEX IX_Client_UniqEntity   
    ON HIBOPClient (UniqEntity);   
GO  

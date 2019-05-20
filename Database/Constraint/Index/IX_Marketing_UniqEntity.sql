IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Marketing_UniqEntity')   
    DROP INDEX IX_Marketing_UniqEntity ON HIBOPActivityMasterMarketing;   
GO  
CREATE NONCLUSTERED INDEX IX_Marketing_UniqEntity   
    ON HIBOPActivityMasterMarketing (UniqEntity);   
GO  

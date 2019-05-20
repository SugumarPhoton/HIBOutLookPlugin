IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_UniqEntity')   
    DROP INDEX IX_Activity_UniqEntity ON HIBOPActivity;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_UniqEntity   
    ON HIBOPActivity (UniqEntity);   
GO  

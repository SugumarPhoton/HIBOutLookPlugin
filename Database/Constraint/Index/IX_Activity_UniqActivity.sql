IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_UniqActivity')   
    DROP INDEX IX_Activity_UniqActivity ON HIBOPActivity;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_UniqActivity   
    ON HIBOPActivity (UniqActivity);   
GO  

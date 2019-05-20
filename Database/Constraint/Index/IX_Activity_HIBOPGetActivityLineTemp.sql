IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_HIBOPGetActivityLineTemp')   
    DROP INDEX IX_Activity_HIBOPGetActivityLineTemp ON HIBOPGetActivityLineTemp;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_HIBOPGetActivityLineTemp   
    ON HIBOPGetActivityLineTemp (UploadGuid);   
GO
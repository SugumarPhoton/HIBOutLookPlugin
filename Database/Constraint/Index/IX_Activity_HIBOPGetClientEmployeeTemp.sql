 IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_HIBOPGetClientEmployeeTemp')   
    DROP INDEX IX_Activity_HIBOPGetClientEmployeeTemp ON HIBOPGetClientEmployeeTemp;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_HIBOPGetClientEmployeeTemp   
    ON HIBOPGetClientEmployeeTemp (UploadGuid);   
GO  
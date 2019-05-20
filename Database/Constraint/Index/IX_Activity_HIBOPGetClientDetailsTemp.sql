IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_HIBOPGetActivityDetailsTemp')   
    DROP INDEX HIBOPGetClientDetailsTemp.IX_Activity_HIBOPGetActivityDetailsTemp;  
GO

CREATE NONCLUSTERED INDEX IX_Activity_HIBOPGetActivityDetailsTemp ON HIBOPGetClientDetailsTemp (UploadGuid);   
GO
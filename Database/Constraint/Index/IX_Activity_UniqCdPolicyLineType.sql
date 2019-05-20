IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_UniqCdPolicyLineType')   
    DROP INDEX IX_Activity_UniqCdPolicyLineType ON HIBOPActivity;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_UniqCdPolicyLineType   
    ON HIBOPActivity (UniqCdPolicyLineType);   
GO  

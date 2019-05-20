IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Marketing_UniqCdPolicyLineType')   
    DROP INDEX IX_Marketing_UniqCdPolicyLineType ON HIBOPActivityMasterMarketing;   
GO  
CREATE NONCLUSTERED INDEX IX_Marketing_UniqCdPolicyLineType  
    ON HIBOPActivityMasterMarketing (UniqCdPolicyLineType);   
GO  

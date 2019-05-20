IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Policy_UniqCdPolicyLineType')   
    DROP INDEX IX_Policy_UniqCdPolicyLineType ON HIBOPPolicy;   
GO  
CREATE NONCLUSTERED INDEX IX_Policy_UniqCdPolicyLineType   
    ON HIBOPPolicy (UniqCdPolicyLineType);   
GO  

IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Policy_UniqEntity')   
    DROP INDEX IX_Policy_UniqEntity ON HIBOPPolicy;   
GO  
CREATE NONCLUSTERED INDEX IX_Policy_UniqEntity   
    ON HIBOPPolicy (UniqEntity);   
GO  

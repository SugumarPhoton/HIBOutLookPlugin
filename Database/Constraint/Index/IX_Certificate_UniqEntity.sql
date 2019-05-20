IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Certificate_UniqEntity')   
    DROP INDEX IX_Certificate_UniqEntity ON HIBOPActivityCertificate;   
GO  
CREATE NONCLUSTERED INDEX IX_Certificate_UniqEntity  
    ON HIBOPActivityCertificate (UniqEntity);   
GO  

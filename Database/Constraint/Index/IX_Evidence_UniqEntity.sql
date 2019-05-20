IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Evidence_UniqEntity')   
    DROP INDEX IX_Evidence_UniqEntity ON HIBOPActivityEvidenceOfInsurance;   
GO  
CREATE NONCLUSTERED INDEX IX_Evidence_UniqEntity  
    ON HIBOPActivityEvidenceOfInsurance (UniqEntityClient);   
GO  

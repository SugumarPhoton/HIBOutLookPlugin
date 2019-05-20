IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Bill_UniqEntity')   
    DROP INDEX IX_Bill_UniqEntity ON HIBOPActivityBill;   
GO  
CREATE NONCLUSTERED INDEX IX_Bill_UniqEntity  
    ON HIBOPActivityBill (UniqEntity);   
GO  

IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Activity_UniqAssociatedItem')   
    DROP INDEX IX_Activity_UniqAssociatedItem ON HIBOPActivity;   
GO  
CREATE NONCLUSTERED INDEX IX_Activity_UniqAssociatedItem   
    ON HIBOPActivity (UniqAssociatedItem);   
GO  

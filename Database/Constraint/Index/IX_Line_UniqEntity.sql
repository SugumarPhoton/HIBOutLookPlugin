IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Line_UniqEntity')   
    DROP INDEX IX_Line_UniqEntity ON HIBOPActivityLine;   
GO  
CREATE NONCLUSTERED INDEX IX_Line_UniqEntity  
    ON HIBOPActivityLine (UniqEntity);   
GO  

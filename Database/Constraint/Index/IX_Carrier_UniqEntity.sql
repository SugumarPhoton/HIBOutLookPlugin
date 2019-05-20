IF EXISTS (SELECT NAME FROM sys.indexes WHERE name = N'IX_Carrier_UniqEntity')
    DROP INDEX HIBOPCarrierSubmission.IX_Carrier_UniqEntity;   
GO

CREATE NONCLUSTERED INDEX IX_Carrier_UniqEntity ON HIBOPCarrierSubmission (UniqEntity);   
GO
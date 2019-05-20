IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='HIBOPActivityPolicyTemp')
BEGIN
CREATE TABLE HIBOPActivityPolicyTemp
			(
			UploadGuid				VARCHAR(128)
			,UniqEntity				int
			,CdPolicyLineTypeCode	VARCHAR(128)
			,UniqPolicy				INT
			,PolicyNumber			VARCHAR(25)
			,DescriptionOf			VARCHAR(120)
			,EffectiveDate			DATETIME
			,ExpirationDate			DATETIME
			,PolicyStatus			VARCHAR(20)
			,[Status]				VARCHAR(20)
			,InsertedDate			DATETIME
			,UpdatedDate			DATETIME
			)
END
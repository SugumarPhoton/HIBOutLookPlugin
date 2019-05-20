IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPCarrierSubmission')
BEGIN	

	CREATE TABLE HIBOPCarrierSubmission
	(
	CarrierSubmissionId			INT IDENTITY(1,1),
	UniqCarrierSubmission		INT,
	Carrier						VARCHAR(125),
	CarrierSubmission			VARCHAR(125),
	UniqMarketingSubmission		INT,
	UniqEntity					INT,
	LastSubmittedDate			DATETIME,
	RequestedPremium			NUMERIC(19,4),
	SubmissionStatus			VARCHAR(70),
	InsertedDate				DATETIME,
	UpdatedDate					DATETIME,
	UniqActivity				INT
	)
END

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='HIBOPGetActivityAccountTemp')
BEGIN
CREATE TABLE HIBOPGetActivityAccountTemp
(AccountId INT IDENTITY(1,1)
,UniqEntity int
,UniqAgency int
,AgencyCode VARCHAR(4)
,AgencyName VARCHAR(100)
,UniqBranch INT
,BranchCode VARCHAR(10)
,BranchName VARCHAR(100)
,InsertedDate datetime
,UpdatedDate datetime
,Lookupcode VARCHAR(6)
)
END



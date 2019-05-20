IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPGetClientDetailsTemp')
BEGIN	
CREATE TABLE HIBOPGetClientDetailsTemp
(
UploadGuid VARCHAR(128),
UniqEntity INT,
LookupCode VARCHAR(10),
NameOf VARCHAR(100),
[Address] VARCHAR(500),
City VARCHAR(50),
StateCode VARCHAR(4),
StateName VARCHAR(50),
PostalCode VARCHAR(10),
CountryCode VARCHAR(4),
Country VARCHAR(125),
UniqAgency INT,
AgencyCode VARCHAR(8),
AgencyName VARCHAR(100),
PrimaryContactName VARCHAR(100),
[Status] VARCHAR(10),
InsertedDate DATETIME,
UpdatedDate DATETIME
)
END
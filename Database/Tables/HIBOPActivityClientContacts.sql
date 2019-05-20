IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE XTYPE='U' AND NAME = 'HIBOPActivityClientContacts')
BEGIN	
CREATE TABLE HIBOPActivityClientContacts
(
ClientContactId		INT IDENTITY(1,1),
UniqEntity			INT	 NOT NULL,
UniqContactName		INT NOT NULL,
ContactName			VARCHAR(125),
ContactType			VARCHAR(10),
ContactValue		VARCHAR(200),
InsertedDate		DATETIME,
UpdatedDate			DATETIME,
UniqContactNumber	INT
)END
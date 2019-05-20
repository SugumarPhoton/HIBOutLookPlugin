
DECLARE
@LookUpTypeCode	VARCHAR(50),
@CreatedDate DATETIME = getdate(),
@CreatedBy VARCHAR(50)='SYS',
@IsDeleted BIT=1


SET NOCOUNT ON

/******************************************************************************************* */
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType')
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1001,'ActivityType','AC','Account','Account Details',1,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END

IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1002,'ActivityType','CL','Claim','Claim Details',2,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1003,'ActivityType','LI','Line','Line',3,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1004,'ActivityType','OP','Opportunities','Opportunities',4,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1005,'ActivityType','P','Policy','Policy',5,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1006,'ActivityType','S','Services','Services',6,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityType' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1007,'ActivityType','MMS','Master Marketing Submission','Master Marketing Submission',7,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityPriority' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1008,'ActivityPriority','L','Low','Low',1,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END

IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityPriority' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1009,'ActivityPriority','N','Normal','Normal',2,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END

IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityPriority' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1010,'ActivityPriority','U','Urgent','Urgent',3,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityUpdate' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1011,'ActivityUpdate','','','Blank',1,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END

IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityUpdate' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1012,'ActivityUpdate','UEC','Update email calendar','Update email calendar',2,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityUpdate' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1013,'ActivityUpdate','UET','Update email tasks \ To do','Update email tasks \ To do',3,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityContact' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1014,'ActivityContact','M','Mail','Mail',1,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityContact' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1015,'ActivityContact','PH','Phone','Phone',2,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityContact' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1016,'ActivityContact','F','Fax','Fax',3,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityContact' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1017,'ActivityContact','E','Email','Email',4,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityAccessLevel' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1018,'ActivityAccessLevel','PUB','Public','Public',1,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityAccessLevel' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1019,'ActivityAccessLevel','ACC','Accounting','Accounting',2,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityAccessLevel' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1020,'ActivityAccessLevel','EM','Empower','Empower',3,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityAccessLevel' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1021,'ActivityAccessLevel','SO','Socius','Socius',4,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END
IF NOT EXISTS ( SELECT 'X' FROM HIBOPCommonLookup WHERE CommonLkpTypeCode = @LookUpTypeCode AND CommonLkpName = 'ActivityAccessLevel' )
BEGIN
	INSERT INTO HIBOPCommonLookup (CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,[IsDeleted],CreatedBy,CreatedDate,ModifiedBy,ModifiedDate)
	VALUES (1022,'ActivityAccessLevel','T','Tangram','Tangram',5,@IsDeleted,@CreatedBy,@CreatedDate,@CreatedBy,@CreatedDate)
END

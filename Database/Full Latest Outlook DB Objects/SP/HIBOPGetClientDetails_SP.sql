/****** Object:  StoredProcedure [dbo].[HIBOPGetClientDetails_SP]    Script Date: 2/7/2019 6:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetClientDetails_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetClientDetails_SP] AS' 
END
GO

/*
Declare @rowcount int 
EXEC HIBOPGetClientDetails_SP 'LEUNA1',NULL,'10.200.6.7',30000,1,@rowcount= @rowcount
select @rowcount
*/
/*
Declare @rowcount int 
EXEC HIBOPGetClientDetails_SP 'VIDJA1','2018-04-01 06:05:06.650','1.1.1.1',30000,1,@rowcount= @rowcount
select @rowcount
*/
ALTER PROCEDURE [dbo].[HIBOPGetClientDetails_SP]
(
@User		  VARCHAR(10),
@LastSyncDate DATETIME,
@IPAddress Varchar(100),
@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT OUTPUT
)
AS 
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
 
	DECLARE @securityuserid AS int
	DECLARE @UniqEmployee INT
	DECLARE @iCheckStructure AS TINYINT
	DECLARE @iCheckEmployeeAccess AS TINYINT
	DECLARE @UploadGuid AS VARCHAR(128)
	SET @UploadGuid=NEWID()

	Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())
	
	SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @User

	SELECT 
	@securityuserid	=	su.uniqsecurityuser,
	@iCheckStructure = CASE WHEN su.typecode IN ('E','I') THEN 0   -- enterprise user, system user
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 1 THEN 0  -- SecurityUser.Grant
	WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 3 THEN 1  -- SecurityUser.Deny
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.securitygroupsecurityuserjt jt WITH (NOLOCK)
	INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup -- SecurityGroup.Grant
	WHERE jt.uniqsecurityuser = su.uniqsecurityuser
	AND CAST(SUBSTRING(sg.programaccess, 431, 1) AS TINYINT) = 1) THEN 0
	ELSE 1 END,
	@iCheckEmployeeAccess = CASE WHEN su.typecode IN ('E','I') THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 1 THEN 0
	WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 3 THEN 1
	WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.SecurityGroupSecurityUserJT jt WITH (NOLOCK)
	INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup 
	WHERE jt.uniqsecurityuser = su.uniqsecurityuser
	AND CAST(SUBSTRING(sg.programaccess, 1467, 1) AS TINYINT) = 1) THEN 0
	ELSE 1 END
	FROM EpicDMZSub.DBO.securityuser su (NOLOCK)
	WHERE su.uniqemployee = @UniqEmployee

	Create Table #SecurityUserClient (uniqentity Int)
	create unique nonclustered index IX_SecurityUserClient on #SecurityUserClient (uniqentity);
		
	INSERT INTO #SecurityUserClient
	SELECT cl.uniqentity FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
	INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
	INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
	WHERE sus.uniqsecurityuser = @securityuserid
	Group By cl.uniqentity

	IF @LastSyncDate IS NOT NULL AND @LastSyncDate>='1900-01-01'
	BEGIN
		INSERT INTO HIBOPGetClientDetailsTemp (UploadGuid,UniqEntity,LookupCode,NameOf,Address,City,StateCode,StateName,PostalCode,CountryCode,Country,UniqAgency
		,AgencyCode,AgencyName,PrimaryContactName,Status,InsertedDate,UpdatedDate)
		Select 
		@UploadGuid,
		C.UniqEntity ,
		C.LookupCode,
		C.NameOf,
		IsNull(ca.Address1,'')+' '+IsNull(ca.Address2,'')+' '+IsNull(ca.Address3,'')						AS [Address],
		ca.City COLLATE Latin1_General_CI_AS																As City,
		ca.CdStateCode COLLATE Latin1_General_CI_AS															As StateCode,
		s.nameof							                                                                AS StateName,	
		ca.PostalCode COLLATE Latin1_General_CI_AS															As PostalCode,
		ca.CdCountryCode COLLATE Latin1_General_CI_AS														As CountryCode,
		cnt.NameOf																							As Country,	
		isnull(cast(cl.UniqAgency as int),0)																AS UniqAgency,
		a.AgencyCode,
		a.NameOf																							AS AgencyName,
		isnull(cntname.firstname,'')+' '+isnull(cntname.lastname,'')										AS PrimaryContactName,
		CASE WHEN C.InactiveDate IS Null THEN 'Active' ELSE 'InActive' END 'Status',
		C.InsertedDate,
		C.UpdatedDate 
		FROM EpicDMZSub.dbo.client c WITH (NOLOCK)
		OUTER APPLY (SELECT top 1 UniqAgency FROM EpicDMZSub.dbo.clientagencybranchjt AS CAB WITH (NOLOCK) WHERE C.UniqEntity=CAB.UniqEntity) AS cl 
		INNER JOIN EpicDMZSub.dbo.Agency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
		LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
		LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
		LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
		LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode COLLATE Latin1_General_CI_AS
		WHERE c.uniqentity <> -1 AND ISNULL(C.UpdatedDate,C.InsertedDate) > @LastSyncDate  
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.flags & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  

		SELECT DISTINCT UniqEntity,LookupCode,NameOf,Address,City,StateCode,StateName,PostalCode,CountryCode,Country,UniqAgency
		,AgencyCode,AgencyName,PrimaryContactName,Status,InsertedDate,UpdatedDate FROM HIBOPGetClientDetailsTemp WITH (NOLOCK)
		WHERE UploadGuid=@UploadGuid 
		order by UniqEntity
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY

		SELECT @RowCount = COUNT(UniqEntity)
		From HIBOPGetClientDetailsTemp C (Nolock)
		WHERE UploadGuid=@UploadGuid  AND 
		ISNULL(C.UpdatedDate,C.InsertedDate) > @LastSyncDate

		DELETE From HIBOPGetClientDetailsTemp WHERE UploadGuid=@UploadGuid 
	END
	ELSE 
	BEGIN
		INSERT INTO HIBOPGetClientDetailsTemp (UploadGuid,UniqEntity,LookupCode,NameOf,Address,City,StateCode,StateName,PostalCode,CountryCode,Country,UniqAgency
		,AgencyCode,AgencyName,PrimaryContactName,Status,InsertedDate,UpdatedDate)
		
		Select 
		@UploadGuid,
		C.UniqEntity ,
		C.LookupCode,
		C.NameOf,
		IsNull(ca.Address1,'')+' '+IsNull(ca.Address2,'')+' '+IsNull(ca.Address3,'')						AS [Address],
		ca.City COLLATE Latin1_General_CI_AS																As City,
		ca.CdStateCode COLLATE Latin1_General_CI_AS															As StateCode,
		s.nameof							                                                                AS StateName,	
		ca.PostalCode COLLATE Latin1_General_CI_AS															As PostalCode,
		ca.CdCountryCode COLLATE Latin1_General_CI_AS														As CountryCode,
		cnt.NameOf																							As Country,	
		isnull(cast(cl.UniqAgency as int),0)																AS UniqAgency,
		a.AgencyCode,
		a.NameOf																							AS AgencyName,
		isnull(cntname.firstname,'')+' '+isnull(cntname.lastname,'')										AS PrimaryContactName,
		CASE WHEN C.InactiveDate IS Null THEN 'Active' ELSE 'InActive' END 'Status',
		C.InsertedDate,
		C.UpdatedDate 
		FROM EpicDMZSub.dbo.client c(nolock)
		OUTER APPLY (SELECT top 1 UniqAgency FROM EpicDMZSub.dbo.clientagencybranchjt AS CAB(nolock) WHERE C.UniqEntity=CAB.UniqEntity) AS cl 
		INNER JOIN EpicDMZSub.dbo.Agency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
		LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
		LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
		LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
		LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode COLLATE Latin1_General_CI_AS
		WHERE c.uniqentity <> -1 
		AND (@iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity))
		AND (@iCheckEmployeeAccess = 0 
		OR (c.flags & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = @UniqEmployee)))  

		SELECT DISTINCT UniqEntity,LookupCode,NameOf,Address,City,StateCode,StateName,PostalCode,CountryCode,Country,UniqAgency
		,AgencyCode,AgencyName,PrimaryContactName,Status,InsertedDate,UpdatedDate FROM HIBOPGetClientDetailsTemp WITH (NOLOCK)
		WHERE UploadGuid=@UploadGuid 
		order by UniqEntity
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY

		SELECT @RowCount = COUNT(UniqEntity)
		From HIBOPGetClientDetailsTemp C (Nolock)
		WHERE UploadGuid=@UploadGuid 
		
		DELETE From HIBOPGetClientDetailsTemp WHERE UploadGuid=@UploadGuid 
	END
	
	DROP TABLE #SecurityUserClient

	 IF @PageNumber = 1 
	 BEGIN
		Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetClientDetails_SP',@DeltaSyncdate
	 END

	END TRY

	 BEGIN CATCH
        
		SELECT 'Select Failed For HIBOPSynzClientToLocal_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

	SET NOCOUNT OFF
END




GO

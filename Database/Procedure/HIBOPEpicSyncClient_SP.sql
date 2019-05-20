/*
-- =============================================
-- Module :	PullFromEPIC
-- Author:      BALACHANDAR.C
-- Create Date: 10-OCT-17
-- Description: This Procedure is used to Pull data from EPIC for new & updated records
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPEpicSyncClient_SP

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPEpicSyncClient_SP')
  EXEC('CREATE PROCEDURE [HIBOPEpicSyncClient_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPEpicSyncClient_SP]
AS 
BEGIN
     SET NOCOUNT ON
     BEGIN TRY
		DECLARE @LastSyncDate DATETIME,@Date	DATETIME = getdate(),@ModifiedMaxDate DATETIME

		IF NOT EXISTS( SELECT 1 FROM HIBOPComEPICSyncInfo)
		BEGIN
			INSERT INTO HIBOPComEPICSyncInfo VALUES ('1900-01-01 01:00:00.000')
		END 
		

		SELECT @LastSyncDate = MAX(EPICLastSyncDatetime) FROM HIBOPComEPICSyncInfo WITH(NOLOCK)
			  
		CREATE TABLE #HIBComEPICSyncInfo (InsertedDateMax DATETIME)
			  
		INSERT INTO #HIBComEPICSyncInfo (InsertedDateMax)
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Client (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Client (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Activity (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Activity (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Claim (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Claim (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Policy (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Policy (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Opportunity (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Opportunity (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Branch (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Branch (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.Agency (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.Agency (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.MarketingSubmission (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.MarketingSubmission (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.LINE (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.LINE (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.ServiceHead (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.ServiceHead (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.ActivityCode (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.ActivityCode (NOLOCK)
		UNION ALL
		SELECT MAX(InsertedDate) AS InsertedDate FROM EpicDMZSUB.DBO.CarrierSubmission (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(UpdatedDate,'')) AS InsertedDate FROM EpicDMZSUB.DBO.CarrierSubmission (NOLOCK)
		UNION ALL
		SELECT MAX(ISNULL(TD.Inserteddate,'')) FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber
		UNION ALL
		SELECT MAX(ISNULL(TD.UpdatedDate,'')) FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber

					
		SELECT @ModifiedMaxDate = MAX(InsertedDateMax)  FROM #HIBComEPICSyncInfo
		
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Client WHERE InactiveDate IS NULL AND InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPCLIENT
			(UniqEntity,LookupCode,NameOf,PrimaryContactName,[Address],City,StateCode,
			StateName,PostalCode,CountryCode,Country,Status,InsertedDate,UpdatedDate,InactiveDate)
			SELECT 
				C.UniqEntity                                                                                        AS ClientId,
				C.LookupCode																						AS LookupCode,
				C.NameOf                                                                                            AS NameOf,
				isnull(cntname.firstname,'')+' '+isnull(cntname.lastname,'')										AS PrimaryContactName,
				IsNull(ca.Address1,'')+' '+IsNull(ca.Address2,'')+' '+IsNull(ca.Address3,'')						AS [Address],
				ca.City COLLATE Latin1_General_CI_AS																As City,
				ca.CdStateCode COLLATE Latin1_General_CI_AS															As StateCode,
				s.nameof							                                                                AS StateName,	
				ca.PostalCode COLLATE Latin1_General_CI_AS															As PostalCode,
				ca.CdCountryCode COLLATE Latin1_General_CI_AS														As CountryCode,
				cnt.NameOf																							As CountryName,	
				c.Flags																								As [Status]	,
				C.InsertedDate,
				C.UpdatedDate,
				C.InactiveDate
			FROM EpicDMZSUB.DBO.Client C WITH(NOLOCK) 
			--INNER JOIN (SELECT DISTINCT UniqAgency,UniqEntity,UniqBranch from EpicDMZSUB.dbo.ClientAgencyBranchJT WITH (NOLOCK)) AS cjt ON cjt.uniqentity=c.uniqentity
			--INNER JOIN (SELECT DISTINCT UniqAgency,AgencyCode,NameOf FROM EpicDMZSUB.dbo.Agency WITH(NOLOCK)) AS a ON a.UniqAgency=cjt.UniqAgency
			----INNER JOIN epicdmzsub.dbo.structurecombination AS sc ON sc.UniqAgency=cjt.UniqAgency AND sc.UniqBranch=cjt.UniqBranch
			--INNER JOIN epicdmzsub.dbo.securityuserstructurecombinationjt AS sujt ON sc.UniqStructure=sujt.UniqStructure
			--INNER JOIN epicdmzsub.dbo.SECURITYUSER as su ON su.UniqSecurityUser=sujt.UniqSecurityUser		
			LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
			LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
			LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
			LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode COLLATE Latin1_General_CI_AS
			WHERE C.InsertedDate > @LastSyncDate AND NOT exists(SELECT 1 FROM HIBOPCLIENT pc WITH(NOLOCK) WHERE pc.UniqEntity =c.UniqEntity)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Client WHERE InactiveDate IS NULL AND UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE pc SET 
			                                                                                     
			pc.lookupcode=c.lookupcode, 
			pc.NameOf = C.NameOf, 
			PC.PrimaryContactName=isnull(cntname.firstname,'')+' '+isnull(cntname.lastname,''),
			pc.[Address] = IsNull(ca.Address1,'')+' '+IsNull(ca.Address2,'')+' '+IsNull(ca.Address3,''),
			pc.City = ca.City, 
			pc.StateCode = ca.CdStateCode,
			pc.StateName = s.nameof,
			pc.PostalCode = ca.PostalCode, 
			pc.CountryCode = ca.CdCountryCode, 
			pc.Country = cnt.NameOf,
			pc.[status]=c.Flags,		
			pc.InsertedDate=C.InsertedDate,
			pc.UpdatedDate=C.UpdatedDate,
			PC.InactiveDate=c.InactiveDate
			FROM EpicDMZSUB.DBO.Client C WITH(NOLOCK) 
			INNER JOIN HIBOPCLIENT pc WITH(NOLOCK) ON C.UniqEntity=pc.UniqEntity 
			--INNER JOIN (SELECT DISTINCT UniqAgency,UniqEntity,UniqBranch from EpicDMZSUB.dbo.ClientAgencyBranchJT WITH (NOLOCK)) AS cjt ON cjt.uniqentity=c.uniqentity
			--INNER JOIN (SELECT DISTINCT UniqAgency,AgencyCode,NameOf FROM EpicDMZSUB.dbo.Agency WITH(NOLOCK)) AS a ON a.UniqAgency=cjt.UniqAgency
			----INNER JOIN epicdmzsub.dbo.structurecombination AS sc ON sc.UniqAgency=cjt.UniqAgency AND sc.UniqBranch=cjt.UniqBranch
			--INNER JOIN epicdmzsub.dbo.securityuserstructurecombinationjt AS sujt ON sc.UniqStructure=sujt.UniqStructure
			--INNER JOIN epicdmzsub.dbo.SECURITYUSER as su ON su.UniqSecurityUser=sujt.UniqSecurityUser		
			LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
			LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
			LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
			LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode COLLATE Latin1_General_CI_AS
			WHERE C.UpdatedDate > @LastSyncDate
		END

		--------Activity data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Activity WHERE InsertedDate Between @LastSyncDate AND @ModifiedMaxDate )
					
		BEGIN

			--IF @LastSyncDate='1900-01-01 01:00:0.000'
			--BEGIN

			--	INSERT INTO HIBOPActivity
			--	(UniqActivity,UniqEntity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,Status,ClosedDate)
				
			--	SELECT UniqActivity,UniqEntity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,EffectiveDate,ExpirationDate,InsertedDate,UpdatedDate,Flags,ClosedDate
			--	FROM
			--	(SELECT A.UniqActivity,A.UniqEntity,A.UniqActivityCode,AC.ActivityCode,A.DescriptionOf,PT.UniqCdPolicyLineType,P.PolicyNumber,P.EffectiveDate,P.ExpirationDate,A.InsertedDate,A.UpdatedDate,A.Flags,A.ClosedDate
			--	FROM EpicDMZSUB.DBO.Activity A WITH(NOLOCK) 
			--	INNER JOIN EpicDMZSUB.dbo.ActivityCode AC WITH(NOLOCK) ON ac.uniqactivitycode=a.uniqactivitycode
			--	LEFT JOIN HIBOPActivity OA WITH(NOLOCK) ON A.UniqActivity = OA.UniqActivity
			--	LEFT OUTER JOIN EpicDMZSUB..ActivityPolicyLineJT AP WITH(NOLOCK) ON A.UniqActivity=AP.UniqActivity
			--	LEFT OUTER JOIN EpicDMZSUB..Policy P WITH(NOLOCK) ON Ap.UniqPolicy=p.UniqPolicy
			--	LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON P.UniqCdPolicyLineType=PT.UniqCdPolicyLineType
			--	WHERE A.InsertedDate > @LastSyncDate AND   A.Flags =6
			--	AND NOT EXISTS(SELECT 1 FROM HIBOPActivity AS A1 WITH(NOLOCK) WHERE A1.UniqActivity =A.UniqActivity)
			
			--	UNION ALL

			--	SELECT A.UniqActivity,A.UniqEntity,A.UniqActivityCode,AC.ActivityCode,A.DescriptionOf,PT.UniqCdPolicyLineType,P.PolicyNumber,P.EffectiveDate,P.ExpirationDate,A.InsertedDate,A.UpdatedDate,A.Flags
			--	FROM EpicDMZSUB.DBO.Activity A WITH(NOLOCK) 
			--	INNER JOIN EpicDMZSUB.dbo.ActivityCode AC WITH(NOLOCK) ON ac.uniqactivitycode=a.uniqactivitycode
			--	LEFT JOIN HIBOPActivity OA WITH(NOLOCK) ON A.UniqActivity = OA.UniqActivity
			--	LEFT OUTER JOIN EpicDMZSUB..ActivityPolicyLineJT AP WITH(NOLOCK) ON A.UniqActivity=AP.UniqActivity
			--	LEFT OUTER JOIN EpicDMZSUB..Policy P WITH(NOLOCK) ON Ap.UniqPolicy=p.UniqPolicy
			--	LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON P.UniqCdPolicyLineType=PT.UniqCdPolicyLineType
			--	WHERE A.Flags =4 AND A.InsertedDate between dateadd(year, -1, getdate()) and getdate() 
			--	AND A.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivity AS A1 WITH(NOLOCK) WHERE A1.UniqActivity =A.UniqActivity)
			--	)T
			--END
			--ELSE
			--BEGIN
					 
				INSERT INTO HIBOPActivity
				(UniqActivity,UniqEntity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,EffectiveDate,ExpirationDate
				,InsertedDate,UpdatedDate,[Status],ClosedDate,
				UniqAgency,UniqBranch,UniqDepartment,UniqProfitCenter,UniqAssociatedItem,AssociationType,UniqEmployee,UniqPolicy,UniqLine,UniqClaim,LossDate,PolicyDescription,
				LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate,uniqentitycompanyissuing,uniqentitycompanybilling)

				
					     
				SELECT A.UniqActivity,A.UniqEntity,A.UniqActivityCode,AC.ActivityCode,A.DescriptionOf
				,ISNULL(PT.UniqCdPolicyLineType,pt1.UniqCdPolicyLineType) as UniqCdPolicyLineType,ISNULL(P.PolicyNumber,P1.PolicyNumber) as PolicyNumber
				,ISNULL(P.EffectiveDate,P1.EffectiveDate) as EffectiveDate,ISNULL(P.ExpirationDate,P1.EffectiveDate) as ExpirationDate
				,A.InsertedDate,A.UpdatedDate,A.Flags,A.ClosedDate,
				A.UniqAgency,A.UniqBranch,A.UniqDepartment,A.UniqProfitCenter,A.UniqAssociatedItem,
				CASE WHEN AssociatedItemCode=1 THEN 'Account'
					 WHEN AssociatedItemCode=2 THEN 'Policy'
					 WHEN AssociatedItemCode=3 THEN 'Line'
					 WHEN AssociatedItemCode=7 THEN 'Claim'	
					 WHEN AssociatedItemCode=18 THEN 'Marketing'
					 WHEN AssociatedItemCode=17 THEN 'Carrier'	
					 WHEN AssociatedItemCode=22 THEN 'Bill'
					 WHEN AssociatedItemCode=12 THEN 'Transaction'
					 WHEN AssociatedItemCode=37 THEN 'Opportunity'
					 WHEN AssociatedItemCode=36 THEN 'Services'
					  WHEN AssociatedItemCode=14 THEN 'Certificate'
					 WHEN AssociatedItemCode=15 THEN 'Evidence'END,a.UniqEmployee,ISNULL(p.UniqPolicy,P1.uniqpolicy) AS UniqPolicy
					 ,ISNULL(l.UniqLine,LT1.UniqLine) AS UniqLine,c.UniqClaim,c.LossDate
					 ,isnull(p.DescriptionOf,P1.DescriptionOf) AS DescriptionOf
					 ,ISNULL(lt.CdPolicyLineTypeCode,lt2.CdPolicyLineTypeCode) as CdPolicyLineTypeCode 
					 ,ISNULL(lkp.ResourceText,lkp1.ResourceText) as ResourceText 
					 ,ISNULL(comp.LookupCode,comp1.LookupCode) AS LookupCode ,
					 ISNULL(L.EffectiveDate,lt1.EffectiveDate)
					 ,ISNULL(L.ExpirationDate,LT1.ExpirationDate)
					 ,A.uniqentitycompanyissuing,A.uniqentitycompanybilling
				FROM EpicDMZSUB.DBO.Activity A WITH(NOLOCK) 
				INNER JOIN EpicDMZSUB.dbo.ActivityCode AC WITH(NOLOCK) ON ac.uniqactivitycode=a.uniqactivitycode and AssociatedItemCode in(1,2,3,7,12,17,18,22,36,37,14,15) 
				--INNER JOIN EpicDMZSUB.dbo.ConfigureLkLanguageResource lk WITH(NOLOCK) ON AC.ConfigureLkLanguageResourceID=LK.ConfigureLkLanguageResourceID
				--LEFT JOIN HIBOPActivity OA WITH(NOLOCK) ON A.UniqActivity = OA.UniqActivity
				--	LEFT OUTER JOIN EpicDMZSUB..ActivityPolicyLineJT AP WITH(NOLOCK) ON A.UniqActivity=AP.UniqActivity
				LEFT OUTER JOIN EpicDMZSUB..Policy P WITH(NOLOCK) ON A.UniqAssociatedItem=p.UniqPolicy and a.AssociatedItemCode=2
				LEFT OUTER JOIN EpicDMZSUB..line l WITH(NOLOCK) ON a.UniqAssociatedItem=l.UniqLine and a.AssociatedItemCode=3
				OUTER APPLY (SELECT top 1 UniqLine,UniqCdPolicyLineType,UniqEntityCompanyIssuing,
				EffectiveDate,ExpirationDate FROM EpicDMZSUB..line AS lt1 WHERE lt1.UniqPolicy=p.UniqPolicy) AS lt1
				--LEFT OUTER JOIN EpicDMZSUB..line lt1 WITH(NOLOCK) ON lt1.UniqPolicy=p.UniqPolicy 
				LEFT OUTER JOIN EpicDMZSUB..Policy P1 WITH(NOLOCK) ON l.UniqPolicy=p1.UniqPolicy 
				LEFT OUTER JOIN EpicDMZSUB..Claim c WITH(NOLOCK) ON A.UniqAssociatedItem=c.UniqClaim and a.AssociatedItemCode=7
				LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON P.UniqCdPolicyLineType=PT.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT1 WITH(NOLOCK) ON P1.UniqCdPolicyLineType=PT1.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..CdPolicyLineType AS lt with(nolock) on l.UniqCdPolicyLineType=lt.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..CdPolicyLineType AS lt2 with(nolock) on lt1.UniqCdPolicyLineType=lt2.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..ConfigureLkLanguageResource as lkp ON lt.ConfigureLkLanguageResourceID=lkp.ConfigureLkLanguageResourceID and lkp.CultureCode='en-US'
				LEFT OUTER JOIN EpicDMZSub..ConfigureLkLanguageResource as lkp1 ON lt2.ConfigureLkLanguageResourceID=lkp1.ConfigureLkLanguageResourceID and lkp1.CultureCode='en-US'
				LEFT OUTER JOIN EpicDMZSub..Company comp WITH(NOLOCK) on l.UniqEntityCompanyIssuing=comp.UniqEntity
				LEFT OUTER JOIN EpicDMZSub..Company comp1 WITH(NOLOCK) on lt1.UniqEntityCompanyIssuing=comp.UniqEntity
				WHERE AssociatedItemCode in(1,2,3,7,12,17,18,22,36,37,14,15) AND A.InsertedDate > @LastSyncDate
				 AND NOT EXISTS(SELECT 1 FROM HIBOPActivity AS A1 WITH(NOLOCK) WHERE A1.UniqActivity =A.UniqActivity)
				  and (a.ClosedDate is null or  a.ClosedDate> DATEADD(mm,-18,getdate()))

				  
				  
				  
			--END								
		END
		
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Activity WHERE UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
					
		BEGIN

			UPDATE OA SET
			
			OA.UniqEntity = A.UniqEntity,
			OA.UniqActivityCode = A.UniqActivityCode, 
			OA.ActivityCode = AC.ActivityCode, 
			OA.DescriptionOf = A.DescriptionOf,
			OA.UniqCdPolicyLineType=ISNULL(PT.UniqCdPolicyLineType,pt1.UniqCdPolicyLineType),
			OA.PolicyNumber=ISNULL(P.PolicyNumber,P1.PolicyNumber),
			OA.EffectiveDate =ISNULL(P.EffectiveDate,L.EffectiveDate),
			OA.ExpirationDate =ISNULL(P.ExpirationDate,L.ExpirationDate),
			OA.InsertedDate=A.InsertedDate,
			OA.UpdatedDate=A.UpdatedDate,			
			OA.[Status] = A.Flags	,
			OA.ClosedDate=A.ClosedDate,
			OA.UniqAssociatedItem=A.UniqAssociatedItem,
			OA.UniqAgency=A.UniqAgency,
			OA.UniqBranch=A.UniqBranch,
			OA.UniqDepartment=A.UniqDepartment,
			OA.UniqProfitCenter=A.UniqProfitCenter,
			OA.AssociationType= CASE WHEN AssociatedItemCode=1 THEN 'Account'
					 WHEN AssociatedItemCode=2 THEN 'Policy'
					 WHEN AssociatedItemCode=3 THEN 'Line'
					 WHEN AssociatedItemCode=7 THEN 'Claim'	
					 WHEN AssociatedItemCode=18 THEN 'Marketing'
					 WHEN AssociatedItemCode=17 THEN 'Carrier'	
					 WHEN AssociatedItemCode=22 THEN 'Bill'
					 WHEN AssociatedItemCode=12 THEN 'Transaction'
					 WHEN AssociatedItemCode=37 THEN 'Opportunity'
					 WHEN AssociatedItemCode=36 THEN 'Services'
					 WHEN AssociatedItemCode=14 THEN 'Certificate'
					 WHEN AssociatedItemCode=15 THEN 'Evidence'END ,
			OA.uniqpolicy=p.UniqPolicy,
			OA.uniqline=ISNULL(l.UniqLine,LT1.UniqLine),
			OA.uniqclaim=c.UniqClaim,
			OA.LossDate=c.LossDate,
			OA.PolicyDescription=isnull(p.DescriptionOf,P1.DescriptionOf),
			OA.LineCode=ISNULL(lt.CdPolicyLineTypeCode,lt2.CdPolicyLineTypeCode),
			OA.LineDescription=isnull(lkp.ResourceText,lkp1.ResourceText),
			OA.ICO=ISNULL(comp.LookupCode,comp1.LookupCode) ,
			OA.LineEffectiveDate=ISNULL(L.EffectiveDate,lt1.EffectiveDate),
			OA.LineExpirationDate=ISNULL(L.ExpirationDate,LT1.ExpirationDate),
			OA.uniqentitycompanyissuing = A.uniqentitycompanyissuing,
			OA.uniqentitycompanybilling = A.uniqentitycompanybilling
	   		FROM HIBOPActivity OA  WITH(NOLOCK) 
			INNER JOIN EpicDMZSUB.DBO.Activity A WITH(NOLOCK) ON A.UniqActivity = OA.UniqActivity and AssociatedItemCode in(1,2,3,7,12,17,18,22,36,37,14,15) 
			INNER JOIN EpicDMZSUB.dbo.ActivityCode AC WITH(NOLOCK) ON ac.uniqactivitycode=a.uniqactivitycode
			--INNER JOIN EpicDMZSUB.dbo.ConfigureLkLanguageResource lk WITH(NOLOCK) ON AC.ConfigureLkLanguageResourceID=LK.ConfigureLkLanguageResourceID
				--LEFT JOIN HIBOPActivity OA WITH(NOLOCK) ON A.UniqActivity = OA.UniqActivity
				--	LEFT OUTER JOIN EpicDMZSUB..ActivityPolicyLineJT AP WITH(NOLOCK) ON A.UniqActivity=AP.UniqActivity
				LEFT OUTER JOIN EpicDMZSUB..Policy P WITH(NOLOCK) ON A.UniqAssociatedItem=p.UniqPolicy and a.AssociatedItemCode=2
				LEFT OUTER JOIN EpicDMZSUB..line l WITH(NOLOCK) ON a.UniqAssociatedItem=l.UniqLine and a.AssociatedItemCode=3
				OUTER APPLY (SELECT top 1 UniqLine,UniqCdPolicyLineType,UniqEntityCompanyIssuing,
				EffectiveDate,ExpirationDate FROM EpicDMZSUB..line AS lt1 WHERE lt1.UniqPolicy=p.UniqPolicy) AS lt1
				--LEFT OUTER JOIN EpicDMZSUB..line lt1 WITH(NOLOCK) ON lt1.UniqPolicy=p.UniqPolicy 
				LEFT OUTER JOIN EpicDMZSUB..Policy P1 WITH(NOLOCK) ON l.UniqPolicy=p1.UniqPolicy 
				LEFT OUTER JOIN EpicDMZSUB..Claim c WITH(NOLOCK) ON A.UniqAssociatedItem=c.UniqClaim and a.AssociatedItemCode=7
				LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON P.UniqCdPolicyLineType=PT.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType PT1 WITH(NOLOCK) ON P1.UniqCdPolicyLineType=PT1.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..CdPolicyLineType AS lt with(nolock) on l.UniqCdPolicyLineType=lt.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..CdPolicyLineType AS lt2 with(nolock) on lt1.UniqCdPolicyLineType=lt2.UniqCdPolicyLineType
				LEFT OUTER JOIN EpicDMZSub..ConfigureLkLanguageResource as lkp ON lt.ConfigureLkLanguageResourceID=lkp.ConfigureLkLanguageResourceID and lkp.CultureCode='en-US'
				LEFT OUTER JOIN EpicDMZSub..ConfigureLkLanguageResource as lkp1 ON lt2.ConfigureLkLanguageResourceID=lkp1.ConfigureLkLanguageResourceID and lkp1.CultureCode='en-US'
				LEFT OUTER JOIN EpicDMZSub..Company comp WITH(NOLOCK) on l.UniqEntityCompanyIssuing=comp.UniqEntity
				LEFT OUTER JOIN EpicDMZSub..Company comp1 WITH(NOLOCK) on lt1.UniqEntityCompanyIssuing=comp.UniqEntity
				WHERE AssociatedItemCode in(1,2,3,7,12,17,18,22,36,37,14,15) AND A.updateddate > @LastSyncDate
				 
	
				END
		
		
		----Activity Code Data load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.ActivityCode WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPActivityCode
			(UniqActivityCode,ActivityCode,ActivityName,UniqActivityEvent,OwnerTypeCode,UniqEmployee,ClosedStatus,Flags,InsertedDate,UpdatedDate)
		
			SELECT
			UniqActivityCode,
			ActivityCode,
			ResourceText as ActivityName,
			UniqActivityEvent,
			OwnerTypeCode,
			UniqEmployee,
			ClosedStatus,
			Flags,
			InsertedDate,
			UpdatedDate
 
			FROM EpicDMZSUB.dbo.ActivityCode ac WITH(NOLOCK)
			INNER JOIN EpicDMZSUB.dbo.ConfigureLkLanguageResource AS c  WITH(NOLOCK) ON ac.ConfigureLkLanguageResourceID=c.ConfigureLkLanguageResourceID  and c.culturecode='en-US'
			WHERE UniqActivityCode<>-1 AND ac.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityCode ACT WITH(NOLOCK) WHERE AC.UniqActivityCode =ACT.UniqActivityCode)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.ActivityCode WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE ACT SET

			
			act.ActivityCode=ac.ActivityCode,
			act.ActivityName =C.ResourceText ,
			act.UniqActivityEvent=ac.UniqActivityEvent,
			act.OwnerTypeCode=ac.OwnerTypeCode,
			act.UniqEmployee=ac.UniqEmployee,
			act.ClosedStatus=ac.ClosedStatus,
			act.Flags=ac.Flags,
			act.InsertedDate=ac.InsertedDate,
			act.UpdatedDate=ac.UpdatedDate
 
			FROM EpicDMZSUB.dbo.ActivityCode ac WITH(NOLOCK)
			INNER JOIN HIBOPActivityCode ACT ON ac.UniqActivityCode=act.UniqActivityCode
			INNER JOIN EpicDMZSUB.dbo.ConfigureLkLanguageResource AS c  WITH(NOLOCK) ON ac.ConfigureLkLanguageResourceID=c.ConfigureLkLanguageResourceID  and c.culturecode='en-US'
			WHERE AC.UniqActivityCode<>-1 AND ac.UpdatedDate > @LastSyncDate 


		END

		----Bracnch Data Load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Branch WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPBranch
			(UniqBranch,BranchCode,BranchName,LicenceNumber,Flags,InsertedDate,UpdatedDate)
			SELECT 
				B.UniqBranch																						As UniqBranch,
				B.BranchCode																						AS UniqCode,
				B.NameOf                                                                                        AS BranchName,
				B.LicenseNumber,
				B.Flags																								As [Status]	,
				B.InsertedDate,
				B.UpdatedDate
			FROM EpicDMZSUB.DBO.Branch B WITH(NOLOCK) 
			WHERE B.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPBranch BT WITH(NOLOCK) WHERE B.UniqBranch =BT.UniqBranch)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Branch WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE BT SET 
			                                                                                     
			BT.Branchcode=b.Branchcode, 
			BT.BranchName = B.NameOf, 
			BT.Flags=B.Flags,		
			BT.InsertedDate=B.InsertedDate,
			BT.UpdatedDate=B.UpdatedDate
			FROM EpicDMZSUB.DBO.Branch B
			INNER JOIN  HIBOPBranch BT ON B.UniqBranch=BT.UniqBranch
			WHERE B.UpdatedDate > @LastSyncDate
		END

		----Agency Data Load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Agency WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPAgency
			(UniqAgency,AgencyCode,AgencyName,LicenseNumber,Flags,InsertedDate,UpdatedDate)
			SELECT 
				B.UniqAgency,
				B.AgencyCode,
				B.NameOf,
				B.LicenseNumber,
				B.Flags	,
				B.InsertedDate,
				B.UpdatedDate
			FROM EpicDMZSUB.DBO.Agency B WITH(NOLOCK) 
			WHERE B.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPAgency BT WITH(NOLOCK) WHERE B.UniqAgency =BT.UniqAgency)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Agency WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE BT SET 
			                                                                          
			BT.AgencyCode=b.AgencyCode, 
			BT.AgencyName = B.NameOf, 
			BT.Flags=B.Flags,		
			BT.InsertedDate=B.InsertedDate,
			BT.UpdatedDate=B.UpdatedDate
			FROM EpicDMZSUB.DBO.Agency B
			INNER JOIN  HIBOPAgency BT ON B.UniqAgency=BT.UniqAgency
			WHERE B.UpdatedDate > @LastSyncDate
		END

		----Claimy Data Load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Claim WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPClaim
			(UniqEntity,UniqClaim,ClaimCode,ClaimName,LossDate,ReportedDate,ClaimNumber,CompanyClaimNumber,ClosedDate,Flags,InsertedDate,UpdatedDate)
			SELECT 
				B.UniqEntity,
				B.UniqClaim,
				'' as LkClaimCode,
				'' as DescriptionOf,
				B.LossDate,
				B.ReportedDate,
				B.ClaimNumber,	
				B.CompanyClaimNumber,
				B.ClosedDate,
				B.Flags	,
				B.InsertedDate,
				B.UpdatedDate
			FROM EpicDMZSUB.DBO.Claim B WITH(NOLOCK)
			--LEFT OUTER JOIN EpicDMZSUB..ClaimCode CC WITH(NOLOCK) ON B.UniqClaim=CC.UniqClaim 
			WHERE B.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPClaim BT WITH(NOLOCK) WHERE B.UniqClaim =BT.UniqClaim)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Claim WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE BT SET 
			                                                                                     
				BT.UniqEntity=b.UniqEntity, 
				BT.ClaimCode = '', 
				BT.ClaimName = '', 
				BT.LossDate = B.LossDate, 
				BT.ReportedDate = B.ReportedDate, 
				BT.ClaimNumber = B.ClaimNumber, 
				BT.CompanyClaimNumber = B.CompanyClaimNumber, 
				BT.ClosedDate = B.ClosedDate, 
				BT.Flags=B.Flags,		
				BT.InsertedDate=B.InsertedDate,
				BT.UpdatedDate=B.UpdatedDate
			FROM EpicDMZSUB.DBO.Claim B WITH(NOLOCK)
			INNER JOIN HIBOPClaim BT ON B.UniqClaim=BT.UniqClaim
			--LEFT OUTER JOIN EpicDMZSUB..ClaimCode CC WITH(NOLOCK) ON B.UniqClaim=CC.UniqClaim 
			WHERE B.UpdatedDate > @LastSyncDate
		END



		----Policy Data Load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Policy WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPPolicy
			(UniqPolicy,UniqEntity,UniqAgency,UniqBranch,UniqDepartment,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,EffectiveDate,ExpirationDate,PolicyStatus,Flags,InsertedDate,UpdatedDate)
			

		SELECT  

			UniqPolicy,
			UniqEntity,
			A.UniqAgency,
			B.UniqBranch,
			P.UniqDepartment,
			DescriptionOf,
			P.UniqCdPolicyLineType,
			PolicyNumber,
			EffectiveDate,
			ExpirationDate,
			CASE WHEN (p.Flags & 2) / 2 =1 THEN 'Prospective' ELSE 'Contracted' END PolicyStatus,
			P.Flags,
			P.InsertedDate,
			P.UpdatedDate
		FROM EpicDMZSUB..Policy P WITH(NOLOCK)
		INNER JOIN EpicDMZSUB..CdPolicyLineType PL WITH(NOLOCK) ON P.UniqCdPolicyLineType=PL.UniqCdPolicyLineType
		INNER JOIN EpicDMZSUB..Agency A WITH(NOLOCK) ON P.UniqAgency=A.UniqAgency
		INNER JOIN EpicDMZSUB..Branch B WITH(NOLOCK) ON P.UniqBranch=B.UniqBranch
		INNER JOIN EpicDMZSUB..Department D WITH(NOLOCK) ON P.UniqDepartment=D.UniqDepartment
		INNER JOIN EpicDMZSUB..ProfitCenter PC WITH(NOLOCK) ON PL.UniqProfitCenter=PC.UniqProfitCenter
		WHERE P.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPPolicy PT WITH(NOLOCK) WHERE P.UniqPolicy =PT.UniqPolicy)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Policy WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE PT SET 
			                                                                                     
			PT.UniqEntity=P.UniqEntity, 
			PT.UniqAgency=P.UniqAgency, 
			PT.UniqBranch=P.UniqBranch, 
			PT.UniqDepartment = P.UniqDepartment,
			PT.DescriptionOf=P.DescriptionOf, 
			PT.UniqCdPolicyLineType=P.UniqCdPolicyLineType, 
			PT.PolicyNumber=P.PolicyNumber, 
			PT.EffectiveDate=P.EffectiveDate, 
			PT.ExpirationDate=P.ExpirationDate, 
			PT.PolicyStatus=CASE WHEN (p.Flags & 2) / 2 =1 THEN 'Prospective' ELSE 'Contracted' END,
			PT.Flags=P.Flags,		
			PT.InsertedDate=P.InsertedDate,
			PT.UpdatedDate=P.UpdatedDate
			FROM EpicDMZSUB.DBO.Policy P WITH(NOLOCK)
			INNER JOIN  HIBOPPolicy PT ON P.UniqPolicy=PT.UniqPolicy
			INNER JOIN EpicDMZSUB..CdPolicyLineType PL WITH(NOLOCK) ON P.UniqCdPolicyLineType=PL.UniqCdPolicyLineType
			INNER JOIN EpicDMZSUB..Agency A WITH(NOLOCK) ON P.UniqAgency=A.UniqAgency
			INNER JOIN EpicDMZSUB..Branch B WITH(NOLOCK) ON P.UniqBranch=B.UniqBranch
			INNER JOIN EpicDMZSUB..Department D WITH(NOLOCK) ON P.UniqDepartment=D.UniqDepartment
			INNER JOIN EpicDMZSUB..ProfitCenter PC WITH(NOLOCK) ON PL.UniqProfitCenter=PC.UniqProfitCenter
				WHERE P.UpdatedDate > @LastSyncDate
		END

		
		----Master Marketting Submission Data Load

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.MarketingSubmission WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

		
		INSERT INTO HIBOPActivityMasterMarketing
		(UniqMarketingSubmission,UniqEntity,UniqAgency,UniqBranch,UniqDepartment,DescriptionOf,UniqCdPolicyLineType,EffectiveDate,ExpirationDate,LastSubmittedDate,Flags,InsertedDate,UpdatedDate)
			

		SELECT  		
				ms.UniqMarketingSubmission,
				ms.UniqEntity,
				ms.UniqAgency,
				ms.UniqBranch,
				ms.UniqDepartment,
				ms.DescriptionOf,
				ml.UniqCdPolicyLineType,
				ms.EffectiveDate,
				ms.ExpirationDate,
				ms.LastSubmittedDate,
				ms.Flags,
				ms.InsertedDate,
				ms.UpdatedDate
				
		FROM EpicDMZSUB..MarketingSubmission ms
		LEFT OUTER JOIN EpicDMZSUB..MarketingLine ml on ms.UniqMarketingSubmission=ml.UniqMarketingSubmission
		LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType pl on ml.UniqCdPolicyLineType=pl.UniqCdPolicyLineType
		WHERE ms.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityMasterMarketing MST WITH(NOLOCK) WHERE MS.UniqMarketingSubmission =MST.UniqMarketingSubmission)

		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.MarketingSubmission WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE MST SET 
			                                                                                     
			MST.UniqEntity=MS.UniqEntity, 
			MST.UniqAgency=MS.UniqAgency, 
			MST.UniqBranch=MS.UniqBranch, 
			MST.UniqDepartment=ms.UniqDepartment,
			MST.DescriptionOf=MS.DescriptionOf, 
			MST.UniqCdPolicyLineType=ML.UniqCdPolicyLineType, 
			MST.EffectiveDate=MS.EffectiveDate, 
			MST.ExpirationDate=MS.ExpirationDate, 
			MST.LastSubmittedDate=MS.LastSubmittedDate, 
			MST.Flags=MS.Flags,		
			MST.InsertedDate=MS.InsertedDate,
			MST.UpdatedDate=MS.UpdatedDate
			FROM EpicDMZSUB.DBO.MarketingSubmission MS
			INNER JOIN  HIBOPActivityMasterMarketing MST ON MS.UniqMarketingSubmission=MST.UniqMarketingSubmission
			LEFT OUTER JOIN EpicDMZSUB..MarketingLine ml on ms.UniqMarketingSubmission=ml.UniqMarketingSubmission
			LEFT OUTER JOIN EpicDMZSUB..CdPolicyLineType pl on ml.UniqCdPolicyLineType=pl.UniqCdPolicyLineType			
			WHERE ms.UpdatedDate > @LastSyncDate
		END

		--------Opportunity data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Opportunity WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

		INSERT INTO HIBOPActivityOpportunity
		(
		UniqOpportunity,UniqEntity,OppDesc,TargetedDate,ActualDate,UniqSalesTeam,UniqEmployeeOwner,UniqOpportunityStage,OwnerName,
		SalesTeam,SalesManager,Stage,Flags,InsertedDate,UpdatedDate,UniqAgency,UniqBranch,UniqDepartment
		)

		SELECT 
			UniqOpportunity,
			O.UniqEntity,
			DescriptionOf,
			TargetedDate,
			ActualDate,
			O.UniqSalesTeam,
			UniqEmployeeOwner,
			O.UniqOpportunityStage,
			em.NameOf as OwnerName,
			st.ResourceText as SalesTeam,
			sm.ManagerName,
			OS.OpportunityStage as Stage,
			O.Flags,
			O.InsertedDate,
			O.UpdatedDate,
			O.UniqAgency,
			O.UniqBranch,
			O.UniqDepartment
			
		FROM EpicDMZSUB..Opportunity O WITH(NOLOCK)
		LEFT OUTER JOIN HIBOPOpportunityStage OS WITH(NOLOCK) ON o.UniqOpportunityStage=os.UniqOpportunityStage
		LEFT OUTER JOIN  EpicDMZSUB.dbo.Employee AS em WITH(NOLOCK) ON em.UniqEntity=O.UniqEmployeeOwner
		LEFT OUTER JOIN
		(
			SELECT DISTINCT UniqSalesTeam,ResourceText
			FROM EpicDMZSUB..ConfigureLkLanguageResource cl
			INNER JOIN EpicDMZSUB..SalesTeam SL WITH(NOLOCK) ON CL.ConfigureLkLanguageResourceID=SL.ConfigureLkLanguageResourceID
			WHERE UniqSalesTeam<>-1 and CultureCode='en-US'
		) ST ON O.UniqSalesTeam=ST.UniqSalesTeam
		LEFT OUTER JOIN
		(
			SELECT DISTINCT 
				em.UniqEntity as ManagerId,
				em.LookupCode as ManagerCode,
				em.nameof	  AS ManagerName,
				sal.UniqSalesTeam

			FROM  EpicDMZSUB.DBO.EmployeeSalesTeamJT AS est WITH(NOLOCK)
			INNER JOIN  EpicDMZSUB.DBO.SalesTeam AS sal WITH(NOLOCK)  ON est.UniqSalesTeam=sal.UniqSalesTeam
			INNER JOIN  EpicDMZSUB.dbo.Employee AS em WITH(NOLOCK) ON em.UniqEntity=sal.UniqEmployeeManager
		)SM ON O.UniqSalesTeam=SM.UniqSalesTeam
		WHERE O.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityOpportunity HO WITH(NOLOCK) WHERE O.UniqOpportunity=HO.UniqOpportunity)
		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Opportunity WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

		UPDATE HO SET 
				
				HO.UniqEntity = O.UniqEntity,
				HO.OppDesc	=	O.DescriptionOf,
				HO.TargetedDate = O.TargetedDate,
				HO.ActualDate  = O.ActualDate,
				HO.UniqSalesTeam = O.UniqSalesTeam,
				HO.UniqEmployeeOwner = O.UniqEmployeeOwner,
				HO.UniqOpportunityStage = O.UniqOpportunityStage,
				HO.OwnerName = em.NameOf,
				HO.SalesTeam = St.ResourceText,
				HO.SalesManager = sm.ManagerName,
				HO.Stage = OS.OpportunityStage,
				HO.Flags = O.Flags,
				HO.InsertedDate =  O.InsertedDate,
				HO.UpdatedDate = O.UpdatedDate,
				HO.UniqAgency = O.UniqAgency,
				HO.UniqBranch = O.UniqBranch,
				HO.UniqDepartment = O.UniqDepartment
		FROM EpicDMZSUB..Opportunity O WITH(NOLOCK)
		LEFT OUTER JOIN HIBOPOpportunityStage OS WITH(NOLOCK) ON o.UniqOpportunityStage=os.UniqOpportunityStage
		INNER JOIN HIBOPActivityOpportunity HO WITH(NOLOCK) ON O.UniqOpportunity=HO.UniqOpportunity
		LEFT OUTER JOIN  EpicDMZSUB.dbo.Employee AS em WITH(NOLOCK) ON em.UniqEntity=O.UniqEmployeeOwner
		LEFT OUTER JOIN
		(
			SELECT DISTINCT UniqSalesTeam,ResourceText
			FROM EpicDMZSUB..ConfigureLkLanguageResource cl
			INNER JOIN EpicDMZSUB..SalesTeam SL WITH(NOLOCK) ON CL.ConfigureLkLanguageResourceID=SL.ConfigureLkLanguageResourceID
			WHERE UniqSalesTeam<>-1 and  CultureCode='en-US'
		) ST ON O.UniqSalesTeam=ST.UniqSalesTeam
		LEFT OUTER JOIN
		(
			SELECT DISTINCT 
				em.UniqEntity as ManagerId,
				em.LookupCode as ManagerCode,
				em.nameof	  AS ManagerName,
				sal.UniqSalesTeam

			FROM  EpicDMZSUB.DBO.EmployeeSalesTeamJT AS est WITH(NOLOCK)
			INNER JOIN  EpicDMZSUB.DBO.SalesTeam AS sal WITH(NOLOCK)  ON est.UniqSalesTeam=sal.UniqSalesTeam
			INNER JOIN  EpicDMZSUB.dbo.Employee AS em WITH(NOLOCK) ON em.UniqEntity=sal.UniqEmployeeManager
		)SM ON O.UniqSalesTeam=SM.UniqSalesTeam
		WHERE O.UpdatedDate > @LastSyncDate
		END

		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.ServiceHead WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPActivityServices
			(
			UniqServiceHead,UniqEntity,ServiceNumber,UniqCdServiceCode,[Description],ContractNumber,InceptionDate,ExpirationDate,Flags,InsertedDate,UpdatedDate,
			UniqAgency,UniqBranch,UniqDepartment
			)
		
			SELECT 
				sh.UniqServiceHead,
				sh.UniqEntity,
				ServiceNumber,
				sc.CdServiceCode,
				cl.ResourceText,
				ContractNumber,
				InceptionDate,
				ExpirationDate,
				sh.Flags,
				sh.InsertedDate,
				sh.UpdatedDate,
				sh.UniqAgency,
				sh.UniqBranch,
				sh.UniqDepartment
			FROM EpicDMZSUB..ServiceHead sh
			INNER JOIN EpicDMZSUB..CdServiceCode sc WITH(NOLOCK) on sh.UniqCdServiceCode=sc.UniqCdServiceCode
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US') cl on sc.ConfigureLkLanguageResourceID=cl.ConfigureLkLanguageResourceID
			WHERE sh.UniqCdServiceCode<>-1
			AND sh.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityServices SHT WITH(NOLOCK) WHERE SH.UniqServiceHead=SHT.UniqServiceHead)
		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.ServiceHead WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN

			UPDATE SHT SET 
				
						sht.UniqServiceHead=sh.UniqServiceHead,
						sht.UniqEntity=sh.UniqEntity,
						sht.ServiceNumber=sh.ServiceNumber,
						sht.UniqCdServiceCode=sc.CdServiceCode,
						sht.[Description]=cl.ResourceText,
						sht.ContractNumber=sh.ContractNumber,
						sht.InceptionDate=sh.InceptionDate,
						sht.ExpirationDate=sh.ExpirationDate,
						sht.flags=sh.Flags,
						sht.InsertedDate=sh.InsertedDate,
						sht.UpdatedDate=sh.UpdatedDate,
						sht.UniqAgency = sh.UniqAgency,
						sht.UniqBranch = sh.UniqBranch,
						sht.UniqDepartment = sh.UniqDepartment
			FROM EpicDMZSUB..ServiceHead sh
			INNER JOIN HIBOPActivityServices SHT WITH(NOLOCK) ON sh.UniqServiceHead=sht.UniqServiceHead
			INNER JOIN EpicDMZSUB..CdServiceCode sc WITH(NOLOCK) on sh.UniqCdServiceCode=sc.UniqCdServiceCode
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US') cl on sc.ConfigureLkLanguageResourceID=cl.ConfigureLkLanguageResourceID
			WHERE sh.UniqCdServiceCode<>-1  AND sh.UpdatedDate > @LastSyncDate
		END
		
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Line WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPActivityLine
			(
			UniqLine,UniqPolicy,UniqEntity,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber,UniqCdPolicyLineType,UniqCdLineStatus,IOC,BillModeCode,ExpirationDate,
			EffectiveDate,Flags,InsertedDate,UpdatedDate
			)
		
		
		SELECT
				l.UniqLine,
				l.UniqPolicy,
				L.UniqEntity,
				p.DescriptionOf,
				pt.CdPolicyLineTypeCode as Line,
				r1.ResourceText LineOfBusiness,
				r.ResourceText as LineStatus,	
				p.PolicyNumber,
				l.UniqCdPolicyLineType,
				l.UniqCdLineStatus,
				c.LookupCode,
				L.BillModeCode,
				l.ExpirationDate,
				l.EffectiveDate,
				l.Flags,
				l.InsertedDate,
				l.UpdatedDate
	
			FROM EpicDMZSUB..Line L WITH(NOLOCK)
			INNER JOIN EpicDMZSUB..Policy p WITH(NOLOCK) on l.UniqPolicy=p.UniqPolicy and l.UniqEntity=p.UniqEntity and l.UniqEntity=p.UniqEntity
			INNER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON l.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			inner join EpicDMZSUB..CdLineStatus cl WITH(NOLOCK) on l.UniqCdLineStatus=cl.UniqCdLineStatus
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US')r ON cl.ConfigureLkLanguageResourceID=r.ConfigureLkLanguageResourceID 
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US')r1 ON pt.ConfigureLkLanguageResourceID=r1.ConfigureLkLanguageResourceID 
			LEFT OUTER JOIN EpicDMZSub..Company c WITH(NOLOCK) on l.UniqEntityCompanyIssuing=c.UniqEntity
		WHERE l.uniqLine<>-1
			AND l.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityLine LT WITH(NOLOCK) WHERE L.UniqLine=LT.UniqLine)
		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.Line WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN


			UPDATE LT SET

				LT.UniqPolicy=l.UniqPolicy,
				LT.UniqEntity=L.UniqEntity,
				LT.PolicyDesc=p.DescriptionOf,
				LT.LineCode=pt.CdPolicyLineTypeCode,
				LT.LineOfBusiness=r1.ResourceText ,
				LT.LineStatus=r.ResourceText ,	
				LT.PolicyNumber=p.PolicyNumber,
				LT.UniqCdPolicyLineType=l.UniqCdPolicyLineType,
				LT.UniqCdLineStatus=l.UniqCdLineStatus,
				LT.BillModeCode=L.BillModeCode,
				LT.ExpirationDate=l.ExpirationDate,
				LT.EffectiveDate=l.EffectiveDate,
				LT.Flags=l.Flags,
				LT.InsertedDate=l.InsertedDate,
				LT.UpdatedDate=l.UpdatedDate,
				LT.IOC=C.Lookupcode
			FROM EpicDMZSUB..Line L WITH(NOLOCK)
			INNER JOIN HIBOPActivityLine LT WITH(NOLOCK) ON L.UniqLine=lt.UniqLine
			INNER JOIN EpicDMZSUB..Policy p WITH(NOLOCK) on l.UniqPolicy=p.UniqPolicy and l.UniqEntity=p.UniqEntity and l.UniqEntity=p.UniqEntity
			INNER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON l.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			inner join EpicDMZSUB..CdLineStatus cl WITH(NOLOCK) on l.UniqCdLineStatus=cl.UniqCdLineStatus
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US')r ON cl.ConfigureLkLanguageResourceID=r.ConfigureLkLanguageResourceID 
			INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US')r1 ON pt.ConfigureLkLanguageResourceID=r1.ConfigureLkLanguageResourceID 
			LEFT OUTER JOIN EpicDMZSub..Company c on l.UniqEntityCompanyIssuing=c.UniqEntity
		WHERE l.UniqLine<>-1  AND L.UpdatedDate > @LastSyncDate
		END


		------Carrrier Submission data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.CarrierSubmission WHERE  InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

			INSERT INTO HIBOPCarrierSubmission
			(
			UniqCarrierSubmission,Carrier,CarrierSubmission,UniqMarketingSubmission,UniqEntity,LastSubmittedDate,RequestedPremium,SubmissionStatus,InsertedDate,UpdatedDate
			)
			
			SELECT 
				DISTINCT cs.UniqCarrierSubmission,'Test' as Carrier,CS.DescriptionOf,CS.UniqMarketingSubmission,
				MS.UniqEntity,CS.SubmittedDate,RequestedTotalPremium,LkMarketingSubmissionStatus,cs.InsertedDate,cs.UpdatedDate
			FROM  EpicDMZSUB..CarrierSubmission CS WITH(NOLOCK) 
			INNER JOIN EpicDMZSUB..MarketingSubmission MS WITH(NOLOCK) ON CS.UniqMarketingSubmission=MS.UniqMarketingSubmission
			--INNER JOIN EpicDMZSUB..Activity A WITH(NOLOCK) ON A.UniqAssociatedItem=CS.UniqCarrierSubmission
			WHERE cs.UniqCarrierSubmission<>-1 
			AND CS.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPCarrierSubmission CST WITH(NOLOCK) WHERE CS.UniqCarrierSubmission=CST.UniqCarrierSubmission)
		END
			  
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB.DBO.CarrierSubmission WHERE  UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
		BEGIN


			UPDATE CST SET

				CST.Carrier='Test',
				CST.CarrierSubmission=CS.DescriptionOf,
				CST.UniqMarketingSubmission=CS.UniqMarketingSubmission,
				CST.UniqEntity=MS.UniqEntity,
				CST.LastSubmittedDate=CS.SubmittedDate,
				CST.RequestedPremium=CS.RequestedTotalPremium,
				CST.SubmissionStatus=CS.LkMarketingSubmissionStatus,
				CST.InsertedDate=CS.InsertedDate,
				CST.UpdatedDate=CS.UpdatedDate

		
			FROM  EpicDMZSUB..CarrierSubmission CS WITH(NOLOCK) 
			INNER JOIN HIBOPCarrierSubmission CST ON CS.UniqCarrierSubmission=CST.UniqCarrierSubmission
			INNER JOIN EpicDMZSUB..MarketingSubmission MS WITH(NOLOCK) ON CS.UniqMarketingSubmission=MS.UniqMarketingSubmission
			WHERE cs.UniqCarrierSubmission<>-1   AND CS.UpdatedDate > @LastSyncDate
		END

		------Activity Bill details data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber WHERE  TD.InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN

	

				INSERT INTO HIBOPActivityBill(UniqTranshead,DescriptionOf,UniqEntity,BillNumber,UniqAgency,AgencyName,Amount,Balance,InsertedDate,UpdatedDate)
				SELECT
					 H.UniqTransHead,D.DescriptionOf,H.UniqEntity as ClientId,H.BillNumber,H.UniqAgency,AG.NameOf as AgencyName,D.TransactionAmountCalc as Amount , 
					D.ItemBalanceCalc as Balance,D.InsertedDate,D.UpdatedDate
				FROM EpicDMZSUB..TransHead H WITH(NOLOCK) 
				--INNER JOIN HIBOPActivityBill AB ON H.UniqEntity=AB.UniqEntity AND AB.UniqTranshead=h.UniqTranshead
				INNER JOIN (SELECT TD.uniqTranshead,DescriptionOf,ItemBalanceCalc,TransactionAmountCalc,InsertedDate,UpdatedDate
				FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber) D ON H.UniqTransHead=D.UniqTransHead
				INNER JOIN EpicDMZSUB..Agency AG WITH(NOLOCK) ON H.UniqAgency=AG.UniqAgency
				WHERE D.uniqTranshead<>-1 
				AND D.InsertedDate > @LastSyncDate
				AND NOT EXISTS(SELECT 1 FROM HIBOPActivityBill CST WITH(NOLOCK) WHERE H.UniqEntity=CST.UniqEntity AND CST.UniqTranshead=H.UniqTranshead)

			END
			  
			IF EXISTS ( SELECT 'X' FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber WHERE  TD.UpdatedDate Between @LastSyncDate AND @ModifiedMaxDate )
			BEGIN



				UPDATE AB SET
					AB.DescriptionOf=D.DescriptionOf,
					AB.UniqEntity=H.UniqEntity,
					AB.BillNumber=H.BillNumber,
					AB.UniqAgency=H.UniqAgency,
					AB.AgencyName=AG.NameOf,
					AB.Amount=D.TransactionAmountCalc,
					AB.Balance=D.ItemBalanceCalc,
					AB.UpdatedDate=D.UpdatedDate
				FROM EpicDMZSUB..TransHead H WITH(NOLOCK) 
				INNER JOIN HIBOPActivityBill AB ON H.UniqEntity=AB.UniqEntity AND AB.UniqTranshead=h.UniqTranshead
				INNER JOIN (SELECT TD.uniqTranshead,DescriptionOf,ItemBalanceCalc,TransactionAmountCalc,InsertedDate,UpdatedDate
				FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber) D ON H.UniqTransHead=D.UniqTransHead
				INNER JOIN EpicDMZSUB..Agency AG WITH(NOLOCK) ON H.UniqAgency=AG.UniqAgency
				WHERE D.uniqTranshead<>-1   AND D.UpdatedDate > @LastSyncDate
		END

			------Activity Transaction details data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber WHERE  TD.InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN
				
			INSERT INTO HIBOPActivityTransaction(UniqTranshead,Code,DescriptionOf,UniqEntity,InvoiceNumber,ItemNumber,Amount,Balance,InsertedDate,UpdatedDate)
			SELECT 
				 H.UniqTransHead,H.Code, D.DescriptionOf,H.UniqEntity as ClientId,H.ItemNumber,H.InvoiceNumber,D.TransactionAmountCalc as Amount , 
				 I.Amount as Balance,D.InsertedDate,D.UpdatedDate
			FROM  EpicDMZSUB..TransHead H WITH(NOLOCK) 
			INNER JOIN (SELECT TD.uniqTranshead,DescriptionOf,ItemBalanceCalc,TransactionAmountCalc,InsertedDate,UpdatedDate
			FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
			INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
			AND TD.TransDetailNumber=TD1.TransDetailNumber) D ON H.UniqTransHead=D.UniqTransHead
			INNER JOIN EpicDMZSUB..Invoice I WITH(NOLOCK) ON H.InvoiceNumber=I.InvoiceNumber AND InvoiceStatus='C'
			WHERE D.uniqTranshead<>-1 
				AND D.InsertedDate > @LastSyncDate AND NOT EXISTS(SELECT 1 FROM HIBOPActivityTransaction CST WITH(NOLOCK) WHERE H.InvoiceNumber=CST.InvoiceNumber AND CST.UniqTranshead=H.UniqTranshead
				AND H.UniqEntity=CST.UniqEntity)
		END
		------Activity Transaction details data load
		IF EXISTS ( SELECT 'X' FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
				INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
				AND TD.TransDetailNumber=TD1.TransDetailNumber WHERE  TD.InsertedDate Between @LastSyncDate AND @ModifiedMaxDate)
		BEGIN
			UPDATE AB SET

				AB.Code=H.Code,
				AB.DescriptionOf=D.DescriptionOf,
				AB.UniqEntity=H.UniqEntity,
				AB.InvoiceNumber=I.InvoiceNumber,
				AB.ItemNumber=H.ItemNumber,
				AB.Amount=D.TransactionAmountCalc,
				AB.Balance=I.Amount,
				AB.UpdatedDate=D.UpdatedDate
			FROM  EpicDMZSUB..TransHead H WITH(NOLOCK) 
			INNER JOIN HIBOPActivityTransaction AB WITH(NOLOCK) ON H.UniqTransHead=AB.UniqTranshead
			INNER JOIN (SELECT TD.uniqTranshead,DescriptionOf,ItemBalanceCalc,TransactionAmountCalc,InsertedDate,UpdatedDate
			FROM EpicDMZSUB..TransDetail TD WITH(NOLOCK) 
			INNER JOIN (SELECT uniqTranshead,Max(TransDetailNumber) as TransDetailNumber FROM EpicDMZSUB..TransDetail WITH(NOLOCK) GROUP BY uniqTranshead) TD1 ON TD.UniqTransHead=TD1.UniqTransHead
			AND TD.TransDetailNumber=TD1.TransDetailNumber) D ON H.UniqTransHead=D.UniqTransHead
			INNER JOIN EpicDMZSUB..Invoice I WITH(NOLOCK) ON H.InvoiceNumber=I.InvoiceNumber AND InvoiceStatus='C'
			WHERE D.uniqTranshead<>-1   AND D.UpdatedDate > @LastSyncDate
		END
		
		UPDATE HIBOPComEPICSyncInfo SET EPICLastSyncDatetime = @ModifiedMaxDate
					
		DROP TABLE #HIBComEPICSyncInfo
	 END TRY

	 BEGIN CATCH
        
		SELECT 'Insert/Update Failed For HIBOPEpicSync_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 

END
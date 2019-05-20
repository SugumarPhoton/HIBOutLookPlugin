/*
-- =============================================
-- Module :		PullFromEPIC
-- Author:      BALACHANDAR.C
-- Create Date: 16-OCT-17
-- Description: This Procedure is used to Pull data from EPIC for new & updated records
-- =============================================
-------------------------------------------------------------------------------------------------------------------------------

EXEC HIBOPEpicSyncPolicyLineType_SP 

------------------------------------------------
-- Change History
---------------------
-- PR   Date        Author               Description 
-- --   --------   -------              ------------------------------------
** 
-------------------------------------------------------------------------------------------------------------------------------

*/
IF NOT EXISTS ( SELECT 'X' FROM SYSOBJECTS WHERE Xtype='P' AND  NAME ='HIBOPEpicSyncPolicyLineType_SP')
   EXEC('CREATE PROCEDURE [HIBOPEpicSyncPolicyLineType_SP] AS BEGIN SET NOCOUNT ON; END')
GO
ALTER PROCEDURE [dbo].[HIBOPEpicSyncPolicyLineType_SP]
AS 
BEGIN
     SET NOCOUNT ON
	  BEGIN TRY
	 ---Merge new and modify policylinetypes from EPIC

		MERGE HIBOPPolicyLineType PL
		USING 
		(
			SELECT ps.UniqCdPolicyLineType,ps.CdPolicyLineTypeCode,lr.ResourceText, ps.flags,UniqDepartment,UniqProfitCenter FROM EpicDMZSUB..CdPolicyLineType ps 
			INNER JOIN EpicDMZSUB..ConfigureLkLanguageResource lr ON lr.ConfigureLkLanguageResourceID = ps.ConfigureLkLanguageResourceID 
			AND lr.CultureCode = 'en-US' AND ps.flags & 2 = 2 AND UniqCdPolicyLineType > -1
		)EP
		ON PL.UniqCdPolicyLineType = EP.UniqCdPolicyLineType 
		AND PL.CdPolicyLineTypeCode=EP.CdPolicyLineTypeCode COLLATE SQL_Latin1_General_CP1_CI_AS
		
		WHEN MATCHED THEN
		UPDATE
		SET PL.PolicyLineTypeDesc=EP.ResourceText,
		PL.[Status]=EP.Flags,
		PL.UniqDepartment=EP.UniqDepartment,
		PL.UniqProfitcenter=EP.UniqProfitcenter
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqCdPolicyLineType, CdPolicyLineTypeCode, PolicyLineTypeDesc,Status,InsertedDate,UpdatedDate,UniqDepartment,UniqProfitcenter)
		VALUES (EP.UniqCdPolicyLineType, EP.CdPolicyLineTypeCode,EP.ResourceText,EP.flags,getdate(),getdate(),EP.UniqDepartment,EP.UniqProfitcenter);

		---Merge new and modify policylinetypes from EPIC
		MERGE HIBOPFolderAttachment OPF
		USING 
		(
			SELECT	AF.UniqAttachmentFolder,AF.UniqAttachmentFolderParent,AF.[Level],LR.ResourceText AS FolderName
			FROM EpicDMZSUB..AttachmentFolder AF
			INNER JOIN EpicDMZSUB..ConfigureLkLanguageResource lr ON lr.ConfigureLkLanguageResourceID = AF.ConfigureLkLanguageResourceID AND lr.CultureCode = 'en-US' 
			WHERE LEVEL IN('MN','S1','S2') 
	    )EPF

		ON OPF.FolderId = EPF.UniqAttachmentFolder 
		AND OPF.ParentFolderId=EPF.UniqAttachmentFolderParent
		
		WHEN MATCHED THEN
		UPDATE
		SET OPF.ParentFolderId=EPF.UniqAttachmentFolderParent,
		OPF.FolderName = EPF.FolderName,
		OPF.FolderType=EPF.Level 
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (FolderId, ParentFolderId, FolderType,FolderName,Status,InsertedDate,UpdatedDate)
		VALUES (EPF.UniqAttachmentFolder, EPF.UniqAttachmentFolderParent,EPF.Level,EPF.FolderName,1,getdate(),getdate());
		
		---Merge new and modify employee and entity mapping
		MERGE HIBOPEntityEmployee EE
		USING 
		(
			SELECT DISTINCT	E1.UniqEntity,UniqEmployee,E.Lookupcode,E.NameOf 
			FROM EpicDMZSUB..EntityEmployeeJT E1 WITH(NOLOCK)
			INNER JOIN EpicDMZSUB..Employee E WITH(NOLOCK) ON  E1.UniqEmployee=E.UniqEntity
			WHERE E.Flags & 2=2
			
	    )EEE

		ON EE.UniqEntity = EEE.UniqEntity AND EE.UniqEmployee = EEE.UniqEmployee 
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,UniqEmployee,Lookupcode,EmployeeName)
		VALUES (EEE.UniqEntity,EEE.UniqEmployee,EEE.Lookupcode,EEE.NameOf);

		
		---Merge new and modify client and agency mapping
		MERGE HIBOPClientAgencyBranch BT
		USING 
		(
			SELECT DISTINCT	UniqEntity,UniqAgency,UniqBranch
			FROM EpicDMZSUB..ClientAgencyBranchJT  WITH(NOLOCK)
			
			
	    )B

		ON B.UniqEntity=BT.UniqEntity AND B.UniqAgency=BT.UniqAgency AND B.UniqBranch =  Bt.UniqBranch
				
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,UniqAgency,UniqBranch)
		VALUES (B.UniqEntity,B.UniqAgency,B.UniqBranch);

		---Merge new and modify employee
		MERGE HIBOPEmployee E
		USING 
		(
			SELECT DISTINCT	UniqEntity,LookupCode,NameOf,Department,JobTitle,InactiveDate,RoleFlags,E.Flags,E.InsertedDate,E.UpdatedDate
			FROM EpicDMZSUB..Employee E  WITH(NOLOCK)
			INNER JOIN epicdmzsub.dbo.SECURITYUSER AS su WITH(NOLOCK) ON E.UniqEntity=su.UniqEmployee AND E.LookupCode=su.UserCode
			INNER JOIN epicdmzsub.dbo.securityuserstructurecombinationjt AS sujt WITH(NOLOCK) ON su.UniqSecurityUser=sujt.UniqSecurityUser
			INNER JOIN epicdmzsub.dbo.structurecombination AS sc WITH(NOLOCK) ON sc.UniqStructure=sujt.UniqStructure
			WHERE SU.Flags & 2=2 
			
			
	    )E1

		ON E.UniqEntity=E1.UniqEntity


		WHEN MATCHED THEN
		UPDATE
		SET 
			E.InactiveDate=E1.InactiveDate,
			E.RoleFlags=E1.RoleFlags,
			E.Flags=E1.Flags,
			E.InsertedDate=E1.InsertedDate,
			E.UpdatedDate=E1.UpdatedDate
		
					
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,LookupCode,EmployeeName,Department,JobTitle,InactiveDate,RoleFlags,Flags,InsertedDate,UpdatedDate)
		VALUES (E1.UniqEntity,E1.LookupCode,E1.NameOf,E1.Department,E1.JobTitle,E1.InactiveDate,E1.RoleFlags,E1.Flags,E1.InsertedDate,E1.UpdatedDate);

		--LOADING DEPARTMENT DATA FROM EPIC
		MERGE HIBOPDepartment D
		USING 
		(
			SELECT UniqDepartment,DepartmentCode,NameOf,InsertedDate,UpdatedDate,Flags
			FROM EpicDMZSUB..Department  WITH(NOLOCK)		
			
	    )DS

		ON D.UniqDepartment=DS.UniqDepartment

		WHEN MATCHED THEN
		UPDATE SET
		D.DepartmentCode=DS.DepartmentCode,
		D.NameOf=DS.NameOf,
		D.InsertedDate=DS.InsertedDate,
		D.UpdatedDate=DS.UpdatedDate,
		D.Flags=DS.Flags
				
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqDepartment,DepartmentCode,NameOf,InsertedDate,UpdatedDate,Flags)
		VALUES (DS.UniqDepartment,DS.DepartmentCode,DS.NameOf,DS.InsertedDate,DS.UpdatedDate,DS.Flags);

		--LOADING Profit Center DATA FROM EPIC
		MERGE HIBOPProfitCenter P
		USING 
		(
			SELECT UniqProfitCenter,ProfitCenterCode,NameOf,InsertedDate,UpdatedDate,Flags
			FROM EpicDMZSUB..ProfitCenter  WITH(NOLOCK)		
			
	    )PS

		ON P.UniqProfitCenter=PS.UniqProfitCenter

		WHEN MATCHED THEN
		UPDATE SET
		P.ProfitCenterCode=PS.ProfitCenterCode,
		P.NameOf=PS.NameOf,
		P.InsertedDate=PS.InsertedDate,
		P.UpdatedDate=PS.UpdatedDate,
		P.Flags=PS.Flags
				
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqProfitCenter,ProfitCenterCode,NameOf,InsertedDate,UpdatedDate,Flags)
		VALUES (PS.UniqProfitCenter,PS.ProfitCenterCode,PS.NameOf,PS.InsertedDate,PS.UpdatedDate,PS.Flags);

		---Merge new employee and agency mapping
		MERGE HIBOPEmployeeStructure T
		USING 
		(Select su.uniqemployee,Sc.UniqAgency,Sc.UniqBranch,Sc.UniqDepartment,sc.UniqProfitCenter From EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) 
		INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) on sus.uniqstructure = sc.uniqstructure
		INNER JOIN EpicDMZSub.DBO.securityuser su (NOLOCK) On su.uniqsecurityuser = sus.UniqSecurityUser And su.uniqemployee > -1) As S
	   
		ON T.UniqEntity=S.uniqemployee AND T.UniqAgency=S.UniqAgency AND T.UniqBranch =  S.UniqBranch
		AND T.UniqDepartment =  S.UniqDepartment AND T.UniqProfitCenter =  S.UniqProfitCenter

		WHEN NOT MATCHED BY TARGET THEN
			INSERT (UniqEntity,UniqAgency,UniqBranch,UniqDepartment,UniqProfitCenter)
			VALUES (S.uniqemployee,S.UniqAgency,S.UniqBranch,S.UniqDepartment,S.UniqProfitCenter)
		WHEN NOT MATCHED BY SOURCE THEN 
			DELETE;

		---Merge new employee and agency mapping
		MERGE HIBOPEmployeeAgency EA
		USING 
		EpicDMZSUB.DBO.EmployeeStructure E WITH(NOLOCK)
	   
		ON EA.UniqEntity=E.UniqEntity AND EA.UniqAgency=E.UniqAgency AND EA.UniqBranch =  E.UniqBranch
		AND EA.UniqDepartment =  E.UniqDepartment AND EA.UniqProfitCenter =  E.UniqProfitCenter

		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,UniqAgency,UniqBranch,UniqDepartment,UniqProfitCenter,InsertedDate,UpdatedDate)
		VALUES (E.UniqEntity,E.UniqAgency,E.UniqBranch,E.UniqDepartment,E.UniqProfitCenter,GETDATE(),GETDATE());


		---Merge new and modify client contact informations
		MERGE HIBOPActivityClientContacts CT
		USING 
		(
		
		SELECT 
			c.UniqEntity,
			cc.UniqContactName,
			cn.UniqContactNumber,
			CASE WHEN ( FullName IS NULL OR FullName ='') THEN FirstName+' '+LastName ELSE FullName END ContactName,
			CASE WHEN CN.TypeCode='EM1' THEN 'Email'
			WHEN CN.TypeCode IN('MOB','BUS') THEN 'Phone'
			WHEN CN.TypeCode='FAX' THEN 'Fax' ELSE '' END 'Type',
			CASE WHEN CN.TypeCode IN('MOB','BUS') THEN Number 
			WHEN CN.TypeCode='Fax' THEN Number 
			WHEN CN.TypeCode='EM1' THEN EmailWeb ELSE '' END  ContactValue,
			CN.InsertedDate,
			CN.UpdatedDate
 
		FROM EpicDMZSUB..Client C WITH(NOLOCK)
		INNER JOIN EpicDMZSUB..ContactName CC WITH(NOLOCK) ON C.UniqContactNamePrimary=CC.UniqContactName AND C.UniqEntity=CC.UniqEntity
		INNER JOIN EpicDMZSUB..ContactNumber CN WITH(NOLOCK) ON C.UniqEntity =CN.UniqEntity  AND CC.UniqContactName=cn.UniqContactName
		AND CN.Typecode in('ACT','MOB','FAX','EM1','BUS') 

		 )ST

		ON ST.UniqEntity = CT.UniqEntity AND ST.UniqContactName=CT.UniqContactName AND CT.UniqContactNumber=ST.UniqContactNumber

		WHEN MATCHED THEN
		UPDATE
		SET 
		CT.UniqEntity= ST.UniqEntity,
		CT.ContactName= ST.ContactName,
		CT.UniqContactNumber=ST.UniqContactNumber,
		CT.ContactType=ST.[Type],
		CT.ContactValue=ST.ContactValue,
		CT.InsertedDate=ST.InsertedDate,
		CT.UpdatedDate=ST.UpdatedDate
				
		
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,UniqContactName,ContactName,ContactType,ContactValue,InsertedDate,UpdatedDate,UniqContactNumber)
		VALUES (ST.UniqEntity,ST.UniqContactName,ST.ContactName,ST.[Type],ST.ContactValue,ST.InsertedDate,ST.UpdatedDate,st.UniqContactNumber);

		MERGE HIBOPActivityLookupDetails LD
		USING 
		(
		

		SELECT distinct
				l.UniqLine,
				l.UniqPolicy,
				L.UniqEntity,
				c.UniqClaim,
				pt.CdPolicyLineTypeCode as LineCode,
				pt.CdPolicyLineTypeCode as PolicyType,
				r1.ResourceText Linedescription,
				p.PolicyNumber,
				P.DescriptionOf as PolicyDesc,
				l.UniqCdPolicyLineType,
				l.UniqCdLineStatus,
				l.ExpirationDate as LineExpDate,
				l.EffectiveDate as LineEffDate,
				p.ExpirationDate as PolicyExpDate,
				p.EffectiveDate as PolicyEffDate,
				c.ClaimNumber,
				c.CompanyClaimNumber,
				c.LossDate as DateLoss,
				c.ClosedDate,
				cl.LookupCode,
				cl.NameOf as AccountName,
				ISNULL(ca.UniqClaimAssociation,-1)as UniqClaimAssociation,
				co.NameOf as IOC,
				CO.LookupCode as IOCCode
		
	
		FROM EpicDMZSUB..Line L WITH(NOLOCK)
		INNER JOIN EpicDMZSUB..Policy p WITH(NOLOCK) on l.UniqPolicy=p.UniqPolicy and l.UniqEntity=p.UniqEntity and l.UniqEntity=p.UniqEntity
		INNER JOIN EpicDMZSUB..CdPolicyLineType PT WITH(NOLOCK) ON l.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
		INNER JOIN EpicDMZSUB..Client cl WITH(NOLOCK) ON L.UniqEntity=cl.UniqEntity
		INNER JOIN EpicDMZSUB..Company co WITH(NOLOCK) ON L.UniqEntityCompanyIssuing=co.UniqEntity
		INNER JOIN (SELECT DISTINCT ResourceText, ConfigureLkLanguageResourceID FROM EpicDMZSUB..ConfigureLkLanguageResource WITH(NOLOCK) where CultureCode='en-US')r1 ON pt.ConfigureLkLanguageResourceID=r1.ConfigureLkLanguageResourceID 
		LEFT OUTER JOIN EpicDMZSUB..ClaimAssociation ca WITH(NOLOCK)on ca.UniqLine=l.UniqLine AND CA.UniqPolicy=P.UniqPolicy
		LEFT OUTER JOIN EpicDMZSUB..Claim C WITH(NOLOCK)on ca.UniqClaim=c.UniqClaim
		)SD ON LD.UniqLine=SD.UniqLine AND LD.UniqPolicy=SD.UniqPolicy AND LD.UniqEntity=LD.UniqEntity AND LD.UniqClaimAssociation=SD.UniqClaimAssociation

		WHEN MATCHED THEN
		UPDATE
		SET 
			LD.UniqClaim=SD.UniqClaim,
			LD.LineCode=SD.LineCode,
			LD.PolicyType=SD.PolicyType,
			LD.Linedescription=SD.Linedescription,
			LD.PolicyNumber=SD.PolicyNumber,
			LD.PolicyDesc=SD.PolicyDesc,
			LD.UniqCdPolicyLineType=SD.UniqCdPolicyLineType,
			LD.UniqCdLineStatus=SD.UniqCdLineStatus,
			LD.LineExpDate=SD.LineExpDate,
			LD.LineEffDate=sd.LineEffDate,
			LD.PolicyExpDate=SD.PolicyExpDate,
			LD.PolicyEffDate=SD.PolicyEffDate,
			LD.ClaimNumber=SD.ClaimNumber,
			LD.DateLoss=SD.DateLoss,
			LD.ClosedDate=SD.ClosedDate,
			LD.LookupCode=SD.LookupCode,
			LD.AccountName=SD.AccountName,
			LD.UpdatedDate=GETDATE(),
			LD.IOC=SD.IOC,
			LD.IOCCode=SD.IOCCode


		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqLine,UniqPolicy,UniqEntity,UniqClaim,LineCode,PolicyType,Linedescription,PolicyNumber,PolicyDesc,UniqCdPolicyLineType,UniqCdLineStatus,LineExpDate,LineEffDate,
				PolicyExpDate,PolicyEffDate,ClaimNumber,CompanyClaimNumber,DateLoss,ClosedDate,LookupCode,AccountName,InsertedDate,UpdatedDate,UniqClaimAssociation,IOC,IOCCode)
		VALUES (SD.UniqLine,SD.UniqPolicy,SD.UniqEntity,SD.UniqClaim,SD.LineCode,SD.PolicyType,SD.Linedescription,SD.PolicyNumber,SD.PolicyDesc,SD.UniqCdPolicyLineType,SD.UniqCdLineStatus,SD.LineExpDate,
		SD.LineEffDate,SD.PolicyExpDate,SD.PolicyEffDate,SD.ClaimNumber,SD.CompanyClaimNumber,SD.DateLoss,SD.ClosedDate,SD.LookupCode,SD.AccountName,GetDate(),'',SD.UniqClaimAssociation,SD.IOC,SD.IOCCode);

		---Activity Evidence Of Insurnace data loading from EPIC
		
		MERGE HIBOPActivityEvidenceOfInsurance AC
		USING
		(
			SELECT UniqEvidence,UniqEntityClient,RTRIM(LTRIM(Title)) as Title,FE.FormEditionDate, E.InsertedDate,E.UpdatedDate 
			FROM EpicDMZSUB..Evidence E WITH(NOLOCK)
			INNER JOIN EpicDMZSUB..FormEditionGrouP FG WITH(NOLOCK) ON E.UniqFormEditionGroup=FG.UniqFormEditionGroup
			INNER JOIN EpicDMZSUB..FormEditionGroupJT FEG WITH(NOLOCK) ON FG.UniqFormEditionGroup=FEG.UniqFormEditionGroup
			INNER JOIN EpicDMZSUB..FormEdition FE WITH(NOLOCK) ON FE.UniqFormEdition=FEG.UniqFormEdition
			AND FE.FormDescription=FG.GroupDescription
		) AT
		ON AC.UniqEvidence=AT.UniqEvidence AND AC.UniqEntityClient=AT.UniqEntityClient

		WHEN MATCHED THEN
		UPDATE
		SET 

		AC.Title=AT.Title,
		AC.FormEditionDate=AT.FormEditionDate,
		AC.InsertedDate=AT.InsertedDate,
		AC.UpdatedDate=AT.UpdatedDate

		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEvidence,UniqEntityClient,Title,FormEditionDate,InsertedDate,UpdatedDate)
		VALUES(AT.UniqEvidence,AT.UniqEntityClient,AT.Title,AT.FormEditionDate,AT.InsertedDate,AT.UpdatedDate)	;


		
		---Activity Certificate data loading from EPIC
		
		MERGE HIBOPActivityCertificate AC
		USING
		(
			SELECT  distinct UniqCertificate,c.UniqEntity,LTRIM(RTRIM(Title)) AS Title,c.InsertedDate,c.UpdatedDate 
			FROM EpicDMZSUB..[Certificate] c
		) AT
		ON AC.UniqCertificate=AT.UniqCertificate AND AC.UniqEntity=AT.UniqEntity

		WHEN MATCHED THEN
		UPDATE
		SET 

		AC.Title=AT.Title,
		AC.InsertedDate=AT.InsertedDate,
		AC.UpdatedDate=AT.UpdatedDate

		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqCertificate,UniqEntity,Title,InsertedDate,UpdatedDate)
		VALUES(AT.UniqCertificate,AT.UniqEntity,AT.Title,AT.InsertedDate,AT.UpdatedDate);	


		---Merge new and modify employee
		MERGE HIBOPActvityOwnerList E
		USING 
		(
			SELECT DISTINCT	UniqEntity,LookupCode,NameOf,InactiveDate,E.Flags,E.InsertedDate,E.UpdatedDate
			FROM EpicDMZSUB..Employee E  WITH(NOLOCK)
			
			
			
	    )E1

		ON E.UniqEntity=E1.UniqEntity


		WHEN MATCHED THEN
		UPDATE
		SET 
			E.InactiveDate=E1.InactiveDate,
			E.Flags=E1.Flags,
			E.InsertedDate=E1.InsertedDate,
			E.UpdatedDate=E1.UpdatedDate
		
					
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (UniqEntity,LookupCode,OwnerName,InactiveDate,Flags,InsertedDate,UpdatedDate)
		VALUES (E1.UniqEntity,E1.LookupCode,E1.NameOf,E1.InactiveDate,E1.Flags,E1.InsertedDate,E1.UpdatedDate);
		
		

			
	 END TRY

	 BEGIN CATCH
        
		SELECT 'Insert/Update Failed For HIBOPEpicSyncPolicyLineType_SP Error MSG : '+ERROR_MESSAGE()

     END CATCH 
END
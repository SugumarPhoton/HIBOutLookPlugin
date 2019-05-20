
/****** Object:  StoredProcedure [dbo].[HIBOPUserDeltaSync_SP]    Script Date: 2/20/2019 3:56:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPUserDeltaSync_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPUserDeltaSync_SP] AS' 
END
GO
--[dbo].[HIBOPUserDeltaSync_SP] 'LEUNA1',Null,23,1,23
Alter PROCEDURE [dbo].[HIBOPUserDeltaSync_SP] --'LEUNA1',Null,23,1,23
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY

    DECLARE @securityuserid AS int
	DECLARE @UniqEmployee INT
	--DECLARE @iCheckStructure AS TINYINT
	--DECLARE @iCheckEmployeeAccess AS TINYINT
	Declare @GetDate datetime = getdate()
	Declare @User char(6),
	        @SpName varchar(255),
	        @LastSyncDate datetime
			
			Select *
			Into #tmp
			from HIBOPUserDeltaSyncInfo(nolock)
			where IsDeltaFlag    =  0
			
			--select * from #tmp
			--select * from Vw_SecurityUser
			--select * from Vw_SecurityUserClient
			--select * from Vw_Market
			--select * from Vw_EntityEmployee
    --code added for disk io issue starts here 
	Create nonclustered index idx_tmp_empid_userlookupcode_spname on #tmp(Employeeid,Userlookupcode,Spname)
	--code added for disk io issue ends here 

	IF Exists(SELECT 'X'
	          FROM HIBOPUserDeltaSyncInfo(NOLOCK)
			  WHERE IsDeltaFlag    =  0
		      AND SpName In ('HIBOPGetActivityAccount_SP','HIBOPGetActivityLine_SP','HIBOPGetActivityMarketing_SP',
							 'HIBOPGetActivityOpportunity_SP','HIBOPGetActivityPolicy_SP','HIBOPGetActivityServices_SP',
							 'HIBOPGetActivityDetails_SP','HIBOPGetActivityEmployee_SP')
		      )
	BEGIN
	
		CREATE TABLE #HIBOPEmployeeStructure
		(
		UniqEntity			int			Null,
		UniqAgency			int			Null,
		UniqBranch			int			Null,
		UniqDepartment		int			Null
		)

		INSERT INTO #HIBOPEmployeeStructure
		SELECT es.UniqEntity,es.UniqAgency,es.UniqBranch,es.UniqDepartment 
		FROM HIBOPEmployeeStructure es(nolock)
			 inner join 
			 #tmp t 
			 on(es.UniqEntity = t.EmployeeId)
		Group By UniqEntity,UniqAgency,UniqBranch,UniqDepartment
		
	END
	
	IF Exists(SELECT 'X'
	          FROM HIBOPUserDeltaSyncInfo(NOLOCK)
			  WHERE IsDeltaFlag    =  0
		      AND SpName In ('HIBOPGetActivityAccount_SP','HIBOPGetActivityLine_SP','HIBOPGetActivityPolicy_SP',
							 'HIBOPGetClientDetails_SP','HIBOPGetClientEmployee_SP','HIBOPGetActivityEmployee_SP')
		      )
	BEGIN
	
		--Create Table #SecurityUserClient (EmployeeId int,uniqsecurityuser int,uniqentity Int)
		--CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #SecurityUserClient (EmployeeId,uniqsecurityuser,uniqentity);
			
		--INSERT INTO #SecurityUserClient
		select t.EmployeeId,a.uniqsecurityuser,a.uniqentity
		into #SecurityUserClient
		from Vw_SecurityUserClient a(nolock) 
			 inner join 
			 Vw_SecurityUser b (nolock)
		on(a.uniqsecurityuser = b.uniqsecurityuser)
			 inner join 
			 (select distinct employeeid from  #tmp) t 
			 on(b.uniqemployee = t.EmployeeId)

			
		CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #SecurityUserClient (EmployeeId,uniqsecurityuser,uniqentity);
		 
	END
	
	IF Exists(SELECT 'X'
	          FROM HIBOPUserDeltaSyncInfo(NOLOCK)
			  WHERE IsDeltaFlag    =  0
		      AND SpName In ('HIBOPGetActivityClaim_SP','HIBOPGetActivityMarketing_SP',
							 'HIBOPGetActivityOpportunity_SP','HIBOPGetActivityServices_SP',
							 'HIBOPGetActivityDetails_SP')
		      )
	BEGIN
		--CREATE TABLE #EntityEmployee (uniqemployee int,UniqEntity Int)
		

		--INSERT INTO #EntityEmployee
		select distinct uniqemployee,uniqentity
		into #EntityEmployee
		from Vw_EntityEmployee a (nolock)
			  inner join 
			 #tmp t 
			on(a.uniqemployee = t.EmployeeId)

			CREATE UNIQUE NONCLUSTERED INDEX IX_SecurityUserClient on #EntityEmployee (uniqemployee,UniqEntity);
	 END
	 
	IF Exists(SELECT 'X'
	          FROM HIBOPUserDeltaSyncInfo(NOLOCK)
			  WHERE IsDeltaFlag    =  0
		      AND SpName In ('HIBOPGetActivityMarketing_SP')
		      )
	BEGIN
		
		CREATE TABLE #Market (UniqMarketingSubmission INT, LineCode VARCHAR(200))

		INSERT INTO #Market
		select UniqMarketingSubmission,LineCode from Vw_Market
	END

    IF 	Exists (Select 'X' 
                from #tmp 
                where spname = 'HIBOPGetActivityAccount_SP')
    BEGIN
 ;with cte 
        as
        (
		Select distinct ES.UniqEntity
		FROM HIBOPClient c WITH (NOLOCK)
		INNER JOIN HIBOPClientAgencyBranch AS cl WITH (NOLOCK) ON C.Uniqentity=cl.uniqentity
		INNER JOIN #HIBOPEmployeeStructure ES WITH (NOLOCK) ON ES.UniqAgency = cl.UniqAgency 
		AND ES.UniqBranch = cl.UniqBranch
		INNER JOIN  (select * from #tmp where spname = 'HIBOPGetActivityAccount_SP') Delta
		 on(Es.UniqEntity = Delta.EmployeeId)
		 inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		INNER JOIN HIBOPAgency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
		INNER JOIN HIBOPBRANCH AS b WITH (NOLOCK) ON cl.Uniqbranch=b.Uniqbranch
		WHERE c.uniqentity <> -1 AND ISNULL(C.UpdatedDate,C.InsertedDate) > Delta.LastSyncDate
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient c2 WHERE c2.uniqentity = c.uniqentity and  c2.EmployeeId =delta.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
				   FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
				   WHERE eejt.UniqEntity = c.UniqEntity 
				   AND eejt.UniqEmployee = Delta.EmployeeId)))
		)
		
		Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityAccount_SP'
        and   IsDeltaFlag    =  0
				   
     END			   
	
	
	IF 	Exists (Select 'X'
                from #tmp 
                where spname = 'HIBOPGetActivityBill_SP'
               )
    BEGIN
         UPDATE a 
         SET a.IsDeltaFlag = 1,
			 a.UpdatedDate = @GetDate
         FROM  HIBOPUserDeltaSyncInfo a (nolock)
			   inner join 
				(
					Select E.UniqEmployee
					FROM HIBOPActivityBill B WITH(NOLOCK)
					INNER JOIN HIBOPClient C WITH(NOLOCK) ON B.UniqEntity=C.UniqEntity
					INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
					INNER JOIN HIBOPAgency A WITH(NOLOCK) ON A.UniqAgency=B.UniqAgency
					inner join (select * from #tmp where spname = 'HIBOPGetActivityBill_SP') delta 
					on(E.UniqEmployee = Delta.EmployeeId)
					WHERE ISNULL(B.UpdatedDate, B.InsertedDate)>Delta.LastSyncDate
				) b
		ON(a.EmployeeId = b.UniqEmployee)
		WHERE SpName         =  'HIBOPGetActivityBill_SP'
        and   IsDeltaFlag    =  0


	End
	
	IF 	Exists (Select 'X'
                from #tmp 
                where spname = 'HIBOPGetActivityCarrierSubmission_SP'
               )
    BEGIN
         UPDATE a 
         SET a.IsDeltaFlag = 1,
			 a.UpdatedDate = @GetDate
         FROM  HIBOPUserDeltaSyncInfo a 
			   inner join 
				(
					Select E.UniqEmployee 
					FROM HIBOPCarrierSubmission CS WITH(NOLOCK)
					INNER JOIN HIBOPActivityMasterMarketing MS WITH(NOLOCK) ON CS.UniqMarketingSubmission=MS.UniqMarketingSubmission
					INNER JOIN HIBOPClient C WITH(NOLOCK) ON CS.UniqEntity=C.UniqEntity
					INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
					inner join (select * from #tmp where spname = 'HIBOPGetActivityCarrierSubmission_SP') delta 
								on(E.UniqEmployee = Delta.EmployeeId)
					WHERE ISNULL(CS.UpdatedDate, CS.InsertedDate)>Delta.LastSyncDate
				) b
		ON(a.EmployeeId = b.UniqEmployee)
		WHERE SpName         =  'HIBOPGetActivityCarrierSubmission_SP'
        and   IsDeltaFlag    =  0
    
	end
	
	IF 	Exists (Select 'X'
				from #tmp 
				where spname = 'HIBOPGetActivityCertificate_SP'
			   )
    BEGIN
    
	 UPDATE a 
	 SET a.IsDeltaFlag = 1,
		 a.UpdatedDate = @GetDate
	 FROM  HIBOPUserDeltaSyncInfo a 
		   inner join 
			(
			select  E.UniqEmployee 
			FROM HIBOPActivityCertificate C WITH(NOLOCK)
			INNER JOIN HIBOPClient CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
			INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) ON CL.UniqEntity=E.UniqEntity
			inner join (select * from #tmp where spname = 'HIBOPGetActivityCertificate_SP') delta 
										on(E.UniqEmployee = Delta.EmployeeId)
			WHERE E.UniqEmployee=Delta.EmployeeId AND ISNULL(C.UpdatedDate, C.InsertedDate)>Delta.LastSyncDate
			)b
		ON(a.EmployeeId = b.UniqEmployee)
	 WHERE SpName         =  'HIBOPGetActivityCertificate_SP'
     and   IsDeltaFlag    =  0
	
	END
	
	IF 	Exists (Select 'X'
				from #tmp 
				where spname = 'HIBOPGetActivityClaim_SP'
			   )
    BEGIN
   /* ; with cte 
    as 
    (
	select c.UniqEntity
	FROM HIBOPClient c with (nolock)
		INNER JOIN HIBOPClaim CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityClaim_SP') delta 
				on(c.UniqEntity = Delta.EmployeeId)inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
	WHERE c.uniqentity <> -1 
	AND (vwsu.iCheckStructure = 0 
	OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and EE.uniqemployee = Delta.EmployeeId))
	AND (vwsu.iCheckEmployeeAccess = 0 
	OR (c.[STATUS] & 8192 = 8192 
	OR EXISTS ( SELECT 1
				FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
				WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId)))
	)
			
			

		Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityClaim_SP'
        and   IsDeltaFlag    =  0	
		*/
		
			Update d 
			set d.IsDeltaFlag = 1,
			d.UpdatedDate = @GetDate
			FROM HIBOPClient c with (nolock)
			INNER JOIN HIBOPClaim CL WITH(NOLOCK) ON C.UniqEntity=CL.UniqEntity
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityClaim_SP' AND D.IsDeltaFlag = 0
			INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			WHERE c.uniqentity <> -1 
			AND (vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and  EE.uniqemployee = D.EmployeeId))
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.[STATUS] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId))) AND 
			ISNULL(CL.UpdatedDate,CL.InsertedDate)>D.LastSyncDate			
				
				
   End				
	
	IF 	Exists (Select 'X' 
				from #tmp 
				where spname = 'HIBOPGetActivityClientContacts_SP'
			   )
    BEGIN
     UPDATE a 
     SET a.IsDeltaFlag = 1,
		 a.UpdatedDate = @GetDate
     FROM  HIBOPUserDeltaSyncInfo a 
		 inner join 
		(
			Select    E.UniqEmployee
			FROM HIBOPActivityClientContacts C WITH(NOLOCK)
				INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) 
				ON C.UniqEntity=E.UniqEntity
				inner join (select * from #tmp where spname = 'HIBOPGetActivityClientContacts_SP') delta 
						on(E.UniqEmployee = Delta.EmployeeId)
			WHERE ISNULL(c.UpdatedDate,c.InsertedDate)>Delta.LastSyncDate -- column not exists in Qa want to revoke after 
		)b
		ON(a.EmployeeId = b.UniqEmployee)
	 WHERE SpName         =  'HIBOPGetActivityClientContacts_SP'
     and   IsDeltaFlag    =  0
	
	END
	
    
	IF 	Exists (Select 'X' 
				from #tmp 
				where spname = 'HIBOPGetActivityEvidence_SP'
			   )
    BEGIN
    
    UPDATE a 
     SET a.IsDeltaFlag = 1,
		 a.UpdatedDate = @GetDate
     FROM  HIBOPUserDeltaSyncInfo a 
		 inner join 
		(
			Select E.UniqEmployee
			FROM HIBOPActivityEvidenceOfInsurance EV WITH(NOLOCK)
				INNER JOIN HIBOPClient C WITH(NOLOCK) 
				ON C.UniqEntity=EV.UniqEntityClient
				INNER JOIN HIBOPEntityEmployee E  WITH(NOLOCK) 
				ON C.UniqEntity=E.UniqEntity
					inner join (select * from #tmp where spname = 'HIBOPGetActivityEvidence_SP') delta 
								on(E.UniqEmployee = Delta.EmployeeId)
			WHERE E.UniqEmployee=Delta.EmployeeId
			AND ISNULL(EV.UpdatedDate, EV.InsertedDate)>Delta.LastSyncDate
		)b
	   ON(a.EmployeeId = b.UniqEmployee)
	 WHERE SpName         =  'HIBOPGetActivityEvidence_SP'
     and   IsDeltaFlag    =  0
     
	 
	End
	
	IF 	Exists (Select 'X' 
		from #tmp 
		where spname = 'HIBOPGetActivityLine_SP'
	   )
    BEGIN
	/*
    ;with cte
    as 
    (
	select  c.UniqEntity 
	FROM HIBOPActivityLine A WITH(NOLOCK)	
			INNER JOIN  HIBOPPolicy p WITH(NOLOCK) ON a.UniqPolicy=p.UniqPolicy
			INNER JOIN HIBOPPolicyLineType AS pt ON a.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			inner join (select * from #tmp where spname = 'HIBOPGetActivityLine_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
				inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
			WHERE c.uniqentity <> -1 
			AND (vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity and cl.EmployeeId = Delta.EmployeeId )) 
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.[status] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId)))  
			AND 
			ISNULL(A.UpdatedDate, A.InsertedDate)>Delta.LastSyncDate
			and  a.ExpirationDate> DATEADD(mm,-18,@GetDate) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END
				and Es.UniqEntity = Delta.EmployeeId)
		)
		
		Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityLine_SP'
        and   IsDeltaFlag    =  0
		*/
		
			Update d 
			set d.IsDeltaFlag = 1,
				d.UpdatedDate = @GetDate
			FROM HIBOPActivityLine A WITH(NOLOCK)	
			INNER JOIN  HIBOPPolicy p WITH(NOLOCK) ON a.UniqPolicy=p.UniqPolicy
			and   a.ExpirationDate> DATEADD(mm,-18,getdate())--code added for diskio issue
			INNER JOIN HIBOPPolicyLineType AS pt ON a.UniqCdPolicyLineType=pt.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityLine_SP' AND D.IsDeltaFlag = 0
			AND ISNULL(A.UpdatedDate, A.InsertedDate)>D.LastSyncDate--code added for diskio issue
			INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			WHERE c.uniqentity <> -1 
			AND (vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity and cl.EmployeeId = D.EmployeeId))
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.[status] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId)))
			/*AND ISNULL(A.UpdatedDate, A.InsertedDate)>D.LastSyncDate
			and   a.ExpirationDate> DATEADD(mm,-18,getdate()) */--code commented for diskio issue
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)
	
	End
		
	IF 	Exists (Select 'X' 
				from #tmp 
				where spname = 'HIBOPGetActivityMarketing_SP'
			   )
    BEGIN		
       /*; with cte 
       as 
       ( 
		Select   c.UniqEntity
		FROM HIBOPActivityMasterMarketing m WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on m.UniqEntity=C.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityMarketing_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
								inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		LEFT OUTER JOIN #Market p on m.UniqMarketingSubmission=p.UniqMarketingSubmission
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and EE.uniqemployee = Delta.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee =  Delta.EmployeeId))) and
		ISNULL(M.UpdatedDate,M.InsertedDate)>Delta.LastSyncDate
		AND m.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = M.UniqAgency AND ES.UniqBranch = M.UniqBranch
				AND ES.UniqDepartment = CASE WHEN M.UniqDepartment = -1 THEN ES.UniqDepartment ELSE M.UniqDepartment END
				and Es.UniqEntity = Delta.EmployeeId)
		)

        Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityMarketing_SP'
        and   IsDeltaFlag    =  0
		*/
		Update d 
		set d.IsDeltaFlag = 1,
			d.UpdatedDate = @GetDate
		FROM HIBOPActivityMasterMarketing m WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on m.UniqEntity=C.UniqEntity
		LEFT OUTER JOIN #Market p on m.UniqMarketingSubmission=p.UniqMarketingSubmission
		INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityMarketing_SP' AND D.IsDeltaFlag = 0
		INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and  EE.uniqemployee = D.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId))) and
		ISNULL(M.UpdatedDate,M.InsertedDate)>D.LastSyncDate
		AND m.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = M.UniqAgency AND ES.UniqBranch = M.UniqBranch
				AND ES.UniqDepartment = CASE WHEN M.UniqDepartment = -1 THEN ES.UniqDepartment ELSE M.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)
    END
	 
	IF 	Exists (Select 'X' 
				from #tmp 
				where spname = 'HIBOPGetActivityOpportunity_SP'
			   )
    BEGIN	
    /*
    ;with cte
    as 
    (
		Select   c.UniqEntity
		FROM HIBOPActivityOpportunity m WITH(NOLOCK)	
		INNER JOIN HIBOPClient c WITH (NOLOCK) on m.UniqEntity=C.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityOpportunity_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
						inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and EE.uniqemployee = Delta.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId))) 
		AND ISNULL(M.UpdatedDate,M.InsertedDate)>Delta.LastSyncDate
		AND TargetedDate> DATEADD(mm,-18,@GetDate)
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = M.UniqAgency AND ES.UniqBranch = M.UniqBranch
				AND ES.UniqDepartment = CASE WHEN M.UniqDepartment = -1 THEN ES.UniqDepartment ELSE M.UniqDepartment END
				and Es.UniqEntity = Delta.EmployeeId)
      )				
		
		
        Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityOpportunity_SP'
        and   IsDeltaFlag    =  0
		*/
		
		Update d 
		set d.IsDeltaFlag = 1,
			d.UpdatedDate = @GetDate
		FROM HIBOPActivityOpportunity m WITH(NOLOCK)	
		INNER JOIN HIBOPClient c WITH (NOLOCK) on m.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityOpportunity_SP' AND D.IsDeltaFlag = 0
		INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and  EE.uniqemployee = D.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId))) 
		AND ISNULL(M.UpdatedDate,M.InsertedDate)>D.LastSyncDate
		AND TargetedDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = M.UniqAgency AND ES.UniqBranch = M.UniqBranch
				AND ES.UniqDepartment = CASE WHEN M.UniqDepartment = -1 THEN ES.UniqDepartment ELSE M.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)
        
    END
		
		--Policy
		IF 	Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityPolicy_SP'
				   )
        BEGIN	
		/*
		; with cte 
		as 
		(
		Select   C.UniqEntity
		FROM HIBOPPolicy p WITH(NOLOCK)
		INNER JOIN HIBOPPolicyLineType PL WITH(NOLOCK) ON P.UniqCdPolicyLineType=PL.UniqCdPolicyLineType
		INNER JOIN HIBOPClient c WITH (NOLOCK) on P.UniqEntity=C.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityPolicy_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
								inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient c2 WHERE c2.uniqentity = c.uniqentity and  c2.EmployeeId=delta.EmployeeId )) 
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId))) 
		AND ISNULL(P.UpdatedDate, P.InsertedDate)>Delta.LastSyncDate
		AND P.ExpirationDate> DATEADD(mm,-18,@GetDate)
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END
				and Es.UniqEntity = Delta.EmployeeId)
		)
		*/

		Update d 
		set d.IsDeltaFlag = 1,
			d.UpdatedDate = @GetDate
		FROM HIBOPPolicy p WITH(NOLOCK)
		INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityPolicy_SP' AND D.IsDeltaFlag = 0
		AND ISNULL(P.UpdatedDate, P.InsertedDate) > D.LastSyncDate
		INNER JOIN HIBOPPolicyLineType PL WITH(NOLOCK) ON P.UniqCdPolicyLineType=PL.UniqCdPolicyLineType
		AND P.ExpirationDate> DATEADD(mm,-18,getdate())--code added for diskio issue
		INNER JOIN HIBOPClient c WITH (NOLOCK) on P.UniqEntity=C.UniqEntity
		--INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityPolicy_SP' AND D.IsDeltaFlag = 0
		--AND ISNULL(P.UpdatedDate, P.InsertedDate) > D.LastSyncDate--code added for diskio issue
		INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity AND cl.EmployeeId = D.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId))) 
		/*
		AND ISNULL(P.UpdatedDate, P.InsertedDate) > D.LastSyncDate
		AND P.ExpirationDate> DATEADD(mm,-18,getdate())
		*/--code commented for diskio issue
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = P.UniqAgency AND ES.UniqBranch = P.UniqBranch
				AND ES.UniqDepartment = CASE WHEN P.UniqDepartment = -1 THEN ES.UniqDepartment ELSE P.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)

		/*
		Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a (nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityPolicy_SP'
        and   IsDeltaFlag    =  0
        */
    END

		
		IF 	Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityServices_SP'
				   )	
        BEGIN	
        /*
        ; with cte 
		as 
		(
		Select C.UniqEntity	
		FROM HIBOPActivityServices S WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on s.UniqEntity=C.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityServices_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
								 inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)

		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and EE.uniqemployee= delta.EmployeeId ))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId)))
		AND ISNULL(S.UpdatedDate, S.InsertedDate)>Delta.LastSyncDate
		AND s.ExpirationDate> DATEADD(mm,-18,@GetDate)
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = S.UniqAgency AND ES.UniqBranch = S.UniqBranch
				AND ES.UniqDepartment = CASE WHEN S.UniqDepartment = -1 THEN ES.UniqDepartment ELSE S.UniqDepartment END
				and Es.UniqEntity = Delta.EmployeeId)
		)
				
				
	    Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetActivityServices_SP'
        and   IsDeltaFlag    =  0
		*/

		Update d 
		set d.IsDeltaFlag = 1,
		d.UpdatedDate = @GetDate
		FROM HIBOPActivityServices S WITH(NOLOCK)
		INNER JOIN HIBOPClient c WITH (NOLOCK) on s.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityServices_SP' AND D.IsDeltaFlag = 0
		INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #EntityEmployee AS EE WHERE EE.UniqEntity = C.UniqEntity and  EE.uniqemployee = D.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId)))
		AND ISNULL(S.UpdatedDate, S.InsertedDate)>D.LastSyncDate
		AND s.ExpirationDate> DATEADD(mm,-18,getdate())
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = S.UniqAgency AND ES.UniqBranch = S.UniqBranch
				AND ES.UniqDepartment = CASE WHEN S.UniqDepartment = -1 THEN ES.UniqDepartment ELSE S.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)
        
        END
		
		
			
		IF 	Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityTransaction_SP'
				   )	
        BEGIN
        		
        UPDATE a 
         SET a.IsDeltaFlag = 1,
			 a.UpdatedDate = @GetDate
         FROM  HIBOPUserDeltaSyncInfo a 
			   inner join 	
        (			
		Select   C.UniqEntity
		FROM HIBOPActivityTransaction T WITH(NOLOCK)
		INNER JOIN HIBOPClient C WITH(NOLOCK) ON T.UniqEntity=C.UniqEntity
		INNER JOIN HIBOPEntityEmployee E WITH(NOLOCK) ON C.UniqEntity=E.UniqEntity
		inner join (select * from #tmp where spname = 'HIBOPGetActivityTransaction_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
		WHERE E.UniqEmployee=Delta.EmployeeId
		AND ISNULL(T.UpdatedDate, T.InsertedDate)>Delta.LastSyncDate
		)b 
		ON(a.EmployeeId = b.UniqEntity)
		WHERE SpName         =  'HIBOPGetActivityTransaction_SP'
        and   IsDeltaFlag    =  0
		
        END
        
        IF 	Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetClientDetails_SP'
				   )	
        BEGIN	
        /*
        ; with cte 
        as 
        (
		Select  c.UniqEntity
		FROM EpicDMZSub.dbo.client c WITH (NOLOCK)
		inner join (select * from #tmp where spname = 'HIBOPGetClientDetails_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
					 inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		OUTER APPLY (SELECT top 1 UniqAgency FROM EpicDMZSub.dbo.clientagencybranchjt AS CAB WITH (NOLOCK) WHERE C.UniqEntity=CAB.UniqEntity) AS cl  
		INNER JOIN EpicDMZSub.dbo.Agency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
		LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
		LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
		LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
		LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode COLLATE Latin1_General_CI_AS
		WHERE c.uniqentity <> -1 AND ISNULL(C.UpdatedDate,C.InsertedDate) > Delta.LastSyncDate
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient c2 WHERE c2.uniqentity = c.uniqentity and delta.EmployeeId =  c2.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.flags & 8192 = 8192 
		OR EXISTS (SELECT 1
		FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
		WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee =  Delta.EmployeeId))) 
		) 
		
			
	    Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetClientDetails_SP'
        and   IsDeltaFlag    =  0
		*/
        
			Update d 
			set d.IsDeltaFlag = 1,
				d.UpdatedDate = @GetDate
			FROM EpicDMZSub.dbo.client c WITH (NOLOCK)
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetClientDetails_SP' AND D.IsDeltaFlag = 0
			AND ISNULL(C.UpdatedDate,C.InsertedDate) > D.LastSyncDate  
			INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			OUTER APPLY (SELECT top 1 UniqAgency FROM EpicDMZSub.dbo.clientagencybranchjt AS CAB WITH (NOLOCK) 
						 WHERE C.UniqEntity=CAB.UniqEntity) AS cl 
			INNER JOIN EpicDMZSub.dbo.Agency AS a WITH (NOLOCK) ON cl.UniqAgency=a.UniqAgency
			/*LEFT OUTER JOIN EpicDMZSUB.DBO.ContactName AS cntname WITH(NOLOCK) ON  cntname.UniqContactName =C.UniqContactNamePrimary
			LEFT OUTER JOIN EpicDMZSUB.DBO.contactaddress AS ca  WITH(NOLOCK) 
			ON ca.UniqEntity=C.UniqEntity AND ca.UniqContactAddress=C.UniqContactAddressAccount
			LEFT OUTER JOIN EpicDMZSUB.DBO.cdstate AS s WITH(NOLOCK) ON ca.CdStateCode=s.CdStateCode COLLATE Latin1_General_CI_AS
			LEFT OUTER JOIN EpicDMZSUB.DBO.CdCountry as cnt WITH(NOLOCK) ON cnt.CdCountryCode = ca.CdCountryCode 
			COLLATE Latin1_General_CI_AS*/ -- code commented for Diskio issue
			WHERE c.uniqentity <> -1 --AND ISNULL(C.UpdatedDate,C.InsertedDate) > D.LastSyncDate  
			AND (vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity and  cl.EmployeeId= D.EmployeeId))
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.flags & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId)))  

        END
        
        IF 	Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetClientEmployee_SP'
				   )
        BEGIN	
		/*
		; with cte
		as 
		(
		Select  c.UniqEntity
		FROM HIBOPClient c
		inner join (select * from #tmp where spname = 'HIBOPGetClientEmployee_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
			 inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		WHERE c.uniqentity <> -1
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient c2 WHERE c2.uniqentity = c.uniqentity and c2.EmployeeId=delta.EmployeeId ))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[STATUS] & 8192 = 8192 
		OR EXISTS (SELECT 1
				   FROM EpicDMZSub.dbo.EntityEmployeeJT eejt
				   WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId))) 
		)
				    
		Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEntity)
        where SpName         =  'HIBOPGetClientEmployee_SP'
        and   IsDeltaFlag    =  0
		*/

			Update d 
			set d.IsDeltaFlag = 1,
				d.UpdatedDate = @GetDate
			FROM HIBOPClient c
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetClientEmployee_SP' AND D.IsDeltaFlag = 0
			--code added for diskio issue starts here 
			and c.uniqentity <> -1
			and  ISNULL(C.UpdatedDate,C.InsertedDate) > D.LastSyncDate 
			--code added for diskio issue ends here
			INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			WHERE 
			/*c.uniqentity <> -1
			and  ISNULL(C.UpdatedDate,C.InsertedDate) > D.LastSyncDate  
			AND */--code commented for diskio issue
			(vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity and  cl.EmployeeId= D.EmployeeId))
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.[STATUS] & 8192 = 8192 
			OR EXISTS (SELECT 1
			FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)
			WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId))) 
        
        END			
        
         IF Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetEmployeeAgency_SP'
					)	   
        BEGIN
            Update a 
			set a.IsDeltaFlag = 1,
			    a.UpdatedDate = @GetDate
			from HIBOPUserDeltaSyncInfo a (nolock)
			inner join
            (
			Select E.LookupCode
			FROM HIBOPEmployeeStructure EA WITH(NOLOCK)
			INNER JOIN HIBOPEmployee E WITH(NOLOCK) ON EA.UniqEntity=E.UniqEntity
			INNER JOIN HIBOPAgency A WITH(NOLOCK) ON EA.UniqAgency=A.UniqAgency
			INNER JOIN HIBOPBranch B WITH(NOLOCK) ON EA.UniqBranch=B.UniqBranch
			INNER JOIN HIBOPDepartment D WITH(NOLOCK) ON EA.UniqDepartment=D.UniqDepartment
			INNER JOIN HIBOPProfitCenter P WITH(NOLOCK) ON EA.UniqProfitCenter=P.UniqProfitCenter
			inner join (select * from #tmp where spname = 'HIBOPGetEmployeeAgency_SP') delta 
								on(E.LookupCode = Delta.UserLookupCode)
			WHERE E.LookupCode=delta.UserLookupCode --AND ISNULL(EA.UpdatedDate,EA.InsertedDate)>@LastSyncDate
			)b
			on( a.UserLookupCode= b.LookupCode)
			where    SpName         =  'HIBOPGetEmployeeAgency_SP'
			and   IsDeltaFlag    =  0
	
        END	
        
		
	     IF Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityDetails_SP'
				   )	
        BEGIN
       
			Update d 
			set d.IsDeltaFlag = 1,
				d.UpdatedDate = @GetDate
			FROM HIBOPActivity A WITH(NOLOCK)
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityDetails_SP' AND D.IsDeltaFlag = 0
			AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > D.LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > D.LastSyncDate)
			INNER JOIN HIBOPActivityCode c WITH (NOLOCK) ON a.uniqactivitycode = c.uniqactivitycode 
			--code added for diskio issue starts here 
			and a.status & 16 = 0 
			and (A.ClosedDate is null or A.ClosedDate> DATEADD(mm,-18,@GetDate))
			--code added for diskio issue ends here 
			INNER JOIN EpicDMZSub.dbo.company ic WITH (NOLOCK) ON a.uniqentitycompanyissuing = ic.uniqentity 
			INNER JOIN EpicDMZSub.dbo.company pc WITH (NOLOCK) ON a.uniqentitycompanybilling = pc.uniqentity 
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON O.UniqEntity = A.UniqEmployee
			--INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityDetails_SP' AND D.IsDeltaFlag = 0
			--AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > D.LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > D.LastSyncDate)
			--INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			INNER JOIN #EntityEmployee EE WITH(NOLOCK) ON EE.UniqEntity = A.UniqEntity and  EE.uniqemployee = D.EmployeeId
			--LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType = p.UniqCdPolicyLineType--code commented for diskio issue
			--INNER JOIN broker  pb ON a.uniqentitybrokerbilling = pb.uniqentity 
			WHERE /*a.status & 16 = 0 
			and (A.ClosedDate is null or A.ClosedDate> DATEADD(mm,-18,@GetDate))
			AND */--code commented for diskio issue
			EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)
			--AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > D.LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > D.LastSyncDate)----code commented for diskio issue
		END	
        
		
		
	    IF Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityEmployee_SP'
				   )
        BEGIN
		/*
        ;with cte 
        as 
        (
		Select  a.UniqEmployee
		FROM HIBOPActivity A WITH(NOLOCK)
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON A.UniqEmployee =O.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType=p.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			inner join (select * from #tmp where spname = 'HIBOPGetActivityEmployee_SP') delta 
								on(c.UniqEntity = Delta.EmployeeId)
								 inner join Vw_SecurityUser vwsu on( vwsu.uniqemployee = Delta.EmployeeId)
		WHERE c.uniqentity <> -1 
		AND (vwsu.iCheckStructure = 0 
		OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity and delta.EmployeeId =  c1.EmployeeId))
		AND (vwsu.iCheckEmployeeAccess = 0 
		OR (c.[Status] & 8192 = 8192 
		OR EXISTS (SELECT 1	FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = Delta.EmployeeId)))  
		AND  ISNULL(A.UpdatedDate, A.InsertedDate)>Delta.LastSyncDate
		and a.UniqAgency<>-1 and a.UniqBranch <>-1 
		and (ClosedDate is null or  ClosedDate> DATEADD(mm,-18,@GetDate)) 
		AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
		AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END
		and Es.UniqEntity = Delta.EmployeeId)
		)
		
		 Update a 
		set a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate
		from HIBOPUserDeltaSyncInfo a(nolock)
			 inner join 
			 cte
		on (EmployeeId = UniqEmployee)
        where SpName         =  'HIBOPGetActivityEmployee_SP'
        and   IsDeltaFlag    =  0
		*/

			Update d 
			set d.IsDeltaFlag = 1,
			d.UpdatedDate = @GetDate
			FROM HIBOPActivity A WITH(NOLOCK)
			INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityEmployee_SP' AND D.IsDeltaFlag = 0
			AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > D.LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > D.LastSyncDate)
			INNER JOIN HIBOPActvityOwnerList O WITH(NOLOCK) ON A.UniqEmployee =O.UniqEntity
			LEFT OUTER JOIN HIBOPPolicyLineType P WITH(NOLOCK)	ON A.UniqCdPolicyLineType=p.UniqCdPolicyLineType
			INNER JOIN HIBOPClient c WITH (NOLOCK) on A.UniqEntity=C.UniqEntity
			--INNER JOIN HIBOPUserDeltaSyncInfo D ON D.SpName = 'HIBOPGetActivityEmployee_SP' AND D.IsDeltaFlag = 0
			INNER JOIN Vw_SecurityUser vwsu on vwsu.uniqemployee = D.EmployeeId
			WHERE c.uniqentity <> -1 
			AND (vwsu.iCheckStructure = 0 
			OR EXISTS (SELECT 1 FROM #SecurityUserClient cl WHERE cl.uniqentity = c.uniqentity AND cl.EmployeeId = D.EmployeeId))
			AND (vwsu.iCheckEmployeeAccess = 0 
			OR (c.[Status] & 8192 = 8192 
			OR EXISTS (SELECT 1	FROM EpicDMZSub.dbo.EntityEmployeeJT eejt WITH (NOLOCK)	WHERE eejt.UniqEntity = c.UniqEntity AND eejt.UniqEmployee = D.EmployeeId)))  
			--AND  ISNULL(A.UpdatedDate, A.InsertedDate)> D.LastSyncDate
			--AND  (ISNULL(A.UpdatedDate, A.InsertedDate) > D.LastSyncDate OR ISNULL(A.ClosedDate, A.InsertedDate) > D.LastSyncDate)
			and a.UniqAgency<>-1 and a.UniqBranch <>-1 
			and (ClosedDate is null or  ClosedDate> DATEADD(mm,-18,getdate())) 
			AND EXISTS(Select 1 From #HIBOPEmployeeStructure ES WITH (NOLOCK) Where ES.UniqAgency = A.UniqAgency AND ES.UniqBranch = A.UniqBranch
				AND ES.UniqDepartment = CASE WHEN A.UniqDepartment = -1 THEN ES.UniqDepartment ELSE A.UniqDepartment END
				AND Es.UniqEntity = D.EmployeeId)

		END
		
		--where condition employee is not there
	    IF Exists (Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetActivityOwnerList_SP'
				   )
        BEGIN
		
		;with cte 
			as 
			(
				select a.UserLookupCode
				from (  select *
						from HIBOPUserDeltaSyncInfo (nolock)
						where    SpName         =  'HIBOPGetActivityOwnerList_SP'
						and   IsDeltaFlag = 0
					 ) a 
					 inner join 
					HIBOPEmployee b
				on( ISNULL(b.UpdatedDate,b.InsertedDate)>a.LastSyncDate)
			)
		   Update a 
		   set  a.IsDeltaFlag = 1,
			    a.UpdatedDate = @GetDate --getdate()
		   from HIBOPUserDeltaSyncInfo a(nolock)
			    inner join 
				cte b 
			on(a.UserLookupCode = b.UserLookupCode)
			where    a.SpName      =  'HIBOPGetActivityOwnerList_SP'
			and      a.IsDeltaFlag = 0
		
		END
		
		
		if exists ( Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetCommonLookUp_SP'
					)
		Begin
			;with cte 
			as 
			(
				select a.IPAddress,a.UserLookupCode
				from (  select *
						from HIBOPUserDeltaSyncInfo (nolock)
						where    SpName         =  'HIBOPGetCommonLookUp_SP'
						and   IsDeltaFlag = 0
					 ) a 
					 inner join 
					(Select  *
					FROM HIBOPCommonLookup F WITH(NOLOCK)
					WHERE IsDeleted=1
					)b
				on( ISNULL(b.ModifiedDate, b.CreatedDate)>a.LastSyncDate)
			)
			--select * from cte
		   Update a 
		   set  a.IsDeltaFlag = 1,
			    a.UpdatedDate = @GetDate --getdate()
		   from HIBOPUserDeltaSyncInfo a(nolock)
			    inner join 
				cte b 
			on( a.UserLookupCode = b.UserLookupCode
			and a.IPAddress      = b.IPAddress
			  )
			where    a.SpName      =  'HIBOPGetCommonLookUp_SP'
			and      a.IsDeltaFlag = 0
		end		

		if exists ( Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetFolders_SP'
					)
		Begin

		;with cte 
			as 
			(
				select a.UserLookupCode
				from (  select *
						from HIBOPUserDeltaSyncInfo (nolock)
						where    SpName         =  'HIBOPGetFolders_SP'
						and   IsDeltaFlag = 0
					 ) a 
					 inner join 
					 HIBOPFolderAttachment b WITH(NOLOCK)
				on( ISNULL(b.UpdatedDate, b.InsertedDate)>a.LastSyncDate)
			)

		   Update a 
		   set  a.IsDeltaFlag = 1,
			    a.UpdatedDate = @GetDate --getdate()
		   from HIBOPUserDeltaSyncInfo a(nolock)
			    inner join 
				cte b 
			on(a.UserLookupCode = b.UserLookupCode)
			where    a.SpName      =  'HIBOPGetFolders_SP'
			and      a.IsDeltaFlag = 0
			/*
			Update a set
			a.IsDeltaFlag = 1,
			a.UpdatedDate = @GetDate --getdate()
			from HIBOPUserDeltaSyncInfo a(nolock)
			inner join HIBOPFolderAttachment b WITH(NOLOCK)
			on		ISNULL(b.UpdatedDate, b.InsertedDate) > a.LastSyncDate
			AND      a.SpName      =  'HIBOPGetFolders_SP'
			and      a.IsDeltaFlag = 0
			*/
        End

		if exists ( Select 'X' 
					from #tmp 
					where spname = 'HIBOPGetPolicyLineType_SP'
					)
		Begin

		;with cte 
			as 
			(
				select a.UserLookupCode
				from (  select *
						from HIBOPUserDeltaSyncInfo (nolock)
						where    SpName         =  'HIBOPGetPolicyLineType_SP'
						and   IsDeltaFlag = 0
					 ) a 
					 inner join 
					 HIBOPPolicyLineType b WITH(NOLOCK)
				on (ISNULL(b.UpdatedDate,b.InsertedDate)>a.LastSyncDate)
			)

		   Update a 
		   set  a.IsDeltaFlag = 1,
			    a.UpdatedDate = @GetDate --getdate()
		   from HIBOPUserDeltaSyncInfo a(nolock)
			    inner join 
				cte b 
			on(a.UserLookupCode = b.UserLookupCode)
			where    a.SpName      =  'HIBOPGetPolicyLineType_SP'
			and      a.IsDeltaFlag = 0

        End									  
		
	  Drop table #tmp

END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For Error MSG : '+ERROR_MESSAGE()

     END CATCH 

SET NOCOUNT OFF
END





GO



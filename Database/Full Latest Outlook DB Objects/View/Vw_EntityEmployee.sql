/****** Object:  View [dbo].[Vw_EntityEmployee]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Vw_EntityEmployee]'))
EXEC dbo.sp_executesql @statement = N'Create View [dbo].[Vw_EntityEmployee]
As
	SELECT su.uniqemployee,cl.uniqentity 
	FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
	INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) 
	on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
	INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK) 
	on sus.uniqstructure = sc.uniqstructure
	INNER JOIN EpicDMZSub.DBO.securityuser su (NOLOCK) 
	On su.uniqsecurityuser = sus.UniqSecurityUser
	-- And su.uniqemployee = @UniqEmployee
' 
GO

/****** Object:  View [dbo].[Vw_SecurityUserClient]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Vw_SecurityUserClient]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[Vw_SecurityUserClient]
as 
select b.uniqsecurityuser,b.uniqentity
from
(
Select a.uniqsecurityuser,a.uniqentity ,row_number() over (partition by a.uniqsecurityuser,a.UniqEntity order by a.UniqEntity asc) id 
from (
SELECT sus.uniqsecurityuser,cl.UniqEntity--,row_number() over (partition by cl.UniqEntity order by c1.UniqEntity asc) id 
FROM HIBOPClientAgencyBranch cl WITH (NOLOCK)  
	INNER JOIN EpicDMZSub.dbo.structurecombination sc WITH (NOLOCK) 
	on sc.uniqagency = cl.uniqagency AND sc.uniqbranch = cl.uniqbranch
	INNER JOIN EpicDMZSub.dbo.securityuserstructurecombinationjt sus WITH (NOLOCK)
	 on sus.uniqstructure = sc.uniqstructure
	--WHERE sus.uniqsecurityuser = @securityuserid
	)a
)b
where b.id  =1 


	
	

' 
GO

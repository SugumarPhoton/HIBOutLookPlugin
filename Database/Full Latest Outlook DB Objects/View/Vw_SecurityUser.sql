/****** Object:  View [dbo].[Vw_SecurityUser]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Vw_SecurityUser]'))
EXEC dbo.sp_executesql @statement = N'Create VIEW [dbo].[Vw_SecurityUser]
AS 
	SELECT  Emp.LookUpCode,su.uniqemployee,
		    su.uniqsecurityuser,
			CASE WHEN su.typecode IN (''E'',''I'') THEN 0   -- enterprise user, system user
			WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 1 THEN 0  -- SecurityUser.Grant
			WHEN CAST(SUBSTRING(su.programaccess, 431, 1) AS TINYINT) = 3 THEN 1  -- SecurityUser.Deny
			WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.securitygroupsecurityuserjt jt WITH (NOLOCK)
			INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup -- SecurityGroup.Grant
			WHERE jt.uniqsecurityuser = su.uniqsecurityuser
			AND CAST(SUBSTRING(sg.programaccess, 431, 1) AS TINYINT) = 1) THEN 0
			ELSE 1 END iCheckStructure,
			CASE WHEN su.typecode IN (''E'',''I'') THEN 0
			WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 1 THEN 0
			WHEN CAST(SUBSTRING(su.programaccess, 1467, 1) AS TINYINT) = 3 THEN 1
			WHEN EXISTS (SELECT 1 FROM EpicDMZSub.DBO.SecurityGroupSecurityUserJT jt WITH (NOLOCK)
			INNER JOIN EpicDMZSub.DBO.securitygroup sg WITH (NOLOCK) ON jt.uniqsecuritygroup = sg.uniqsecuritygroup 
			WHERE jt.uniqsecurityuser = su.uniqsecurityuser
			AND CAST(SUBSTRING(sg.programaccess, 1467, 1) AS TINYINT) = 1) THEN 0
			ELSE 1 END iCheckEmployeeAccess
	FROM EpicDMZSub.DBO.securityuser su (NOLOCK)
		 inner join 
		 dbo.HIBOPEmployee Emp(NOLOCK)
      on (su.uniqemployee = Emp.UniqEntity)
' 
GO

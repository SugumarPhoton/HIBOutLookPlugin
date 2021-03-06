/****** Object:  View [dbo].[Vw_Market]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Vw_Market]'))
EXEC dbo.sp_executesql @statement = N'	Create view [dbo].[Vw_Market]
	As
	SELECT DISTINCT m.UniqMarketingSubmission,
			STUFF((SELECT DISTINCT '','' + p.CdPolicyLineTypeCode
			FROM HIBOPActivityMasterMarketing m1 WITH(NOLOCK)
			INNER JOIN HIBOPPolicyLineType p WITH(NOLOCK)on m1.uniqcdpolicylinetype=p.uniqcdpolicylinetype
			WHERE m.UniqMarketingSubmission = m1.UniqMarketingSubmission
			FOR XML PATH(''''), TYPE
			).value(''.'', ''NVARCHAR(MAX)'')
			,1,1,'''') LineCode
	FROM HIBOPActivityMasterMarketing m WITH(NOLOCK);
	
' 
GO

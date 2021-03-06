/****** Object:  StoredProcedure [dbo].[HIBOPGetActivityOwnerList_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPGetActivityOwnerList_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPGetActivityOwnerList_SP] AS' 
END
GO
/*
Declare @rowcount int 
EXEC [HIBOPGetActivityOwnerList_SP] 'VIDJA1',null,'1.1.1.1',30000,1,@rowcount= @rowcount
select @rowcount
*/
/*
Declare @rowcount int 
EXEC [HIBOPGetActivityOwnerList_SP] 'LEUNA1','2018-08-14 23:56:29.489','1.1.1.1',30000,1,@rowcount= @rowcount
select @rowcount
*/

ALTER PROCEDURE [dbo].[HIBOPGetActivityOwnerList_SP]
(@User VARCHAR(10),
@LastSynDate			DATETIME,
@IPAddress Varchar(100),
@RowsPerPage			INT , 
@PageNumber				INT ,
@RowCount				BIGINT OUTPUT
) 
AS 
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

	Declare @DeltaSyncdate datetime = GETUTCDATE()--dateadd(HH,-8,getdate())

	IF @LastSynDate IS NULL
	BEGIN
		SET @LastSynDate='1900-01-01'
	END

	IF @LastSynDate IS NOT NULL AND @LastSynDate >= '1900-01-01'
	BEGIN
		SELECT
		DISTINCT E.Lookupcode,E.EmployeeName
		FROM HIBOPEmployee E WITH(NOLOCK)
		WHERE ISNULL(UpdatedDate, InsertedDate) > @LastSynDate
		ORDER BY Lookupcode
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY

		SELECT @RowCount = COUNT(*)
		FROM HIBOPEmployee E WITH(NOLOCK)
		WHERE ISNULL(UpdatedDate, InsertedDate) > @LastSynDate
	END
	ELSE
	BEGIN
		SELECT
		DISTINCT E.Lookupcode,E.EmployeeName
		FROM HIBOPEmployee E WITH(NOLOCK)
		ORDER BY Lookupcode
		OFFSET (@PageNumber-1)*@RowsPerPage ROWS
		FETCH NEXT @RowsPerPage ROWS ONLY

		SELECT @RowCount = COUNT(*)
		FROM HIBOPEmployee E WITH(NOLOCK)
	END

	Exec [dbo].[HIBOPUserDeltaSyncUpdate_SP] @User,@IPAddress,'HIBOPGetActivityOwnerList_SP',@DeltaSyncdate

	END TRY

	BEGIN CATCH 
		SELECT 'Select Failed For HIBOPGetActivityOwnerList_SP Error MSG : '+ERROR_MESSAGE()
	END CATCH 

	SET NOCOUNT OFF
END

GO

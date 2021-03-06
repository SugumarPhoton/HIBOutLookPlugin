/****** Object:  StoredProcedure [dbo].[HIBOPClearLogData_SP]    Script Date: 2/7/2019 6:42:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HIBOPClearLogData_SP]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[HIBOPClearLogData_SP] AS' 
END
GO
ALTER PROCEDURE [dbo].[HIBOPClearLogData_SP]
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE
	@OlderLogdate	DATETIME =  DATEADD(DAY, -15, GETDATE()) ,
	@get_date Datetime = getdate()


	DELETE FROM HIBOPErrorLog			WHERE LogDate		< @OlderLogdate
	DELETE FROM HIBOPErrorLog_bk			WHERE LogDate		< @OlderLogdate
	DELETE FROM HIBOPOutlookPluginLog	WHERE InsertedDate	< @OlderLogdate

	Delete From HIBOPGetClientEmployeeTemp where  EntryDate < dateadd(Mi,-5,@get_date)
	Delete From HIBOPGetActivityLineTemp where  EntryDate < dateadd(Mi,-5,@get_date)
	Delete From HIBOPGetClientDetailsTemp where  EntryDate < dateadd(Mi,-5,@get_date)

	--Delete from HIBOPEmployeeStructure_Temp  where  EntryDate < dateadd(Mi,-5,@get_date)
	--Delete from SecurityUserClient_Temp where  EntryDate < dateadd(Mi,-5,@get_date)
	--Delete from EntityEmployee_Temp where  EntryDate < dateadd(Mi,-5,@get_date)
	--Delete from HIBOPGetActivityDetails_Temp where  EntryDate < dateadd(Mi,-5,@get_date)
	--Delete from Market_Temp where  EntryDate < dateadd(Mi,-5,@get_date)
	--Delete from HIBOPActivityPolicy_Temp where  EntryDate < dateadd(Mi,-5,@get_date)


	Insert into Monitor_Disk(DBName,DBType,SpaceOccupiedinGB)
	select d.name DBName, case when m.type = 0 then 'Data' else 'Log' end DBType,  Round((m.size * 8.00 / 1024.00)/ 1024.00,2) As SpaceOccupiedinGB
	from sys.master_files m JOIN sys.databases d ON d.database_id = m.database_id
	Order By 2 Desc,3 Desc


	insert into Monitor_Drive(Drive,MbFree)
	EXEC master..xp_fixeddrives

	update Monitor_Drive
	set IN_TB = Round((MbFree / 1024.00)/ 1024.00,2),
	 IN_GB =Round((MbFree / 1024.00),2)

	SET NOCOUNT OFF
END


GO

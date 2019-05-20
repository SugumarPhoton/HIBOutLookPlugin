--[dbo].[HIBOPGetDeltaSync_SP] 'LEUNA1','165.165.165.12',null,1
alter  PROCEDURE [dbo].[HIBOPGetDeltaSync_SP] --'LEUNA1'
(@User VARCHAR(10),
 @IPAddress Varchar(100),
  @Is_Client bit,
  @Is_FirstSync bit
)
AS
BEGIN
SET NOCOUNT ON
BEGIN TRY

	DECLARE @UniqEmployee INT 
	
    SELECT @UniqEmployee = UniqEntity FROM HIBOPEmployee WITH (NOLOCK) WHERE LookUpCode = @User

If not exists (Select 'X'
			   from HIBOPUserDeltaSyncInfo(nolock)
			   where UserLookupCode = @User
			   and   IPAddress      = @IPAddress
			   )		
begin

Declare @Sp_called Table (SpName varchar(255),IsDeltaFlag bit)

insert into @Sp_called(SpName)
select 'HIBOPGetActivityAccount_SP'
union all 
select 'HIBOPGetActivityBill_SP'
union all
select 'HIBOPGetActivityCarrierSubmission_SP'
union all
select 'HIBOPGetActivityCertificate_SP'
union all
select 'HIBOPGetActivityClaim_SP'
union all
select 'HIBOPGetActivityClientContacts_SP'
union all
select 'HIBOPGetActivityEvidence_SP'
union all
select 'HIBOPGetActivityLine_SP'
union all
select 'HIBOPGetActivityList_SP'
union all
select 'HIBOPGetActivityMarketing_SP'
union all
select 'HIBOPGetActivityOpportunity_SP'
union all
select 'HIBOPGetActivityPolicy_SP'
union all
select 'HIBOPGetActivityServices_SP'
union all
select 'HIBOPGetActivityTransaction_SP'
union all
select 'HIBOPGetClientDetails_SP'
union all
select 'HIBOPGetClientEmployee_SP'
union all
select 'HIBOPGetEmployeeAgency_SP'
union all
select 'HIBOPGetActivityDetails_SP'
union all
select 'HIBOPGetActivityEmployee_SP'
union all
select 'HIBOPGetActivityOwnerList_SP'
union all
select 'HIBOPGetCommonLookUp_SP'
union all
select 'HIBOPGetFolders_SP'
union all
select 'HIBOPGetPolicyLineType_SP'


   

insert into HIBOPUserDeltaSyncInfo(IPAddress,EmployeeId,UserLookupCode,SpName,LastSyncDate,IsDeltaFlag)
select @IPAddress,@UniqEmployee,@User,SpName,NULL LastSyncDate,1 IsDeltaFlag from @Sp_called

   if @Is_Client is null 
   begin 
   select IPAddress,UserLookupCode,SpName,LastSyncDate,IsDeltaFlag
   from HIBOPUserDeltaSyncInfo(nolock)
   where UserLookupCode = @User
   and   IPAddress      = @IPAddress
   and   IsDeltaFlag    = 1
   end


end
Else 
begin

   IF @Is_FirstSync = 1 
   BEGIN
      Update a 
	  set a.IsDeltaFlag  = 1, 
		  a.LastSyncDate = NULL,
		  a.UpdatedDate  = NULL
	  from HIBOPUserDeltaSyncInfo a (nolock)
	  where UserLookupCode = @User
      and   IPAddress      = @IPAddress

   END

   if @Is_Client = 1 
   begin 
	   select IPAddress,UserLookupCode,SpName,LastSyncDate,IsDeltaFlag
	   from HIBOPUserDeltaSyncInfo(nolock)
	   where UserLookupCode = @User
	   and   IPAddress      = @IPAddress
	   and   IsDeltaFlag    = 1
	   and   Spname in ('HIBOPGetClientDetails_SP','HIBOPGetClientEmployee_SP')
   end 
   else if @Is_Client = 0
   begin
       select IPAddress,UserLookupCode,SpName,LastSyncDate,IsDeltaFlag
	   from HIBOPUserDeltaSyncInfo(nolock)
	   where UserLookupCode = @User
	   and   IPAddress      = @IPAddress
	   and   IsDeltaFlag    = 1
	   and   Spname not in ('HIBOPGetClientDetails_SP','HIBOPGetClientEmployee_SP')
   end
   else 
   begin
       select IPAddress,UserLookupCode,SpName,LastSyncDate,IsDeltaFlag
	   from HIBOPUserDeltaSyncInfo(nolock)
	   where UserLookupCode = @User
	   and   IPAddress      = @IPAddress
	   and   IsDeltaFlag    = 1
   end
 
   end

--select * from HIBOPUserDeltaSyncInfo(nolock)
	
END TRY

	BEGIN CATCH
        
	SELECT 'Select Failed For Error MSG : '+ERROR_MESSAGE()

     END CATCH 

SET NOCOUNT OFF
END


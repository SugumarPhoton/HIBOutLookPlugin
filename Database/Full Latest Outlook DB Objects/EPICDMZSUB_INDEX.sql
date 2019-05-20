USE [EpicDMZSub]

--This index is created for HIBOPEpicSyncClient_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_TransDetail_InsertedDate')
BEGIN
	DROP INDEX TransDetail.IX_TransDetail_InsertedDate;
END
GO
CREATE NONCLUSTERED INDEX [IX_TransDetail_InsertedDate] 
ON [dbo].[TransDetail] ([InsertedDate])
INCLUDE ([UniqTransHead],[TransDetailNumber])
GO

--This index is created for HIBOPEpicSyncClient_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_TransDetail_UpdatedDate')
BEGIN
	DROP INDEX TransDetail.IX_TransDetail_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX [IX_TransDetail_UpdatedDate] 
ON [dbo].[TransDetail] ([UpdatedDate])
INCLUDE ([UniqTransHead],[TransDetailNumber])
GO

--This index is for HIBOPGetClientDetails_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_securityuser_uniqemployee')
BEGIN
	DROP INDEX Securityuser.IX_securityuser_uniqemployee;
END
GO
Create nonclustered index [IX_securityuser_uniqemployee] on [dbo].[Securityuser] ([uniqemployee])
GO



--This index is for HIBOPEpicSyncPolicyLineType_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_EntityEmployeeJT_UniqEmployee')
BEGIN
	DROP INDEX EntityEmployeeJT.IX_EntityEmployeeJT_UniqEmployee;
END
GO
Create nonclustered index [IX_EntityEmployeeJT_UniqEmployee] on [EntityEmployeeJT] ([UniqEmployee])
INCLUDE ([UniqEntity])
GO


--This index is for HIBOPEpicSyncClient_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Activity_InsertedDate')
BEGIN
	DROP INDEX Activity.IX_Activity_InsertedDate;
END
GO
CREATE NONCLUSTERED INDEX [IX_Activity_InsertedDate] 
ON [dbo].[Activity] ([InsertedDate])
GO


--This index is for HIBOPEpicSyncClient_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Activity_UpdatedDate')
BEGIN
	DROP INDEX Activity.IX_Activity_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX [IX_Activity_UpdatedDate] 
ON [dbo].[Activity] ([UpdatedDate])
GO

--This index is for HIBOPEpicSyncClient_SP
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Activity_ClosedDate')
BEGIN
	DROP INDEX Activity.IX_Activity_ClosedDate;
END
GO
CREATE NONCLUSTERED INDEX [IX_Activity_ClosedDate] 
ON [dbo].[Activity] ([ClosedDate])
GO



--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in [Client]
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Client_InactiveDate_InsertedDate')
BEGIN
	DROP INDEX Client.IX_Client_InactiveDate_InsertedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Client_InactiveDate_InsertedDate ON [dbo].[Client] ([InsertedDate],[InactiveDate])
GO

IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Client_InactiveDate_UpdatedDate')
BEGIN
	DROP INDEX Client.IX_Client_InactiveDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Client_InactiveDate_UpdatedDate ON [dbo].[Client] (UpdatedDate)
GO

--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in ActivityCode
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_ActivityCode_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX ActivityCode.IX_ActivityCode_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_ActivityCode_InsertedDate_UpdatedDate ON [dbo].ActivityCode (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in Claim
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Claim_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Claim.IX_Claim_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Claim_InsertedDate_UpdatedDate ON [dbo].Claim ([InsertedDate],[UpdatedDate])
GO


--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in Policy
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Policy_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Policy.IX_Policy_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Policy_InsertedDate_UpdatedDate ON [dbo].Policy (InsertedDate,UpdatedDate)
GO



--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in MarketingSubmission
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_MarketingSubmission_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX MarketingSubmission.IX_MarketingSubmission_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_MarketingSubmission_InsertedDate_UpdatedDate ON [dbo].MarketingSubmission (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in Opportunity
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Opportunity_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Opportunity.IX_Opportunity_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Opportunity_InsertedDate_UpdatedDate ON [dbo].Opportunity (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in Line
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Line_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Line.IX_Line_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Line_InsertedDate_UpdatedDate ON [dbo].Line (InsertedDate,UpdatedDate)
GO



--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in CarrierSubmission
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_CarrierSubmission_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX CarrierSubmission.IX_CarrierSubmission_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_CarrierSubmission_InsertedDate_UpdatedDate ON [dbo].CarrierSubmission (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in Employee
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Employee_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Employee.IX_Employee_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Employee_InsertedDate_UpdatedDate ON [dbo].Employee (InsertedDate,UpdatedDate)
GO


/*
--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in structurecombination
CREATE NONCLUSTERED INDEX IX_structurecombination_InsertedDate_UpdatedDate
ON [dbo].structurecombination (InsertedDate,UpdatedDate)
GO
--drop index IX_structurecombination_InsertedDate_UpdatedDate on structurecombination
*/


--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in Department
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Department_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Department.IX_Department_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Department_InsertedDate_UpdatedDate ON [dbo].Department (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in ProfitCenter
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_ProfitCenter_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX ProfitCenter.IX_ProfitCenter_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_ProfitCenter_InsertedDate_UpdatedDate ON [dbo].ProfitCenter (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in Evidence
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Evidence_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Evidence.IX_Evidence_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Evidence_InsertedDate_UpdatedDate ON [dbo].Evidence (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncPolicyLineType_SP for DiskIO improvement in Certificate
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_Certificate_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX Certificate.IX_Certificate_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_Certificate_InsertedDate_UpdatedDate ON [dbo].Certificate (InsertedDate,UpdatedDate)
GO


--This index is created when walkthorugh HIBOPEpicSyncClient_SP for DiskIO improvement in ServiceHead
IF EXISTS ( SELECT 'X' FROM SYS.INDEXES WHERE NAME ='IX_ServiceHead_InsertedDate_UpdatedDate')
BEGIN
	DROP INDEX ServiceHead.IX_ServiceHead_InsertedDate_UpdatedDate;
END
GO
CREATE NONCLUSTERED INDEX IX_ServiceHead_InsertedDate_UpdatedDate ON [dbo].ServiceHead (InsertedDate,UpdatedDate)
GO









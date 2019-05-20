@echo off
cls
set /p SName=Server Name :
set /p UName=User Name :
set /p Pwd=Password :
set /p DbName=Database Name :
set /p Opath=Objects Path :
set /p choice=ARE YOU SURE TO EXECUTE SCRIPTS in %DbName% (y/n) ?
if '%choice%'=='y' goto begin
goto end
:begin
@echo on


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPClient.sql -o %Opath%\Error\Tables\HIBOPClient.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivity.sql -o %Opath%\Error\Tables\HIBOPActivity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\H1BOPComEPICSyncInfo.sql -o %Opath%\Error\Tables\H1BOPComEPICSyncInfo.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPAgency.sql -o %Opath%\Error\Tables\HIBOPAgency.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPBranch.sql -o %Opath%\Error\Tables\HIBOPBranch.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPDepartment.sql -o %Opath%\Error\Tables\HIBOPDepartment.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPProfitCenter.sql -o %Opath%\Error\Tables\HIBOPProfitCenter.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPEmployee.sql -o %Opath%\Error\Tables\HIBOPEmployee.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPEntityEmployee.sql -o %Opath%\Error\Tables\HIBOPEntityEmployee.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPEmployeeAgency.sql -o %Opath%\Error\Tables\HIBOPEmployeeAgency.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPClientAgencyBranch.sql -o %Opath%\Error\Tables\HIBOPClientAgencyBranch.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPCommonLookup.sql -o %Opath%\Error\Tables\HIBOPCommonLookup.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPOpportunityStage.sql -o %Opath%\Error\Tables\HIBOPOpportunityStage.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPPolicy.sql -o %Opath%\Error\Tables\HIBOPPolicy.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityCode.sql -o %Opath%\Error\Tables\HIBOPActivityCode.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPClaim.sql -o %Opath%\Error\Tables\HIBOPClaim.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityMasterMarketing.sql -o %Opath%\Error\Tables\HIBOPActivityMasterMarketing.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityOpportunity.sql -o %Opath%\Error\Tables\HIBOPActivityOpportunity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityServices.sql -o %Opath%\Error\Tables\HIBOPActivityServices.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityLine.sql -o %Opath%\Error\Tables\HIBOPActivityLine.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityClientContacts.sql -o %Opath%\Error\Tables\HIBOPActivityClientContacts.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPFolderAttachment.sql -o %Opath%\Error\Tables\HIBOPFolderAttachment.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityPolicyLineMapping.sql -o %Opath%\Error\Tables\HIBOPActivityPolicyLineMapping.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPPolicyLineType.sql -o %Opath%\Error\Tables\HIBOPPolicyLineType.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityLookupDetails.sql -o %Opath%\Error\Tables\HIBOPActivityLookupDetails.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPCarrierSubmission.sql -o %Opath%\Error\Tables\HIBOPCarrierSubmission.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityTransaction.sql -o %Opath%\Error\Tables\HIBOPActivityTransaction.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityBill.sql -o %Opath%\Error\Tables\HIBOPActivityBill.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityBinder.sql -o %Opath%\Error\Tables\HIBOPActivityBinder.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityCertificate.sql -o %Opath%\Error\Tables\HIBOPActivityCertificate.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityEvidenceOfInsurance.sql -o %Opath%\Error\Tables\HIBOPActivityEvidenceOfInsurance.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPFavourite.sql -o %Opath%\Error\Tables\HIBOPFavourite.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPOutlookPluginLog.sql -o %Opath%\Error\Tables\HIBOPOutlookPluginLog.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActivityPolicyTemp.sql -o %Opath%\Error\Tables\HIBOPActivityPolicyTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPGetActivityAccountTemp.sql -o %Opath%\Error\Tables\HIBOPGetActivityAccountTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPErrorLog.sql -o %Opath%\Error\Tables\HIBOPErrorLog.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPGetActivityLineTemp.sql -o %Opath%\Error\Tables\HIBOPGetActivityLineTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPGetClientDetailsTemp.sql -o %Opath%\Error\Tables\HIBOPGetClientDetailsTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPGetClientEmployeeTemp.sql -o %Opath%\Error\Tables\HIBOPGetClientEmployeeTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPEmployeeStructure.sql -o %Opath%\Error\Tables\HIBOPEmployeeStructure.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Tables\HIBOPActvityOwnerList.sql -o %Opath%\Error\Tables\HIBOPActvityOwnerList.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\UserType\HIBOPErrorLog_UT.sql -o %Opath%\Error\UserType\HIBOPErrorLog_UT.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\UserType\HIBOPFavourite_UT.sql -o %Opath%\Error\UserType\HIBOPFavourite_UT.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\UserType\HIBOPLogInfo_UT.sql -o %Opath%\Error\UserType\HIBOPLogInfo_UT.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPActivity_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPActivity_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPClient_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPClient_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPEmp_UniqEntity_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPEmp_UniqEntity_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\UniqCdPolicyLineType_PK.sql -o %Opath%\Error\Constraint\PK\UniqCdPolicyLineType_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPActivityOpportunity_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPActivityOpportunity_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPClaim_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPClaim_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPPolicy_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPPolicy_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPActivityLine_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPActivityLine_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPProfitCenter_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPProfitCenter_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPBranch_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPBranch_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPAgency_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPAgency_PK.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\PK\HIBOPFolderAttachment_PK.sql -o %Opath%\Error\Constraint\PK\HIBOPFolderAttachment_PK.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_ClientAgencyBranch_UniqAgency.sql -o %Opath%\Error\Constraint\Index\IX_ClientAgencyBranch_UniqAgency.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_ClientAgencyBranch_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_ClientAgencyBranch_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Services_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Services_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Carrier_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Carrier_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Transaction_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Transaction_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Bill_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Bill_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Evidence_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Evidence_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Certificate_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Certificate_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Line_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Line_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Marketing_UniqCdPolicyLineType.sql -o %Opath%\Error\Constraint\Index\IX_Marketing_UniqCdPolicyLineType.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Marketing_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Marketing_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Opportunity_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Opportunity_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Policy_UniqCdPolicyLineType.sql -o %Opath%\Error\Constraint\Index\IX_Policy_UniqCdPolicyLineType.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Policy_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Policy_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_UniqCdPolicyLineType.sql -o %Opath%\Error\Constraint\Index\IX_Activity_UniqCdPolicyLineType.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_UniqAssociatedItem.sql -o %Opath%\Error\Constraint\Index\IX_Activity_UniqAssociatedItem.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_UniqActivity.sql -o %Opath%\Error\Constraint\Index\IX_Activity_UniqActivity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Activity_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Client_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_Client_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_EntityEmployee_UniqEmployee.sql -o %Opath%\Error\Constraint\Index\IX_EntityEmployee_UniqEmployee.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_EntityEmployee_UniqEntity.sql -o %Opath%\Error\Constraint\Index\IX_EntityEmployee_UniqEntity.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\HIBOPEmployeeAgency_NDX1.sql -o %Opath%\Error\Constraint\Index\HIBOPEmployeeAgency_NDX1.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\HIBOPClientAgencyBranch_NDX1.sql -o %Opath%\Error\Constraint\Index\HIBOPClientAgencyBranch_NDX1.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_HIBOPGetActivityLineTemp.sql -o %Opath%\Error\Constraint\Index\IX_Activity_HIBOPGetActivityLineTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_HIBOPGetClientDetailsTemp.sql -o %Opath%\Error\Constraint\Index\IX_Activity_HIBOPGetClientDetailsTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_Activity_HIBOPGetClientEmployeeTemp.sql -o %Opath%\Error\Constraint\Index\IX_Activity_HIBOPGetClientEmployeeTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\IX_ActivityAccount_HIBOPGetActivityAccountTemp.sql -o %Opath%\Error\Constraint\Index\IX_ActivityAccount_HIBOPGetActivityAccountTemp.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\HIBOPActivityCode_IDX1.sql -o %Opath%\Error\Constraint\Index\HIBOPActivityCode_IDX1.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Constraint\Index\HIBOPActvityOwnerList_IDX1.sql -o %Opath%\Error\Constraint\Index\HIBOPActvityOwnerList_IDX1.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Metadata\CommonLookup.sql -o %Opath%\Error\Metadata\CommonLookup.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Metadata\OpportunityStage.sql -o %Opath%\Error\Metadata\OpportunityStage.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Function\HIBOPSplitString.sql -o %Opath%\Error\Function\HIBOPSplitString.err


sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityAccount_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityAccount_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityClaim_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityClaim_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityPolicy_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityPolicy_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityServices_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityServices_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityOpportunity_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityOpportunity_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityLine_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityLine_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityList_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityList_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityOwnerList_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityOwnerList_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetCommonLookUp_SP.sql -o %Opath%\Error\Procedure\HIBOPGetCommonLookUp_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityMarketing_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityMarketing_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityClientContacts_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityClientContacts_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncFavouriteToCentralized_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncFavouriteToCentralized_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetClientDetails_SP.sql -o %Opath%\Error\Procedure\HIBOPGetClientDetails_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPEpicSyncClient_SP.sql -o %Opath%\Error\Procedure\HIBOPEpicSyncClient_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityCarrierSubmission_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityCarrierSubmission_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityBill_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityBill_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityTransaction_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityTransaction_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetLogDetails_SP.sql -o %Opath%\Error\Procedure\HIBOPGetLogDetails_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPEpicSyncPolicyLineType_SP.sql -o %Opath%\Error\Procedure\HIBOPEpicSyncPolicyLineType_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityLookupDetails_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityLookupDetails_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetFolders_SP.sql -o %Opath%\Error\Procedure\HIBOPGetFolders_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetPolicyLineType_SP.sql -o %Opath%\Error\Procedure\HIBOPGetPolicyLineType_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncAuditLogToLocal_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncAuditLogToLocal_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncErrorLogToLocal_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncErrorLogToLocal_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetEmployeeAgency_SP.sql -o %Opath%\Error\Procedure\HIBOPGetEmployeeAgency_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetEmployee_SP.sql -o %Opath%\Error\Procedure\HIBOPGetEmployee_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncErrorLogToCenterlized_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncErrorLogToCenterlized_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncFavouriteToLocal_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncFavouriteToLocal_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityCertificate_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityCertificate_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityEvidence_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityEvidence_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetClientEmployee_SP.sql -o %Opath%\Error\Procedure\HIBOPGetClientEmployee_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPSyncAuditLogToCentralized_SP.sql -o %Opath%\Error\Procedure\HIBOPSyncAuditLogToCentralized_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityDetails_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityDetails_SP.err
sqlcmd -S %SName% -U %UName% -P %Pwd% -d %DbName% -I -i %Opath%\Procedure\HIBOPGetActivityEmployee_SP.sql -o %Opath%\Error\Procedure\HIBOPGetActivityEmployee_SP.err


setLocal EnableDelayedExpansion
for /f "tokens=* delims= " %%a in ('dir/s/b/a-d %Opath%') do if %%~za equ 0 del "%%a"

:end
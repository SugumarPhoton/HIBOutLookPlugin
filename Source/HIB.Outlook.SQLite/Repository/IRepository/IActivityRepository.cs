using HIB.Outlook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface IActivityRepository
    {
        ResultInfo SyncActivity(List<ActivityInfo> activityList);
        ResultInfo SyncActivityEmployees(List<ActivityEmployee> activityList);
        ResultInfo SyncActivityClaim(List<ActivityClaimInfo> activityClaimList);
        ResultInfo SyncActivityPolicy(List<ActivityPolicyInfo> activityPolicyList);
        ResultInfo SyncActivityOpportunity(List<ActivityOpportunityInfo> activityOpportunityList);
        ResultInfo SyncActivityMarketing(List<ActivityMarketingInfo> activityMarketingList);
        ResultInfo SyncActivityClientContact(List<ActivityClientContactInfo> activityClientContactList);
        ResultInfo SyncActivityCommonLookUp(List<ActivityCommonLookUpInfo> activityCommonLookUpList);
        ResultInfo SyncActivityService(List<ActivityServiceInfo> activityServiceList);
        ResultInfo SyncActivityLine(List<ActivityLineInfo> activityLineList);
        ResultInfo SyncActivityOwnerList(List<ActivityOwnerListInfo> activityOwnerList);
        ResultInfo SyncActivityList(List<ActivityListInfo> activityList);
        ResultInfo SyncActivityAccount(List<ActivityAccountInfo> activityAccountList);
        ResultInfo SyncActivityBill(List<ActivityBillInfo> activityBillList);
        ResultInfo SyncActivityTransaction(List<ActivityTransactionInfo> activityTransactionList);
        ResultInfo SyncActivityCertificate(List<ActivityCertificateInfo> activityCertificateList);
        ResultInfo SyncActivityEvidence(List<ActivityEvidenceInfo> activityEvidenceList);
        ResultInfo SyncActivityCarrierSubmission(List<ActivityCarrierInfo> activityCarrierList);
        ResultInfo SyncActivityLookUp(List<ActivityLookUpInfo> activityLookUpList);
        ResultInfo SyncActivityEmployeeAgency(List<EmployeeAgencyInfo> activityEmployeeAgencyList);
        ResultInfo SyncActivityEmployee(List<EmployeeInfo> activityEmployeeList);
        ResultInfo InsertEmployeeLookUpCode(List<string> lookUpList, bool Status);
        List<string> GetUserLookUpCode();

    }
}

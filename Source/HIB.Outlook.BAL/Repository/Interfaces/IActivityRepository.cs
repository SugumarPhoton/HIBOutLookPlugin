using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface IActivityRepository
    {
        ActivityDetail SyncActivities(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        ActivityEmployeeDetail SyncActivityEmployees(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<ActivityClaimInfo> SyncActivityClaims(string userId, DateTime? lastSyncDate, string ipAddress);
        ActivityPolicyDetail SyncActivityPolicies(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<ActivityServiceInfo> SyncActivityServices(string userId, DateTime? lastSyncDate, string ipAddress);
        ActivityLineDetail SyncActivityLines(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<ActivityOpportunityInfo> SyncActivityOpportunities(string userId, DateTime? lastSyncDate, string ipAddress);
        ActivityAccountDetail SyncActivityAccounts(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<ActivityMarketingInfo> SyncActivityMarketing(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityClientContactInfo> SyncActivityClientContacts(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityCommonLookUpInfo> SyncActivityCommonLookUp(string userId, DateTime? lastSyncDate, string ipAddress);
        ActivityOwnerListDetail SyncActivityOwnerList(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<ActivityListInfo> SyncActivityList(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityBillInfo> SyncActivityBills(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityCarrierInfo> SyncActivityCarriers(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityTransactionInfo> SyncActivityTransactions(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityCertificateInfo> SyncActivityCertificate(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityEvidenceInfo> SyncActivityEvidence(string userId, DateTime? lastSyncDate, string ipAddress);
        List<ActivityLookUpInfo> SyncActivityLookUps(string userId, DateTime? lastSyncDate);
        List<EmployeeAgencyInfo> SyncActivityEmployeeAgencies(string userId, DateTime? lastSyncDate, string ipAddress);
        List<EmployeeInfo> SyncActivityEmployee(string userId);
    }
}

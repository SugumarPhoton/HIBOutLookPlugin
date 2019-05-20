using System;
using System.Collections.Generic;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.Model;
using HIB.Outlook.BAL.Repository.Interfaces;

namespace HIB.Outlook.BAL.Managers
{
    public class Activities : IActivities
    {
        IActivityRepository _activityRepository;
        public Activities(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
        /// <summary>
        /// Get list of activity detail to sync local database
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>    
        public ActivityDetail SyncActivities(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivities(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public ActivityEmployeeDetail SyncActivityEmployees(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivityEmployees(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public List<ActivityClaimInfo> SyncActivityClaims(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityClaims(userId, lastSyncDate, ipAddress);
        }

        public ActivityPolicyDetail SyncActivityPolicies(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivityPolicies(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public List<ActivityServiceInfo> SyncActivityServices(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityServices(userId, lastSyncDate, ipAddress);
        }

        public ActivityLineDetail SyncActivityLines(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivityLines(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public List<ActivityOpportunityInfo> SyncActivityOpportunities(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityOpportunities(userId, lastSyncDate, ipAddress);
        }

        public ActivityAccountDetail SyncActivityAccounts(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivityAccounts(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public List<ActivityMarketingInfo> SyncActivityMarketing(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityMarketing(userId, lastSyncDate, ipAddress);
        }
        public List<ActivityClientContactInfo> SyncActivityClientContacts(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityClientContacts(userId, lastSyncDate, ipAddress);
        }
        public List<ActivityCommonLookUpInfo> SyncActivityCommonLookUp(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityCommonLookUp(userId, lastSyncDate, ipAddress);
        }
        public ActivityOwnerListDetail SyncActivityOwnerList(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _activityRepository.SyncActivityOwnerList(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }
        public List<ActivityListInfo> SyncActivityList(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityList(userId, lastSyncDate, ipAddress);
        }

        public List<ActivityBillInfo> SyncActivityBills(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityBills(userId, lastSyncDate, ipAddress);
        }
        public List<ActivityCarrierInfo> SyncActivityCarriers(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityCarriers(userId, lastSyncDate, ipAddress);
        }

        public List<ActivityTransactionInfo> SyncActivityTransactions(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityTransactions(userId, lastSyncDate, ipAddress);
        }

        public List<ActivityCertificateInfo> SyncActivityCertificate(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityCertificate(userId, lastSyncDate, ipAddress);
        }

        public List<ActivityEvidenceInfo> SyncActivityEvidence(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityEvidence(userId, lastSyncDate, ipAddress);
        }

        public List<ActivityLookUpInfo> SyncActivityLookUps(string userId, DateTime? lastSyncDate)
        {
            return _activityRepository.SyncActivityLookUps(userId, lastSyncDate);
        }
        public List<EmployeeAgencyInfo> SyncActivityEmployeeAgencies(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _activityRepository.SyncActivityEmployeeAgencies(userId, lastSyncDate, ipAddress);
        }
        public List<EmployeeInfo> SyncActivityEmployee(string userId)
        {
            return _activityRepository.SyncActivityEmployee(userId);
        }
    }

}
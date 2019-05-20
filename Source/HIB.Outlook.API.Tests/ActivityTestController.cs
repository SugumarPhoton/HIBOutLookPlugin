using HIB.Outlook.API.Controllers;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
namespace HIB.Outlook.API.Tests
{
    [TestClass]
    public class ActivityTestController
    {
        IActivityRepository _activityRepository = new ActivityRepository();
        IClientRepository _clientRepository = new ClientRepository();
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        IFolderRepository _folderRepository = new FolderRepository();
        ILogRepository _logRepository = new LogRepository();
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();

        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "CARAM1",
            LastSyncDate = Convert.ToDateTime("1900-01-01"),
            PageNumber = 1,
            RowsPerPage = 15000

        };



        private readonly SyncController _syncController;
        public ActivityTestController()
        {
            IActivities activity = new Activities(_activityRepository);
            IClients clients = new Clients(_clientRepository);
            IFavourites favourites = new Favourites(_favouriteRepository);
            IFolders folders = new Folders(_folderRepository);
            ILogs logs = new Logs(_logRepository);
            IPolicyLineTypes policyLineTypes = new PolicyLineTypes(_policyLineTypeRepository);


            _syncController = new SyncController(activity, clients, policyLineTypes, folders, logs, favourites)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }


        [TestMethod]
        public void SyncActivities()
        {
            var response = _syncController.SyncActivities(syncParams);
            Assert.IsNotNull(response);
            ActivityDetail activityList = JsonConvert.DeserializeObject<ActivityDetail>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNull(activityList);
        }

        [TestMethod]
        public void SyncActivityEmployees()
        {
            var response = _syncController.SyncActivityEmployees(syncParams);
            Assert.IsNotNull(response);
            ActivityEmployeeDetail activityList = JsonConvert.DeserializeObject<ActivityEmployeeDetail>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNull(activityList);
        }


        [TestMethod]
        public void SyncActivityClaims()
        {
            var response = _syncController.SyncActivityClaims(syncParams);
            List<ActivityClaimInfo> activityClaimList = JsonConvert.DeserializeObject<List<ActivityClaimInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityClaimList.Count);
        }


        [TestMethod]
        public void SyncActivityPolicies()
        {
            var response = _syncController.SyncActivityPolicies(syncParams);
            List<ActivityPolicyInfo> activityPolicyList = JsonConvert.DeserializeObject<List<ActivityPolicyInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityPolicyList.Count);
        }


        [TestMethod]
        public void SyncActivityService()
        {
            var response = _syncController.SyncActivityServices(syncParams);
            List<ActivityServiceInfo> activityServiceList = JsonConvert.DeserializeObject<List<ActivityServiceInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityServiceList.Count);
        }



        [TestMethod]
        public void SyncActivityLines()
        {
            var response = _syncController.SyncActivityLines(syncParams);
            List<ActivityLineInfo> activityLineList = JsonConvert.DeserializeObject<List<ActivityLineInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityLineList.Count);
        }


        [TestMethod]
        public void SyncActivityOpportunities()
        {
            var response = _syncController.SyncActivityOpportunities(syncParams);
            List<ActivityOpportunityInfo> activityOpportunityList = JsonConvert.DeserializeObject<List<ActivityOpportunityInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityOpportunityList.Count);
        }

        [TestMethod]
        public void SyncActivityAccounts()
        {
            var response = _syncController.SyncActivityAccounts(syncParams);
            List<ActivityAccountInfo> activityAccountList = JsonConvert.DeserializeObject<List<ActivityAccountInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityAccountList.Count);
        }

        [TestMethod]
        public void SyncActivityMarketing()
        {
            var response = _syncController.SyncActivityMarketing(syncParams);
            List<ActivityMarketingInfo> activityMarketingList = JsonConvert.DeserializeObject<List<ActivityMarketingInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityMarketingList.Count);
        }

        [TestMethod]
        public void SyncActivityClientContacts()
        {
            var response = _syncController.SyncActivityClientContacts(syncParams);
            List<ActivityClientContactInfo> activityClientContactList = JsonConvert.DeserializeObject<List<ActivityClientContactInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityClientContactList.Count);
        }

        [TestMethod]
        public void SyncActivityCommmonLookUp()
        {
            var response = _syncController.SyncActivityCommonLookUp(syncParams);
            List<ActivityCommonLookUpInfo> activityCommonLookUpList = JsonConvert.DeserializeObject<List<ActivityCommonLookUpInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityCommonLookUpList.Count);
        }


        [TestMethod]
        public void GetActivity()
        {
            
        }

        [TestMethod]
        public void SyncActivityOwnerList()
        {

            var response = _syncController.SyncActivityOwnerList(syncParams);
            List<ActivityOwnerListInfo> activityOwnerList = JsonConvert.DeserializeObject<List<ActivityOwnerListInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityOwnerList.Count);
        }
        [TestMethod]
        public void SyncActivityList()
        {

            var response = _syncController.SyncActivityList(syncParams);
            List<ActivityListInfo> activityList = JsonConvert.DeserializeObject<List<ActivityListInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityList.Count);
        }

        [TestMethod]
        public void SyncActivityBills()
        {
            var response = _syncController.SyncActivityBills(syncParams);
            List<ActivityBillInfo> activityBill = JsonConvert.DeserializeObject<List<ActivityBillInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityBill.Count);
        }

        [TestMethod]
        public void SyncActivityCarriers()
        {
            var response = _syncController.SyncActivityCarriers(syncParams);
            List<ActivityCarrierInfo> activityCarrier = JsonConvert.DeserializeObject<List<ActivityCarrierInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityCarrier.Count);
        }

        [TestMethod]
        public void SyncActivityTransactions()
        {
            var response = _syncController.SyncActivityTransactions(syncParams);
            List<ActivityTransactionInfo> activityTransaction = JsonConvert.DeserializeObject<List<ActivityTransactionInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityTransaction.Count);
        }

        [TestMethod]
        public void SyncActivityCertificate()
        {
            var response = _syncController.SyncActivityCertificate(syncParams);
            List<ActivityCertificateInfo> activityCertificateCollection = JsonConvert.DeserializeObject<List<ActivityCertificateInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityCertificateCollection.Count);
        }

        [TestMethod]
        public void SyncActivityEvidence()
        {
            var response = _syncController.SyncActivityEvidence(syncParams);
            List<ActivityEvidenceInfo> activityEvidenceCollection = JsonConvert.DeserializeObject<List<ActivityEvidenceInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityEvidenceCollection.Count);
        }



        [TestMethod]
        public void SyncActivityLookUps()
        {
            var response = _syncController.SyncActivityLookUps(syncParams);
            List<ActivityLookUpInfo> activityLookUp = JsonConvert.DeserializeObject<List<ActivityLookUpInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityLookUp.Count);
        }

        [TestMethod]
        public void SyncActivityEmployeeAgencies()
        {
            var response = _syncController.SyncActivityEmployeeAgencies(syncParams);
            List<EmployeeAgencyInfo> activityEmployeeAgency = JsonConvert.DeserializeObject<List<EmployeeAgencyInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityEmployeeAgency.Count);
        }

        [TestMethod]
        public void SyncActivityEmployee()
        {
            var response = _syncController.SyncActivityEmployee(syncParams);
            List<EmployeeInfo> activityEmployee = JsonConvert.DeserializeObject<List<EmployeeInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, activityEmployee.Count);
        }
    }
}


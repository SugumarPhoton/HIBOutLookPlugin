using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class ActivityTest
    {
        #region Private Prperties
        IActivityRepository _activityRepository = new ActivityRepository();
        private readonly IActivities _activities = null;
        private readonly string userId = "FAGJO1";
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");
        private readonly int pageNumber = 1;
        private readonly int rowsPerPage = 5000;
        private readonly string ipAddress = "";
        #endregion

        #region Constructor
        public ActivityTest()
        {
            _activities = new Activities(_activityRepository);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void SyncActivities()
        {
            var result = _activities.SyncActivities(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void SyncActivityEmployees()
        {
            var result = _activities.SyncActivityEmployees(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SyncActivityClaims()
        {
            var result = _activities.SyncActivityClaims(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }


        [TestMethod]
        public void SyncActivityPolicies()
        {
            var result = _activities.SyncActivityPolicies(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void SyncActivityService()
        {
            var result = _activities.SyncActivityServices(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }



        [TestMethod]
        public void SyncActivityLines()
        {
            var result = _activities.SyncActivityLines(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void SyncActivityOpportunities()
        {
            var result = _activities.SyncActivityOpportunities(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityAccounts()
        {
            var result = _activities.SyncActivityAccounts(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void SyncActivityMarketing()
        {
            var result = _activities.SyncActivityMarketing(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityClientContacts()
        {
            var result = _activities.SyncActivityClientContacts(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityCommmonLookUp()
        {
            var result = _activities.SyncActivityCommonLookUp(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityOwnerList()
        {

            var result = _activities.SyncActivityOwnerList(userId, lastSyncDate, ipAddress, rowsPerPage, pageNumber);
            Assert.IsNotNull(result);
            //Assert.AreNotEqual(0, result.Count);
        }
        [TestMethod]
        public void SyncActivityList()
        {

            var result = _activities.SyncActivityList(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityBill()
        {

            var result = _activities.SyncActivityBills(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityCarriers()
        {

            var result = _activities.SyncActivityCarriers(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityTransactions()
        {
            var result = _activities.SyncActivityTransactions(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityCertificate()
        {
            var result = _activities.SyncActivityCertificate(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityEvidence()
        {
            var result = _activities.SyncActivityEvidence(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }


        [TestMethod]
        public void SyncActivityLookUps()
        {
            var result = _activities.SyncActivityLookUps(userId, lastSyncDate);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityEmployeeAgencies()
        {
            var result = _activities.SyncActivityEmployeeAgencies(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void SyncActivityEmployee()
        {
            var result = _activities.SyncActivityEmployee(userId);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        #endregion
    }
}

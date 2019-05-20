using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using HIB.Outlook.Sync;
using HIB.Outlook.Model;
using System.Collections.Generic;

namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public class ActivityTest
    {
        SyncLocal _syncLocal;
        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "FAGJO1",
            LastSyncDate = Convert.ToDateTime("1900-01-01"),
            RowsPerPage=5000,
            PageNumber=1
        };
        private readonly List<string> lookUpCodeTest = new List<string>()
        {
            "CARAM1"
        };

        private string serviceMethodURL = string.Empty;
        public ActivityTest()
        {
            _syncLocal = new SyncLocal();
        }

        [TestMethod]
        public void SyncActivities()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Activity);
            bool result = _syncLocal.SyncActivities(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void SyncActivityClaims()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityClaim);
            bool result = _syncLocal.SyncActivityClaims(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void SyncActivityPolicies()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityPolicy);
            bool result = _syncLocal.SyncActivityPolicies(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityService()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityService);
            bool result = _syncLocal.SyncActivityService(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }



        [TestMethod]
        public void SyncActivityLines()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityLine);
            bool result = _syncLocal.SyncActivityLines(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }


        [TestMethod]
        public void SyncActivityOpportunities()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityOpportunity);
            bool result = _syncLocal.SyncActivityOpportunities(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityAccounts()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityAccount);
            bool result = _syncLocal.SyncActivityAccounts(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityMarketing()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityMarketing);
            bool result = _syncLocal.SyncActivityMarketing(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityClientContacts()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityClientContact);
            bool result = _syncLocal.SyncActivityClientContacts(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityCommmonLookUp()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCommonLookUp);
            bool result = _syncLocal.SyncActivityCommonLookUp(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityOwnerList()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityOwnerList);
            bool result = _syncLocal.SyncActivityOwnerList(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }
        [TestMethod]
        public void SyncActivityList()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityList);
            bool result = _syncLocal.SyncActivityList(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void SyncActivityLookUps()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityLookUp);
            bool result = _syncLocal.SyncActivityLookUp(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SyncActivityBill()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityBill);
            bool result = _syncLocal.SyncActivityBill(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SyncActivityCarrier()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCarrierSubmission);
            bool result = _syncLocal.SyncActivityCarrier(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SyncActivityTransaction()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityTransaction);
            bool result = _syncLocal.SyncActivityTransaction(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SyncActivityCertificate()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCertificate);
            bool result = _syncLocal.SyncActivityCertificate(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SyncActivityEvidence()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEvidence);
            bool result = _syncLocal.SyncActivityEvidence(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void SyncActivityEmployee()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEmployee);
            var result = _syncLocal.SyncActivityEmployee(serviceMethodURL, syncParams, lookUpCodeTest);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        [TestMethod]
        public void SyncActivityEmployeeAgency()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEmployeeAgency);
            bool result = _syncLocal.SyncActivityEmployeeAgency(serviceMethodURL, syncParams);
            Assert.IsTrue(result);
        }
    }
}


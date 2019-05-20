using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class PolicyLineTypeTest
    {
        #region Private Prperties
        private string userId = "";
        private readonly string ipAddress = "";
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();
        private readonly PolicyLineTypes policyLineTypes = null;
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");
        #endregion

        #region Constructor
        public PolicyLineTypeTest()
        {

            policyLineTypes = new PolicyLineTypes(_policyLineTypeRepository);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void GetPolicyLineTypesTest()
        {
            var result = policyLineTypes.GetPolicyLineTypes(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        #endregion
    }
}

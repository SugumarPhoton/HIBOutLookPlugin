using HIB.Outlook.Model;
using HIB.Outlook.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;


namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public class PolicyLineTypeTest
    {
        #region Private Prperties
        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "FAGJO1",
            LastSyncDate = Convert.ToDateTime("1900-01-01")
        };
        SyncLocal _syncLocal;
        #endregion
        public PolicyLineTypeTest()
        {
            _syncLocal = new SyncLocal();
        }


        #region Methods
        [TestMethod]
        public void SyncPolicyLineType()
        {
            string serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.PolicyLineType);
            bool result = _syncLocal.SyncPolicyLineTypes(serviceMethodURL, syncParams);
            Assert.IsTrue(result);

        }
        #endregion
    }
}




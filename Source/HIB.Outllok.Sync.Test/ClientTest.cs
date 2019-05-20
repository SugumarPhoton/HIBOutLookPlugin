using HIB.Outlook.Model;
using HIB.Outlook.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;


namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public class ClientTest
    {
        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "FAGJO1",
            LastSyncDate = Convert.ToDateTime("1900-01-01")
        };
        SyncLocal _syncLocal;
        private string serviceMethodURL = string.Empty;
        public ClientTest()
        {
            _syncLocal = new SyncLocal();
        }

        [TestMethod]
        public void SyncClientTest()
        {
          
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Client);
            bool clientVal = _syncLocal.SyncClients(serviceMethodURL, syncParams);
            Assert.IsTrue(clientVal);
       
        }

        [TestMethod]
        public void SyncClientEmployeeTest()
        {

            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ClientEmployee);
            bool clientEmployeeVal = _syncLocal.SyncClientEmployee(serviceMethodURL, syncParams);
            Assert.IsTrue(clientEmployeeVal);

        }
    }
}

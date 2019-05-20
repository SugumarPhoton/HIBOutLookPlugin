using HIB.Outlook.Model;
using HIB.Outlook.Sync;
using HIB.Outlook.Sync.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;


namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public class FolderTest
    {
        #region Private Prperties

        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "FAGJO1",
            LastSyncDate = Convert.ToDateTime("1900-01-01")
        };
        SyncLocal _syncLocal;
        private string serviceMethodURL = string.Empty;
        #endregion

        #region Constructor
        public FolderTest()
        {
            _syncLocal = new SyncLocal();
        }

        #endregion

        #region Methods

        [TestMethod]
        public void SyncFolderGetTest()
        {
          
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Folder);
            bool result= _syncLocal.SyncFolders(serviceMethodURL, syncParams);
            Assert.IsTrue(result);           
        }

        [TestMethod]
        public void GetUserLookupCode()
        {

           
            string result = CommonHelper.GetLookUpCode();
            Assert.IsNotNull(result);
        }
        #endregion
    }
}

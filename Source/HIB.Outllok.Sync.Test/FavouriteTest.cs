using HIB.Outlook.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;


namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public  class FavouriteTest
    {
        #region Private Prperties
        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");
        SyncLocal _syncLocal;

        private string serviceMethodURL = string.Empty;
        #endregion

        #region Constructor
        public FavouriteTest()
        {
            _syncLocal = new SyncLocal();
        }

        #endregion

        #region Methods

        [TestMethod]
        public void GetFavouriteDetails()
        {
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Favourite);
            bool result = _syncLocal.GetFavouriteDetails(serviceMethodURL, lastSyncDate);
            Assert.IsTrue(result);
        }

       
        #endregion
    }
}

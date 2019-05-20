using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class FolderTest
    {
        #region Private Prperties

        private readonly IFolders _folders = null;
        private string userId = "";
        private readonly string ipAddress = "";
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");
        IFolderRepository _folderRepository = new FolderRepository();
        #endregion

        #region Constructor
        public FolderTest()
        {
            _folders = new Folders(_folderRepository);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void SyncFolderGetTest()
        {
            var result = _folders.SyncFolders(userId, lastSyncDate, ipAddress);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        #endregion
    }
}


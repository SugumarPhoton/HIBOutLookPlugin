using HIB.Outlook.API.Controllers;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HIB.Outlook.API.Tests
{
    [TestClass]
    public class FavouriteTestController
    {
        IActivityRepository _activityRepository = new ActivityRepository();
        IClientRepository _clientRepository = new ClientRepository();
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        IFolderRepository _folderRepository = new FolderRepository();
        ILogRepository _logRepository = new LogRepository();
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();
        private readonly List<FavouriteInfo> favouriteInfoList = new List<FavouriteInfo>()
        {
            new FavouriteInfo { FavourtieName = "fav 1", UniqEmployee = "SHEJO1", UniqEntity = 73939, UniqActivity = 2524668, PolicyYear = "2015", PolicyType = "CBOP", DescriptionType = "Use Email Subj.", Description = "[Reminder - No reply] Submit your pending timesheets", FolderId = 65602, SubFolder1Id = 77078, SubFolder2Id = 77079, ClientAccessibleDate = DateTime.Now.ToString(), CreatedDate = DateTime.Now.ToString() }
        };
        private readonly SyncController _syncController;

        public FavouriteTestController()
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
        public void SaveFavouriteDetails()
        {
            var result = _syncController.SaveFavouriteDetails(favouriteInfoList);
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual("-1", result.Content.ReadAsStringAsync().Result);
        }
    }
}

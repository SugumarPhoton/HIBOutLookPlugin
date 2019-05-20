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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HIB.Outlook.API.Tests
{
    [TestClass]
    public class FolderTestController
    {
        IActivityRepository _activityRepository = new ActivityRepository();
        IClientRepository _clientRepository = new ClientRepository();
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        IFolderRepository _folderRepository = new FolderRepository();
        ILogRepository _logRepository = new LogRepository();
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();
        private readonly SyncController _syncController;
        private readonly SyncParams syncParams = new SyncParams()
        {
            UserId = "FAGJO1",
            LastSyncDate = Convert.ToDateTime("1900-01-01")
        };

        public FolderTestController()
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
        public void SyncFolderTest()
        {
            var response = _syncController.SyncFolders(syncParams);
            List<FolderInfo> clientList = JsonConvert.DeserializeObject<List<FolderInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual(0, clientList.Count);
        }

    }
}

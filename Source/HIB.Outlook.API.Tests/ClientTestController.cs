using System;
using HIB.Outlook.API.Controllers;
using HIB.Outlook.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Http;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.API.Tests
{
    [TestClass]
    public class ClientTestController
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
        public ClientTestController()
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
        public void SyncClientTest()
        {
            var response = _syncController.SyncClients(syncParams);
            List<ClientInfo> clientList = JsonConvert.DeserializeObject<List<ClientInfo>>(response.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual(0, clientList.Count);
        }

        [TestMethod]
        public void SyncClientEmployeeTest()
        {
            var response = _syncController.SyncClientEmployee(syncParams);
            List<ClientEmployee> clientEmployee = JsonConvert.DeserializeObject<List<ClientEmployee>>(response.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual(0, clientEmployee.Count);
        }

    }
}

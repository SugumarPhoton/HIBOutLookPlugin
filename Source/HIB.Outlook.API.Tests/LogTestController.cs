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
    public class LogTestController
    {
        IActivityRepository _activityRepository = new ActivityRepository();
        IClientRepository _clientRepository = new ClientRepository();
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        IFolderRepository _folderRepository = new FolderRepository();
        ILogRepository _logRepository = new LogRepository();
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();
        private readonly SyncController _syncController;
        private readonly List<LogInfo> logInfo = new List<LogInfo>()
        {
           new LogInfo { Id = "CABADCAA-F460-451C-8C7A-C40254254", EmployeeId = "SHEJO1", EntityId = 73669, ActivityId = 2524668, PolicyYear = "2017", PolicyType = "(none)", DescriptionType= "Use Activity Desc.",Description= "Amberwood Products, Inc.", FolderId= 65652, SubFolder1Id= 75971,SubFolder2Id= 75990, ClientAccessibleDate=Convert.ToString(DateTime.Now),EmailAction="Test", InsertedDate=DateTime.Now, UpdatedDate=DateTime.Now }
        };
        private readonly List<ErrorLogInfo> errorLogInfo = new List<ErrorLogInfo>()
        {
            new ErrorLogInfo { Source= "Heffernan Sales Portal", Thread=8, Level= "ERROR", Logger= "HIB.SalesPortal.Utility.Helper", Message= "System.NullReferenceException: Object reference not set to an instance of an object.", LoggedBy= "TestUser", LogDate = DateTime.Now }
        };

        public LogTestController()
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
        public void SaveAuditLogDetails()
        {
            var response = _syncController.SaveAuditLogDetails(logInfo);
            Assert.IsNotNull(response);
            Assert.AreEqual("1", response.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual("-1", response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void SaveErrorLogDetails()
        {
            var response = _syncController.SaveErrorLogDetails(errorLogInfo);
            Assert.IsNotNull(response);
            Assert.AreEqual("1", response.Content.ReadAsStringAsync().Result);
            Assert.AreNotEqual("-1", response.Content.ReadAsStringAsync().Result);

        }
    }
}

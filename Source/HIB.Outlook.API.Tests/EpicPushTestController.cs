using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.API.Controllers;
using System.Web.Http;
using HIB.Outlook.Model;
using System.IO;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.API.Tests
{
    /// <summary>
    /// Summary description for EpicPushTestController
    /// </summary>
    [TestClass]
    public class EpicPushTestController
    {
        IActivityRepository _activityRepository = new ActivityRepository();
        IClientRepository _clientRepository = new ClientRepository();
        IFavouriteRepository _favouriteRepository = new FavouriteRepository();
        IFolderRepository _folderRepository = new FolderRepository();
        ILogRepository _logRepository = new LogRepository();
        IPolicyLineTypeRepository _policyLineTypeRepository = new PolicyLineTypeRepository();
        private readonly SyncController _syncController;
        public EpicPushTestController()
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

        private readonly AddActivity addActivity = new AddActivity()
        {
            Id = 1,
            ClientId = 85526,
            AddtoType = "Claim",
            AddToTypeId = 78544,
            AddActivityCode = "1ACH",
            AddActivityDescription = "&LineCode&",
            AddActivityDisplayDescription = "",
            AddActivityId = 65628,
            OwnerCode = "CARAM1",
            OwnerDecription = "Amber Carlson",
            PriorityId = 1009,
            Priority = "Normal",
            UpdateId = 1012,
            Update = "Update email calendar",
            //ReminderDate = "1/3/2018",
            //ReminderTime = "3:10:00 AM",
            ReminderDate = "1/2/2018",
            ReminderTime = "6:10:00 AM",
            StartDate = "3/14/2018",
            StartTime = "3:10:00 AM",
            EndDate = "3/19/2018",
            EndTime = "3:10:00 AM",
            WhoToContactName = "Mike McHugh (mmchugh@inlandmetal.com)",
            WhoToContactId = 285572,
            ContactMode = "Email",
            ContactModeId = 285572,
            ContactDetail = "mmchugh@inlandmetal.com",
            ContactDetailId = 285572,
            AccessLevel = "Public",
            AccessLevelId = 1018,
            Description = "2345555t",
            CurrentlyLoggedUserCode = "CARAM1",
            Status="Closed"

        };

        


        [TestMethod]
        public void SaveEpicAttachments()
        {

             AttachmentInfo attachmentInfo = new AttachmentInfo()
            {
                FileDetails = new Model.FileInfo
                {
                    FileName = "TTTTestmail build 4000 1.1 - Attach & Delete 1232RE: Testmail build 4000 1.1 - Attach & Delete 1232",
                    FilePath = @"C:\HG IT Services\MailItems\MailItem_636504641775248961.msg",
                    FileExtension = ".msg",
                    FileContentMemStream = File.ReadAllBytes(@"C:\HG IT Services\MailItems\MailItem_636504641775248961.msg"),
                },
                FolderDetails = new FolderInfo
                {
                    ParentFolderId = 65889,
                    ParentFolderName = "HIB Bond Dept Folder",
                    FolderId = 76114,
                    FolderName = "Correspondence",
                    SubFolderId = 0,
                    SubFolderName = "",
                },

                AttachmentId = 1,
                ClientId = 72378,
                ActivityId = 2267747,
                Description = "RE: Testmail build 4000 1.1 - Attach & Delete 1232",
                PolicyType = "(none)",
                IsEmail = true,
                PolicyCode = "",
                PolicyYear = "2018",
                ClientAccessible = "1/23/2018 12:00:00 AM",
                ReceivedDate = "1/2/2018 4:29:37 AM",
                EmailFromAddress = "",
                EmailToAddress = "abc@gmail.com",
                Subject = "RE: Testmail build 4000 1.1 - Attach & Delete 1232",
                AttachmentFilePath = @"C:\Publish\OptmailFolder\MailItem_636504641775248961.msg",
                IsPushedToEpic = false,
                IsAttachDelete = false,
                DomainName = Environment.UserDomainName,
                UserName = "Josh Fagin",
                EmployeeCode = "FAGJO1",
                MailBody = "Test",
                EmailFromDisplayName = "test",
                EmailToDisplayName = "test",
                EmailCCAddress = "abc@gmail.com",
                EmailCCDisplayName = "Test",
            };
        var result = _syncController.SaveEpicAttachments(attachmentInfo);
            Assert.IsNotNull(result.Content.ReadAsStringAsync().Result);
    
        }
        [TestMethod]
        public void CreateActivity()
        {
            var result = _syncController.CreateActivity(addActivity);
            Assert.IsNotNull(result.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public async void SaveOptimalAttachment()
        {

            var result = await _syncController.SaveAttachments();
            Assert.IsNotNull(result);
            //var result = _syncController.SaveAttachments();
            //Assert.IsNotNull(result.Result);
        }


  
    }
}

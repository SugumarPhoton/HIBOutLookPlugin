using Microsoft.VisualStudio.TestTools.UnitTesting;
using HIB.Outlook.BAL.Managers;
using System;
using HIB.Outlook.Model;
using System.Collections.Generic;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.BAL.Repository;

namespace HIB.Outlook.BAL.Tests
{
    [TestClass]
    public class LogTest
    {
        #region Private Prperties
        ILogRepository _logRepository = new LogRepository();
        private readonly ILogs _logs = null;

        private readonly List<LogInfo> logInfo = new List<LogInfo>()
        {
           new LogInfo { Id = "CABADCAA-F460-451C-8C7A-C40254254", EmployeeId = "SHEJO1", EntityId = 73669, ActivityId = 2524668, PolicyYear = "2017", PolicyType = "(none)", DescriptionType= "Use Activity Desc.",Description= "Amberwood Products, Inc.", FolderId= 65652, SubFolder1Id= 75971,SubFolder2Id= 75990, ClientAccessibleDate=Convert.ToString(DateTime.Now),EmailAction="Test", InsertedDate= DateTime.Now, UpdatedDate=DateTime.Now }
        };
        private readonly List<ErrorLogInfo> errorLogInfo = new List<ErrorLogInfo>()
        {
            new ErrorLogInfo { Source= "Heffernan Sales Portal", Thread=8, Level= "ERROR", Logger= "HIB.SalesPortal.Utility.Helper", Message= "System.NullReferenceException: Object reference not set to an instance of an object.", LoggedBy= "TestUser", LogDate = DateTime.Now }
        };


        #endregion

        #region Constructor
        public LogTest()
        {
            _logs = new Logs(_logRepository);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void SaveAuditLogDetails()
        {
             var result = _logs.SaveAuditLogDetails(logInfo);
             Assert.IsNotNull(result);
             Assert.AreEqual(1, result);
             Assert.AreNotEqual(-1, result);
        }

        [TestMethod]
        public void SaveErrorLogDetails()
        {        
            var result = _logs.SaveErrorLogDetails(errorLogInfo);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.AreNotEqual(-1, result);

        }
        #endregion
    }
}

using HIB.Outlook.SQLite.Repository;
using HIB.Outlook.SQLite.Repository.IRepository;
using HIB.Outlook.Sync;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace HIB.Outllok.Sync.Test
{
    [TestClass]
    public class LogTest
    {
        #region Private Prperties
        private readonly string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        private readonly DateTime lastSyncDate = Convert.ToDateTime("1900-01-01");      
         SyncLocal _syncLocal;
        ILogRepository _logRepository;
        private string serviceMethodURL = string.Empty;
        private string lookUpCode = "FAGJO1";
      //  private string userName = "venkatesh_ma";
        #endregion

        #region Constructor
        public LogTest()
        {
            _logRepository = new LogRepository();
            _syncLocal = new SyncLocal();
        }

        #endregion
        [TestMethod]
        public void GetSyncLog()
        {          
            List<Outlook.Model.SyncLog> syncLogList = _logRepository.GetSyncLog();
            Assert.IsNotNull(syncLogList);
        }

        #region Methods

        [TestMethod]
        public void SaveAuditLogDetails()
        {      
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.AuditLog);
            bool auditVal = _syncLocal.GetAuditLogDetails(serviceMethodURL, lastSyncDate, lookUpCode);
            Assert.IsTrue(auditVal);              
        }

        [TestMethod]
        public void SaveErrorLogDetails()
        {         
            serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ErrorLog);
            bool errorVal = _syncLocal.GetErrorLogDetails(serviceMethodURL, lastSyncDate, lookUpCode);
            Assert.IsTrue(errorVal);         
        }

        [TestMethod]
        public void GetExcelAuditLogDetails()
        {          
            DataTable excelAuditlogTable = _syncLocal.GetExcelAuditLogDetails(lookUpCode);
            Assert.IsNotNull(excelAuditlogTable);       
        }

        [TestMethod]
        public void GetExcelErrorLogDetails()
        {
            DataTable excelErrorLogTable = _syncLocal.GetExcelErrorLogDetails(lookUpCode);
            Assert.IsNotNull(excelErrorLogTable);
        }
        #endregion
    }
}

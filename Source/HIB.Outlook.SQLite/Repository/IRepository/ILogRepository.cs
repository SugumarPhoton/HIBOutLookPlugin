using HIB.Outlook.Model;
using System;
using System.Collections.Generic;
using System.Data;


namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface ILogRepository
    {
      
        List<LogInfo> GetAuditLogDetails(DateTime lastSyncDate);

        List<ErrorLogInfo> GetErrorLogDetails(DateTime lastSyncDate, string lookUpCode);

        DataTable GetExcelAuditLogDetails(string lookUpCode);
        DataTable GetExcelErrorLogDetails(string lookUpCode);
        ResultInfo SaveSyncLog(string fieldName, DateTime lastSyncDate);
        List<SyncLog> GetSyncLog();
    }
}

using HIB.Outlook.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using System.Data;
using System.Data.SqlClient;
using log4net;
using System.Configuration;

namespace HIB.Outlook.BAL.Repository
{
    public class LogRepository : ILogRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

        ///<summary>
        /// Save Audit Log Details from local to Sync centralized database
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        public int SaveAuditLogDetails(List<LogInfo> auditLogInfo)
        {
            int result = -1;
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    SqlParameter HIBOPLogInfo = new SqlParameter("HIBOPLogInfo", SqlDbType.Structured) { Value = ConvertAuditLogListToDatatableUT(auditLogInfo), TypeName = "dbo.HIBOPLogInfo_UT" };
                    context.Database.ExecuteSqlCommand("exec HIBOPSyncAuditLogToCentralized_SP @HIBOPLogInfo", HIBOPLogInfo);
                    result = 1;           
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;              
            }
            return result;
        }
        ///<summary>
        ///Save error Log Details from local to Sync centralized database
        /// </summary>
        /// <param name="errorlogInfo"></param>
        /// <returns></returns>
        public int SaveErrorLogDetails(List<ErrorLogInfo> errorlogInfo)
        {
            int result = -1;
            try
            {
              
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    SqlParameter HIBOPLogInfo = new SqlParameter("ErrorLogInfo", SqlDbType.Structured) { Value = ConvertErrorLogListToDatatableUT(errorlogInfo), TypeName = "dbo.HIBOPErrorLog_UT" };
                    context.Database.ExecuteSqlCommand("exec HIBOPSyncErrorLogToCenterlized_SP @ErrorLogInfo", HIBOPLogInfo);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;                
            }
            return result;
        }

        ///<summary>
        // Convert from List to datatable for error Log Details 
        /// </summary>
        /// <param name="errorlogInfo"></param>
        /// <returns></returns>

        private DataTable ConvertErrorLogListToDatatableUT(List<ErrorLogInfo> errorlogInfo)
        {
            DataTable logInfoUtTable = new DataTable("HIBOPErrorLog_UT");
            try
            {
                logInfoUtTable.Columns.Add("Source", typeof(string));
                logInfoUtTable.Columns.Add("Thread", typeof(int));
                logInfoUtTable.Columns.Add("Level", typeof(string));
                logInfoUtTable.Columns.Add("Logger", typeof(string));
                logInfoUtTable.Columns.Add("Message", typeof(string));
                logInfoUtTable.Columns.Add("Exception", typeof(string));
                logInfoUtTable.Columns.Add("LoggedBy", typeof(string));               
                logInfoUtTable.Columns.Add("LogDate", typeof(string));

                if (errorlogInfo != null && errorlogInfo.Any())
                {
                    foreach (var l in errorlogInfo)
                    {
                        DataRow row = logInfoUtTable.NewRow();
                        row["Source"] = l.Source;                                            
                        row["Thread"] =  Convert.ToInt32(l.Thread);
                        row["Level"] = l.Level;
                        row["Logger"] = l.Logger;
                        row["Message"] = l.Message;
                        row["Exception"] = l.Exception;
                        row["LoggedBy"] = l.LoggedBy;
                        row["LogDate"] = ExtensionClass.SqlDateTimeFormat(l.LogDate);                     
                        logInfoUtTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return logInfoUtTable;
        }

        ///<summary>
        // Convert from List to datatable for Audit Log Details 
        /// </summary>
        /// <param name="errorlogInfo"></param>
        /// <returns></returns>
        private DataTable ConvertAuditLogListToDatatableUT(List<LogInfo> logInfo)
        {
            DataTable logInfoUtTable = new DataTable("HIBOPLogInfo_UT");
            try
            {
                logInfoUtTable.Columns.Add("UniqId", typeof(string));
                logInfoUtTable.Columns.Add("UniqEmployee", typeof(string));
                logInfoUtTable.Columns.Add("UniqEntity", typeof(int));
                logInfoUtTable.Columns.Add("UniqActivity", typeof(int));
                logInfoUtTable.Columns.Add("PolicyYear", typeof(string));
                logInfoUtTable.Columns.Add("PolicyType", typeof(string));
                logInfoUtTable.Columns.Add("DescriptionType", typeof(string));
                logInfoUtTable.Columns.Add("Description", typeof(string));
                logInfoUtTable.Columns.Add("FolderId", typeof(int));
                logInfoUtTable.Columns.Add("SubFolder1Id", typeof(int));
                logInfoUtTable.Columns.Add("SubFolder2Id", typeof(int));
                logInfoUtTable.Columns.Add("ClientAccessibleDate", typeof(string));
                logInfoUtTable.Columns.Add("EmailAction", typeof(string));
                logInfoUtTable.Columns.Add("Version", typeof(int));
                logInfoUtTable.Columns.Add("InsertedDate", typeof(string));
                logInfoUtTable.Columns.Add("UpdatedDate", typeof(string));
                logInfoUtTable.Columns.Add("ClientLookupCode", typeof(string));
                if (logInfo != null && logInfo.Any())
                {
                    foreach (var l in logInfo)
                    {
                        DataRow row = logInfoUtTable.NewRow();
                        row["UniqId"] = l.Id;
                        row["UniqEmployee"] = l.EmployeeId;
                        row["UniqEntity"] = l.EntityId;
                        row["UniqActivity"] = l.ActivityId;
                        row["PolicyYear"] = l.PolicyYear;
                        row["PolicyType"] = l.PolicyType;
                        row["DescriptionType"] = l.DescriptionType;
                        row["Description"] = l.Description;
                        row["FolderId"] = l.FolderId;
                        row["SubFolder1Id"] = l.SubFolder1Id;
                        row["SubFolder2Id"] = l.SubFolder2Id;
                        row["EmailAction"] = l.EmailAction;
                        row["Version"] = l.Version;

                        if(!string.IsNullOrEmpty(l.ClientAccessibleDate))
                        row["ClientAccessibleDate"] = ExtensionClass.SqlDateTimeFormat(Convert.ToDateTime(l.ClientAccessibleDate));

                        row["InsertedDate"] = ExtensionClass.SqlDateTimeFormat(l.InsertedDate);
                        row["UpdatedDate"] = ExtensionClass.SqlDateTimeFormat(l.UpdatedDate);

                        row["ClientLookupCode"] = l.ClientLookupCode;
                        logInfoUtTable.Rows.Add(row);    
                                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return logInfoUtTable;
        }        
    }
}


using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace HIB.Outlook.SQLite.Repository
{
    public class LogRepository : ILogRepository
    {

        #region Public Members
        /// <summary>
        /// Get Audit Log Details from Local Database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<LogInfo> GetAuditLogDetails(DateTime lastSyncDate)
        {
            var Logs = new List<LogInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    var lastSyncDatetime = lastSyncDate;
                    var result = context.HIBOPOutlookPluginLogs.Where(d => d.InsertedDate >= lastSyncDatetime).ToList();
                    Logs.AddRange(result.Select(l => new LogInfo { Id = l.UniqId, EntityId = Convert.ToInt32(l.UniqEntity), EmployeeId = l.UniqEmployee, ActivityId = l.UniqActivity, PolicyYear = l.PolicyYear, PolicyType = l.PolicyType, DescriptionType = l.DescriptionType, Description = l.Description, FolderId = Convert.ToInt32(l.FolderId), SubFolder1Id = Convert.ToInt32(l.SubFolder1Id), SubFolder2Id = Convert.ToInt32(l.SubFolder2Id), ClientAccessibleDate = l.ClientAccessibleDate, EmailAction = l.EmailAction, Version = 1, InsertedDate = l.InsertedDate, UpdatedDate = l.UpdatedDate, ClientLookupCode = l.ClientCode }));

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

            }
            finally
            {
                Logger.save();
            }

            return Logs;
        }


        /// <summary>
        /// Get Error Log Details from Local Database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        ///     
        public List<ErrorLogInfo> GetErrorLogDetails(DateTime lastSyncDate, string lookUpCode)
        {
            var erroLogs = new List<ErrorLogInfo>();
            try
            {
                string attachmentInfoQuery = $"Select * From HIBOPErrorLog where LogDate >= '{lastSyncDate}'";
                var sqliteDataReaderDataTable = SqliteHelper.ExecuteSelectQuery(attachmentInfoQuery);

                if (sqliteDataReaderDataTable != null)
                {


                    if (sqliteDataReaderDataTable != null)
                    {
                        while (sqliteDataReaderDataTable.Read())
                        {
                            try
                            {
                                var errorLogInfo = new ErrorLogInfo();
                                errorLogInfo.Level = Convert.ToString(sqliteDataReaderDataTable["Level"]);
                                errorLogInfo.Logger = Convert.ToString(sqliteDataReaderDataTable["Logger"]);
                                errorLogInfo.Message = Convert.ToString(sqliteDataReaderDataTable["Message"]);
                                errorLogInfo.Exception = Convert.ToString(sqliteDataReaderDataTable["Exception"]);
                                errorLogInfo.LoggedBy = Convert.ToString(sqliteDataReaderDataTable["LoggedBy"]);
                                errorLogInfo.LogDate = Convert.ToDateTime(sqliteDataReaderDataTable["LogDate"]);
                                errorLogInfo.Source = Convert.ToString(sqliteDataReaderDataTable["Source"]);
                                erroLogs.Add(errorLogInfo);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);
                            }
                        }
                        sqliteDataReaderDataTable.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);

            }
            finally
            {
                Logger.save();
            }

            return erroLogs;
        }
        /// <summary>
        /// Get audit Log Details for exporting Excel
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// Get audit Log list for exporting Excel 
        /// </summary>
        /// <returns></returns>
        public List<LogInfo> GetExcelAuditLog(string lookUpCode)
        {
            var Logs = new List<LogInfo>();

            using (var context = new HIBOutlookEntities())
            {
                try
                {

                    var todayDate = DateTime.Today;

                    var result = (from l in context.HIBOPOutlookPluginLogs
                                  join c in context.HIBOPClients on l.UniqEntity equals c.UniqEntity into lc
                                  from l1 in lc.DefaultIfEmpty()
                                  join a in context.HIBOPActivities on l.UniqActivity equals a.UniqActivity into la
                                  from l2 in la.DefaultIfEmpty()
                                  join f in context.HIBOPFolderAttachments on l.FolderId equals f.FolderId into lf
                                  from l3 in lf.DefaultIfEmpty()
                                  join s in context.HIBOPFolderAttachments on l.SubFolder1Id equals s.FolderId into ls
                                  from l4 in ls.DefaultIfEmpty()
                                  join s2 in context.HIBOPFolderAttachments on l.SubFolder2Id equals s2.FolderId into ls2
                                  from l5 in ls2.DefaultIfEmpty()
                                  where l.UniqEmployee.Equals(lookUpCode) && l.InsertedDate >= todayDate
                                  select new LogInfo
                                  {
                                      LogId = (int)l.LogId,
                                      EntityId = (int)l.UniqEntity,
                                      ClientName = l1.NameOf,
                                      ActivityId = l.UniqActivity,
                                      ActivityDescription = l2.DescriptionOf,
                                      PolicyYear = l.PolicyYear,
                                      PolicyType = l.PolicyType,
                                      DescriptionType = l.DescriptionType,
                                      Description = l.Description,
                                      FolderId = (int)l.FolderId,
                                      FolderName = l3.FolderName,
                                      SubFolder1Id = (int)l.SubFolder1Id,
                                      FolderName1 = l4.FolderName,
                                      SubFolder2Id = (int)l.SubFolder2Id,
                                      FolderName2 = l5.FolderName,
                                      ClientAccessibleDate = l.ClientAccessibleDate,
                                      EmailAction = l.EmailAction,
                                      EmailSubject = l.EmailSubject,
                                      InsertedDate = l.InsertedDate,
                                      ClientLookupCode = l.ClientCode,
                                      Status = l.Status,
                                      Message = l.ErrorMessage

                                  }).ToList<LogInfo>();

                    Logs = result.ToList<LogInfo>();

                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);

                }
                finally
                {
                    Logger.save();
                }

            }
            return Logs;

        }
        /// <summary>
        /// Get audit Log list to datatable conversion for exporting Excel 
        /// </summary>
        /// <returns></returns>
        public DataTable GetExcelAuditLogDetails(string lookUpCode)
        {
            List<LogInfo> auditLogList = GetExcelAuditLog(lookUpCode);
            DataTable logInfoUtTable = new DataTable("AuditLog");
            try
            {
                logInfoUtTable.Columns.Add("LogId", typeof(int));
                logInfoUtTable.Columns.Add("Time", typeof(DateTime));
                logInfoUtTable.Columns.Add("Email Subject", typeof(string));
                logInfoUtTable.Columns.Add("Client Id", typeof(int));
                logInfoUtTable.Columns.Add("Client Name", typeof(string));
                logInfoUtTable.Columns.Add("Client Lookup Code", typeof(string));
                logInfoUtTable.Columns.Add("Activity Description", typeof(string));
                logInfoUtTable.Columns.Add("Activity Id", typeof(int));
                logInfoUtTable.Columns.Add("Policy Year", typeof(string));
                logInfoUtTable.Columns.Add("Policy Type", typeof(string));
                logInfoUtTable.Columns.Add("Description Type", typeof(string));
                logInfoUtTable.Columns.Add("Description", typeof(string));
                logInfoUtTable.Columns.Add("Folder Id", typeof(int));
                logInfoUtTable.Columns.Add("Folder Name", typeof(string));
                logInfoUtTable.Columns.Add("Sub Folder 1 Id", typeof(int));
                logInfoUtTable.Columns.Add("Sub Folder 1 Name", typeof(string));
                logInfoUtTable.Columns.Add("Sub Folder 2 Id", typeof(int));
                logInfoUtTable.Columns.Add("Sub Folder 2 Name", typeof(string));
                logInfoUtTable.Columns.Add("Client Accessible", typeof(string));
                logInfoUtTable.Columns.Add("Action", typeof(string));
                logInfoUtTable.Columns.Add("Status", typeof(string));
                logInfoUtTable.Columns.Add("Message", typeof(string));

                if (auditLogList != null && auditLogList.Any())
                {
                    foreach (var l in auditLogList)
                    {
                        DataRow row = logInfoUtTable.NewRow();
                        row["LogId"] = l.LogId;
                        row["Time"] = l.InsertedDate;
                        row["Email Subject"] = l.EmailSubject;
                        row["Client Id"] = l.EntityId;
                        row["Client Name"] = l.ClientName;
                        row["Client Lookup Code"] = l.ClientLookupCode;
                        row["Activity Id"] = l.ActivityId;
                        row["Activity Description"] = l.ActivityDescription;
                        row["Policy Year"] = l.PolicyYear;
                        row["Policy Type"] = l.PolicyType;
                        row["Description Type"] = l.DescriptionType;
                        row["Description"] = l.Description;
                        row["Folder Id"] = l.FolderId;
                        row["Folder Name"] = l.FolderName;
                        row["Sub Folder 1 Id"] = l.SubFolder1Id;
                        row["Sub Folder 1 Name"] = l.FolderName1;
                        row["Sub Folder 2 Id"] = l.SubFolder2Id;
                        row["Sub Folder 2 Name"] = l.FolderName2;

                        if (!string.IsNullOrEmpty(l.ClientAccessibleDate))
                            row["Client Accessible"] = l.ClientAccessibleDate;

                        row["Action"] = l.EmailAction;

                        if (l.Status == 1)
                        {
                            row["Status"] = "Success";
                            row["Message"] = l.Message;
                        }
                        else if (l.Status == 0)
                        {
                            row["Status"] = "Failed";
                            row["Message"] = l.Message;
                        }
                        else
                        {
                            row["Status"] = "In Queue";
                        }

                        logInfoUtTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

            }
            finally
            {
                Logger.save();
            }

            return logInfoUtTable;
        }

        /// <summary>
        /// Get Error Log list to datatable conversion for exporting Excel 
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public DataTable GetExcelErrorLogDetails(string lookUpCode)
        {
            //List<ErrorLogInfo> errorlogInfo = GetErrorLogDetails(DateTime.Today, lookUpCode);
            List<ErrorLogInfo> errorlogInfo = GetExcelErrorLog(DateTime.Today, lookUpCode);
            DataTable logInfoUtTable = new DataTable("Error logs");
            try
            {
                logInfoUtTable.Columns.Add("LogId", typeof(int));
                logInfoUtTable.Columns.Add("Time", typeof(DateTime));
                logInfoUtTable.Columns.Add("Logger", typeof(string));
                logInfoUtTable.Columns.Add("Unhandled Exceptions", typeof(string));
                logInfoUtTable.Columns.Add("Source", typeof(string));

                if (errorlogInfo != null && errorlogInfo.Any())
                {
                    foreach (var l in errorlogInfo)
                    {
                        DataRow row = logInfoUtTable.NewRow();
                        row["LogId"] = l.LogId;
                        row["Time"] = l.LogDate;
                        row["Logger"] = l.Logger;
                        row["Unhandled Exceptions"] = l.Message;
                        row["Source"] = l.Source;
                        logInfoUtTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);

            }
            finally
            {
                Logger.save();
            }

            return logInfoUtTable;
        }


        public ResultInfo SaveSyncLog(string fieldName, DateTime lastSyncDate)
        {
            var resultInfo = new ResultInfo();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    var isAdd = false;
                    var syncLogItem = context.HIBOPSyncLogs.FirstOrDefault(a => a.Fields == fieldName);
                    if (syncLogItem == null)
                    {
                        isAdd = true;
                        syncLogItem = new HIBOPSyncLog();
                    }
                    syncLogItem.Fields = fieldName;
                    syncLogItem.SyncDate = lastSyncDate;
                    if (isAdd)
                        context.HIBOPSyncLogs.Add(syncLogItem);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }
        public List<SyncLog> GetSyncLog()
        {
            List<SyncLog> syncLogList = new List<SyncLog>();
            try
            {

                var quertstring = "SELECT SL.SLId,SL.Fields,SL.SyncDate,USL.EmployeeLookupcode,USL.SyncDate as UserSyncDate FROM HIBOPSyncLog SL LEFT JOIN HIBOPSyncLogUserDetails USL ON SL.SLId = USL.SyncLogId";

                var sqliteDataReaderDataTable = SqliteHelper.ExecuteSelectQuery(quertstring);

                if (sqliteDataReaderDataTable != null)
                {
                    if (sqliteDataReaderDataTable != null)
                    {
                        while (sqliteDataReaderDataTable.Read())
                        {
                            try
                            {
                                var SyncLogInfo = new SyncLog();
                                SyncLogInfo.Fields = Convert.ToString(sqliteDataReaderDataTable["Fields"]);
                                SyncLogInfo.UserName = Convert.ToString(sqliteDataReaderDataTable["EmployeeLookupcode"]);
                                SyncLogInfo.SyncDate = sqliteDataReaderDataTable["SyncDate"] is DBNull ? default(Nullable<DateTime>) : Convert.ToDateTime(sqliteDataReaderDataTable["SyncDate"]);
                                SyncLogInfo.UserSyncDate = sqliteDataReaderDataTable["UserSyncDate"] is DBNull ? default(Nullable<DateTime>) : Convert.ToDateTime(sqliteDataReaderDataTable["UserSyncDate"]);

                                syncLogList.Add(SyncLogInfo);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }

                        }
                        sqliteDataReaderDataTable.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

            }
            finally
            {
                Logger.save();
            }
            return syncLogList;
        }



        public List<ErrorLogInfo> GetExcelErrorLog(DateTime lastSyncDate, string lookUpCode)
        {
            var erroLogs = new List<ErrorLogInfo>();
            try
            {
                string attachmentInfoQuery = $"Select * From HIBOPErrorLog where LoggedBy ='{lookUpCode}' and LogDate >= '{lastSyncDate.ToString("yyyy-MM-dd")}'";
                var sqliteDataReaderDataTable = SqliteHelper.ExecuteSelectQuery(attachmentInfoQuery);

                if (sqliteDataReaderDataTable != null)
                {
                    if (sqliteDataReaderDataTable != null)
                    {
                        while (sqliteDataReaderDataTable.Read())
                        {
                            try
                            {
                                var errorLogInfo = new ErrorLogInfo();
                                errorLogInfo.LogId = Convert.ToInt32(sqliteDataReaderDataTable["LogId"]);
                                errorLogInfo.Level = Convert.ToString(sqliteDataReaderDataTable["Level"]);
                                errorLogInfo.Logger = Convert.ToString(sqliteDataReaderDataTable["Logger"]);
                                errorLogInfo.Message = Convert.ToString(sqliteDataReaderDataTable["Message"]);
                                errorLogInfo.Exception = Convert.ToString(sqliteDataReaderDataTable["Exception"]);
                                errorLogInfo.LoggedBy = Convert.ToString(sqliteDataReaderDataTable["LoggedBy"]);
                                errorLogInfo.LogDate = Convert.ToDateTime(sqliteDataReaderDataTable["LogDate"]);
                                errorLogInfo.Source = Convert.ToString(sqliteDataReaderDataTable["Source"]);
                                erroLogs.Add(errorLogInfo);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);
                            }
                        }
                        sqliteDataReaderDataTable.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);

            }
            finally
            {
                Logger.save();
            }

            return erroLogs;
        }
        #endregion
    }
}

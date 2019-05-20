using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using log4net;

namespace HIB.Outlook.Sync
{
    public class SqliteHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SqliteHelper));
        string _sqliteConnectionString = string.Empty;
        public SqliteHelper(string sqliteConnectionString)
        {
            _sqliteConnectionString = sqliteConnectionString;
        }

        internal SQLiteConnection _connection = null;
        internal SQLiteConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = new SQLiteConnection(_sqliteConnectionString);
                    _connection.Open();
                }
                return _connection;
            }

        }


        /// <summary>
        /// Save Client detail to local Database
        /// </summary>
        /// <param name="clientList"></param>
        /// <returns></returns>
        public int SyncDBClient(List<ClientInfo> clientList)
        {
            int result = -1;
            try
            {


                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var C in clientList)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPClient WHERE UniqEntity= '" + C.EnitityId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {

                                    cmd.CommandText = "INSERT INTO HIBOPClient(UniqEntity,LookupCode,NameOf,Address,PrimaryContactName,Status,City,StateCode,StateName,PostalCode,Country,CountryCode,InsertedDate,UpdatedDate,AgencyCode,AgencyName) VALUES ('" + C.EnitityId + "', '" + C.LookupCode + "','" + C.Nameof.Replace("'", "''") + "','" + C.Address.Replace("'", "''") + "','" + C.PrimaryContactName + "','" + C.Status + "','" + C.City + "','" + C.StateCode + "','" + C.StateName + "','" + C.PostalCode + "','" + C.Country + "','" + C.CountryCode + "','" + ExtensionClass.SqliteDateTimeFormat(C.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(C.UpdatedDate) + "','" + C.AgencyCode + "','" + C.AgencyName + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);

                            }
                        }
                        transaction.Commit();
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }
        /// <summary>
        /// Save Folder detail to local Database
        /// </summary>
        /// <param name="folderInfo"></param>
        /// <returns></returns>
        public int SyncDBFolder(List<FolderInfo> folderInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var F in folderInfo)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPFolderAttachment WHERE FolderId= '" + F.FolderId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPFolderAttachment(FolderId,ParentFolderId,FolderName,FolderType,InsertedDate,UpdatedDate) VALUES ('" + F.FolderId + "', '" + F.ParentFolderId + "','" + F.FolderName + "','" + F.FolderType + "','" + ExtensionClass.SqliteDateTimeFormat(F.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(F.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);

                            }
                        }
                        transaction.Commit();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }

        /// <summary>
        /// Save Activity detail to local Database
        /// </summary>
        /// <param name="oListActivity"></param>
        /// <returns></returns>
        public int SyncDBActivity(List<ActivityInfo> oListActivity)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in oListActivity)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivity WHERE UniqActivity= '" + a.ActivityId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPActivity(UniqEntity,UniqActivity,UniqActivityCode,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,InsertedDate,UpdatedDate,Status,ExpirationDate,EffectiveDate) VALUES ('" + a.EntityId + "','" + a.ActivityId + "', '" + a.ActivityIdCode + "', '" + a.ActivityCode + "','" + a.DescriptionOf.Replace("'", "''") + "','" + a.CdPolicyLineTypeCode + "','" + a.PolicyNumber + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Status + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);

                            }
                        }
                        transaction.Commit();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }

        /// <summary>
        /// Save Policy detail to local Database
        /// </summary>
        /// <param name="policyTypeInfo"></param>
        /// <returns></returns>
        public int SyncDBPolicy(List<PolicyTypeInfo> policyTypeInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var p in policyTypeInfo)
                        {
                            try
                            {

                                cmd.CommandText = "SELECT count(*) FROM HIBOPPolicyLineType WHERE UniqCdPolicyLineType= '" + p.PolicyLineTypeId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPPolicyLineType(UniqCdPolicyLineType,CdPolicyLineTypeCode,PolicyLineTypeDesc,InsertedDate,UpdatedDate) VALUES ('" + p.PolicyLineTypeId + "', '" + p.PolicyLineTypeCode + "','" + p.PolicyLineTypeDesc.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(p.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(p.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);

                            }
                        }
                        transaction.Commit();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        /// <summary>
        /// Get Error Log Details from Local Database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ErrorLogInfo> GetErrorLogDetails(DateTime lastSyncDate)
        {
            List<ErrorLogInfo> listErrorLog = new List<ErrorLogInfo>();
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPErrorLog WHERE LogDate > '" + ExtensionClass.SqliteDateTimeFormat(lastSyncDate) + "'";
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        ErrorLogInfo errorLogInfo = new ErrorLogInfo();
                        errorLogInfo.Level = Convert.ToString(r["Level"]);
                        errorLogInfo.Logger = Convert.ToString(r["Logger"]);
                        errorLogInfo.Message = Convert.ToString(r["Message"]);
                        errorLogInfo.Exception = Convert.ToString(r["Exception"]);
                        errorLogInfo.LoggedBy = Convert.ToString(r["LoggedBy"]);
                        errorLogInfo.LogDate = Convert.ToDateTime(r["LogDate"].ToString());
                        errorLogInfo.Thread = Convert.ToInt32(r["Thread"]);
                        errorLogInfo.Source = Convert.ToString(r["Source"]);
                        listErrorLog.Add(errorLogInfo);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return listErrorLog;
        }
        /// <summary>
        /// Get Audit Log Details from Local Database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<LogInfo> GetAuditLogDetails(DateTime lastSyncDate)
        {
            List<LogInfo> auditLogList = new List<LogInfo>();
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    string selectAuditLog = "SELECT * FROM HIBOPOutlookPluginLog WHERE InsertedDate > '" + ExtensionClass.SqliteDateTimeFormat(lastSyncDate) + "'";
                    fmd.CommandText = selectAuditLog;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        LogInfo auditLogInfo = new LogInfo();
                        auditLogInfo.Id = Convert.ToString(r["UniqId"]);
                        auditLogInfo.EmployeeId = Convert.ToString(r["UniqEmployee"]);
                        auditLogInfo.EntityId = Convert.ToInt32(r["UniqEntity"]);
                        auditLogInfo.ActivityId = Convert.ToInt32(r["UniqActivity"]);
                        auditLogInfo.PolicyYear = Convert.ToString(r["PolicyYear"]);
                        auditLogInfo.PolicyType = Convert.ToString(r["PolicyType"]);
                        auditLogInfo.DescriptionType = Convert.ToString(r["DescriptionType"]);
                        auditLogInfo.Description = Convert.ToString(r["Description"]);
                        auditLogInfo.FolderId = Convert.ToInt32(r["FolderId"]);
                        auditLogInfo.SubFolder1Id = Convert.ToInt32(r["SubFolder1Id"]);
                        auditLogInfo.SubFolder2Id = Convert.ToInt32(r["SubFolder2Id"]);
                        auditLogInfo.ClientAccessibleDate = Convert.ToString(r["ClientAccessibleDate"])?.StringToDateTime();
                        auditLogInfo.EmailAction = Convert.ToString(r["EmailAction"]);
                        auditLogInfo.Version = 1;
                        auditLogInfo.InsertedDate = Convert.ToDateTime(r["InsertedDate"]);
                        auditLogInfo.UpdatedDate = Convert.ToDateTime(r["UpdatedDate"]);
                        auditLogList.Add(auditLogInfo);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return auditLogList;
        }

        public List<FavouriteInfo> GetFavouriteDetails(DateTime lastSyncDate)
        {
            List<FavouriteInfo> favouriteList = new List<FavouriteInfo>();
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    string selectAuditLog = "SELECT * FROM HIBOPFavourites WHERE CreatedDate > '" + ExtensionClass.SqliteDateTimeFormat(lastSyncDate) + "'";
                    fmd.CommandText = selectAuditLog;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        FavouriteInfo favouriteInfo = new FavouriteInfo();
                        favouriteInfo.FavourtieName = Convert.ToString(r["FavouriteName"]);
                        favouriteInfo.UniqEmployee = Convert.ToString(r["UniqEmployee"]);
                        favouriteInfo.UniqEntity = Convert.ToInt32(r["UniqEntity"]);
                        favouriteInfo.UniqActivity = Convert.ToInt32(r["UniqActivity"]);
                        favouriteInfo.PolicyYear = Convert.ToString(r["PolicyYear"]);
                        favouriteInfo.PolicyType = Convert.ToString(r["PolicyType"]);
                        favouriteInfo.DescriptionType = Convert.ToString(r["DescriptionType"]);
                        favouriteInfo.Description = Convert.ToString(r["Description"]);
                        favouriteInfo.FolderId = Convert.ToInt32(r["FolderId"]);
                        favouriteInfo.SubFolder1Id = Convert.ToInt32(r["SubFolder1Id"]);
                        favouriteInfo.SubFolder2Id = Convert.ToInt32(r["SubFolder2Id"]);
                        favouriteInfo.ClientAccessibleDate = Convert.ToString(r["ClientAccessibleDate"]);
                        favouriteInfo.CreatedDate = Convert.ToDateTime(r["CreatedDate"]);
                        favouriteList.Add(favouriteInfo);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return favouriteList;
        }




        /// <summary>
        /// Get error Log Details for exporting Excel
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ErrorLogInfo> GetExcelErrorLogDetails()
        {
            List<ErrorLogInfo> listErrorLog = new List<ErrorLogInfo>();
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPErrorLog where LogDate > '" + DateTime.Now.ToString("yyyy-MM-dd") + "'"; ;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        ErrorLogInfo errorLogInfo = new ErrorLogInfo();
                        errorLogInfo.Level = Convert.ToString(r["Level"]);
                        errorLogInfo.Logger = Convert.ToString(r["Logger"]);
                        errorLogInfo.Message = Convert.ToString(r["Message"]);
                        errorLogInfo.Exception = Convert.ToString(r["Exception"]);
                        errorLogInfo.LoggedBy = Convert.ToString(r["LoggedBy"]);
                        errorLogInfo.LogDate = Convert.ToDateTime(r["LogDate"].ToString());
                        errorLogInfo.Thread = Convert.ToInt32(r["Thread"]);
                        errorLogInfo.Source = Convert.ToString(r["Source"]);
                        listErrorLog.Add(errorLogInfo);
                    }
                }

            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return listErrorLog;
        }
        /// <summary>
        /// Get audit Log Details for exporting Excel
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<LogInfo> GetExcelAuditLogDetails()
        {
            List<LogInfo> auditLogList = new List<LogInfo>();

            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPOutlookPluginLog where InsertedDate > '" + DateTime.Now.ToString("yyyy-MM-dd") + "'"; ;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            LogInfo auditLogInfo = new LogInfo();
                            auditLogInfo.Id = Convert.ToString(r["UniqId"]);
                            auditLogInfo.EntityId = Convert.ToInt32(r["UniqEntity"]);
                            auditLogInfo.ClientName = GetClientNameFromClientTable(auditLogInfo.EntityId, _sqliteConnectionString);
                            auditLogInfo.ActivityId = Convert.ToInt32(r["UniqActivity"]);
                            auditLogInfo.ActivityDescription = GetActivityDescriptionFromActivityTable(auditLogInfo.ActivityId, _sqliteConnectionString);
                            auditLogInfo.PolicyYear = Convert.ToString(r["PolicyYear"]);
                            auditLogInfo.PolicyType = Convert.ToString(r["PolicyType"]);
                            auditLogInfo.DescriptionType = Convert.ToString(r["DescriptionType"]);
                            auditLogInfo.Description = Convert.ToString(r["Description"]);
                            auditLogInfo.FolderId = Convert.ToInt32(r["FolderId"]);
                            auditLogInfo.FolderName = GetFolderNameFromFoldersTable(auditLogInfo.FolderId, _sqliteConnectionString);
                            auditLogInfo.SubFolder1Id = Convert.ToInt32(r["SubFolder1Id"]);
                            auditLogInfo.FolderName1 = GetFolderNameFromFoldersTable(auditLogInfo.SubFolder1Id, _sqliteConnectionString);
                            auditLogInfo.SubFolder2Id = Convert.ToInt32(r["SubFolder2Id"]);
                            auditLogInfo.FolderName2 = GetFolderNameFromFoldersTable(auditLogInfo.SubFolder2Id, _sqliteConnectionString);
                            auditLogInfo.ClientAccessibleDate = string.IsNullOrEmpty(Convert.ToString(r["ClientAccessibleDate"])) ? default(Nullable<DateTime>) : Convert.ToDateTime(Convert.ToString(r["ClientAccessibleDate"]));
                            auditLogInfo.EmailAction = Convert.ToString(r["EmailAction"]);
                            auditLogInfo.EmailSubject = Convert.ToString(r["EmailSubject"]);
                            //auditLogInfo.Version = Convert.ToInt32(r["Version"]);
                            auditLogInfo.Version = 1;
                            auditLogInfo.InsertedDate = Convert.ToDateTime(r["InsertedDate"]);
                            auditLogInfo.UpdatedDate = Convert.ToDateTime(r["UpdatedDate"]);
                            auditLogList.Add(auditLogInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }

                }

            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return auditLogList;
        }

        private string GetClientNameFromClientTable(Int32 UniqEntity, string connectionString)
        {
            var clientName = string.Empty;
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPClient where UniqEntity=" + UniqEntity;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            clientName = Convert.ToString(r["NameOf"]);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return clientName;
        }
        private string GetActivityDescriptionFromActivityTable(Int32 UniqActivity, string connectionString)
        {
            var activityDescription = string.Empty;
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPActivity where UniqActivity=" + UniqActivity;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            activityDescription = Convert.ToString(r["DescriptionOf"]);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityDescription;
        }

        private string GetFolderNameFromFoldersTable(Int32? FolderId, string connectionString)
        {
            var folderName = string.Empty;
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPFolderAttachment where FolderId=" + FolderId;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        try
                        {
                            folderName = Convert.ToString(r["FolderName"]);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //Error Log
            }
            return folderName;
        }




        public System.Data.DataTable MapAuditLog()
        {
            List<LogInfo> auditLogList = GetExcelAuditLogDetails();
            DataTable logInfoUtTable = new DataTable("AuditLog");
            try
            {

                logInfoUtTable.Columns.Add("Time", typeof(DateTime));
                logInfoUtTable.Columns.Add("Email Subject", typeof(string));
                logInfoUtTable.Columns.Add("Client Id", typeof(int));
                logInfoUtTable.Columns.Add("Client Name", typeof(string));
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

                if (auditLogList != null && auditLogList.Any())
                {
                    foreach (var l in auditLogList)
                    {
                        DataRow row = logInfoUtTable.NewRow();
                        row["Time"] = l.InsertedDate;
                        row["Email Subject"] = l.EmailSubject;
                        row["Client Id"] = l.EntityId;
                        row["Client Name"] = l.ClientName;
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
                        row["Client Accessible"] = l.ClientAccessibleDate == null ? default(string) : l.ClientAccessibleDate.ToString();
                        row["Action"] = l.EmailAction;
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

        public DataTable MapErrorLogUT()
        {
            List<ErrorLogInfo> errorlogInfo = GetExcelErrorLogDetails();
            DataTable logInfoUtTable = new DataTable("Error logs");
            try
            {
                logInfoUtTable.Columns.Add("Time", typeof(DateTime));
                logInfoUtTable.Columns.Add("Logger", typeof(string));
                logInfoUtTable.Columns.Add("Unhandled Exceptions", typeof(string));
                logInfoUtTable.Columns.Add("Source", typeof(string));

                if (errorlogInfo != null && errorlogInfo.Any())
                {
                    foreach (var l in errorlogInfo)
                    {
                        DataRow row = logInfoUtTable.NewRow();
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
                Logger.Error(ex);
            }
            return logInfoUtTable;
        }


        public List<SyncLog> GetSyncLog()
        {
            List<SyncLog> syncLogList = new List<SyncLog>();

            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM HIBOPSyncLog";
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        SyncLog syncLog = new SyncLog();
                        syncLog.Fields = Convert.ToString(r["Fields"]);
                        syncLog.SyncDate = string.IsNullOrEmpty(Convert.ToString(r["SyncDate"])) ? default(Nullable<DateTime>) : Convert.ToDateTime(r["SyncDate"]);
                        syncLogList.Add(syncLog);
                    }
                }

            }
            catch (SQLiteException ex)
            {
                Logger.Error(ex);
            }
            return syncLogList;
        }


        /// <summary>
        /// Save Sync log to local Database
        /// </summary>
        /// <param name="policyTypeInfo"></param>
        /// <returns></returns>
        public int SaveLog(string field, string lastSyncDate)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        cmd.CommandText = "Update HIBOPSyncLog set SyncDate ='" + lastSyncDate + "' where Fields = '" + field + "'";
                        result = cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }

        /// <summary>
        /// Save Activity Claims to local Database
        /// </summary>
        /// <param name="activityClaimInfo"></param>
        /// <returns></returns>
        public int SyncActivityClaims(List<ActivityClaimInfo> activityClaimInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityClaimInfo)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPClaim WHERE UniqClaim= '" + a.ClaimId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPClaim(UniqEntity,UniqClaim,ClaimCode,ClaimName,LossDate,ReportedDate,ClaimNumber,CompanyClaimNumber,ClosedDate,InsertedDate,UpdatedDate) VALUES ('" + a.EntityId + "','" + a.ClaimId + "', '" + a.ClaimCode + "', '" + a.ClaimName + "','" + ExtensionClass.SqliteDateTimeFormat(a.LossDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ReportedDate) + "','" + a.AgencyClaimNumber + "','" + a.CompanyClaimNumber + "','" + ExtensionClass.SqliteDateTimeFormat(a.ClosedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        /// <summary>
        /// Save Activity policy to local Database
        /// </summary>
        /// <param name="activityPolicyInfo"></param>
        /// <returns></returns>
        public int SyncActivityPolicies(List<ActivityPolicyInfo> activityPolicyInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityPolicyInfo)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPPolicy WHERE UniqPolicy= '" + a.PolicyId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPPolicy(UniqPolicy,UniqEntity,CdPolicyLineTypeCode,PolicyNumber,DescriptionOf,EffectiveDate,ExpirationDate,PolicyStatus,Flags,InsertedDate,UpdatedDate) VALUES ('" + a.PolicyId + "','" + a.EntityId + "', '" + a.PolicyLineTypeCode + "', '" + a.PolicyNumber.Replace("'", "''") + "','" + a.DescriptionOf.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + a.PolicyStatus + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                                return result;
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        public int SyncActivityServices(List<ActivityServiceInfo> activityServiceInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityServiceInfo)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityServices WHERE UniqServiceHead= '" + a.ServiceHeadId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPActivityServices(UniqServiceHead,UniqEntity,ServiceNumber,UniqCdServiceCode,Description,ContractNumber,InceptionDate,ExpirationDate,Flags,InsertedDate,UpdatedDate) VALUES ('" + a.ServiceHeadId + "','" + a.EntityId + "', '" + a.ServiceNumber + "', '" + a.ServiceCodeId + "','" + a.Description.Replace("'", "''") + "','" + a.ContractNumber + "','" + ExtensionClass.SqliteDateTimeFormat(a.InceptionDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }

        public int SyncActivityLines(List<ActivityLineInfo> activityLineInfo)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityLineInfo)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityLine WHERE UniqLine= '" + a.LineId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPActivityLine(UniqLine,UniqPolicy,UniqEntity,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber,UniqCdPolicyLineType,UniqCdLineStatus,BillModeCode,ExpirationDate,EffectiveDate,IOC,Flags,InsertedDate,UpdatedDate) VALUES ('" + a.LineId + "','" + a.PolicyId + "', '" + a.EntityId + "', '" + a.PolicyDesc.Replace("'", "''") + "','" + a.LineCode + "','" + a.LineOfBusiness.Replace("'", "''") + "','" + a.LineStatus + "','" + a.PolicyNumber + "','" + a.PolicyLineTypeId + "','" + a.LineStatusId + "' ,'" + a.BillModeCode + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + a.IOC + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        public int SyncActivityOpportunities(List<ActivityOpportunityInfo> activityOpportunity)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityOpportunity)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityOpportunity WHERE UniqOpportunity= '" + a.OpportunityId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPActivityOpportunity(UniqOpportunity,UniqEntity,OppDesc,TargetedDate,OwnerName,SalesManager,Stage,Flags,InsertedDate,UpdatedDate) VALUES ('" + a.OpportunityId + "','" + a.EntityId + "', '" + a.OppDesc.Replace("'", "''") + "', '" + ExtensionClass.SqliteDateTimeFormat(a.TargetedDate) + "','" + a.OwnerName + "','" + a.SalesManager + "','" + a.Stage + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        public int SyncActivityAccounts(List<ActivityAccountInfo> activityAccount)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityAccount)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityAccount WHERE AccountId= '" + a.RowNumber + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPActivityAccount(AccountId,UniqEntity,UniqAgency,AgencyCode,AgencyName,UniqBranch,BranchCode,BranchName,InsertedDate,UpdatedDate) VALUES ('" + a.RowNumber + "','" + a.EntityId + "','" + a.AgencyId + "', '" + a.AgencyCode + "', '" + a.AgencyName + "','" + a.BranchId + "','" + a.BranchCode + "','" + a.BranchName + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }
        public int SyncActivityMarketing(List<ActivityMarketingInfo> activityMarketing)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityMarketing)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityMasterMarketing WHERE UniqMarketingSubmission= '" + a.UniqMarketingSubmission + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPActivityMasterMarketing(UniqMarketingSubmission,UniqEntity,UniqAgency,UniqBranch,DescriptionOf,UniqCdPolicyLineType,EffectiveDate,ExpirationDate,LastSubmittedDate,Flags,InsertedDate,UpdatedDate) VALUES ('" + a.UniqMarketingSubmission + "','" + a.UniqEntity + "', '" + a.UniqAgency + "', '" + a.UniqBranch + "','" + a.MarketingSubbmission.Replace("'", "''") + "','" + a.LineOfBusiness + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.LastSubmittedDate) + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }



        public int SyncActivityClientContacts(List<ActivityClientContactInfo> activityClientContact)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityClientContact)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityClientContacts WHERE ClientContactId= '" + a.ClientContactId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    cmd.CommandText = "INSERT INTO HIBOPActivityClientContacts(ClientContactId,UniqContactNumber,UniqEntity,UniqContactName,ContactName,ContactType,ContactValue,InsertedDate,UpdatedDate) VALUES (" + a.ClientContactId + "," + a.ContactNumberId + ",'" + a.EntityId + "','" + a.ContactNameId + "', '" + a.ContactName.Replace("'", "''") + "', '" + a.ContactType + "','" + a.ContactValue.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        public int SyncActivityCommonLookUp(List<ActivityCommonLookUpInfo> activityCommonLookUp)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityCommonLookUp)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPCommonLookup WHERE CommonLkpId= '" + a.CommonLkpId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                if (IdCount == 0)
                                {
                                    int isDeleted = a.IsDeleted ? 1 : 0;
                                    cmd.CommandText = "INSERT INTO HIBOPCommonLookup(CommonLkpId,CommonLkpTypeCode,CommonLkpCode,CommonLkpName,CommonLkpDescription,SortOrder,IsDeleted,CreatedDate,ModifiedDate) VALUES ('" + a.CommonLkpId + "','" + a.CommonLkpTypeCode + "', '" + a.CommonLkpCode + "', '" + a.CommonLkpName.Replace("'", "''") + "','" + a.CommonLkpDescription.Replace("'", "''") + "'," + a.SortOrder + "," + isDeleted + ",'" + ExtensionClass.SqliteDateTimeFormat(a.CreatedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ModifiedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }



        public int SyncActivityOwnerList(List<ActivityOwnerListInfo> activityOwnerList)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityOwnerList)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(a.Lookupcode))
                                {
                                    cmd.CommandText = "SELECT count(*) FROM HIBOPActivityOwnerList WHERE Lookupcode= '" + a.Lookupcode + "'";
                                    int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                    if (IdCount == 0)
                                    {
                                        cmd.CommandText = "INSERT INTO HIBOPActivityOwnerList(Lookupcode,EmployeeName) VALUES ('" + a.Lookupcode + "','" + a.EmployeeName + "')";
                                        result = cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }


        public int SyncActivityList(List<ActivityListInfo> activityList)
        {
            int result = -1;
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    using (var transaction = Connection.BeginTransaction())
                    {
                        foreach (var a in activityList)
                        {
                            try
                            {
                                cmd.CommandText = "SELECT count(*) FROM HIBOPActivityList WHERE UniqActivityCode= '" + a.ActivityCodeId + "'";
                                int IdCount = Convert.ToInt32(cmd.ExecuteScalar());
                                if (IdCount == 0)
                                {

                                    cmd.CommandText = "INSERT INTO HIBOPActivityList(UniqActivityCode,ActivityCode,ActivityName,InsertedDate,UpdatedDate) VALUES ('" + a.ActivityCodeId + "','" + a.ActivityCode + "','" + a.ActivityName + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    result = cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                        transaction.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }
    }
}

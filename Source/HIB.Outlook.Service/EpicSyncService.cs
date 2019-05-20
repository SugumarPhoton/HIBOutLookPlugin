using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.SQLite;
using HIB.Outlook.Sync;
using HIB.Outlook.Sync.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace HIB.Outlook.Service
{
    partial class EpicSyncService : ServiceBase
    {
        #region Private variables

        bool IsSyncCompleted = true;
        bool IsSyncCompletedForClient = true;

        System.Timers.Timer timerDelay;
        System.Timers.Timer timerDelayForClient;
        System.Timers.Timer timerDelayForFirstSync;
        internal SyncLocal _syncLocal;
        string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
        List<string> ValidEmployees = new List<string>();
        //private string FileName = "SyncOnProgress";
        private string FilePath = string.Empty;
        internal string CurrentlyLoggedUserName { get; set; }

        internal string CurrentlyLoggedLookupCode { get; set; }
        FileSystemWatcher ServiceFileWatcher = null;

        FileSystemWatcher AddinFileWatcher = null;

        XmlToSqliteHandler xmlToSqliteHandler = new XmlToSqliteHandler();

        #endregion

        public EpicSyncService()
        {
            try
            {

                InitializeComponent();
                FilePath = ConfigurationManager.AppSettings["DBFolderPath"]?.ToString();

                InitialiseFileWatcher(ConfigurationManager.AppSettings["ServiceFolderPath"]?.ToString(), ServiceFileWatcher);
                InitialiseFileWatcher(ConfigurationManager.AppSettings["AddinFolderPath"]?.ToString(), AddinFileWatcher);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }

        private void LoadTimer(Int64 intervalTime)
        {
            try
            {

                timerDelay = new System.Timers.Timer();
                double interval = Convert.ToDouble(ConfigurationManager.AppSettings["EpicInterval"]);
                timerDelay.Interval = (intervalTime != 0) ? (intervalTime * 60 * 1000) : (interval != 0) ? (interval * 60 * 1000) : (10 * 60 * 1000);
                timerDelay.Elapsed += new System.Timers.ElapsedEventHandler(PerformSyncProcess);

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }
        private void LoadTimerForFirstSync()
        {
            try
            {

                timerDelayForFirstSync = new System.Timers.Timer
                {
                    Interval = 1 * 60 * 1000,
                    AutoReset = false
                };
                timerDelayForFirstSync.Elapsed += new System.Timers.ElapsedEventHandler(FirstSyncProcess);

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }

        private void LoadTimerForClient(Int64 intervalTime)
        {
            try
            {
                timerDelayForClient = new System.Timers.Timer();
                timerDelayForClient.Interval = (intervalTime != 0) ? (intervalTime * 60 * 1000) : (20 * 60 * 1000);
                timerDelayForClient.Elapsed += new System.Timers.ElapsedEventHandler(SyncProcessForClient);

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }


        /// <summary>
        /// PerformEpicSync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformSyncProcess(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsSyncCompleted && !File.Exists(FilePath))
            {
                if (ValidEmployees.Count == 0)
                {
                    ValidEmployees = GetValidEmployeesToBeSynced();
                }
                var deltaSyncObjects = GetDeltaSyncObjects(ValidEmployees, false, false);
                SyncProcess(ValidEmployees, deltaSyncObjects);
            }
        }


        private void FirstSyncProcess(object sender, System.Timers.ElapsedEventArgs e)
        {
            FirstSync();
        }
        private void FirstSync()
        {
            ValidEmployees = GetValidEmployeesToBeSynced();
            var deltaSyncObjects = GetDeltaSyncObjects(ValidEmployees, null, true);
            SyncOnlyClient(ValidEmployees, deltaSyncObjects);
            SyncProcess(ValidEmployees, deltaSyncObjects);
        }
        public void ParallelSyncForTest()
        {
            ValidEmployees = new List<string>
            {
                "MASME1",
                "MARJA1",
                "MATMI1",
                "HOUMA1",
                "MILRA1",
                "MATKA1",
                "STEBE1",
                "WILJU1",
                "LALCR1",
                "BENLE1",
                "MAGMA1",
                "MCMAN1",
                "ONEAL1",
                "KAWMA1",
                "MURBR1",
                "MAHTH1",
                "GOLMA1",
                "HUDRE1",
                "RUSJO1",
                "SCOFR1",
                "EVEAS1",
                "BENSA1",
                "LUKAN1",
                "DUNJE1",
                "BRASH1",
                "ODOSH1",
                "SAURY1",
                "REUKE1",
                "OWEDE1",
                "SANME1",
                "ISAIN1",
                "ETHCA1",
                "BOOST1",
                "RAMSY1",
                "SILKA1",
                "FLYCH1",
                "LUMNI1",
                "EZEAL1",
                "LUKLI1",
                "WALFA1",
                "LEEBE1",
                "MARSH1",
                "LONRO1",
                "EDLNA1",
                "LEUNA1",
                "RIOLI1",
                "CHOAM1",
                "CONMO1",
                "ADAMI1",
                "CAMSA1",
                "DELDA1",
                "JAHKR1",
                "FARAM1",
                "MERRU1",
                "GARLA1",
                "BENTY1",
                "HILMA1",
                "LAUED1",
                "COSDE1",
                "SPRKE1",
                "ADAAR1",
                "BOTSU1",
                "BEAJE1",
                "TURCH1",
                "BISKA1",
                "SKAMA1",
                "KIRBR1",
                "DAVMA1",
                "JIMMA1",
                "SOOWA1",
                "KUNCH1",
                "PIESA1",
                "THINI1",
                "FLOSE1",
                "MEWJU1",
                "FUJMI1",
                "TARKE1",
                "YETBI1",
                "HAWKI1",
                "LOJJO1",
                "COVCH1",
                "MARME1",
                "MAHDA1",
                "GLODE1"
            };
            //ValidEmployees = new List<string>
            //{
            //    "MARJA1"
            //};

            //Parallel.ForEach(ValidEmployees, employee => {
            //    List<string> employeeList = new List<string>
            //    {
            //        employee
            //    };
            //    var deltaSyncObjects = GetDeltaSyncObjects(employeeList, null, true);
            //    SyncOnlyClient(employeeList, deltaSyncObjects);
            //    SyncProcess(employeeList, deltaSyncObjects);
            //});

            Parallel.ForEach(ValidEmployees, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, DeltaSyncTest);


        }

        private void DeltaSyncTest(string LookupCode)
        {
            List<string> employeeList = new List<string>
                {
                    LookupCode
                };
            var deltaSyncObjects = GetDeltaSyncObjects(employeeList, null, true);
            SyncOnlyClient(employeeList, deltaSyncObjects);
            SyncProcess(employeeList, deltaSyncObjects);

        }
        private void SyncProcess(List<string> ValidEmployees, List<DeltaSyncObjectInfo> deltaSyncObjects)
        {
            try
            {
                if (_syncLocal == null)
                {
                    _syncLocal = new SyncLocal();
                    _syncLocal.SyncCompleted -= _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompletedForClient -= _syncLocal_SyncCompletedForClient;
                    _syncLocal.SyncCompletedForClient += _syncLocal_SyncCompletedForClient;
                }
                if (IsSyncCompleted && !File.Exists(FilePath))
                {
                    var fileStream = File.Create(FilePath);
                    if (fileStream != null)
                        fileStream.Dispose();
                    IsSyncCompleted = false;
                    //PushNewActivityToEpicFromSqliteDB();
                    //PushToEpicFromSqliteDB();
                    SyncData(ValidEmployees, deltaSyncObjects);
                    IsSyncCompleted = true;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Common.DeleteFile(FilePath);
                Logger.save();
            }
        }


        /// <summary>
        /// On Start
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            ServiceStartProcess();
            DisableOutlookPane(0);
        }

        public void ServiceStartProcess()
        {
            var intervalTimeDetail = GetIntervalTimeFromAPI();
            LoadTimerForClient(intervalTimeDetail.ClientActivityIntervalMinutes);
            LoadTimer(intervalTimeDetail.IntervalMinutes);
            timerDelay.Enabled = true;
            timerDelayForClient.Enabled = true;

            var firstSyncStatus = GetFirstSyncStatus();
            if (firstSyncStatus == 0)
            {
                LoadTimerForFirstSync();
                timerDelayForFirstSync.Enabled = true;
                UpdateFirstSyncStatus(1);
            }


            //ValidEmployees = GetValidEmployeesToBeSynced();
            //var deltaSyncObjects = GetDeltaSyncObjects(ValidEmployees, null, true);
            //SyncOnlyClient(ValidEmployees, deltaSyncObjects);
            //SyncProcess(ValidEmployees, deltaSyncObjects);
        }

        private void DisableOutlookPane(int isReadPaneToBeDisabled)
        {
            string outlookdisableUpdateQuery = $"update HIBOPDisablePane set IsPaneToBeDisabled = {isReadPaneToBeDisabled}";
            SqliteHelper.ExecuteCreateOrInsertQuery(outlookdisableUpdateQuery);
        }

        private void UpdateFirstSyncStatus(int status)
        {
            string FirstSyncQuery = $"update HIBOPFirstSyncStatus set IsFirstSyncFinished = {status}";
            SqliteHelper.ExecuteCreateOrInsertQuery(FirstSyncQuery);
        }
        private Int32 GetFirstSyncStatus()
        {
            var result = 0;
            string FirstSyncQuery = $"select IsFirstSyncFinished from HIBOPFirstSyncStatus";
            var sqliteDataReader = SqliteHelper.ExecuteSelectQuery(FirstSyncQuery);
            while (sqliteDataReader.Read())
            {
                try
                {
                    result = Convert.ToInt32(sqliteDataReader["IsFirstSyncFinished"]);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");
                }

            }
            return result;
        }

        private IntervalTimeDetail GetIntervalTimeFromAPI()
        {
            IntervalTimeDetail intervalTimeDetail = new IntervalTimeDetail();
            try
            {
                var serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.SyncIntervalTime);
                string intervalResponse = GetWebAPIResponse(serviceMethodURL);
                if (!string.IsNullOrEmpty(intervalResponse))
                {
                    intervalTimeDetail = JsonConvert.DeserializeObject<IntervalTimeDetail>(intervalResponse);
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return intervalTimeDetail;
        }

        private string GetWebAPIResponse(string serviceUrl)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpResponseMessage response = client.PostAsync(serviceUrl, null).Result;
                string result = "";
                using (HttpContent content = response.Content)
                {
                    Task<string> responseResult = content.ReadAsStringAsync();
                    result = responseResult.Result;
                }
                return result;
            }
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop()
        {
            if (timerDelay != null)
                timerDelay.Enabled = false;
            if (timerDelayForClient != null)
                timerDelayForClient.Enabled = false;
            if (timerDelayForFirstSync != null)
                timerDelayForFirstSync.Enabled = false;
            string FilePath = ConfigurationManager.AppSettings["DBFolderPath"]?.ToString();
            Common.DeleteFile(FilePath);
            DisableOutlookPane(1);
        }

        private bool IsOutlookOpen
        {
            get
            {
                return CheckOutlookOpened();
            }
        }
        private bool CheckOutlookOpened()
        {
            var result = false;
            try
            {
                int procCount = 0;


                Process[] processlist = Process.GetProcessesByName("OUTLOOK").ToArray();//Where(m => m.StartInfo.UserName == CurrentlyLoggedUserName)
                foreach (Process theprocess in processlist)
                {

                    procCount++;
                }
                if (procCount > 0)
                {
                    result = true;
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
            return result;
        }

        private void InitialiseFileWatcher(string filePath, FileSystemWatcher fileSystemWatcher)
        {
            fileSystemWatcher = new FileSystemWatcher();
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            fileSystemWatcher.Path = filePath;
            /* Watch for changes in LastAccess and LastWrite times, and  the renaming of files or directories. */
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Security | NotifyFilters.DirectoryName;
            // Only watch text files.
            fileSystemWatcher.Filter = "*.xml";
            fileSystemWatcher.InitializeLifetimeService();
            //FileWatcher.WaitForChanged(WatcherChangeTypes.All);

            // Add event handlers.
            fileSystemWatcher.Changed -= new FileSystemEventHandler(OnChanged);
            fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            //fileSystemWatcher.Created -= new FileSystemEventHandler(OnCreated);
            //fileSystemWatcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
            fileSystemWatcher.EnableRaisingEvents = true;
            GC.KeepAlive(fileSystemWatcher);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {

                if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Renamed)
                {
                    if (e.FullPath.Contains(XMLFolderType.AddIn.ToString()))
                    {
                        if (!xmlToSqliteHandler.MappingCollection.Contains(Path.GetFileNameWithoutExtension(e.FullPath)))
                            xmlToSqliteHandler.MappingCollection.Insert(0, Path.GetFileNameWithoutExtension(e.FullPath), e.FullPath);
                    }
                    else
                    {
                        if (!xmlToSqliteHandler.MappingCollection.Contains(Path.GetFileNameWithoutExtension(e.FullPath)))
                            xmlToSqliteHandler.MappingCollection.Add(Path.GetFileNameWithoutExtension(e.FullPath), e.FullPath);
                    }
                    xmlToSqliteHandler.UpdateXmltoSqlite();
                }
            }

        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            if (File.Exists(e.FullPath))
            {
                if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Renamed)
                {
                    if (e.FullPath.Contains(XMLFolderType.AddIn.ToString()))
                    {
                        if (!xmlToSqliteHandler.MappingCollection.Contains(Path.GetFileNameWithoutExtension(e.FullPath)))
                            xmlToSqliteHandler.MappingCollection.Insert(0, Path.GetFileNameWithoutExtension(e.FullPath), e.FullPath);
                    }
                    else
                    {
                        if (!xmlToSqliteHandler.MappingCollection.Contains(Path.GetFileNameWithoutExtension(e.FullPath)))
                            xmlToSqliteHandler.MappingCollection.Add(Path.GetFileNameWithoutExtension(e.FullPath), e.FullPath);
                    }
                    xmlToSqliteHandler.UpdateXmltoSqlite();
                }
            }
        }


        #region DataSync
        ///<summary>
        ///Sync data from centralized to local
        /// </summary>
        /// <returns></returns>
        private void SyncData(List<string> validEmployees, List<DeltaSyncObjectInfo> deltaSyncObjects)
        {
            try
            {
                if (_syncLocal == null)
                {
                    _syncLocal = new SyncLocal();
                    _syncLocal.SyncCompleted -= _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompletedForClient -= _syncLocal_SyncCompletedForClient;
                    _syncLocal.SyncCompletedForClient += _syncLocal_SyncCompletedForClient;
                }
                if (ValidEmployees != null && ValidEmployees.Count > 0 && deltaSyncObjects != null && deltaSyncObjects.Count > 0)
                    _syncLocal.SyncData(validEmployees, deltaSyncObjects);
                ExportAuditAndErrorLogFiles();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

        }

        private void SyncProcessForClient(object sender, System.Timers.ElapsedEventArgs e)
        {
            var intervalTimeDetail = GetIntervalTimeFromAPI();
            if (intervalTimeDetail != null)
            {
                timerDelayForClient.Interval = (intervalTimeDetail.ClientActivityIntervalMinutes != 0) ? (intervalTimeDetail.ClientActivityIntervalMinutes * 60 * 1000) : (20 * 60 * 1000);
                timerDelayForClient.Enabled = false;
                timerDelayForClient.Enabled = true;

                timerDelay.Interval = (intervalTimeDetail.IntervalMinutes != 0) ? (intervalTimeDetail.IntervalMinutes * 60 * 1000) : (10 * 60 * 1000);
                timerDelay.Enabled = false;
                timerDelay.Enabled = true;
            }

            ValidEmployees = GetValidEmployeesToBeSynced();
            if (ValidEmployees != null && ValidEmployees.Count > 0)
            {
                if (IsSyncCompletedForClient)
                {
                    var deltaSyncObjects = GetDeltaSyncObjects(ValidEmployees, true, false);
                    SyncOnlyClient(ValidEmployees, deltaSyncObjects);
                }
            }

        }

        private List<string> GetValidEmployeesToBeSynced()
        {
            List<string> ValidEmployeeList = new List<string>();
            try
            {
                if (_syncLocal == null)
                {
                    _syncLocal = new SyncLocal();
                    _syncLocal.SyncCompleted -= _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;
                    _syncLocal.SyncCompletedForClient -= _syncLocal_SyncCompletedForClient;
                    _syncLocal.SyncCompletedForClient += _syncLocal_SyncCompletedForClient;
                }
                ValidEmployeeList = _syncLocal.GetValidEmployees();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return ValidEmployeeList;
        }

        private List<DeltaSyncObjectInfo> GetDeltaSyncObjects(List<string> ValidEmployees, bool? isClient, bool? isFirstSync)
        {
            if (_syncLocal == null)
            {
                _syncLocal = new SyncLocal();
                _syncLocal.SyncCompleted -= _syncLocal_SyncCompleted;
                _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;
                _syncLocal.SyncCompletedForClient -= _syncLocal_SyncCompletedForClient;
                _syncLocal.SyncCompletedForClient += _syncLocal_SyncCompletedForClient;
            }
            return _syncLocal.GetDeltaSyncObjects(ValidEmployees, isClient, isFirstSync);
        }


        ///<summary>
        ///Sync data from centralized to local only for clients
        /// </summary>
        /// <returns></returns>
        private void SyncOnlyClient(List<string> ValidEmployees, List<DeltaSyncObjectInfo> deltaSyncObjects)
        {
            try
            {
                if (IsSyncCompletedForClient)
                {
                    if (_syncLocal == null)
                    {
                        _syncLocal = new SyncLocal();
                        _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;
                        _syncLocal.SyncCompletedForClient += _syncLocal_SyncCompletedForClient;
                    }
                    IsSyncCompletedForClient = false;
                    if (ValidEmployees != null && ValidEmployees.Count > 0 && deltaSyncObjects != null && deltaSyncObjects.Count > 0)
                        _syncLocal.SyncOnlyClient(ValidEmployees, deltaSyncObjects);
                    IsSyncCompletedForClient = true;
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

        }

        private void _syncLocal_SyncCompleted(bool status, bool isValidUser)
        {
            IsSyncCompleted = true;
            Common.DeleteFile(FilePath);
            //Logger.InfoLog($"Sync Completed - {DateTime.Now.ToString()}", typeof(SyncLocal), Logger.SourceType.WindowsService, "");
        }

        private void _syncLocal_SyncCompletedForClient(bool status, bool isValidUser)
        {
            IsSyncCompletedForClient = true;
            //Logger.InfoLog($"Sync Completed - {DateTime.Now.ToString()}", typeof(SyncLocal), Logger.SourceType.WindowsService, "");
        }

        ///<summary>
        ///Export audit and error log file to excel
        /// </summary>     
        private void ExportAuditAndErrorLogFiles()
        {

            string errorlookUpCode = string.Empty;
            try
            {

                string domain = ConfigurationManager.AppSettings["Domains"];
                List<string> userList = CommonHelper.GetUserandDomains(domain);
                foreach (var userDomain in userList)
                {
                    string[] UserandDomain = userDomain.Split('-');
                    string userName = UserandDomain[0];
                    string domainName = UserandDomain[1];
                    string UserNameWithDomain = string.Format(@"{0}\{1}", domainName, userName);
                    List<string> users = new List<string> { UserNameWithDomain };
                    List<string> lookUpCodes = CommonHelper.GetLookUpCodeByUser(users);
                    foreach (var lookUpCode in lookUpCodes)
                    {
                        if (!string.IsNullOrEmpty(lookUpCode))
                        {
                            errorlookUpCode = lookUpCode;
                            DataSet set = new DataSet();
                            DataTable auditlogDataTable = _syncLocal.GetExcelAuditLogDetails(lookUpCode);
                            auditlogDataTable.TableName = "HIBOPOutlookPluginLog";
                            set.Tables.Add(auditlogDataTable);

                            DataTable errorLogDataTable = _syncLocal.GetExcelErrorLogDetails(lookUpCode);
                            errorLogDataTable.TableName = "HIBOPErrorLog";
                            set.Tables.Add(errorLogDataTable);

                            if (auditlogDataTable.Rows.Count > 0 || errorLogDataTable.Rows.Count > 0)
                            {
                                //var excelFolderPath = Path.Combine($"C:\\HG IT Services\\{userDomain}", "OutlookEmail Attachmentlogs");

                                var excelFolderPath = Path.Combine(ConfigurationManager.AppSettings["OutlookEmailAttachmentPath"]?.ToString(), userDomain);

                                DeleteTwoWeekFile(excelFolderPath);
                                if (!Directory.Exists(excelFolderPath))
                                    Directory.CreateDirectory(excelFolderPath);

                                var fileName = DateTime.Now.ToString("yyyyMMdd") + "-EpicAttachmentLog-" + userName + ".xls";

                                string excelPath = Path.Combine(excelFolderPath, fileName);

                                ExportLogger _exportLogger = new ExportLogger();

                                foreach (var item in set.Tables)
                                {
                                    var dt = item as DataTable;
                                    _exportLogger.EXPORT_DATATABLE_TO_EXCEL_XLS_USE_NPOI(dt, dt.TableName, excelPath);
                                }
                                DeleteRecordFromLocalTable(set);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, errorlookUpCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void DeleteRecordFromLocalTable(DataSet set)
        {
            try
            {
                foreach (DataTable item in set.Tables)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        var LogId = row["LogId"];
                        var deleteQuery = string.Format("Delete from {0} where LogId={1}", item.TableName, LogId);
                        SqliteHelper.ExecuteCreateOrInsertQuery(deleteQuery);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
        }
        private void DeleteTwoWeekFile(string excelFolderPath)
        {
            try
            {
                if (Directory.Exists(excelFolderPath))
                {
                    string[] files = Directory.GetFiles(excelFolderPath);

                    foreach (string file in files)
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.CreationTime < DateTime.Now.AddDays(-14))
                        {
                            fi.Delete();
                        }
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
        }

        #endregion
    }
}


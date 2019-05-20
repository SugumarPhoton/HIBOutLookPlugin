using System;
using System.ServiceProcess;
using System.Configuration;
using System.Data;
using HIB.Outlook.Sync;
using System.DirectoryServices.AccountManagement;
using System.Timers;
using log4net;
using System.DirectoryServices;
using HIB.Outlook.Helper.Helper;
using System.IO;
using System.Runtime.InteropServices;

namespace HIB.Outlook.Service
{
    public partial class SyncService : ServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SyncService));

        System.Timers.Timer _timer;
        SyncLocal _syncLocal;


        public SyncService()
        {
            _syncLocal = new SyncLocal();
            _syncLocal.SyncCompleted += _syncLocal_SyncCompleted;

        //    SyncData();         
            InitializeComponent();

        }

        private void _syncLocal_SyncCompleted(bool status)
        {

        }

        ///<summary>
        ///Sync data from centralized to local
        /// </summary>
        /// <returns></returns>
        public void SyncData()
        {
            try
            {
                Logger.Info("Sync data Method Called");

                //_syncLocal.SyncData(serviceMethodURL, "A0002");
                if (_syncLocal.SyncData())
                {
                    //Log (Successfully Loaded)
                }
                Logger.Info("Export Excel Called");
                ExportAuditAndErroLogFiles();
                Logger.Info(" Excel Generated Called");

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        ///<summary>
        ///Export audit and error log file to excel
        /// </summary>     
        public void ExportAuditAndErroLogFiles()
        {
            try
            {
                DataSet set = new DataSet();
                DataTable auditlogDataTable = _syncLocal.GetExcelAuditLogDetails();
                Logger.Debug($"auditlogDataTable Count:{auditlogDataTable.Rows.Count}");
                set.Tables.Add(auditlogDataTable);
                DataTable errorLogDataTable = _syncLocal.GetExcelErrorLogDetails();
                set.Tables.Add(errorLogDataTable);
                var excelFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OutlookEmail Attachmentlogs");
                Logger.Debug($"Appdata Path :{excelFolderPath1} ");
                var userName = System.Convert.ToString(ConfigurationManager.AppSettings["UserName"]); //Environment.UserName.ToString()

                var excelFolderPath = Path.Combine($"C:\\Users\\{userName}\\AppData\\Roaming", "OutlookEmail Attachmentlogs");

                //"
                if (!Directory.Exists(excelFolderPath))
                    Directory.CreateDirectory(excelFolderPath);

                var fileName = DateTime.Now.ToString("yyyyMMdd") + "-EpicAttachmentLog-" + userName + ".xls";  

                string excelPath = Path.Combine(excelFolderPath, fileName);
                Logger.Debug($"File Path:{excelPath}");

                ExportLogger _exportLogger = new ExportLogger();

                foreach (var item in set.Tables)
                {
                    var dt = item as DataTable;
                    _exportLogger.EXPORT_DATATABLE_TO_EXCEL_XLS_USE_NPOI(dt, dt.TableName, excelPath);
                }
                // _exportLogger.GenerateExcelFile(set, excelPath);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        protected override void OnStart(string[] args)
        {
            try
            {

                Logger.Error("Sync Service onStart");
                //  SyncData();

                double interval = Convert.ToDouble(ConfigurationManager.AppSettings["Interval"]);
                _timer = new Timer();
                _timer.Interval = (interval != 0) ? (interval * 60 * 1000) : (5 * 60 * 1000);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                _timer.Enabled = true;
                Logger.Error("Sync Service Started");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {

                SyncData();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

    }


}


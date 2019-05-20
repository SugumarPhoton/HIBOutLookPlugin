
using HIB.Outlook.SQLite.Repository;
using System;
using System.Configuration;
using System.ServiceProcess;

namespace HIB.Outlook.Service
{
    static class Program
    {
        // private static readonly ILog Logger = null;
        static Program()
        {
            //string file = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["LogConfigPath"]);
            ////string file = @"C:\HIB.Projects\HIBOUTLOOK\Source\HIB.Outlook.Service\"+ ConfigurationManager.AppSettings["LogConfigPath"];
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(file));
            //string userName = Environment.UserName;
            //var userId = userName.Split('\\');
            //if (userId.Length == 1)
            //{
            //    GlobalContext.Properties["LoggedBy"] = userId[0];
            //}
            //else if (userId.Length > 1)
            //{
            //    GlobalContext.Properties["LoggedBy"] = userId[1];
            //}
            //GlobalContext.Properties["source"] = "Windows Service";

            // Logger = LogManager.GetLogger(typeof(Program));
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if DEBUG
            ////////////, new EpicSyncService()
            //  new EpicSyncService().PushToEpicFromSqliteDB();

            ////////////, new EpicSyncService()
            //new EpicSyncService().PushToEpicFromSqliteDB();

            //////////////, new EpicSyncService()
            ////new EpicSyncService().PushToEpicFromSqliteDB();
            // Logger.Info("Check");
            var service = new EpicSyncService();
            service.ServiceStartProcess();
            
            //service._syncLocal = new Sync.SyncLocal();
            //service.SyncData();
            CommonProperties.LocalIPAddress();
            // service.ParallelSyncForTest();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
             new EpicSyncService()
            };
            ServiceBase.Run(ServicesToRun);

#endif




        }
    }
}

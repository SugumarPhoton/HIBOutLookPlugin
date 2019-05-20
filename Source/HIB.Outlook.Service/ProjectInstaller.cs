using HIB.Outlook.Helper.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace HIB.Outlook.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {

        public ProjectInstaller()
        {
            InitializeComponent();
            // this.Committed += new InstallEventHandler(ServiceInstaller_Committed);
        }

        void ServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
            {
                serviceController.Start();
            }
        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            try
            {
                base.OnAfterInstall(savedState);
                try
                {
                    File.Delete(@"C:\HG IT Services\HIBOutlook.db-wal");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }
                try
                {
                    File.Delete(@"C:\HG IT Services\HIBOutlook.db-shm");
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }
                using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
                {
                    serviceController.Start();
                }
                GrantAccess(folder);
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

        string folder = @"C:\HG IT Services";
        private static void GrantAccess(string folder)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(folder);
                if (!exists)
                {
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(folder);
                    //Logger.InfoLog("The Folder is created Sucessfully", typeof(ProjectInstaller), Logger.SourceType.WindowsService, "");

                }
                else
                {
                    // Logger.InfoLog("The Folder already exists", typeof(ProjectInstaller), Logger.SourceType.WindowsService, "");
                }
                DirectoryInfo dInfo = new DirectoryInfo(folder);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);

                //Logger.InfoLog("Folder Permission assigned to everyone", typeof(ProjectInstaller), Logger.SourceType.WindowsService, "");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("Folder Permission assigning failed to everyone", Logger.SourceType.WindowsService, "");
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            try
            {
                string FilePath = ConfigurationManager.AppSettings["DBFolderPath"]?.ToString();
                Common.DeleteFile(FilePath);
                using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
                {
                    if (serviceController != null && serviceController.CanStop)
                        serviceController.Stop();
                }
                try
                {
                    string notificationPath = ConfigurationManager.AppSettings["NotificationFolderPath"]?.ToString();
                    System.IO.DirectoryInfo di = new DirectoryInfo(notificationPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    //Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }
                try
                {
                    File.Delete(ConfigurationManager.AppSettings["SqliteDBWALPath"]?.ToString());
                }
                catch (Exception ex)
                {
                    // Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }
                try
                {
                    File.Delete(ConfigurationManager.AppSettings["SqliteDBSHMPath"]?.ToString());
                }
                catch (Exception ex)
                {
                    //Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }
                try
                {
                    Directory.Delete(ConfigurationManager.AppSettings["MainFolderPath"]?.ToString(), true);
                }
                catch (Exception ex)
                {
                    //Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
                }

                base.OnBeforeUninstall(savedState);
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

    }
}

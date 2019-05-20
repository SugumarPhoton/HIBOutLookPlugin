using HIB.Outlook.Helper.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace HIB.Outlook.RestartService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            try
            {
                base.OnAfterInstall(savedState);

                using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
                {
                    serviceController.Start();
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
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            try
            {
                using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
                {
                    if (serviceController != null && serviceController.CanStop)
                        serviceController.Stop();
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

using HIB.Outlook.Helper.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HIB.Outlook.RestartService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timerServiceReset;
        DateTime _scheduleTime;
        public Service1()
        {
            InitializeComponent();
            timerServiceReset = new System.Timers.Timer();
            double RestartTime = 0;
            var restartTimeInHour = ConfigurationManager.AppSettings["RestartTime"]?.ToString();
            if (!string.IsNullOrEmpty(restartTimeInHour))
            {
                RestartTime = Convert.ToDouble(restartTimeInHour);
            }
            else
            {
                RestartTime = 4;
            }
            _scheduleTime = DateTime.Today.AddDays(1).AddHours(RestartTime);
        }

        protected override void OnStart(string[] args)
        {
            ServiceResetTimer();
        }
        public void ServiceReset(object sender, System.Timers.ElapsedEventArgs e) //
        {
            try
            {
                // Logger.InfoLog("Service going to be restarted", typeof(Service1), Logger.SourceType.WindowsService, "");
                ServiceController service = new ServiceController("Epic Attachment Service");
                TimeSpan timeout = TimeSpan.FromMinutes(1);

                if (service.Status != ServiceControllerStatus.Stopped && service.CanStop)
                {
                    // Stop Service
                    service.Stop();
                }
                CloseAllOutlookWindows();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                DeleteTempFilesCreatedBySqlite();
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                //Logger.InfoLog("Service restarted", typeof(Service1), Logger.SourceType.WindowsService, "");

                if (timerServiceReset.Interval != 24 * 60 * 60 * 1000)
                {
                    timerServiceReset.Interval = 24 * 60 * 60 * 1000;
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

        private void DeleteTempFilesCreatedBySqlite()
        {
            try
            {
                File.Delete(ConfigurationManager.AppSettings["SqliteDBWALPath"]?.ToString());
                File.Delete(ConfigurationManager.AppSettings["SqliteDBSHMPath"]?.ToString());
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.Message, Logger.SourceType.WindowsService, "");
            }
        }
        private void CloseAllOutlookWindows()
        {
            Process[] processlist = Process.GetProcessesByName("OUTLOOK").ToArray();//Where(m => m.StartInfo.UserName == CurrentlyLoggedUserName)
            foreach (Process theprocess in processlist)
            {
                try
                {
                    theprocess.Kill();
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        public void ServiceResetTimer()
        {
            try
            {

                timerServiceReset = new System.Timers.Timer();
                timerServiceReset.Enabled = true;
                if (DateTime.Now > _scheduleTime)
                    _scheduleTime = _scheduleTime.AddHours(24);
                timerServiceReset.Interval = _scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;
                // timerServiceReset.Interval = 20 * 60 * 1000;
                timerServiceReset.Elapsed += new System.Timers.ElapsedEventHandler(ServiceReset);

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

        protected override void OnStop()
        {
            if (timerServiceReset != null)
                timerServiceReset.Enabled = false;
        }
    }
}

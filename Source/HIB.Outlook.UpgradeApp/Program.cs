using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.UpgradeApp
{
    class Program
    {
        public const string installCommand = "/qn /i \"{0}\" ALLUSERS=1";
        public const string uninstallappCommand = "wmic product where name='HGITS Outlook Attachment Plugin' call uninstall";
        static void Main(string[] args)
        {
            UninstallIISInWebServerMachine(uninstallappCommand);
            var filePackageName = string.Format(@"{0}\{1}", System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "HGITS Outlook Attachment Plugin.msi");
            var installCommandEXE = string.Format(installCommand, filePackageName);
            var fileName = "msiexec.exe";
            InstallOrUninstallPrerequisiteInTargetServer(installCommandEXE, fileName);            
        }
       
        public static void UninstallIISInWebServerMachine(string Command)
        {
            try
            {
                ProcessStartInfo pStartInfo = new ProcessStartInfo("cmd.exe", "/c " + Command);
                pStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pStartInfo.CreateNoWindow = true;
                pStartInfo.UseShellExecute = false;
                Process p = new Process
                {
                    StartInfo = pStartInfo
                };
                p.Start();
                
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal static bool InstallOrUninstallPrerequisiteInTargetServer(string arguments, string fileName)
        {
            var result = false;
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                result = true;
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message, ex);
                result = false;
            }
            return result;
        }




    }
}

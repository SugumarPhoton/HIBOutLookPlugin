using HIB.Outlook.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Helper.Common
{
    public static class Logger
    {
        public enum ErrorType
        {
            Error,
            Info
        }

        public enum SourceType
        {
            AddIn,
            WindowsService
        }
        public static List<ErrorLogInfo> LogInfoList = new List<ErrorLogInfo>();
        public static void ErrorLog(object Error, SourceType sourceType, string userName, Type className = default(Type))
        {
            try
            {

                ErrorLogInfo errorLog = new ErrorLogInfo();
                errorLog.LogDate = DateTime.Now;
                errorLog.Source = sourceType.ToString();//memberInfo.GetCustomAttribute<DescriptionAttribute>().ToString();
                errorLog.Thread = 1;
                errorLog.Level = ErrorType.Error.ToString();
                if (Error is Exception)
                {
                    var ex = Error as Exception;
                    errorLog.Logger = ExtractBracketed(new StackTrace(ex).GetFrame(0).GetMethod().ReflectedType.FullName);
                    errorLog.Message = ex.ToString().Replace("'", "''"); 
                }
                else if (Error is string)
                {
                    if (className != null)
                        errorLog.Logger = className.Name;
                    errorLog.Message = Error as string;
                }
                errorLog.LoggedBy = userName;
                LogInfoList.Add(errorLog);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, userName);
            }
        }


        public static void InfoLog(string message, Type className, SourceType sourceType, string userName)
        {
            try
            {
                ErrorLogInfo errorLog = new ErrorLogInfo();
                errorLog.LogDate = DateTime.Now;
                errorLog.Source = sourceType.ToString(); //memberInfo.GetCustomAttribute<DescriptionAttribute>().ToString();
                errorLog.Thread = 1;
                errorLog.Level = ErrorType.Info.ToString();
                errorLog.Logger = className.Name;
                errorLog.Message = message;
                errorLog.LoggedBy = userName;
                LogInfoList.Add(errorLog);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, userName);
            }
        }

        public static void save()
        {
            try
            {
                var addinErrorList = LogInfoList.Where(x => x.Source == SourceType.AddIn.ToString()).ToList();
                var ServiceErrorList = LogInfoList.Where(x => x.Source == SourceType.WindowsService.ToString()).ToList();


                LogInfoList = new List<ErrorLogInfo>();
                if (addinErrorList.Count > 0)
                    XMLSerializeHelper.Serialize<ErrorLogInfo>(addinErrorList, XMLFolderType.AddIn, "AddInErrorLog");
                if (ServiceErrorList.Count > 0)
                    XMLSerializeHelper.Serialize<ErrorLogInfo>(ServiceErrorList, XMLFolderType.Service, "ServiceErrorLog");

            }
            catch (Exception)
            {
            }
        }

        private static string ExtractBracketed(string str)
        {
            string s;
            if (str.IndexOf('<') > -1) //using the Regex when the string does not contain <brackets> returns an empty string.
                s = System.Text.RegularExpressions.Regex.Match(str, @"\<([^>]*)\>").Groups[1].Value;
            else
                s = str;
            if (s == "")
                return "'Emtpy'"; //for log visibility we want to know if something it's empty.
            else
                return s;

        }

        private static string GetExecutingMethodName(Exception exception)
        {
            var s = new StackTrace(exception);
            var currentAssembly = Assembly.GetExecutingAssembly();
            var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == currentAssembly).Name;

            return methodname;
        }


    }
}

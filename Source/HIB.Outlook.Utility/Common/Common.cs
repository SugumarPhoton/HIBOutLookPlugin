using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace HIB.Outlook.Utility.Common
{
    public class Common : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _databaseName;
        private static string _authenticationKey;
        private static string _accountTypeCode;
        private static string _attachedToType;
        private static string _securityAccessLevelCode;
        private static string _typeOfServiceBinding;
        private static string _fileAttachmentSummary;
        private static string _emptyAttachFileName;

        public static string DatabaseName
        {
            get
            {
                if (string.IsNullOrEmpty(_databaseName))
                {
                   // if (ConfigurationManager.AppSettings["DatabaseName"] != null)
                        _databaseName = "PhotonTest";// ConfigurationManager.AppSettings["DatabaseName"];
                }
                return _databaseName;
            }
        }

        public static string AuthenticationKey
        {
            get
            {
                if (string.IsNullOrEmpty(_authenticationKey))
                {
                    //  if (ConfigurationManager.AppSettings["AuthenticationKey"] != null)
                    _authenticationKey = "Zd9O18zALj/9/UxflcTOZnhwIXVURh24w64NpJUjPbQ="; //ConfigurationManager.AppSettings["AuthenticationKey"];
                }
                return _authenticationKey;
            }
        }

        public static string AccountTypeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_accountTypeCode))
                {
                    if (ConfigurationManager.AppSettings["AccountTypeCode"] != null)
                        _accountTypeCode = ConfigurationManager.AppSettings["AccountTypeCode"];
                }
                return _accountTypeCode;
            }
        }

        public static string AttachedToType
        {
            get
            {
                if (string.IsNullOrEmpty(_attachedToType))
                {
                    if (ConfigurationManager.AppSettings["AttachedToType"] != null)
                        _attachedToType = ConfigurationManager.AppSettings["AttachedToType"];
                }
                return _attachedToType;
            }
        }

        public static string SecurityAccessLevelCode
        {
            get
            {
                if (string.IsNullOrEmpty(_securityAccessLevelCode))
                {
                    if (ConfigurationManager.AppSettings["SecurityAccessLevelCode"] != null)
                        _securityAccessLevelCode = ConfigurationManager.AppSettings["SecurityAccessLevelCode"];
                }
                return _securityAccessLevelCode;
            }
        }

        public static string TypeOfServiceBinding
        {
            get
            {
                if (string.IsNullOrEmpty(_typeOfServiceBinding))
                {
                    if (ConfigurationManager.AppSettings["TypeOfServiceBinding"] != null)
                        _typeOfServiceBinding = ConfigurationManager.AppSettings["TypeOfServiceBinding"];
                }
                return _typeOfServiceBinding;
            }
        }


        public static string FileAttachSummary
        {
            get
            {
                if (string.IsNullOrEmpty(_fileAttachmentSummary))
                {
                    if (ConfigurationManager.AppSettings["FileAttachSummary"] != null)
                        _fileAttachmentSummary = ConfigurationManager.AppSettings["FileAttachSummary"];
                }
                return _fileAttachmentSummary;
            }
        }

        public static string EmptyAttachFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_emptyAttachFileName))
                {
                    if (ConfigurationManager.AppSettings["EmptyAttachFileName"] != null)
                        _emptyAttachFileName = ConfigurationManager.AppSettings["EmptyAttachFileName"];
                }
                return _emptyAttachFileName;
            }
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            return str.Length <= maxLength ? str : str.Remove(maxLength);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str.Replace('"', '\'').Replace('\\', ' ').Replace('/', ' ').Replace('|', ' ').Replace('<', ' ').Replace('>', ' ').Replace('?', ' ').Replace('*', ' ').Replace(':', ' ');
            else
                return string.Empty;
        }

        public static string EmptyFileName(string str)
        {
            if (str != null && str.Length > 0) str = str.Trim();

            if (string.IsNullOrEmpty(str))
                return EmptyAttachFileName;
            else
                return str;
        }

        public void Dispose()
        {

        }

        public Exception ServiceException { get; set; }

        public static byte[] GetFileBytes(string filename)
        {
            var fileBytes = new byte[] { };
            try
            {
                fileBytes = File.ReadAllBytes(filename);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return fileBytes;
        }

        public string DateTimeSQLite(DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }

}

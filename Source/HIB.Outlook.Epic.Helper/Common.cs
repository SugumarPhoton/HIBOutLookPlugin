using System;
using System.Configuration;

namespace HIB.Outlook.Epic.Helper
{
    public class Common : IDisposable
    {
        private static string _databaseName;
        private static string _authenticationKey;
        private static string _accountTypeCode;
        private static string _attachedToType;
        private static string _securityAccessLevelCode;
        private static string _serviceBinding;
        private static string _typeOfServiceBinding;
        private static string _fileAttachmentSummary;
        private static string _emptyAttachFileName;

        public static string DatabaseName
        {
            get
            {
                if (string.IsNullOrEmpty(_databaseName))
                {
                    if (ConfigurationManager.AppSettings["DatabaseName"] != null)
                        _databaseName = ConfigurationManager.AppSettings["DatabaseName"];
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
                    if (ConfigurationManager.AppSettings["AuthenticationKey"] != null)
                        _authenticationKey = ConfigurationManager.AppSettings["AuthenticationKey"];
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
    }
}

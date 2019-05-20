using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Helper.Common
{
    public class Constants
    {
        public class ToastNotifications
        {
            public const string PermissionHeaderText = "You do not have right permission to use 'EPIC Attachment Add-in'.";
            public const string PermissionContentText = "Please contact EPIC Admin.";

            public const string DataSyncHeaderText = "Data sync with EPIC Status";
            public const string DataSyncStartedContentText = "Data sync with EPIC started";
            public const string DataSyncCompletedContentText = "Data sync with EPIC successfully completed.";
            public const string DataSyncFailedContentText = "Data sync with EPIC failed. Please contact EPIC Admin.";



        }
    }
}

using System;

namespace HIB.Outlook.Model
{
    public class SyncLog
    {
        public string Fields { get; set; }
        public DateTime? SyncDate { get; set; }
        public DateTime? UserSyncDate { get; set; }
        public string UserName { get; set; }

    }
}

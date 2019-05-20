using System;


namespace HIB.Outlook.Model
{
    public class NotificationInfo
    {
        public Int32 NotificationType { get; set; } // 0- Started , 1- Completed,2 -Insufficient Permission, 3- Error, 4 - AttachmentSuccess
        public string TitleMessage { get; set; }
        public string ContentMessage { get; set; }
        public string UserName { get; set; }
        public string LookupCode { get; set; }
    }
}

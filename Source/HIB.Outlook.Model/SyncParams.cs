using System;


namespace HIB.Outlook.Model
{
    public class SyncParams
    {
        public string UserId { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public int RowsPerPage { get; set; }
        public int PageNumber { get; set; }
        public string IPAddress { get; set; }
        public bool? IsClient { get; set; }
        public bool? isFirstSync { get; set; }
    }
}

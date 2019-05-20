using System;

namespace HIB.Outlook.Sync
{
    public class ActivityListInfo
    {
        public int ActivityCodeId { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}

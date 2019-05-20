using System;

namespace HIB.Outlook.Sync
{
    public class ActivityClientContactInfo
    {
        #region Public Members
        public int ClientContactId { get; set; }
        public Nullable<int> ContactNumberId { get; set; }
        public int EntityId { get; set; }
        public int ContactNameId { get; set; }
        public string ContactName { get; set; }
        public string ContactType { get; set; }
        public string ContactValue { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}

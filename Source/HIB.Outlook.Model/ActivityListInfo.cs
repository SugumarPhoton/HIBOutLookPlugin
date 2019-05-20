
using System;

namespace HIB.Outlook.Model
{
    public class ActivityListInfo
    {
        #region Public Members
        public int ActivityCodeId { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string lookupCode { get; set; }
        public string EmployeeName { get; set; }
        public string UserLookupCode { get; set; }
        public int IsClosedStatus { get; set; }

        #endregion
    }
}

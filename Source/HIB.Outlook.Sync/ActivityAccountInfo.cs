
using System;

namespace HIB.Outlook.Sync
{
    public class ActivityAccountInfo
    {
        #region Public Members
        public Nullable<long> RowNumber { get; set; }
        public int EntityId { get; set; }
        public int AgencyId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}

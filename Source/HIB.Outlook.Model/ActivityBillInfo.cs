using System;
using System.ComponentModel;

namespace HIB.Outlook.Model
{
    public class ActivityBillInfo
    {
        #region Public Members
        [DisplayName("No display")]
        public int BillId { get; set; }
        [DisplayName("No display")]
        public Nullable<int> EntityId { get; set; }

        [DisplayName("No display")]
        public Nullable<int> TransheadId { get; set; }

        [DisplayName("Bill #")]
        public Nullable<int> BillNumber { get; set; }
        [DisplayName("No display")]
        public Nullable<int> AgencyId { get; set; }

        [DisplayName("Agency")]
        public string AgencyName { get; set; }

        public Nullable<decimal> Amount { get; set; }

        public Nullable<decimal> Balance { get; set; }

        [DisplayName("No display")]
        public Nullable<System.DateTime> InsertedDate { get; set; }

        [DisplayName("No display")]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        [DisplayName("No display")]
        public string UserLookupCode { get; set; }
        #endregion
    }
}

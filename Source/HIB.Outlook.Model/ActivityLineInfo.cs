using System;


namespace HIB.Outlook.Model
{
    public class ActivityLineInfo
    {
        #region Public Members
        public Nullable<int> LineId { get; set; }
        public Nullable<int> PolicyId { get; set; }
        public Nullable<int> EntityId { get; set; }
        public string PolicyType { get; set; }
        public string PolicyDesc { get; set; }
        public string LineCode { get; set; }
        public string LineOfBusiness { get; set; }
        public string LineStatus { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<int> PolicyLineTypeId { get; set; }
        public Nullable<int> LineStatusId { get; set; }
        public string IOC { get; set; }
        public string BillModeCode { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UserLookupCode { get; set; }
        public int? Flags { get; set; }
        #endregion

    }
}

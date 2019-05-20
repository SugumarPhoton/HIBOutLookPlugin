using System;


namespace HIB.Outlook.Sync
{
    public class ActivityInfo
    {
        #region Public Members
        public int EntityId { get; set; }
        public int ActivityId { get; set; }
        public Nullable<int> ActivityIdCode { get; set; }
        public string ActivityCode { get; set; }
        public string DescriptionOf { get; set; }
        public string CdPolicyLineTypeCode { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Status { get; set; }
        #endregion
    }
}

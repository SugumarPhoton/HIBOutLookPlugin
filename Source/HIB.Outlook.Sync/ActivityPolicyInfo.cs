using System;


namespace HIB.Outlook.Sync
{
    public class ActivityPolicyInfo
    {
        public int EntityId { get; set; }
        public string PolicyLineTypeCode { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string DescriptionOf { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string PolicyStatus { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}

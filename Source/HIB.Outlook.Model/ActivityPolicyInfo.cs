using System;


namespace HIB.Outlook.Model
{
    public class ActivityPolicyInfo
    {
        #region Public Members
        public string EmployeeLookUpCode { get; set; }
        public int EntityId { get; set; }
        public string PolicyLineTypeCode { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string DescriptionOf { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PolicyStatus { get; set; }
        public string Status { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserLookupCode { get; set; }
        public int? Flags { get; set; }
        #endregion
    }
}

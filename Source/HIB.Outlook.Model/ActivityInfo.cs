using System;

namespace HIB.Outlook.Model
{
    public class ActivityInfo
    {
        #region Public Members
        public int EntityId { get; set; }
        public int ActivityId { get; set; }
        public Nullable<int> ActivityCodeId { get; set; }
        public string ActivityCode { get; set; }
        public string DescriptionOf { get; set; }
        public Nullable<int> CdPolicyLineTypeId { get; set; }
        public string CdPolicyLineTypeCode { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> AgencyId{ get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ProfitCenterId { get; set; }
        public Nullable<int> AssociatedItemId { get; set; }
        public string AssociationType { get; set; }
        public string UserLookupCode { get; set; }
        public string OwnerName { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<int> UniqPolicy { get; set; }
        public Nullable<int> UniqLine { get; set; }
        public Nullable<int> UniqClaim { get; set; }
        public Nullable<System.DateTime> LossDate { get; set; }
        public string Policydescription { get; set; }
        public string LineCode { get; set; }
        public string LineDescription { get; set; }
        public string ICO { get; set; }
        public Nullable<System.DateTime> LineEffectiveDate { get; set; }
        public Nullable<System.DateTime> LineExpirationDate { get; set; }
        #endregion
    }
}

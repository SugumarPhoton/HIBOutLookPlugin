using System;


namespace HIB.Outlook.Model
{
    public class ActivityLookUpInfo
    {
        public int ALDId { get; set; }
        public int LineId { get; set; }
        public int PolicyId { get; set; }
        public int EntityId { get; set; }
        public Nullable<int> ClaimId { get; set; }
        public string LineCode { get; set; }
        public string PolicyType { get; set; }
        public string Linedescription { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyDesc { get; set; }
        public int CdPolicyLineTypeId { get; set; }
        public int CdLineStatusId { get; set; }
        public System.DateTime LineExpDate { get; set; }
        public System.DateTime LineEffDate { get; set; }
        public System.DateTime PolicyExpDate { get; set; }
        public System.DateTime PolicyEffDate { get; set; }
        public Nullable<int> ClaimNumber { get; set; }
        public string CompanyClaimNumber { get; set; }
        public Nullable<System.DateTime> DateLoss { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public string LookupCode { get; set; }
        public string AccountName { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ClaimAssociationId { get; set; }
        public string IOC { get; set; }
        public string IOCCode { get; set; }
        public string AttachmentDesc { get; set; }
        
        public string UserLookupCode { get; set; }
    }
}

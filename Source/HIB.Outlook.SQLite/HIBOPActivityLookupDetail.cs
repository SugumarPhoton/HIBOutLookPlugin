//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIB.Outlook.SQLite
{
    using System;
    using System.Collections.Generic;
    
    public partial class HIBOPActivityLookupDetail
    {
        public int ALDId { get; set; }
        public int UniqLine { get; set; }
        public int UniqPolicy { get; set; }
        public long UniqEntity { get; set; }
        public Nullable<int> UniqClaim { get; set; }
        public string LineCode { get; set; }
        public string PolicyType { get; set; }
        public string Linedescription { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyDesc { get; set; }
        public int UniqCdPolicyLineType { get; set; }
        public int UniqCdLineStatus { get; set; }
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
        public Nullable<int> UniqClaimAssociation { get; set; }
        public string IOC { get; set; }
        public string IOCCode { get; set; }
        public string AttachmentDesc { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
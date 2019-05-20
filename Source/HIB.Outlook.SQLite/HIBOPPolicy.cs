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
    
    public partial class HIBOPPolicy
    {
        public int UniqPolicy { get; set; }
        public long UniqEntity { get; set; }
        public Nullable<int> UniqAgency { get; set; }
        public Nullable<int> UniqBranch { get; set; }
        public string DescriptionOf { get; set; }
        public string CdPolicyLineTypeCode { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string PolicyStatus { get; set; }
        public Nullable<int> Flags { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
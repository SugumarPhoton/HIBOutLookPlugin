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
    
    public partial class HIBOPActivityAccount
    {
        public int AccountId { get; set; }
        public long UniqEntity { get; set; }
        public Nullable<int> UniqAgency { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public Nullable<int> UniqBranch { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string LookupCode { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}

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
    
    public partial class HIBOPActivityLine
    {
        public int UniqLine { get; set; }
        public Nullable<int> UniqPolicy { get; set; }
        public long UniqEntity { get; set; }
        public string PolicyDesc { get; set; }
        public string LineCode { get; set; }
        public string LineOfBusiness { get; set; }
        public string LineStatus { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<int> UniqCdPolicyLineType { get; set; }
        public Nullable<int> UniqCdLineStatus { get; set; }
        public string IOC { get; set; }
        public string BillModeCode { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<int> Flags { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
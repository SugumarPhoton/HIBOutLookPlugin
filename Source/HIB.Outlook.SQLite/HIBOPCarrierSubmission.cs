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
    
    public partial class HIBOPCarrierSubmission
    {
        public int CarrierSubmissionId { get; set; }
        public Nullable<int> UniqCarrierSubmission { get; set; }
        public string Carrier { get; set; }
        public string CarrierSubmission { get; set; }
        public Nullable<int> UniqMarketingSubmission { get; set; }
        public string MarkettingSubmission { get; set; }
        public long UniqEntity { get; set; }
        public Nullable<System.DateTime> LastSubmittedDate { get; set; }
        public Nullable<decimal> RequestedPremium { get; set; }
        public string SubmissionStatus { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
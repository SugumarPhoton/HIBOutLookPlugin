//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIB.Outlook.DAL
{
    using System;
    
    public partial class HIBOPGetActivityMarketing_SP_Result
    {
        public int UniqMarketingSubmission { get; set; }
        public Nullable<int> UniqEntity { get; set; }
        public Nullable<int> UniqAgency { get; set; }
        public Nullable<int> UniqBranch { get; set; }
        public string MarketingSubbmission { get; set; }
        public string LineOfBusiness { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> LastSubmittedDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> Flags { get; set; }
    }
}
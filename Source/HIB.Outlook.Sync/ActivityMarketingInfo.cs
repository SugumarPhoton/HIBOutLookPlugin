using System;

namespace HIB.Outlook.Sync
{
    public class ActivityMarketingInfo
    {
        public int UniqMarketingSubmission { get; set; }
        public int UniqEntity { get; set; }
        public int UniqAgency { get; set; }
        public int UniqBranch { get; set; }
        public string MarketingSubbmission { get; set; }
        public string LineOfBusiness { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> LastSubmittedDate { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}

using System;
using System.ComponentModel;

namespace HIB.Outlook.Model
{
    public class ActivityCarrierInfo
    {
        #region Public Members
        [DisplayName("No display")]
        public int CarrierSubmissionId { get; set; }
        [DisplayName("No display")]
        public Nullable<int> UniqCarrierSubmission { get; set; }

        [DisplayName("Master marketing submission")]
        public string MarketingSubmission { get; set; }

        public string Carrier { get; set; }
        [DisplayName("Carrier Submission")]
        public string CarrierSubmission { get; set; }
        [DisplayName("No display")]
        public Nullable<int> MarketingSubmissionId { get; set; }

        [DisplayName("No display")]
        public Nullable<int> EntityId { get; set; }

        [DisplayName("Last submitted")]
        public Nullable<System.DateTime> LastSubmittedDate { get; set; }
        [DisplayName("Requested Premium")]
        public Nullable<decimal> RequestedPremium { get; set; }
        [DisplayName("Submission status")]
        public string SubmissionStatus { get; set; }
        [DisplayName("No display")]
        public string UserLookupCode { get; set; }
        #endregion
    }
}

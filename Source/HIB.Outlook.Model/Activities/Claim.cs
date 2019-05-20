using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Claim
    {
        [DisplayName("No display")]
        public Int64 ClaimId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        [DisplayName("Date of Loss")]
        public string DateOfLoss { get; set; }

        [DisplayName("Date Reported")]
        public string DateReported { get; set; }

        [DisplayName("Agency Claim Number")]
        public Int64 AgencyClaimNumber { get; set; }

        [DisplayName("Co Claim Number")]
        public string CoClaimNumber { get; set; }

        [DisplayName("Date Closed")]
        public string DateClosed { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }
    }
}

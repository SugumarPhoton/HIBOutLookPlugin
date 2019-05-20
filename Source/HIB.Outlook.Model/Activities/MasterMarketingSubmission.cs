using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class MasterMarketingSubmission
    {
        [DisplayName("No display")]
        public Int64 MasterMarketingId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        [DisplayName("Master Marketing Submission")]
        public string MasterMarketing { get; set; }

        [DisplayName("Lines of Business")]
        public string LinesOfBusiness { get; set; }
        public string Effective { get; set; }
        public string Expiration { get; set; }

        [DisplayName("Last Submitted")]
        public string LastSubmitted { get; set; }

        [DisplayName("No display")]
        public Int32 Flags { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }
    }
}

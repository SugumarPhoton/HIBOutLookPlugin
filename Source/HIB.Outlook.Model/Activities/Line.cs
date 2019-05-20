using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Line
    {
        [DisplayName("No display")]
        public Int64 LineId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        [DisplayName("No display")]
        public Int64 UniqPolicy { get; set; }

        [DisplayName("Line")]        
        public string LineCode { get; set; }

        [DisplayName("Line of Business")]
        public string LineOfBusiness { get; set; }

        public string Status { get; set; }
        public string Effective { get; set; }
        public string Expiration { get; set; }
        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        public string ICO { get; set; }

        public string Billing { get; set; }

        [DisplayName("Policy Description")]
        public string PolicyDescription { get; set; }

        [DisplayName("No display")]
        public Int32 Flags { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }

    }
}

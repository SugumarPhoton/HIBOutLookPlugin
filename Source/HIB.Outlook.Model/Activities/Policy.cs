using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Policy
    {
        [DisplayName("No display")]
        public Int64 PolicyId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        public string Type { get; set; }
        
        public string Status { get; set; }
        
        public string Effective { get; set; }
        
        public string Expiration { get; set; }

        [DisplayName("Policy Number")]        
        public string PolicyNumber { get; set; }

        [DisplayName("Policy Description")]
        public string PolicyDescription { get; set; }

        [DisplayName("No display")]
        public Int32 Flags { get; set; }

        [DisplayName("No display")]
        public Int64 UniqAgency { get; set; }

        [DisplayName("No display")]
        public Int64 UniqBranch { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }

    }
}

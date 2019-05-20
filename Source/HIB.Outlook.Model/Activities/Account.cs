using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Account
    {
        [DisplayName("No display")]
        public Int64 AccountId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        [DisplayName("Agency Code")]
        public string AgencyCode { get; set; }

        [DisplayName("No display")]
        public Int32 UniqAgency { get; set; }

        [DisplayName("No display")]
        public Int32 UniqBranch { get; set; }

        [DisplayName("Agency Name")]
        public string AgencyName { get; set; }

        [DisplayName("Branch Code")]
        public string BranchCode { get; set; }

        [DisplayName("Branch Name")]
        public string BranchName { get; set; }

        [DisplayName("No display")]
        public string LookupCode { get; set; }


    }
}

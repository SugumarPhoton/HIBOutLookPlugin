using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class ActivityLookupDetails
    {
        public Int32 ActivityLookUpId { get; set; }

        public Int32 UniqLine { get; set; }

        public Int32 UniqPolicy { get; set; }

        public Int64 UniqEntity { get; set; }

        public Int32 UniqClaim { get; set; }

        public string LineCode { get; set; }

        public string PolicyType { get; set; }

        public string LineDesc { get; set; }

        public string PolicyNumber { get; set; }

        public string PolicyDesc { get; set; }

        public DateTime LineExpDate { get; set; }

        public DateTime LineEffDate { get; set; }

        public DateTime PolicyExpDate { get; set; }

        public DateTime PolicyEffDate { get; set; }

        public Int32 ClaimNumber { get; set; }

        public string CompanyClaimNumber { get; set; }

        public DateTime DateOfLoss { get; set; }

        public DateTime ClosedDate { get; set; }

        public string LookupCode { get; set; }

        public string AccountName { get; set; }

        public DateTime InsertedDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Int32 UniqClaimAssociation { get; set; }

        public string IOC { get; set; }

        public string IOCCOde { get; set; }

        public string AttachmentDesc { get; set; }
    }
}

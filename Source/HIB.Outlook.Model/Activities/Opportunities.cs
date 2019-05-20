using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Opportunities
    {
        [DisplayName("No display")]
        public Int64 OpportunityId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        public string Description { get; set; }

        [DisplayName("Target Close Date")]
        public string TargetCloseDate { get; set; }
        public string Owner { get; set; }

        [DisplayName("No display")]
        public string SalesTeam { get; set; }

        [DisplayName("Sales Manager")]
        public string SalesManager { get; set; }
        public string Stage { get; set; }
        [DisplayName("No display")]
        public Int32 Flags { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class Services
    {
        [DisplayName("No display")]
        public Int64 ServiceHeaderId { get; set; }

        [DisplayName("No display")]
        public Int64 ClientId { get; set; }

        [DisplayName("Services")]
        public string ServiceId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        [DisplayName("Contract number")]
        public string ContactNumber { get; set; }
        [DisplayName("Inception date")]
        public string InceptionDate { get; set; }

        [DisplayName("Expiration date")]
        public string Expiration { get; set; }

        //[DisplayName("No display")]
        //public string Stage { get; set; }

        [DisplayName("No display")]
        public Int32 Flags { get; set; }

        [DisplayName("No display")]
        public Int64 DelFlag { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityCertificateInfo
    {
        [DisplayName("No display")]
        public Int32 UniqCertificate { get; set; }

        [DisplayName("No display")]
        public Int32 UniqEntity { get; set; }

        [DisplayName("Created")]
        public DateTime InsertedDate { get; set; }

        public string Title { get; set; }

        [DisplayName("Last Updated")]
        public DateTime? UpdatedDate { get; set; }
        [DisplayName("No display")]
        public string UserLookupCode { get; set; }
    }
}
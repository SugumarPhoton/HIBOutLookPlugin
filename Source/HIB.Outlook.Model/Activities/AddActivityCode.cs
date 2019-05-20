using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class AddActivityCode
    {
        public Int64 ActivityId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Int32 IsClosedStatus { get; set; }
    }

    public class OwnerCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class CommonLookup
    {
        public Int32 CommonLookupId { get; set; }
        public string CommonLookupTypeCode { get; set; }
        public string CommonLookupCode { get; set; }
        public string CommonLookupName { get; set; }
        public string CommonLookupDescription { get; set; }
        public Int32 SortOrder { get; set; }
        public Int32 IsDeleted { get; set; }
    }
}

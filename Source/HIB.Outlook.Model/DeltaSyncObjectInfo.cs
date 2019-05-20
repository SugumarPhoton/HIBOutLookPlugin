using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class DeltaSyncObjectInfo
    {
        public string IPAddress { get; set; }
        public string UserLookupCode { get; set; }
        public string SpName { get; set; }
        public Nullable<System.DateTime> LastSyncDate { get; set; }
        public Nullable<bool> IsDeltaFlag { get; set; }
    }
}

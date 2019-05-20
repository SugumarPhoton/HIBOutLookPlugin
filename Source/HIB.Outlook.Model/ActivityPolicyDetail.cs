using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityPolicyDetail
    {
        public List<ActivityPolicyInfo> ActivityPolicies { get; set; }
        public long RowCount { get; set; }
    }
}

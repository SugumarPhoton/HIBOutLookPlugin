using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityAccountDetail
    {
        public List<ActivityAccountInfo> ActivityAccounts { get; set; }
        public long RowCount { get; set; }
    }
}

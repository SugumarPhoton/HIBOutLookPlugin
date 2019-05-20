using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityOwnerListDetail
    {
        public List<ActivityOwnerListInfo> ActivityOwnerLists { get; set; }
        public long RowCount { get; set; }
    }
}

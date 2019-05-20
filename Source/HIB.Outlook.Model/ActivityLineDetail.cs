using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityLineDetail
    {
        public List<ActivityLineInfo> ActivityLines { get; set; }
        public long RowCount { get; set; }
    }
}

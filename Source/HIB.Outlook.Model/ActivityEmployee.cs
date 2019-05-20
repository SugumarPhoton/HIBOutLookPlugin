using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityEmployeeDetail
    {
        public List<ActivityEmployee> ActivityEmployees { get; set; }
        public long RowCount { get; set; }
    }
    public class ActivityEmployee
    {
        public int UniqEmployee { get; set; }
        public string EmployeeLookupcode { get; set; }
        public int UniqEntity { get; set; }
        public int UniqActivity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.BAL
{
    public static class ExtensionClass
    {
        public static string SqlDateTimeFormat(this DateTime? sqlDate)
        {
            if (sqlDate != null)
            {
                DateTime sqlDateTime = Convert.ToDateTime(sqlDate);
                return sqlDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
            {
                return null;
            }
        }
    }
}

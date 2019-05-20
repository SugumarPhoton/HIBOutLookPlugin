using System;

namespace HIB.Outlook.Sync
{
    public static class ExtensionClass
    {
        public static string SqliteDateTimeFormat(this DateTime? sqlDate)
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

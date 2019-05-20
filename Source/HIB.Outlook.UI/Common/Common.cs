using HIB.Outlook.Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Common
{
    public class Common
    {
        #region Static Property
     
        #endregion
        public static string DateTimeSQLite(DateTime datetime)
        {
            string formattedDate = string.Empty;
            try
            {
                string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
                formattedDate = string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return formattedDate;
        }

        public static string UniversalDateTimeConversionToSQLite(DateTime datetime)
        {
            string formattedDate = string.Empty;
            try
            {

                formattedDate = datetime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return formattedDate;
        }
    }
}

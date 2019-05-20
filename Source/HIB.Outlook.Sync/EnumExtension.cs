using System;
using System.ComponentModel;
using System.Reflection;


namespace HIB.Outlook.Sync
{
    public class EnumExtension
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

    }

    public static class DateConvertion
    {
        public static Nullable<DateTime> StringToDateTime(this string formatedDate)
        {
            DateTime result;
            DateTime.TryParse(formatedDate, out result);
            return default(DateTime) == result ? default(Nullable<DateTime>) : result;
        }
        public static Nullable<DateTime> ConvertDateTimeFormat(this DateTime lastSyncDate)
        {
            DateTime result;
            DateTime.TryParse(lastSyncDate.ToString("yyyy-M-dd hh:mm:ss"), out result);
            return default(DateTime) == result ? default(Nullable<DateTime>) : result;
        }
        public static Nullable<DateTime> ConvertToUTC(this DateTime formatedDate)
        {
            DateTime convertedDate;
            DateTime.TryParse(formatedDate.ToString("yyyy-M-dd hh:mm:ss"), out convertedDate);
            convertedDate = convertedDate.ToUniversalTime().AddMinutes(-2);
            return default(DateTime) == convertedDate ? default(Nullable<DateTime>) : convertedDate;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HIB.Outlook.UI.Converter
{
    public class WidthConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = System.Convert.ToString(parameter);
            var width = 0.0;
            if (param == "TabItemWidth")
            {
                if (value != null)
                {
                    width = (System.Convert.ToDouble(value) - 6) / 2;
                }
            }
            else
            {
                if (value != null)
                {
                    width = System.Convert.ToDouble(value) - 3;
                }
            }


            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlidingPuzzleView
{
    [ValueConversion(typeof(byte), typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        public static IntToStringConverter Instance = new IntToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (byte) value == 0)
                return String.Empty;
            else
            {
                return value?.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) value == String.Empty)
                return 0;
            else
            {
                return Byte.Parse((string) value);
            }
        }
    }
}

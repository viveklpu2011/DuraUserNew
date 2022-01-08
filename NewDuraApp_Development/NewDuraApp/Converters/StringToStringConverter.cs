using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
    public class StringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value) == "1" ? "Verified" : "Not Verified";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

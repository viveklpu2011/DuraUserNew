using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
    public class BoolToFontIconsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) == true ? FontIcons.FontIconsClass.Wifi : FontIcons.FontIconsClass.WifiOff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

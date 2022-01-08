using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
    public class StringToFonticonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value) == "Home" ? FontIcons.FontIconsClass.Home : FontIcons.FontIconsClass.BriefcaseVariant;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

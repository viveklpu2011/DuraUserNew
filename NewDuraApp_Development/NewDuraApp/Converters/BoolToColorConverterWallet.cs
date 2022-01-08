using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
	public class BoolToColorConverterWallet : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((bool)value) == true ? Color.Green : Color.Red;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}

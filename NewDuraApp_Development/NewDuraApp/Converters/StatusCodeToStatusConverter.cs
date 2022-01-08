using System;
using System.Globalization;
using DuraApp.Core.Helpers.Enums;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
    public class StatusCodeToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((string)value == "1")
            {
                return OrderStatusType.Pending.ToString();
            }
            else if ((string)value == "2")
            {
                return OrderStatusType.Ongoing.ToString();
            }
            else if ((string)value == "3")
            {
                return OrderStatusType.Completed.ToString();
            }
            else
            {
                return OrderStatusType.Cancelled.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

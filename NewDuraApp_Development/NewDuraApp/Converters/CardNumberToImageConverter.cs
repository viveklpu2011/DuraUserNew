using System;
using System.Globalization;
using DuraApp.Core.Helpers;
using Xamarin.Forms;

namespace NewDuraApp.Converters
{
    public class CardNumberToImageConverter : BaseCardValidator, IValueConverter
    {
        public ImageSource Visa { get; set; }
        public ImageSource Amex { get; set; }
        public ImageSource MasterCard { get; set; }
        public ImageSource Dinners { get; set; }
        public ImageSource Discover { get; set; }
        public ImageSource JCB { get; set; }
        public ImageSource NotRecognized { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                SettingsExtension.CardLogo = "nocards";
                return NotRecognized;
            }

            var number = value.ToString();
            var numberNormalized = number.Replace("-", string.Empty);

            if (visaRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_visa";
                return Visa;
            }

            if (amexRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_amex";
                return Amex;
            }

            if (masterRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_mastercard";
                return MasterCard;
            }

            if (dinnersRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_dinersclub";
                return Dinners;
            }

            if (discoverRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_discover";
                return Discover;
            }

            if (jcbRegex.IsMatch(numberNormalized))
            {
                SettingsExtension.CardLogo = "ic_jcb";
                return JCB;
            }

            return NotRecognized;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

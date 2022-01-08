using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NewDuraApp.Helpers
{
    public sealed class GetCultureFromLanguage
    {
        public static CultureInfo GetCulture(string language)
        {
            CultureInfo culture = new CultureInfo(language);
            return culture;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Helpers
{
    public static class Misc
    {
        public static bool VerifyHasValue(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return true;
        }
    }
}

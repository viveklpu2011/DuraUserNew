using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DuraApp.Core.Helpers
{
    public class RegexUtilities
    {
        public static bool EmailValidation(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return false;
            var regex = new Regex(AppConstant.EmailRegex, RegexOptions.IgnoreCase);
            var isMatch = regex.IsMatch(Email);
            return isMatch;
        }

        public static bool PhoneNumberValidation(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            var regex = new Regex(AppConstant.PhoneRegex, RegexOptions.IgnoreCase);
            return regex.IsMatch(PhoneNumber);
        }

        public static bool PasswordValidation(string Password)
        {
            if (string.IsNullOrEmpty(Password))
                return false;
            var regex = new Regex(AppConstant.PasswordRegex);
            var isMatch = regex.IsMatch(Password);
            return isMatch;
        }

        public static bool NameValidation(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            var regex = new Regex(AppConstant.ValidName);
            var isMatch = regex.IsMatch(Name);
            return isMatch;
        }

        public static bool AlphanumericNameValidation(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            var regex = new Regex(AppConstant.ValidAlphaNumeric);
            var isMatch = regex.IsMatch(Name);
            return isMatch;
        }

        public static bool FullNameValidation(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            var regex = new Regex(AppConstant.ValiFullName);
            var isMatch = regex.IsMatch(Name);
            return isMatch;
        }

        public static bool ConfirmPasswordValidation(string Password, string ConfirmPassword)
        {
            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
                return false;
            var isMatch = (Password == ConfirmPassword) ? true : false;
            return isMatch;
        }

        public static bool EmptyString(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else
                return true;
        }
    }
}

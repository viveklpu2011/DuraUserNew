using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace DuraApp.Core.Helpers
{
    public class SettingsExtension
    {
        public static string GetStringForKey(string key)
        {
            var value = Preferences.Get(key, string.Empty);
            return value;
        }

        public static void AddOrUpdateStringForKey(string key, string value)
        {
            try
            {
                Preferences.Set(key, value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings value: {ex}");
            }
        }

        public static T GetClassForKey<T>(string key, T @default) where T : class
        {
            string serialized = Preferences.Get(key, string.Empty);
            T result = @default;

            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                result = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing settings value: {ex}");
            }
            return result;
        }

        public static void AddClassForKey<T>(string key, T obj) where T : class
        {
            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                string serialized = JsonConvert.SerializeObject(obj, serializeSettings);
                Preferences.Set(key, serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        public static void DeleteClassForKey(string key)
        {
            try
            {
                Preferences.Remove(key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        public static void DeletePreferenceKey(string key)
        {
            try
            {
                Preferences.Remove(key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static string CardLogo
        {
            get => Preferences.Get(nameof(CardLogo), string.Empty);
            set => Preferences.Set(nameof(CardLogo), value);
        }

        public static string UserEmail
        {
            get => Preferences.Get(nameof(UserEmail), string.Empty);
            set => Preferences.Set(nameof(UserEmail), value);
        }

        public static string UserFullName
        {
            get => Preferences.Get(nameof(UserFullName), string.Empty);
            set => Preferences.Set(nameof(UserFullName), value);
        }

        public static bool IsVisibleBusinessDocument
        {
            get => Preferences.Get(nameof(IsVisibleBusinessDocument), false);
            set => Preferences.Set(nameof(IsVisibleBusinessDocument), value);
        }

        public static bool IsLoggedIn
        {
            get => Preferences.Get(nameof(IsLoggedIn), false);
            set => Preferences.Set(nameof(IsLoggedIn), value);
        }

        public static bool SelectedLanguage
        {
            get => Preferences.Get(nameof(SelectedLanguage), false);
            set => Preferences.Set(nameof(SelectedLanguage), value);
        }

        public static bool IsPermissionEnabled
        {
            get => Preferences.Get(nameof(IsPermissionEnabled), false);
            set => Preferences.Set(nameof(IsPermissionEnabled), value);
        }

        public static string FirstName
        {
            get => Preferences.Get(nameof(FirstName), string.Empty);
            set => Preferences.Set(nameof(FirstName), value);
        }

        public static string LastName
        {
            get => Preferences.Get(nameof(LastName), string.Empty);
            set => Preferences.Set(nameof(LastName), value);
        }

        public static string Token
        {
            get => Preferences.Get(nameof(Token), string.Empty);
            set => Preferences.Set(nameof(Token), value);
        }

        public static string ProfileImageUrl
        {
            get => Preferences.Get(nameof(ProfileImageUrl), string.Empty);
            set => Preferences.Set(nameof(ProfileImageUrl), value);
        }

        public static string Area
        {
            get => Preferences.Get(nameof(Area), string.Empty);
            set => Preferences.Set(nameof(Area), value);
        }

        public static string User
        {
            get => Preferences.Get(nameof(User), string.Empty);
            set => Preferences.Set(nameof(User), value);
        }

        public static string BeARiderString
        {
            get => Preferences.Get(nameof(BeARiderString), string.Empty);
            set => Preferences.Set(nameof(BeARiderString), value);
        }

        public static string Phone
        {
            get => Preferences.Get(nameof(Phone), string.Empty);
            set => Preferences.Set(nameof(Phone), value);
        }

        public static bool IsPhoneVerified
        {
            get => Preferences.Get(nameof(IsPhoneVerified), false);
            set => Preferences.Set(nameof(IsPhoneVerified), value);
        }

        public static string Country
        {
            get => Preferences.Get(nameof(Country), string.Empty);
            set => Preferences.Set(nameof(Country), value);
        }

        public static int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }

        public static string UserType

        {
            get => Preferences.Get(nameof(UserType), string.Empty);
            set => Preferences.Set(nameof(UserType), value);
        }

        public static string Preferedlanguage
        {
            get => Preferences.Get(nameof(Preferedlanguage), string.Empty);
            set => Preferences.Set(nameof(Preferedlanguage), value);
        }

        public static string AppName
        {
            get => Preferences.Get(nameof(AppName), string.Empty);
            set => Preferences.Set(nameof(AppName), value);
        }

        public static string PickupAddress
        {
            get => Preferences.Get(nameof(PickupAddress), string.Empty);
            set => Preferences.Set(nameof(PickupAddress), value);
        }

        public static string CurrentUserCulture
        {
            get => Preferences.Get(nameof(CurrentUserCulture), string.Empty);
            set => Preferences.Set(nameof(CurrentUserCulture), value);
        }

        public const string AppDbSecret = "TechMedDBSecret";
        public SettingsExtension()
        {
        }
    }
}

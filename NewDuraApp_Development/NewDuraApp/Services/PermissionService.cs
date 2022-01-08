using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace NewDuraApp.Services
{
    public static class PermissionService
    {
        [Obsolete]
        public static async Task<bool> CheckPermissionsForLocationAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var result = await UserDialogs.Instance.ConfirmAsync(message: "Location permission is required. Please go into Settings and turn on Storage for the app.",
                         okText: "Settings",
                        cancelText: "Maybe Later");
                        if (result == true)
                        {
                            CrossPermissions.Current.OpenAppSettings();
                        }
                        return false;
                    }
                }
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await UserDialogs.Instance.AlertAsync("Location permission is needed");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                    {
                        status = results[Permission.Location];
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await UserDialogs.Instance.AlertAsync("Location permission was denied.");
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

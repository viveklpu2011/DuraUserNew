using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace NewDuraApp.Services.LocationService
{
    class LocationService : ILocationService
    {
        public async Task<bool> IsLocationPermissionGranted()
        {
            try
            {
                if (await Permissions.CheckStatusAsync<Permissions.LocationAlways>() == PermissionStatus.Granted ||
               await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<PermissionStatus> RequestLocationPermission()
        {
            return await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        public async Task<PermissionStatus> CheckLocationPermission()
        {
            return await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }
    }
}

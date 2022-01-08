
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace NewDuraApp.Services.LocationService
{
    public interface ILocationService
    {
        Task<PermissionStatus> RequestLocationPermission();
        Task<PermissionStatus> CheckLocationPermission();
        Task<bool> IsLocationPermissionGranted();
        Task<PermissionStatus> CheckAndRequestLocationPermission();
    }
}

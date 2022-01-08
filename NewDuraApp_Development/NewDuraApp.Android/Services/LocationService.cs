using System;
using Android.Content;
using Android.Locations;
using NewDuraApp.Droid.Services;
using NewDuraApp.Services.LocationService;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]
namespace NewDuraApp.Droid.Services
{
    public class LocationService : IPlatformSpecificLocationService
    {
        public bool IsLocationServiceEnabled()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            try
            {
                return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool OpenDeviceLocationSettingsPage()
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
            return true;
        }
    }

}

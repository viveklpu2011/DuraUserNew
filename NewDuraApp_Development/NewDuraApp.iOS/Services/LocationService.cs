using CoreLocation;
using Foundation;
using NewDuraApp.iOS.Services;
using NewDuraApp.Services.LocationService;
using UIKit;
using Xamarin.Forms;
[assembly: Dependency(typeof(LocationService))]
namespace NewDuraApp.iOS.Services
{
    public class LocationService : IPlatformSpecificLocationService
    {
        public bool IsLocationServiceEnabled()
        {
            return CLLocationManager.LocationServicesEnabled;
        }

        public bool OpenDeviceLocationSettingsPage()
        {
            var url = new NSUrl("prefs:root=Settings");
            UIApplication.SharedApplication.OpenUrl(url);
            return true;
        }
    }
}

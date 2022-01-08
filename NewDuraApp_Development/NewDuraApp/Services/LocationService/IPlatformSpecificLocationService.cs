using System;
namespace NewDuraApp.Services.LocationService
{
    public interface IPlatformSpecificLocationService
    {
        bool IsLocationServiceEnabled();
        bool OpenDeviceLocationSettingsPage();
    }
}

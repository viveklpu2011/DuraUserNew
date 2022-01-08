using System.Linq;
using Xamarin.Essentials;

namespace NewDuraApp.Services
{
    public class ConnectivityService
    {
        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Cellular))
            {
                if (current == NetworkAccess.Internet)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}

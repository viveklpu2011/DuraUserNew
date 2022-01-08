using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using FreshMvvm;
using NewDuraApp.Bootstrap;
using NewDuraApp.LocationTracker.Services;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.Services.LocationService;
using NewDuraApp.SocialLogin.Constants;
using NewDuraApp.ViewModels;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp
{
    public partial class App : Application
    {
        //vivek branch
        public static AppState State = AppState.Undefinded;
        public static double MainPageScreenWidth { get; set; } //screen width
        public static double MainPageScreenHeight { get; set; }//screen height
        public static ViewModelLocator Locator { get; set; }
        public INavigationService _navigationService;
        public static string User = "Dura";
        public static string UserPhone = "";
        public static string VerifyOTPWay = "";
        public static string DuraExpressTrackOrderWay = "";
        public static bool IsVisiblePromoEntry = false;
        public static bool IsSelectVehicleFromExpress = false;
        public static string DeviceToken = "";
        public static IFreshIOC Container => FreshIOC.Container;

        public App()
        {
            InitializeComponent();
            DependencyService.Register<LocationService>();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            VersionTracking.Track();
            GoogleMapsApiService.Initialize(AppConstant.GooglePlaceKey);
            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application.SetWindowSoftInputModeAdjust(this, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);
            BootstrapConfig.RegisterViewModel();
            BootstrapConfig.RegisterService();
            //GoogleMapsApiService.Initialize(Tech.Med.Core.Helpers.Constants.GoogleMapsApiKey);
            Locator = new ViewModelLocator();
            _navigationService = Container.Resolve<INavigationService>();
            _navigationService.InitializeAsync();
            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                }

            };

            //var aa = CrossFirebasePushNotification.Current.Token;
            //if (Device.RuntimePlatform == "iOS")
            //{
            //    DeviceToken = aa;
            //}
        }
        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {

        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            if (Device.RuntimePlatform == "Android")
            {
                DeviceToken = e.Token;
            }
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

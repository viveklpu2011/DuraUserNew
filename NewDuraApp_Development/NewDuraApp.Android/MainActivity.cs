using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Acr.UserDialogs;
using NewDuraApp.Droid.Renderers;
using Plugin.Permissions;
using Android;
using FFImageLoading;
using Android.Content;
using Plugin.FacebookClient;
using Xamarin.Auth;
using Plugin.GoogleClient;
using Plugin.CurrentActivity;
using Xamarin.Forms.GoogleMaps.Android;
using Plugin.FirebasePushNotification;

namespace NewDuraApp.Droid
{
    [Activity(Label = "Duradrive", Icon = "@drawable/appLogo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            var pixelWidth = (double)Resources.DisplayMetrics.WidthPixels;
            var pixelHeight = (double)Resources.DisplayMetrics.HeightPixels;
            var density = (double)Resources.DisplayMetrics.Density;
            App.MainPageScreenWidth = (double)((pixelWidth - 0.5f) / density);
            App.MainPageScreenHeight = (double)((pixelHeight - 0.5f) / density);


            base.OnCreate(savedInstanceState);
            Forms.SetFlags("Expander_Experimental");
            Forms.SetFlags("SwipeView_Experimental");
            Rg.Plugins.Popup.Popup.Init(this);
            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            //AnimationViewRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig);
            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };

            ImageService.Instance.Initialize(config);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            UserDialogs.Init(this);
            GoogleClientManager.Initialize(this, null, "216962613796-69f8c6bhjh0ili6p1anv87434a6mf0ah.apps.googleusercontent.com");
            ImageCircleRenderer.Init();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            FacebookClientManager.Initialize(this);
            //Sharpnado.Shades.Droid.AndroidShadowsRenderer.AutofillFlagIncludeNotImportantViews
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            //global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;
            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }
        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
    Manifest.Permission.AccessCoarseLocation,
    Manifest.Permission.AccessFineLocation
};
        protected override void OnStart()
        {
            base.OnStart();

            //if ((int)Build.VERSION.SdkInt >= 23) {
            //	if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted) {
            //		RequestPermissions(LocationPermissions, RequestLocationId);
            //	} else {
            //		// Permissions already granted - display a message.
            //	}
            //}
            //try
            //{

            //    PackageInfo info = this.PackageManager.GetPackageInfo("com.dura.duraapp", PackageInfoFlags.Signatures);

            //    foreach (Android.Content.PM.Signature signature in info.Signatures)
            //    {
            //        MessageDigest md = MessageDigest.GetInstance("SHA");
            //        md.Update(signature.ToByteArray());

            //        string keyhash = Convert.ToBase64String(md.Digest());
            //        Console.WriteLine("KeyHash:", keyhash);
            //    }
            //}
            //catch (NameNotFoundException e)
            //{

            //}
            //catch (NoSuchAlgorithmException e)
            //{

            //}
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, intent);
        }
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                System.Diagnostics.Debug.WriteLine("Android back button: There are some pages in the PopupStack");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Android back button: There are not any pages in the PopupStack");
            }
        }

    }
}
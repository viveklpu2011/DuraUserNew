using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using NewDuraApp.SocialLogin.AuthHelpers;

namespace NewDuraApp.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataSchemes = new[] { "com.googleusercontent.apps.119818815824-paqgee72mennolomhpqegtng60o7bc5t" },
    DataPath = "/oauth2redirect")]

    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //var uri = new Uri(Intent.Data.ToString());

            //// Load redirectUrl page
            //AuthenticationState.Authenticator.OnPageLoading(uri);

            //Finish();
            //return;
            // Convert Android.Net.Url to Uri


            // Create your application here
            global::Android.Net.Uri uri_android = Intent.Data;

            Uri uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            this.Finish();

            return;

        }
    }
}

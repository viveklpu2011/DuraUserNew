using System;
using System.Collections.Generic;
using System.Linq;
using DuraApp.Core.Helpers;
using Foundation;
using NewDuraApp.iOS.Renderers;
using NewDuraApp.SocialLogin.AuthHelpers;
using NewDuraApp.SocialLogin.Constants;
using Plugin.FacebookClient;
using Plugin.FirebasePushNotification;
using Plugin.GoogleClient;
using UIKit;
using Xamarin.Forms.GoogleMaps.iOS;

namespace NewDuraApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "CollectionView_Experimental", "Expander_Experimental" });
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            Xamarin.FormsMaps.Init();
            ImageCircleRenderer.Init();
            var platformConfig = new PlatformConfig
            {
                ImageFactory = new CachingImageFactory()
            };
            Xamarin.FormsGoogleMaps.Init(AppConstant.GooglePlaceKey, platformConfig);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            FacebookClientManager.OnActivated();
            FacebookClientManager.Initialize(app, options);
            GoogleClientManager.Initialize();
            LoadApplication(new App());
            FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            {
                new NotificationUserCategory("message",new List<NotificationUserAction> {
                    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                }),
                new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept"),
                    new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                })
            });
            return base.FinishedLaunching(app, options);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.
            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
           
            var uri = new Uri(url.AbsoluteString);
            if (uri.ToString().Contains("google"))
            {
                return GoogleClientManager.OnOpenUrl(app, url, options);
            }
            else
            {
                AuthenticationState.Authenticator.OnPageLoading(uri);
                return FacebookClientManager.OpenUrl(app, url, options);
            }
        }
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }
       
    }
}

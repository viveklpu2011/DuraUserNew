using System;
namespace NewDuraApp.SocialLogin.Constants
{
    public class AppConstants
    {
        public static string AppName = "Dura App";

        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "119818815824-hgudnj3no7gbn25hg1huct98rom0h5li.apps.googleusercontent.com";
        public static string AndroidClientId = "119818815824-paqgee72mennolomhpqegtng60o7bc5t.apps.googleusercontent.com";

        // These values do not need changing

        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "com.googleusercontent.apps.119818815824-hgudnj3no7gbn25hg1huct98rom0h5li:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.119818815824-paqgee72mennolomhpqegtng60o7bc5t:/oauth2redirect";
    }
}

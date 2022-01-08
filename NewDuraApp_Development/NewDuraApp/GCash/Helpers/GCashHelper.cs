using System;
namespace NewDuraApp.GCash.Helpers
{
    public class GCashHelper
    {
        #region Sandbox Account
        public static string Public_Key = "pk_test_etzAn7435T8Jjtw5m2HY2kBu";
        public static string Secret_Key = "sk_test_3HHUFPBpGSmEHGbs5ZxuAZ25";
        #endregion

        #region Production Account
        //public static string Public_Key = "pk_test_etzAn7435T8Jjtw5m2HY2kBu";
        //public static string Secret_Key = "sk_test_3HHUFPBpGSmEHGbs5ZxuAZ25";
        #endregion

        public static string GCashPaymentUrlSource = "https://api.paymongo.com/v1/sources";
        public static string GCashPaymentUrl = "https://api.paymongo.com/v1/payments";

        public static string GCashSuccessUrl = "https://embed.lottiefiles.com/animation/62669";
        public static string GCashFailedUrl = "https://embed.lottiefiles.com/animation/9607";

    }
}

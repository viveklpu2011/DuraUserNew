using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Helpers
{
    public class Urls
    {
        //development base url
        //public static string BASE_URL = "https://162.241.87.160";

        //production base url
        //public static string BASE_URL = "https://duradriver.com/duradrive";

        //production base url New
        public static string BASE_URL = "https://duradriver.com/duradrive";

        public static string GET_ALL_LOCATION = "/duradrive_api_dev/alllocation";
        public static string LOGIN = "/duradrive_api_dev/api/login";
        public static string REGISTER = "/duradrive_api_dev/api/register";
        public static string PROFILE = "/duradrive_api_dev/api/profile";
        public static string UPDATE_USER_PASSWORD = "/duradrive_api_dev/api/user_password_update";
        public static string GET_LOCATION = "/duradrive_api_dev/api/getlocation";
        public static string SEND_OTP = "/duradrive_api_dev/api/send_otp";
        public static string VERIFY_OTP = "/duradrive_api_dev/api/varify_otp";
        public static string RESET_PASSWORD = "/duradrive_api_dev/api/forgot_passworduser";
        public static string PROMO_LIST = "/duradrive_api_dev/api/getpromocode";
        public static string VERIFY_PROMO = "/duradrive_api_dev/api/varifypromocode";
        public static string SCHEDULE_PICKUP = "/duradrive_api_dev/api/shedule_pickup";
        public static string GET_VEHICLE_LIST = "/duradrive_api_dev/api/getvehicle";
        public static string SAVE_ADD_MORE_DETAILS = "/duradrive_api_dev/api/savepickup_details";
        public static string GET_WALLET_AMOUNT = "/duradrive_api_dev/api/walletpackege";
        public static string GET_WALLET_AMOUNT_LIST = "/duradrive_api_dev/api/walletpackege";
        public static string Get_Save_Wallet_Amount = "/duradrive_api_dev/api/addtowallet";
        public static string Find_Driver = "/duradrive_api_dev/api/finddriver";
        public static string Tip_Price = "/duradrive_api_dev/api/tipprice";
        public static string PaymentMode = "/duradrive_api_dev/api/paymentmode";
        public static string GetDriverDetails = "/duradrive_api_dev/api/get_driver_details";
        public static string GetPickupDetails = "/duradrive_api_dev/api/get_pickup_details";
        public static string GetOrderDetails = "/duradrive_api_dev/api/order_details";
        public static string GetProfileDetails = "/duradrive_api_dev/api/profile";
        public static string UpdateSocialLink = "/duradrive_api_dev/api/updatesociallink";
        public static string UpdateUserName = "/duradrive_api_dev/api/update_user_name";
        public static string AddAddress = "/duradrive_api_dev/api/addaddress";
        public static string UpdateAddress = "/duradrive_api_dev/api/updateaddress";
        public static string GetAddress = "/duradrive_api_dev/api/getaddress";
        public static string DeleteAddress = "/duradrive_api_dev/api/deleteaddress";

        public static string AddCard = "/duradrive_api_dev/api/addcard";
        public static string UpdateCard = "/duradrive_api_dev/api/updatecard";
        public static string DeleteCard = "/duradrive_api_dev/api/deletecard";
        public static string GetCard = "/duradrive_api_dev/api/getcard";
        public static string ReferCode = "/duradrive_api_dev/refercode";

        public static string GetPersonalDocId = "/duradrive_api_dev/api/getpersonaldoc";
        public static string GetBusinessDocId = "/duradrive_api_dev/api/getbusinessdoc";

        public static string UploadDocument = "/duradrive_api_dev/api/updatedocument";
        public static string GetPersonalDocuments = "/duradrive_api_dev/api/userpersonaldoc";
        public static string DeleteDocument = "/duradrive_api_dev/api/deletedocument";
        public static string GetBusinessDocuments = "/duradrive_api_dev/api/userbusinessdoc";
        public static string GetMyOrders = "/duradrive_api_dev/api/allorder_ByUser_id";

        public static string GetNotification = "/duradrive_api_dev/api/get_notification";
        public static string ReadNotification = "/duradrive_api_dev/api/read_notification";
        public static string SocialLogin = "/duradrive_api_dev/api/register";  //https://www.getpostman.com/collections/f45bf801faf9c85e7898

        public static string GetWalletHistory = "/duradrive_api_dev/api/wallet_history";
        public static string CancelRide = "/duradrive_api_dev/api/cancel_details";
        public static string UpdateCountry = "/duradrive_api_dev/api/update_country";
        public static string BeARider = "/duradrive_api_dev/api/user_driverrequest";
        public static string UpdatePhone = "/duradrive_api_dev/api/update_phone";

        public static string GetRating = "/duradrive_api_dev/api/userReviewRating";
        public static string PostRating = "/duradrive_api_dev/api/userDriverRating";
        public static string SubmitReferral = "/duradrive_api_dev/api/userReferralCode";
        public static string GetGoogleTime = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={0}&destinations={1}&key={AppConstant.GooglePlaceKey}";
        public static string UpdateVerifiedPhone = "/duradrive_api_dev/api/update_phone";

        #region Old Url's
        //public static string GET_ALL_LOCATION = "/duradrive_api/alllocation";
        //public static string LOGIN = "/duradrive_api/api/login";
        //public static string REGISTER = "/duradrive_api/api/register";
        //public static string PROFILE = "/duradrive_api/api/profile";
        //public static string UPDATE_USER_PASSWORD = "/duradrive_api/api/user_password_update";
        //public static string GET_LOCATION = "/duradrive_api/api/getlocation";
        //public static string SEND_OTP = "/duradrive_api/api/send_otp";
        //public static string VERIFY_OTP = "/duradrive_api/api/varify_otp";
        //public static string RESET_PASSWORD = "/duradrive_api/api/forgot_passworduser";
        //public static string PROMO_LIST = "/duradrive_api/api/getpromocode";
        //public static string VERIFY_PROMO = "/duradrive_api/api/varifypromocode";
        //public static string SCHEDULE_PICKUP = "/duradrive_api/api/shedule_pickup";
        //public static string GET_VEHICLE_LIST = "/duradrive_api/api/getvehicle";
        //public static string SAVE_ADD_MORE_DETAILS = "/duradrive_api/api/savepickup_details";
        //public static string GET_WALLET_AMOUNT = "/duradrive_api/api/walletpackege";
        //public static string GET_WALLET_AMOUNT_LIST = "/duradrive_api/api/walletpackege";
        //public static string Get_Save_Wallet_Amount = "/duradrive_api/api/addtowallet";
        //public static string Find_Driver = "/duradrive_api/api/finddriver";
        //public static string Tip_Price = "/duradrive_api/api/tipprice";
        //public static string PaymentMode = "/duradrive_api/api/paymentmode";
        //public static string GetDriverDetails = "/duradrive_api/api/get_driver_details";
        //public static string GetPickupDetails = "/duradrive_api/api/get_pickup_details";
        //public static string GetOrderDetails = "/duradrive_api/api/order_details";
        //public static string GetProfileDetails = "/duradrive_api/api/profile";
        //public static string UpdateSocialLink = "/duradrive_api/api/updatesociallink";
        //public static string UpdateUserName = "/duradrive_api/api/update_user_name";
        //public static string AddAddress = "/duradrive_api/api/addaddress";
        //public static string UpdateAddress = "/duradrive_api/api/updateaddress";
        //public static string GetAddress = "/duradrive_api/api/getaddress";
        //public static string DeleteAddress = "/duradrive_api/api/deleteaddress";

        //public static string AddCard = "/duradrive_api/api/addcard";
        //public static string UpdateCard = "/duradrive_api/api/updatecard";
        //public static string DeleteCard = "/duradrive_api/api/deletecard";
        //public static string GetCard = "/duradrive_api/api/getcard";
        //public static string ReferCode = "/duradrive_api/refercode";

        //public static string GetPersonalDocId = "/duradrive_api/api/getpersonaldoc";
        //public static string GetBusinessDocId = "/duradrive_api/api/getbusinessdoc";

        //public static string UploadDocument = "/duradrive_api/api/updatedocument";
        //public static string GetPersonalDocuments = "/duradrive_api/api/userpersonaldoc";
        //public static string DeleteDocument = "/duradrive_api/api/deletedocument";
        //public static string GetBusinessDocuments = "/duradrive_api/api/userbusinessdoc";
        //public static string GetMyOrders = "/duradrive_api/api/allorder_ByUser_id";

        //public static string GetNotification = "/duradrive_api/api/get_notification";
        //public static string ReadNotification = "/duradrive_api/api/read_notification";
        //public static string SocialLogin = "/duradrive_api/api/register";  //https://www.getpostman.com/collections/f45bf801faf9c85e7898

        //public static string GetWalletHistory = "/duradrive_api/api/wallet_history";
        //public static string CancelRide = "/duradrive_api/api/cancel_details";
        //public static string UpdateCountry = "/duradrive_api/api/update_country";
        //public static string BeARider = "/duradrive_api/api/user_driverrequest";
        //public static string UpdatePhone = "/duradrive_api/api/update_phone";

        //public static string GetRating = "/duradrive_api/api/userReviewRating";
        //public static string PostRating = "/duradrive_api/api/userDriverRating";
        //public static string SubmitReferral = "/duradrive_api/api/userReferralCode";
        //public static string GetGoogleTime = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={0}&destinations={1}&key={AppConstant.GooglePlaceKey}";
        //public static string UpdateVerifiedPhone = "/duradrive_api/api/update_phone";
        #endregion
    }
}

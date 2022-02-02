using System.Collections.Generic;
using DuraApp.Core.Models.ResponseModels;

namespace DuraApp.Core.Helpers
{
    public class AppConstant
    {
        public const string TransitionMessage = "Transition";

        #region regex
        public static string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        public static string PhoneRegex = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";
        public static string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
        public static string ValidName = @"^[a-zA-Z]*$";
        public static string ValiFullName = @"^[a-zA-Z]+[\s|-]?[a-zA-Z]+[\s|-]?[a-zA-Z]+[\s|-]?[a-zA-Z]+[\s|-]?[a-zA-Z]+$";
        public static string ValidAlphaNumeric = @"^[A-Za-z0-9- ]+$";
        #endregion

        public static string TermsServicesUrl = $"{Urls.BASE_URL}/terms-services";
        public static string AboutusUrl = $"{Urls.BASE_URL}/terms-and-conditions";
        public static string PrivacyPolicyUrl = $"{Urls.BASE_URL}/privacypolicy";
        public static string EULAUrl = $"{Urls.BASE_URL}/end-user-license-agreement";
        public static string DuraDriveDeliveryTermsUrl = $"{Urls.BASE_URL}/terms-service-transport-delivery-logistics";
        //public static string GooglePlaceUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?key={0}&input={1}&components=country:in|country:us|country:ph";

        public static string GooglePlaceUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?key={0}&input={1}&components=country:ph";
        public static string GooglePlaceKey = "AIzaSyB23eIri9-RdhbbqzTf71nlOpa7c9bVNgo";

        public static VehicleListData SelectedVehicle { get; set; }
        public static double ServiceTotal { get; set; }
        public static double Discount { get; set; }
        public static string TotalFinalFare { get; set; }
        public static double TipAmount { get; set; }
        public static List<ServicesModel> ServiceList { get; set; }
    }

}

using System;
namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class SignupRequestModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirmation { get; set; }
        public string area { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string login_type { get; set; }
        public string referral_code { get; set; }
        public string country_id { get; set; }
        public byte[] userImage { get; set; }

    }
}

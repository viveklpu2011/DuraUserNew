using System;
namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class LoginRequestModel
    {
        public string user_area { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string country_id { get; set; }
        public string device_id { get; set; }
        public string device_type { get; set; }
    }
}

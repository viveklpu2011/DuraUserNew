using System;
namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class OTPRequestModel
    {
        public string country_code { get; set; }
        public string phone { get; set; }
    }
}

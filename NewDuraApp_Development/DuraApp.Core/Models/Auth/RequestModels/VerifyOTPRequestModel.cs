using System;
namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class VerifyOTPRequestModel
    {
        public string country_code { get; set; }
        public string phone { get; set; }
        public string otp { get; set; }
    }
}

using System;
namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class ResetPasswordRequestModel
    {
        public string country_code { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
    }
}

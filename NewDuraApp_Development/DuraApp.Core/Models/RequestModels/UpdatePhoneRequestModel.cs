using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class UpdatePhoneRequestModel : CommonUserIdRequestModel
    {
        public string phone { get; set; }
        public string country_code { get; set; }
    }
}

using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class VerifyPromoCodeRequestModel : CommonUserIdRequestModel
    {
        public string promocode { get; set; }
    }
}

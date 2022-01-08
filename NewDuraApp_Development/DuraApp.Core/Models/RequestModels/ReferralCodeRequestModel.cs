using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class ReferralCodeRequestModel : CommonUserIdRequestModel
    {
        public string referal_code { get; set; }
    }
}

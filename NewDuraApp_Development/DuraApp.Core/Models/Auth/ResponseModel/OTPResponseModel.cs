using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.Auth.ResponseModel
{
    public class OTPResponseModel: CommonResponseModel
    {
        public string otp { get; set; }
    }
}

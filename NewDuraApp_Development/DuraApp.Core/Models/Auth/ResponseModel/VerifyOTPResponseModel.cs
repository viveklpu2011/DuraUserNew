using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.Auth.ResponseModel
{
    public class VerifyOTPResponseModel: CommonResponseModel
    {
        public int id { get; set; }
        public string phone { get; set; }
        public long otp { get; set; }
        public string created { get; set; }
        public string email { get; set; }
    }
}

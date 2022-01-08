using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class ChangePasswordRequestModel : CommonUserIdRequestModel
    {
        public string password { get; set; }
        public string password_confirmation { get; set; }
        public string old_password { get; set; }
    }
}

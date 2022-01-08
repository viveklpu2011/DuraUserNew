using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class UpdateSocialLinkRequestModel:CommonUserIdRequestModel
    {
        public string google { get; set; }
        public string facebook { get; set; }
    }
}

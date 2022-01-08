using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class AddCardRequestModel : CommonUserIdRequestModel
    {
        public string name { get; set; }
        public string number { get; set; }
        public string expirydate { get; set; }
        public string securitycode { get; set; }
    }
    public class UpdateCardRequestModel : CommonUserIdRequestModel
    {
        public string name { get; set; }
        public string number { get; set; }
        public string expirydate { get; set; }
        public string securitycode { get; set; }
        public long id { get; set; }
    }
    public class DeleteCardRequestModel : CommonUserIdRequestModel
    {
        public long id { get; set; }
    }
}

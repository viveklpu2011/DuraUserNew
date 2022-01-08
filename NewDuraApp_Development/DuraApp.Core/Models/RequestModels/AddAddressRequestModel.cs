using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class AddAddressRequestModel : CommonUserIdRequestModel
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        //public string city { get; set; }
        public string contactperson { get; set; }
        public string mobile { get; set; }
        public string isdefault { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        //public string type { get; set; }
    }
    public class UpdateAddressRequestModel : CommonUserIdRequestModel
    {
        public long addressid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        //public string city { get; set; }
        public string contactperson { get; set; }
        public string mobile { get; set; }
        public string isdefault { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        //public string type { get; set; }
    }
    public class DeleteAddressRequestModel : CommonUserIdRequestModel
    {
        public long addressid { get; set; }
    }
}

using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class GetDriverDetailsRequestModel: CommonUserIdRequestModel
    {
        public string step { get; set; }

        public long pickup_id { get; set; }
    }
}

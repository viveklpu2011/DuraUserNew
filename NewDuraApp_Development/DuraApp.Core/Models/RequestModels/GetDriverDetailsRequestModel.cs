using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class GetDriverDetailsRequestModel: CommonUserIdRequestModel
    {
        public long pickup_id { get; set; }
    }
}

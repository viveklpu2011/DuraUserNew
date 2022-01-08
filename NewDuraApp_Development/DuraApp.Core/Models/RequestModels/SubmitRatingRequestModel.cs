using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class SubmitRatingRequestModel : CommonUserIdRequestModel
    {
        public long driver_id { get; set; }
        public string order_id { get; set; }
        public int rating { get; set; }
        public string remarks { get; set; }
        public string service_type { get; set; }
    }
}

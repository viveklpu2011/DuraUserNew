using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class CancelRideRequestModel
    {
        public long pickup_id { get; set; }
        public long user_id { get; set; }
        public long driver_id { get; set; }
        public string canceledby { get; set; }
        public string reason { get; set; }
    }
}

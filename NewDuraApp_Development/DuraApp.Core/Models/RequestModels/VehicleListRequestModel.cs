using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class VehicleListRequestModel
    {
        public double pickuplat { get; set; }
        public double pickuplon { get; set; }
        public double destinationlat { get; set; }
        public double destinationlon { get; set; }
        public long pickup_id { get; set; }
    }
}

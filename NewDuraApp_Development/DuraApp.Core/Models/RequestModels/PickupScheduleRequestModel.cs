using System;
using System.Collections.Generic;

namespace DuraApp.Core.Models.RequestModels
{
    public class PickupScheduleRequestModel : PickupScheduleRequestModelCheck
    {
        public string pickup_address1 { get; set; }
        public string pickup_address2 { get; set; }
        public string pickup_mobile { get; set; }
        public string pickup_name { get; set; }
        public string destination_address1 { get; set; }
        public string destination_address2 { get; set; }
        public string destination_mobile { get; set; }
        public string destination_name { get; set; }
        public string type { get; set; }
        public string pdate { get; set; }
        public string stop_address1 { get; set; }
        public string stop_address2 { get; set; }
        public string stop_name { get; set; }
        public string stop_mobile { get; set; }
        public DateTime created_at { get; set; }
        public long user_id { get; set; }
        public double pickuplat { get; set; }
        public double pickuplon { get; set; }
        public double destinationlat { get; set; }
        public double destinationlon { get; set; }
        public double stoplat { get; set; }
        public double stoplon { get; set; }
    }
    public class PickupScheduleReqModel
    {
        public string pickup_address1 { get; set; }
        public string pickup_address2 { get; set; }
        public string pickup_mobile { get; set; }
        public string pickup_name { get; set; }
        public string destination_address1 { get; set; }
        public string destination_address2 { get; set; }
        public string destination_mobile { get; set; }
        public string destination_name { get; set; }
        public string type { get; set; }
        public string pdate { get; set; }
        //public string stop_address1 { get; set; }
        //public string stop_address2 { get; set; }
        //public string stop_name { get; set; }
        //public string stop_mobile { get; set; }
        public DateTime created_at { get; set; }
        public long user_id { get; set; }
        public double pickuplat { get; set; }
        public double pickuplon { get; set; }
        public double destinationlat { get; set; }
        public double destinationlon { get; set; }
        //public double stoplat { get; set; }
        //public double stoplon { get; set; }
        public List<PickupScheduleRequestStopModelWithoutId> stopData { get; set; }
    }
    public class DuraExpressAddressModel
    {
        public int AddressId { get; set; }
        public string pickup_address1 { get; set; }
        public string pickup_address2 { get; set; }
        public string pickup_mobile { get; set; }
        public string pickup_name { get; set; }
        public double pickuplat { get; set; }
        public double pickuplon { get; set; }
    }
    public class PickupScheduleRequestModelCheck
    {

        public bool IsAvailablePickupSchedule { get; set; }
        public bool IsAvailablePickupLocation { get; set; }
        public bool IsAvailableWhereToLocation { get; set; }
        public bool IsAvailableAddStopLocationLocation { get; set; }
    }
    public class PickupScheduleRequestStopModel
    {
        public int StopId { get; set; }
        public string StopText { get; set; }
        public string stop_address1 { get; set; }
        public string stop_address2 { get; set; }
        public string stop_name { get; set; }
        public string stop_mobile { get; set; }
        public double stoplat { get; set; }
        public double stoplon { get; set; }
        public bool IsAvailableAddStopLocationLocation { get; set; }
    }
    public class PickupScheduleRequestStopModelWithoutId
    {
        public string stop_address1 { get; set; }
        public string stop_address2 { get; set; }
        public string stop_name { get; set; }
        public string stop_mobile { get; set; }
        public double stoplat { get; set; }
        public double stoplon { get; set; }
    }
}

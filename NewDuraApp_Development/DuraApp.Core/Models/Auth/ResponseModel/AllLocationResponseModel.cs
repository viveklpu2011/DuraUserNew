using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.Auth.ResponseModel
{
    public class LocationDataResponse
    {
        public int id { get; set; }
        public string service_name { get; set; }
        public string country { get; set; }
        public string area { get; set; }
        public string timezone { get; set; }
        public string vehicle_service { get; set; }
        public string driver_doc { get; set; }
        public string vehicle_doc { get; set; }
        public string driver_minbal { get; set; }
        public string delivery_area { get; set; }
        public string max_tollprice { get; set; }
        public string bill_method { get; set; }
        public string latitute { get; set; }
        public string longitute { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class AllLocationResponseModel : CommonResponseModel
    {

        public List<LocationDataResponse> data { get; set; }
    }

    public class NewLocationDataResponse
    {
        public int id { get; set; }
        public string iso { get; set; }
        public string country_name { get; set; }
        public int country_code { get; set; }
    }
    public class GetAllLocationResponseModel : CommonResponseModel
    {

        public List<NewLocationDataResponse> data { get; set; }
    }
}

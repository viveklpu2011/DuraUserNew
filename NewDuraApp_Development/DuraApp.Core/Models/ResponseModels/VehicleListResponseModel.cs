using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class VehicleListResponseModel : CommonResponseModel
    {
        public List<VehicleListData> data { get; set; }
    }

    public class VehicleListData
    {
        public int id { get; set; }
        public string service { get; set; }
        public string currency { get; set; }
        public string city { get; set; }
        public string vehicle_type { get; set; }
        public string weight_limit { get; set; }
        public string size_limit { get; set; }
        public string priceby { get; set; }
        public string basefare { get; set; }
        public string image { get; set; }
        public string kmfare { get; set; }
        public string priceafterbasefare { get; set; }
        public string stopprice { get; set; }
        public string description { get; set; }
        public double totalfare { get; set; }
        public string created_at { get; set; }
        public List<AdditionalServices> services { get; set; }
        public ImageSource VehicleImage { get; set; }
        public byte[] ProductImage { get; set; }

        //public ImageSource VehicleImage { get {

        //        var imagedata = string.IsNullOrEmpty(image) ? ImageHelper.GetStreamFormResource("camera.png") : await ImageHelper.GetImageFromUrl(VehicleListSelectedData?.image);
        //        return ImageSource.FromStream(() => new MemoryStream(ProductImage));
        //    } }
    }
    public class AdditionalServices
    {
        public string currency { get; set; }
        public int id { get; set; }
        public string service { get; set; }
        public int price { get; set; }
    }
    public class VehicleServicesRequest
    {
        public long service_id { get; set; }
        public string services { get; set; }
        public string serviceCurrency { get; set; } = "₱";
        public string services_fee { get; set; }
        public double ServicesFee
        {
            get
            {
                return Convert.ToDouble(services_fee);
            }
        }
    }
    public class PriceBreakUpList
    {
        public string km_fare;

        public string price { get; set; }
        public string distance { get; set; }
        public string tip { get; set; }
        public string discount { get; set; }
        public string finalprice { get; set; }
        public string base_fare { get; set; }
    }
}

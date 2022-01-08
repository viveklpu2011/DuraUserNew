using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetDriverDetailsModel
    {
        public string drivername { get; set; }
        public string driverphoto { get; set; }
        public string distance { get; set; }
        public string time { get; set; }
        public string servicetype { get; set; }
        public string price { get; set; }
        public string vehicleno { get; set; }
    }

    public class GetDriverDetailsResponseModel : CommonResponseModel
    {
        public GetDriverDetailsModel data { get; set; }
    }
}

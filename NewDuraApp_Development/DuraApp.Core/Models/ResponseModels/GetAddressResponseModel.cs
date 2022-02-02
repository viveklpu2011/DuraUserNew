using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetAddressResponseModel : CommonResponseModel
    {
        public List<GetAddressModel> data { get; set; }
    }

    public class GetAddressModel
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contactperson { get; set; }
        public string mobile { get; set; }
        public string type { get; set; }
        public string isdefault { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string FullAddress { get { return $"{address1}"; } }
        public bool IsDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(isdefault) && isdefault == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

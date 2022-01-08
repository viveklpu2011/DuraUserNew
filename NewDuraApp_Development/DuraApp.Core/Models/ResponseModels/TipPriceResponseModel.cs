using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{

    public class TipPriceModel
    {
        public int id { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        public string created_at { get; set; }
        public string FullPrice
        {
            get
            {
                if (string.IsNullOrEmpty(currency))
                {
                    return $"₱{price}";
                }
                else
                {
                    return $"{currency}{price}";
                }

            }
        }
    }

    public class TipPriceResponseModel : CommonResponseModel
    {
        public List<TipPriceModel> data { get; set; }
    }
}

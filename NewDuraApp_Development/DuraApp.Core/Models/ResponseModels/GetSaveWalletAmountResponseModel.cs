using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetSaveWalletAmountResponseModel : CommonResponseModel
    {
        public Amount amount { get; set; }
    }

    public class Amount
    {
        public int id { get; set; }
        public string transactionid { get; set; }
        public long user_id { get; set; }
        public string phone { get; set; }
        public string method { get; set; }
        public string amount { get; set; }
        public string change_amount { get; set; }
        public string description { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool IsCredited
        {
            get
            {
                if (method == "credit")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string Sign
        {
            get
            {
                if (method == "credit")
                {
                    return "+";
                }
                else
                {
                    return "-";
                }
            }
        }
    }

    public class GetWalletAmountResponseModel : CommonResponseModel
    {
        public List<Amount> data { get; set; }
    }
}

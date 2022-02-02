using System;
using System.Collections.Generic;

namespace DuraApp.Core.Models.ResponseModels
{
    public class WalletAmountListResponseModel
    {
        public List<WalletAmountList> data { get; set; }
    }

    public class WalletAmountList
    {
        public int id { get; set; }
        public string amount { get; set; }
        public int status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}

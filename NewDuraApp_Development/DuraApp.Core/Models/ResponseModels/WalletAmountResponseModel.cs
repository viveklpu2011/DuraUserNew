using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class WalletAmountResponseModel : CommonResponseModel
    {
        public List<WalletAmountModel> data { get; set; }
    }

    public class WalletAmountModel
    {
        public int id { get; set; }
        public string amount { get; set; }
        public int status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string Backgrondcolor { get; set; }
        public string Bordercolor { get; set; }
        public string AmountWithCurrency
        {
            get
            {
                if (!string.IsNullOrEmpty(amount))
                {
                    return $"₱ {amount}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

}

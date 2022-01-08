using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class GetSaveWalletAmountRequestModel
    {
        public long user_id { get; set; }
        public double amount { get; set; }
        public string transactionid { get; set; }
        public string transactiontype { get; set; }
    }
}

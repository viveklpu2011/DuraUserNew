using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class PaymentModeRequestModel
    {
        public long pickup_id { get; set; }
        public string paymentmode { get; set; }
        public double price { get; set; }
        public long user_id { get; set; }
    }
}

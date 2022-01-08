using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class OrderDetailsRequestModel
    {
        public long user_id { get; set; }
        public int order_id { get; set; }
    }
}

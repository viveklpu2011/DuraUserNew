using System;
using System.Collections.Generic;
using System.Text;
using DuraApp.Core.Helpers.Enums;

namespace NewDuraApp.Models
{
    public class MyOrdersModel
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
        public string OrderNumber { get; set; }
        public string OrderOn { get; set; }
        public string OrderStatus { get; set; }
        public string ChooseOrderStatus { get; set; }
        public string CompleteOrderNumber { get; set; }
        public string CompleteOrderDate { get; set; }
        public string CompletedOrderAddress1 { get; set; }
        public string OrderAddress { get; set; }

    }
    public class OrderStatusModel
    {
        public OrderStatusType OrderId { get; set; }
        public string ChooseOrderStatus { get; set; }

    }
}

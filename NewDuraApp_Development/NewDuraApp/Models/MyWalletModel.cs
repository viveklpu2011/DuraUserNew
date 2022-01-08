using System;
namespace NewDuraApp.Models
{
    public class MyWalletModel
    {
        public long MyWalletId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionColor { get; set; }
        public string OrderId { get; set; }
        public string DateTime { get; set; }
        public string Price { get; set; }
    }
}

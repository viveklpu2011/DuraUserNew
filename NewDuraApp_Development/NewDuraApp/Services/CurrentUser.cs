
using System;
using System.Collections.Generic;
using System.Text;
using DuraApp.Core.Helpers;

namespace NewDuraApp.Services
{
    public class CurrentUser : BindableBase
    {
        public CurrentUser()
        {

        }
        private string _sendWay;
        public string SendWay
        {
            get => _sendWay;
            set => SetProperty(ref _sendWay, value);
        }
        private string _paymentWay;
        public string PaymentWay
        {
            get => _paymentWay;
            set => SetProperty(ref _paymentWay, value);
        }
    }
}

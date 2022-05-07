using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetOrderDetailsResponseModel : CommonResponseModel
    {
        public GetOrderDetailsModel data { get; set; }
    }

    public class GetOrderDetailsModel : ObservableProperty
    {
        public string driverlocationlink { get; set; }
        public string servicetype { get; set; }
        public int pickup_id { get; set; }
        public string order_no { get; set; }
        public string pickupdate { get; set; }
        public string pickupaddress { get; set; }
        public string pickupaddresssub { get; set; }
        public string stopaddress { get; set; }
        public string stopaddresssub { get; set; }
        public string wheretoaddress { get; set; }
        public string wheretoaddresssub { get; set; }
        public string distance { get; set; }
        public string time { get; set; }
        public string driverphoto { get; set; }
        public string drivermobile { get; set; }
        public string driverdescription { get; set; }
        public string drivername { get; set; }
        public string accounttype { get; set; }
        public string pickup_name { get; set; }
        public string pickuplat { get; set; }
        public string pickuplon { get; set; }
        public string destinationlat { get; set; }
        public string destinationlon { get; set; }
        public string pickup_mobile { get; set; }
        public string destination_mobile { get; set; }
        public Price price { get; set; }
        public string wheretoname { get; set; }
        public string wheretomobile { get; set; }
        public string vehicle { get; set; }
        public string username { get; set; }
        public long driver_id { get; set; }
        public string paymentmode { get; set; }
        public string orderdate { get; set; }
        public string status { get; set; }
        public string otp { get; set; }

        private string _displaystatus = string.Empty;
        public string displaystatus
        {
            get { return _displaystatus; }
            set
            {
                _displaystatus = value;
                OnPropertyChanged(nameof(displaystatus));
            }
        }
        private Color _displaystatustextcolor;
        public Color displaystatustextcolor
        {
            get { return _displaystatustextcolor; }
            set
            {
                _displaystatustextcolor = value;
                OnPropertyChanged(nameof(displaystatustextcolor));
            }
        }
        public bool IsOngoingViewVisible
        {
            get
            {
                if (!string.IsNullOrEmpty(displaystatus))
                {
                    if (displaystatus.ToLower() == "Ongoing-OrderPicked")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsCouponApplied
        {
            get
            {
                if (coupon != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Coupon coupon { get; set; }
        public string DiscountedAmount
        {
            get
            {
                if (coupon != null)
                {
                    var convertedAmount = Convert.ToDouble(coupon.discount);
                    var actualAmount = Convert.ToDouble(price.total);
                    var totalDiscount = actualAmount - (actualAmount * (convertedAmount) / 100);
                    return $"{price.currency}{totalDiscount}";
                }
                else
                {
                    return $"{price.currency}{price.total}";
                }
            }
        }
        public List<Stopdata> stopdata { get; set; }
        public bool IsAvailableStopData
        {
            get
            {
                if (stopdata != null && stopdata.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

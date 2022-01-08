using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DuraApp.Core.Models.Common;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetpickupDetailsResponseModel : CommonResponseModel
    {
        public GetpickupDetailsModel data { get; set; }
    }
    public class Coupon
    {
        public int id { get; set; }
        public string couponname { get; set; }
        public string discount { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
    }

    public class Price : ObservableProperty
    {

        private string _distance = string.Empty;
        public string distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(distance));
            }
        }
        private double _totalWithoutDiscounted;
        public double totalWithoutDiscounted
        {
            get { return _totalWithoutDiscounted; }
            set
            {
                _totalWithoutDiscounted = value;
                OnPropertyChanged(nameof(totalWithoutDiscounted));
            }
        }
        private double _totalWithDiscounted;
        public double totalWithDiscounted
        {
            get { return _totalWithDiscounted; }
            set
            {
                _totalWithDiscounted = value;
                OnPropertyChanged(nameof(totalWithDiscounted));
            }
        }
        private double _kmprice;
        public double kmprice
        {
            get { return _kmprice; }
            set
            {
                _kmprice = value;
                OnPropertyChanged(nameof(kmprice));
            }
        }
        private string _basefare = string.Empty;
        public string basefare
        {
            get { return _basefare; }
            set
            {
                _basefare = value;
                OnPropertyChanged(nameof(basefare));
            }
        }
        private double _total;
        public double total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged(nameof(total));
            }
        }
        //public double total
        //      {
        //          get
        //          {
        //		if (total == null)
        //		{
        //			return 0;
        //		}
        //		else
        //		{
        //			return total;
        //		}
        //          }
        //      }
        //public string currency { get; set; }

        private string _currency = string.Empty;
        public string currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(currency));
            }
        }


        public double surgecharge { get; set; }
        public string tip { get; set; }

        private List<ServicesModel> _services;
        public List<ServicesModel> services
        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged(nameof(services));
            }
        }


        public double TotalAmount
        {
            get
            {
                if (services != null && services.Count > 0)
                {
                    return services.Sum(item => item.ServicesFee);
                }
                else
                {
                    return 0;
                }
            }
        }

        private string _per_km = string.Empty;
        public string per_km
        {
            get { return _per_km; }
            set
            {
                _per_km = value;
                OnPropertyChanged(nameof(per_km));
            }
        }
    }
    public class GetpickupDetailsModel : ObservableProperty
    {
        //public string displaystatustextcolor;

        //public string displaystatus { get; set; }

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

        public string servicetype { get; set; }
        public int pickup_id { get; set; }
        public string order_no { get; set; }
        public string pickup { get; set; }
        public string stoplocation { get; set; }
        public string dropoff { get; set; }
        public string distance { get; set; }
        public string time { get; set; }
        public string pickup_name { get; set; }
        public string pickuplat { get; set; }
        public string pickuplon { get; set; }
        public string destinationlat { get; set; }
        public string destinationlon { get; set; }
        public string pickup_mobile { get; set; }
        public string destination_mobile { get; set; }
        public Price price { get; set; }
        public string drivername { get; set; }
        public string driverphoto { get; set; }
        public string drivermobile { get; set; }
        public string driverdescription { get; set; }
        public string vehicle { get; set; }
        public string username { get; set; }
        public string usermobile { get; set; }
        public string vehicleno { get; set; }
        public string orderdate { get; set; }
        public string status { get; set; }
        public string verification_code { get; set; }
        public string driverlocationlink { get; set; }

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
        public bool IsAvailableStop
        {
            get
            {
                if (string.IsNullOrEmpty(stoplocation))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public List<Stopdata> stopdata { get; set; }
        public string wheretoname { get; set; }
        public string wheretomobile { get; set; }
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
    public class Stopdata
    {
        public string stop_address1 { get; set; }
        public string stop_address2 { get; set; }
        public string stop_name { get; set; }
        public string stop_mobile { get; set; }
        public string stoplat { get; set; }
        public string stoplon { get; set; }
    }
    public class ServicesModel
    {
        public int id { get; set; }
        public string services { get; set; }
        public string serviceCurrency { get; set; } = "₱";
        public string servicefee { get; set; }
        public double ServicesFee
        {
            get
            {
                return Convert.ToDouble(servicefee);
            }
        }
    }
}

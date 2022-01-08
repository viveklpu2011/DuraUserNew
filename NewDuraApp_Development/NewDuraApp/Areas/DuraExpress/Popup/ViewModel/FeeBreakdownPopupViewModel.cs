using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Models.ResponseModels;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraExpress.Popup.ViewModel
{
    public class FeeBreakdownPopupViewModel : AppBaseViewModel
    {
        private GetpickupDetailsModel _getpickupData;
        public GetpickupDetailsModel GetPickupData
        {
            get { return _getpickupData; }
            set
            {
                try
                {
                    _getpickupData = value;
                    OnPropertyChanged(nameof(GetPickupData));
                }
                catch (Exception ex) { }
            }
        }
        public FeeBreakdownPopupViewModel()
        {

        }
        internal async Task InitilizeData(GetpickupDetailsModel getpickupDetailsModel = null)
        {
            if (getpickupDetailsModel != null)
            {
                GetPickupData = getpickupDetailsModel;
            }
            //await GetPickupDetails();
        }

        internal async Task InitilizeDataBeforeOrder()
        {
            //AppConstant.ServiceTotal
            //AppConstant.SelectedVehicle

            GetPickupData = new GetpickupDetailsModel();
            GetPickupData.price = new Price();
            var lst = new List<ServicesModel>();
            if (AppConstant.ServiceList != null && AppConstant.ServiceList.Count > 0)
            {
                lst = AppConstant.ServiceList;
            }
            else
            {
                lst = new List<ServicesModel>();
            }
            GetPickupData.price.services = lst;
            GetPickupData.price.currency = AppConstant.SelectedVehicle.currency;
            GetPickupData.vehicle = AppConstant.SelectedVehicle.vehicle_type;

            var fare = Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare) - (Convert.ToDouble(AppConstant.SelectedVehicle.basefare) + GetPickupData.price.services.Sum(x => Convert.ToInt32(x.ServicesFee)));
            var total = GetPickupData.price.services.Sum(x => Convert.ToInt32(x.ServicesFee)) + Convert.ToDouble(AppConstant.SelectedVehicle.basefare) + fare;
            GetPickupData.price.totalWithoutDiscounted = GetPickupData.price.services.Sum(x => Convert.ToInt32(x.ServicesFee)) + Convert.ToDouble(AppConstant.SelectedVehicle.basefare) + fare;
            if (App.Locator.AddMoreDetails.VerifyPromoCode != null)
            {
                var coupondata = App.Locator.AddMoreDetails.VerifyPromoCode;
                Coupon coupon = new Coupon
                {
                    couponname = coupondata?.name,
                    currency = AppConstant.SelectedVehicle.currency,
                    description = coupondata?.application,
                    discount = coupondata?.discount,
                    id = coupondata.id
                };
                GetPickupData.coupon = coupon;
                total = Convert.ToDouble(total) - (Convert.ToDouble(total) * (Convert.ToDouble(coupondata.discount) / 100));
                GetPickupData.price.totalWithDiscounted = total;
                //fare = Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare) - (Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare) * (Convert.ToDouble(coupondata.discount) / 100));
            }
            else
            {
                GetPickupData.coupon = null;
            }

            total = total + AppConstant.TipAmount;

            GetPickupData.price.basefare = AppConstant.SelectedVehicle.basefare;
            GetPickupData.price.per_km = AppConstant.SelectedVehicle.kmfare;
            GetPickupData.price.distance = Convert.ToString(fare / Convert.ToDouble(GetPickupData.price.per_km));
            //if (Convert.ToDouble(GetPickupData.price.distance) <= 1)
            //	GetPickupData.price.distance = "0";
            GetPickupData.price.kmprice = Math.Round(Convert.ToDouble(Convert.ToDouble(String.Format("{0:0.00}", GetPickupData.price.distance)) * Convert.ToDouble(String.Format("{0:0.00}", GetPickupData.price.per_km))));
            GetPickupData.price.tip = AppConstant.TipAmount.ToString();
            GetPickupData.price.services = lst;
            GetPickupData.price.total = Math.Round(total);

            GetPickupData.distance = String.Format("{0:0.00}", GetPickupData.price.distance);
            await Task.CompletedTask;
        }
    }
}

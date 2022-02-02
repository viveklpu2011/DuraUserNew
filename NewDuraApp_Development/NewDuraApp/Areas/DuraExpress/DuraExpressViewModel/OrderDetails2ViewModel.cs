using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class OrderDetails2ViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToHomeCmd { get; set; }
        public IAsyncCommand RateCommand { get; set; }
        public IAsyncCommand CallNowCommand { get; set; }
        public IAsyncCommand SMSCommand { get; set; }
        private IUserCoreService _userCoreService;
        private GetOrderDetailsModel _getOrderData;
        public IAsyncCommand TrackNowCommand { get; set; }
        public IAsyncCommand DuraHelpCommand { get; set; }
        public GetOrderDetailsModel GetOrderData
        {
            get { return _getOrderData; }
            set
            {
                _getOrderData = value;
                OnPropertyChanged(nameof(GetOrderData));
            }
        }

        private bool _isVisibleRateButton;
        public bool IsVisibleRateButton
        {
            get { return _isVisibleRateButton; }
            set
            {
                _isVisibleRateButton = value;
                OnPropertyChanged(nameof(IsVisibleRateButton));
            }
        }

        private bool _isVisibleCouponView;
        public bool IsVisibleCouponView
        {
            get { return _isVisibleCouponView; }
            set
            {
                _isVisibleCouponView = value;
                OnPropertyChanged(nameof(IsVisibleCouponView));
            }
        }

        private bool _isVisibleHome = true;
        public bool IsVisibleHome
        {
            get { return _isVisibleHome; }
            set
            {
                _isVisibleHome = value;
                OnPropertyChanged(nameof(IsVisibleHome));
            }
        }

        private GetpickupDetailsModel _getpickupData;
        public GetpickupDetailsModel GetPickupData
        {
            get { return _getpickupData; }
            set
            {
                _getpickupData = value;
                OnPropertyChanged(nameof(GetPickupData));
            }
        }

        public OrderDetails2ViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToHomeCmd = new AsyncCommand(GoToHomeCmdExecute, allowsMultipleExecutions: false);
            RateCommand = new AsyncCommand(RateCommandExecute, allowsMultipleExecutions: false);
            TrackNowCommand = new AsyncCommand(TrackNowCommandExecute, allowsMultipleExecutions: false);
            DuraHelpCommand = new AsyncCommand(DuraHelp, allowsMultipleExecutions: false);
            CallNowCommand = new AsyncCommand(CallNowCommandExecute, allowsMultipleExecutions: false);
            SMSCommand = new AsyncCommand(SMSCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task SMSCommandExecute()
        {
            await PhoneHelper.SendSms($"Hi this is {GetOrderData?.username}, have booked your {GetOrderData?.vehicle}", GetOrderData?.drivermobile);
        }

        private async Task CallNowCommandExecute()
        {
            PhoneHelper.PlacePhoneCall(GetOrderData?.drivermobile);
        }

        private async Task DuraHelp()
        {
            await _navigationService.NavigateToAsync<ChatViewModel>();
        }

        private async Task TrackNowCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderTrackerPageViewModel))
            {
                await _navigationService.NavigateToAsync<OrderTrackerPageViewModel>();
                await App.Locator.OrderTrackerPage.InitializedData();
            }
        }

        private async Task RateCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddRatingPopupPageViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new AddRatingPopupPage());
                await App.Locator.AddRatingPopupPage.InitilizeData(GetOrderData);
            }
        }

        private async Task GoToHomeCmdExecute()
        {
            App.Locator.AddStopLocation.StopAddressList.Clear();
            App.Locator.AddStopLocation.stopid = 0;
            App.Locator.AddStopLocation.StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.TrackOrder.StopAddressList.Clear();
            App.Locator.AddMoreDetails.LstServices = new List<VehicleServicesRequest>();
            App.Locator.AddMoreDetails.LstServices.Clear();
            App.Locator.SelectVehicle.LstServices = new List<VehicleServicesRequest>();
            App.Locator.SelectVehicle.LstServices.Clear();
            App.Locator.AddMoreDetails.TipAmount = "0";
            App.Locator.AddMoreDetails.VehicleListSelectedData = null;
            App.Locator.AddMoreDetails.VerifyPromoCode = null;
            App.Locator.SelectVehicle.VehicleListSelectedData = null;
            App.Locator.PickupSchedule.IsVisibleLaterView = false;
            App.Locator.PickupSchedule.AsapIsChecked = true;
            App.Locator.PickupSchedule.LaterIsCheck = false;
            App.Locator.PickupSchedule.IsButtonEnabled = true;
            App.Locator.PickupSchedule.DuraAddressCommon = null;
            App.Locator.PickupSchedule.PickupScheduleRequest = null;
            App.Locator.PickupLocation.PickupScheduleRequest = null;
            App.Locator.WhereTo.PickupScheduleRequest = null;
            App.Locator.AddStopLocation.PickupScheduleRequest = null;
            App.Locator.DuraExpress.PickupLocationText = string.Empty;
            App.Locator.DuraExpress.PickupLocationTextVisible = false;
            SettingsExtension.PickupAddress = string.Empty;
            App.Locator.DuraExpress.PickupScheduleLocText = string.Empty;
            App.Locator.DuraExpress.PickupScheduleLocTextVisible = false;
            App.Locator.DuraExpress.PickupWhereToText = string.Empty;
            App.Locator.DuraExpress.PickupWhereToTextVisible = false;
            App.Locator.PickupLocation.Address2 = string.Empty;
            App.Locator.WhereTo.Address2 = string.Empty;
            App.Locator.AddStopLocation.Address2 = string.Empty;
            if (_navigationService.GetCurrentPageViewModel() != typeof(HomePageViewModel))
            {
                TabNavigationHelper.ForceFullyRedirectingTab(0);
            }
        }

        internal async Task InitilizeData(GetpickupDetailsModel getpickupDetailsModel, bool isVisibleHome = true)
        {
            if (getpickupDetailsModel != null)
            {
                IsVisibleHome = isVisibleHome;
                if (!string.IsNullOrEmpty(getpickupDetailsModel.status))
                {
                    if (getpickupDetailsModel.status.ToLower() == "completed")
                    {
                        IsVisibleRateButton = true;
                    }
                    else
                    {
                        IsVisibleRateButton = false;
                    }
                }
                else
                {
                    IsVisibleRateButton = false;
                }
                await GetOrderDetails(getpickupDetailsModel.pickup_id);
            }
        }

        private async Task GetOrderDetails(int orderid)
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    OrderDetailsRequestModel commonUserIdRequestModel = new OrderDetailsRequestModel
                    {
                        user_id = SettingsExtension.UserId,
                        order_id = orderid
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetOrderDetails(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (result?.Data?.data != null)
                        {
                            GetOrderData = result?.Data?.data;
                            if (result?.Data?.data?.coupon != null)
                            {
                                IsVisibleCouponView = true;
                            }
                            else
                            {
                                IsVisibleCouponView = false;
                            }
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    ShowToast(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}
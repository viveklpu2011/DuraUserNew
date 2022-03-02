using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Areas.DuraShop.Popup.View;
using NewDuraApp.Areas.DuraShop.Popup.ViewModel;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class PaymentViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToSelectAmount { get; set; }
        public IAsyncCommand GoToFindingDriverCmd { get; set; }
        public IAsyncCommand GoToSuccessPopupCmd { get; set; }
        public IAsyncCommand GoToTopCmd { get; set; }
        public IAsyncCommand GoToCODCmd { get; set; }
        public IAsyncCommand GoToFeeBreakdownPopup { get; set; }
        private IUserCoreService _userCoreService;
        public IAsyncCommand GoToCashOnDeliveryPopup { get; set; }

        private string _totalFinalFare;
        public string TotalFinalFare
        {
            get { return _totalFinalFare; }
            set { _totalFinalFare = value; OnPropertyChanged(nameof(TotalFinalFare)); }
        }

        private GetSaveWalletAmountResponseModel _getSaveWalletAmount;
        public GetSaveWalletAmountResponseModel GetSaveWalletAmount
        {
            get { return _getSaveWalletAmount; }
            set { _getSaveWalletAmount = value; OnPropertyChanged(nameof(GetSaveWalletAmount)); }
        }

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }

        private bool _eWalletIsvisible;
        public bool EWalletIsvisible
        {
            get { return _eWalletIsvisible; }
            set { _eWalletIsvisible = value; OnPropertyChanged(); }
        }

        private bool _eWallet2Isvisible;
        public bool EWallet2Isvisible
        {
            get { return _eWallet2Isvisible; }
            set { _eWallet2Isvisible = value; OnPropertyChanged(); }
        }

        public PaymentViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToFindingDriverCmd = new AsyncCommand(GoToFindingDriverCmdExecute, allowsMultipleExecutions: false);
            GoToSuccessPopupCmd = new AsyncCommand(GoToSuccessPopupCmdExecute, allowsMultipleExecutions: false);
            GoToTopCmd = new AsyncCommand(GoToTopCmdExecute, allowsMultipleExecutions: false);
            NavigateToSelectAmount = new AsyncCommand(NavigateToSelectAmountPage, allowsMultipleExecutions: false);
            GoToCashOnDeliveryPopup = new AsyncCommand(GoToCashOnDeliveryPopupExecute, allowsMultipleExecutions: false);
            GoToCODCmd = new AsyncCommand(GoToCODCmdExecute, allowsMultipleExecutions: false);
            GoToFeeBreakdownPopup = new AsyncCommand(GoToFeeBreakdownPopupExecute, allowsMultipleExecutions: false);
            EWalletIsvisible = false;
            EWallet2Isvisible = true;
        }

        private async Task GoToFeeBreakdownPopupExecute()
        {
            await App.Locator.FeeBreakdownPopup.InitilizeDataBeforeOrder();
            if (_navigationService.GetCurrentPageViewModel() != typeof(FeeBreakdownPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new FeeBreakdownPopup());
            }
        }

        private async Task GoToCODCmdExecute()
        {
            await AddpaymentMode("COD");
        }

        private async Task GoToCashOnDeliveryPopupExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(CashOnDeliveryPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new CashOnDeliveryPopup());
            }
        }

        private async Task NavigateToSelectAmountPage()
        {
            TopupAmuntPopupPageViewModel topupAmuntPopupPageViewModel = new TopupAmuntPopupPageViewModel(_navigationService, _userCoreService);
            await _navigationService.NavigateToPopupAsync<TopupAmuntPopupPageViewModel>(true, topupAmuntPopupPageViewModel);
        }

        private async Task GoToFindingDriverCmdExecute()
        {
            if (GetSaveWalletAmount != null && GetSaveWalletAmount.amount != null)
            {
                double walletAmount = Convert.ToDouble(GetSaveWalletAmount.amount.amount);
                double orderAmount = Convert.ToDouble(TotalFinalFare);
                if (walletAmount <= 0)
                {
                    ShowLoading();
                    var resultpayment = await App.Current.MainPage.DisplayAlert(AppResources.Alert, AppResources.Your_wallet_amount_is_notsufficient_Do_you_want_to_proceed_with_COD, "COD", AppResources.Recharge_Now);
                    HideLoading();
                    if (resultpayment)
                    {
                        await AddpaymentMode("COD");
                    }
                    else
                    {
                        TopupAmuntPopupPageViewModel topupAmuntPopupPageViewModel = new TopupAmuntPopupPageViewModel(_navigationService, _userCoreService);
                        await _navigationService.NavigateToPopupAsync<TopupAmuntPopupPageViewModel>(true, topupAmuntPopupPageViewModel);
                    }
                }
                else
                {
                    var resultpayment = await App.Current.MainPage.DisplayAlert(AppResources.Alert, $"{TotalFinalFare}" + AppResources.will_be_deducted_from_your_wallet_Do_you_really_want_to_proceed, AppResources.Yes, AppResources.No);
                    if (resultpayment)
                    {
                        if (walletAmount < orderAmount)
                        {
                            var resultcheck = await App.Current.MainPage.DisplayAlert(AppResources.Alert, AppResources.Wallet_Amount_is_less_than_order_amount_Do_you_really_want_to_proceed_with_COD, AppResources.Yes, AppResources.No);
                            if (resultcheck)
                            {
                                await AddpaymentMode("COD");
                            }
                        }
                        else
                        {
                            await AddpaymentMode("Wallet");
                        }
                    }
                }
            }
            else
            {
                var resultpayment = await App.Current.MainPage.DisplayAlert(AppResources.Alert, AppResources.Your_wallet_amount_is_notsufficient_Do_you_want_to_proceed_with_COD, "COD", AppResources.Recharge_Now);
                if (resultpayment)
                {
                    await AddpaymentMode("COD");
                }
                else
                {
                    TopupAmuntPopupPageViewModel topupAmuntPopupPageViewModel = new TopupAmuntPopupPageViewModel(_navigationService, _userCoreService);
                    await _navigationService.NavigateToPopupAsync<TopupAmuntPopupPageViewModel>(true, topupAmuntPopupPageViewModel);
                }
            }
        }

        private async Task GoToSuccessPopupCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(SuccessOrderPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new SuccessOrderPopup());
            }
        }

        private async Task GoToTopCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(SuccessOrderPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new SuccessOrderPopup());
            }
        }

        public async Task InitilizeData()
        {
            TotalFinalFare = App.Locator.AddMoreDetails.TotalFinalFare;
            Currency = App.Locator.SelectVehicle.Currency;
            await GetWalletAmount();
        }

        private async Task GetWalletAmount()
        {
            if (CheckConnection())
            {
                try
                {
                    GetSaveWalletAmountRequestModel getSaveWalletAmountRequestModel = new GetSaveWalletAmountRequestModel
                    {
                        user_id = SettingsExtension.UserId,
                        amount = 0
                    };
                    ShowLoading();
                    var result = await TryWithErrorAsync(_userCoreService.SaveWalletAmount(getSaveWalletAmountRequestModel, SettingsExtension.Token), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        GetSaveWalletAmount = result?.Data;
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        private async Task FindDriver()
        {
            if (CheckConnection())
            {
                try
                {
                    FindDriverRequestModel findDriverRequestModel = new FindDriverRequestModel
                    {
                        pickup_id = App.Locator.AddMoreDetails.PickupScheduleResponse.data
                    };
                    ShowLoading();
                    var result = await TryWithErrorAsync(_userCoreService.FindDriver(findDriverRequestModel, SettingsExtension.Token), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (_navigationService.GetCurrentPageViewModel() != typeof(FindingDriverViewModel))
                        {
                            await _navigationService.NavigateToAsync<FindingDriverViewModel>();
                            await App.Locator.FindingDriver.InitilizeData();
                        }
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
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

        private async Task AddpaymentMode(string mode)
        {
            if (CheckConnection())
            {
                try
                {
                    PaymentModeRequestModel paymentModeRequestModel = new PaymentModeRequestModel();
                    if (mode == "COD")
                    {
                        paymentModeRequestModel.pickup_id = App.Locator.AddMoreDetails.PickupScheduleResponse.data;
                        paymentModeRequestModel.paymentmode = mode;
                        paymentModeRequestModel.user_id = SettingsExtension.UserId;
                    }
                    else
                    {
                        paymentModeRequestModel.pickup_id = App.Locator.AddMoreDetails.PickupScheduleResponse.data;
                        paymentModeRequestModel.paymentmode = mode;
                        paymentModeRequestModel.price = Convert.ToDouble(TotalFinalFare);
                        paymentModeRequestModel.user_id = SettingsExtension.UserId;
                    }

                    ShowLoadingWithTitle(AppResources.Searching_Driver);
                    var result = await TryWithErrorAsync(_userCoreService.PaymentMode(paymentModeRequestModel, SettingsExtension.Token), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (mode == "COD")
                        {
                            ShowToast(AppResources.You_will_have_to_pay + " " + TotalFinalFare + " " + AppResources.in_cash_when_delivered);
                        }
                        if (App.Locator.PickupSchedule.DuraAddressCommon.PickupType.ToLower().Contains("later"))
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetailsViewModel))
                            {
                                await _navigationService.NavigateToAsync<OrderDetailsViewModel>();
                                await App.Locator.OrderDetails.InitilizeData();
                            }
                        }
                        else
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(FindingDriverViewModel))
                            {
                                await _navigationService.NavigateToAsync<FindingDriverViewModel>();
                                await App.Locator.FindingDriver.InitilizeData();
                            }
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

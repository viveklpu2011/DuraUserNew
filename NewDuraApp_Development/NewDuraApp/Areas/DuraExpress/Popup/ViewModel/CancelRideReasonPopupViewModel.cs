using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.Popup.ViewModel
{
    public class CancelRideReasonPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand CancelRideCommand { get; set; }
        public IAsyncCommand ConfirmOrderBackRideCommand { get; set; }
        private GetDriverDetailsModel driverDetails;
        public GetDriverDetailsModel DriverDetails
        {
            get { return driverDetails; }
            set
            {
                driverDetails = value;
                OnPropertyChanged(nameof(DriverDetails));
            }
        }

        private byte[] _productImage;
        public byte[] ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }

        private string _reasonText;
        public string ReasonText
        {
            get { return _reasonText; }
            set
            {
                _reasonText = value;
                if (RegexUtilities.EmptyString(_reasonText))
                {
                    IsReasonTextErrorVisible = false;
                }
                else
                {
                    IsReasonTextErrorVisible = true;
                }
                CheckReasonValidation();
                OnPropertyChanged(nameof(ReasonText));
            }
        }

        private bool _isbuttonEnabled;
        public bool IsButtonEnabled
        {
            get { return _isbuttonEnabled; }
            set
            {
                _isbuttonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private bool _isReasonTextErrorVisible;
        public bool IsReasonTextErrorVisible
        {
            get { return _isReasonTextErrorVisible; }
            set
            {
                _isReasonTextErrorVisible = value;
                OnPropertyChanged(nameof(IsReasonTextErrorVisible));
            }
        }

        public CancelRideReasonPopupViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            CancelRideCommand = new AsyncCommand(CancelRideCommandExecute, allowsMultipleExecutions: false);
            ConfirmOrderBackRideCommand = new AsyncCommand(ConfirmOrderBackRideCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task ConfirmOrderBackRideCommandExecute()
        {
            await _navigationService.ClosePopupsAsync();
            await App.Locator.FoundDriverPopup.InitilizeData(DriverDetails);
            await PopupNavigation.Instance.PushAsync(new FoundDriverPopup());
        }

        private async Task CancelRideCommandExecute()
        {
            var result = await ShowConfirmationAlert(AppResources.Do_you_want_to_cancel_your_order);
            if (result)
            {
                await CancelRideExecute();
            }
            else
            {
                await _navigationService.ClosePopupsAsync();
                Device.StartTimer(new TimeSpan(0, 0, 0), () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.Locator.FoundDriverPopup.InitilizeData(DriverDetails);
                        await PopupNavigation.Instance.PushAsync(new FoundDriverPopup());
                    });
                    return false; // runs again, or false to stop
                });
            }
        }

        internal async Task InitilizeData(GetDriverDetailsModel getDriverDetailsModel = null)
        {
            if (getDriverDetailsModel != null)
            {
                DriverDetails = getDriverDetailsModel;
            }
        }

        private async Task CancelRideExecute()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    long pickupId = 0;
                    if (App.Locator.AddMoreDetails != null && App.Locator.AddMoreDetails.PickupScheduleResponse != null)
                    {
                        pickupId = App.Locator.AddMoreDetails.PickupScheduleResponse.data;
                    }
                    else
                    {
                        pickupId = App.Locator.MyOrders.PickUpId;
                    }
                    CancelRideRequestModel cancelRideRequestModel = new CancelRideRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                        driver_id = 0,
                        canceledby = "user",
                        pickup_id = pickupId,
                        reason = ReasonText
                    };
                    var result = await TryWithErrorAsync(_userCoreService.CancelRide(cancelRideRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (App.Locator.AddMoreDetails != null && App.Locator.AddMoreDetails.PickupScheduleResponse != null)
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetailsViewModel))
                            {
                                App.Locator.FindingDriver.SearchText = AppResources.Search_for_the_driver;
                                App.Locator.FindingDriver.IsVisibleSearchImage = false;
                                App.Locator.FindingDriver.CancelButtonText = AppResources.Search;
                                await _navigationService.ClosePopupsAsync();
                                await _navigationService.NavigateToAsync<OrderDetailsViewModel>();
                                await App.Locator.OrderDetails.InitilizeData();
                            }
                        }
                        else
                        {
                            await _navigationService.ClosePopupsAsync();
                            await App.Locator.MyOrders.InitilizeDataMyOrder();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }

        private bool CheckReasonValidation()
        {
            if (IsReasonTextErrorVisible)
            {
                IsButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(ReasonText))
            {
                IsButtonEnabled = false;
                return false;
            }
            else
            {
                IsButtonEnabled = true;
                return true;
            }
        }
    }
}

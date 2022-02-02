using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
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
    public class FindingDriverViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToCancelDriverCmd { get; set; }
        public IAsyncCommand TryAgainCmd { get; set; }
        private IUserCoreService _userCoreService;
        private string _searchText = AppResources.Please_Wait_Finding_Driver;

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set
            {
                _isBackEnabled = value;
                OnPropertyChanged(nameof(IsBackEnabled));
            }
        }

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

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string _cancelButtonText = AppResources.Cancel;
        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                _cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }

        private bool _isVisibleSearchImage = true;
        public bool IsVisibleSearchImage
        {
            get { return _isVisibleSearchImage; }
            set
            {
                _isVisibleSearchImage = value;
                OnPropertyChanged(nameof(IsVisibleSearchImage));
            }
        }

        public FindingDriverViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToCancelDriverCmd = new AsyncCommand(GoToCancelDriverCmdExecute, allowsMultipleExecutions: false);
            TryAgainCmd = new AsyncCommand(TryAgainCmdExecute, allowsMultipleExecutions: false);
        }

        private async Task TryAgainCmdExecute()
        {
            await GetdriverDetails();
        }

        private async Task GoToCancelDriverCmdExecute()
        {
            if (CancelButtonText == AppResources.Try_Again || CancelButtonText == AppResources.Search)
            {
                IsBackEnabled = false;
                await GetdriverDetails();
            }
            else
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(CancelDriverPopupViewModel))
                {
                    await PopupNavigation.Instance.PushAsync(new CancelDriverPopup());
                }
            }

        }

        internal async Task<DuraApp.Core.Models.Result.Result<GetDriverDetailsResponseModel>> CheckDriver(GetDriverDetailsRequestModel getDriverDetailsRequestModel)
        {
            var result = await TryWithErrorAsync(_userCoreService.GetDriverDetails(getDriverDetailsRequestModel, SettingsExtension.Token), true, false);
            return result;
        }

        private async Task GetdriverDetails()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    IsVisibleSearchImage = true;
                    GetDriverDetailsRequestModel getDriverDetailsRequestModel = new GetDriverDetailsRequestModel
                    {
                        pickup_id = App.Locator.AddMoreDetails.PickupScheduleResponse.data,
                        user_id = SettingsExtension.UserId,
                        step = "1",
                    };

                    GetDriverDetailsRequestModel getDriverDetailsRequestModel1 = new GetDriverDetailsRequestModel
                    {
                        pickup_id = App.Locator.AddMoreDetails.PickupScheduleResponse.data,
                        user_id = SettingsExtension.UserId,
                        step = "0",
                    };

                    DriverDetails = new GetDriverDetailsModel();
                    ShowLoadingWithTitle(AppResources.Searching_Driver);
                    var result = await CheckDriver(getDriverDetailsRequestModel);
                    if (result.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (result.Data?.data.drivername.ToString().Trim() == string.Empty)
                        {
                            var end = DateTime.Now.AddMinutes(1);
                            while (end > DateTime.Now)
                            {

                                var result1 = await CheckDriver(getDriverDetailsRequestModel1);

                                if (result1.Data?.data.drivername.ToString().Trim() != string.Empty)
                                {
                                    HideLoading();
                                    DriverDetails = result1?.Data?.data;
                                    await App.Locator.FoundDriverPopup.InitilizeData(DriverDetails);
                                    await PopupNavigation.Instance.PushAsync(new FoundDriverPopup());
                                    break;
                                }
                            }
                        }
                        else
                        {
                            HideLoading();
                            DriverDetails = new GetDriverDetailsModel();
                            DriverDetails = result?.Data?.data;
                            await App.Locator.FoundDriverPopup.InitilizeData(DriverDetails);
                            await PopupNavigation.Instance.PushAsync(new FoundDriverPopup());
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        HideLoading();
                        IsBackEnabled = true;
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        HideLoading();
                        IsBackEnabled = true;
                        CancelButtonText = AppResources.Try_Again;
                        SearchText = AppResources.Sorry_we_dont_serve_this_location_yet;
                        ShowAlert(AppResources.Our_Service_are_currently_not_available_in_this_city_We_will_notify_you_as_soon_as_we_launch);
                    }

                    if (DriverDetails.drivername == null)
                    {
                        HideLoading();
                        SearchText = "Please select different location for your order.";
                        ShowAlert("No Driver Found Near your location");
                    }
                    IsVisibleSearchImage = false;
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        internal async Task InitilizeData()
        {
            await GetdriverDetails();
        }
    }
}

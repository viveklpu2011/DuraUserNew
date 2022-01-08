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
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class FindingDriverViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToCancelDriverCmd { get; set; }
        private IUserCoreService _userCoreService;
        private string _searchText =AppResources.Please_Wait_Finding_Driver;


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
        internal async Task InitilizeData()
        {
            await GetdriverDetails();
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
                        user_id = SettingsExtension.UserId
                    };
                    ShowLoadingWithTitle(AppResources.Searching_Driver);
                    var result = await TryWithErrorAsync(_userCoreService.GetDriverDetails(getDriverDetailsRequestModel, SettingsExtension.Token), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        DriverDetails = new GetDriverDetailsModel();
                        DriverDetails = result?.Data?.data;
                        await App.Locator.FoundDriverPopup.InitilizeData(DriverDetails);
                        await PopupNavigation.Instance.PushAsync(new FoundDriverPopup());
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        IsBackEnabled = true;
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        IsBackEnabled = true;
                        CancelButtonText = AppResources.Try_Again;
                        SearchText = AppResources.Sorry_we_dont_serve_this_location_yet;
                        ShowAlert(AppResources.Our_Service_are_currently_not_available_in_this_city_We_will_notify_you_as_soon_as_we_launch);
                    }
                    IsVisibleSearchImage = false;
                }
                catch (Exception ex)
                {
                    //ShowToast(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
        //public ICommand GoToCancelDriverCmd => new Command(async (obj) =>
        //{
        //    MessagingCenter.Send<FindingDriverViewModel>(this, "FindingDriver");
        //    await PopupNavigation.Instance.PushAsync(new CancelDriverPopup());
        //});
    }
}

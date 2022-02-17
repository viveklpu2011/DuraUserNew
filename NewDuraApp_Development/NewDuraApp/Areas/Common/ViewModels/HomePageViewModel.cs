using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.Services.LocationService;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class HomePageViewModel : AppBaseViewModel
    {
        public IAsyncCommand GoToNotificationsCmd { get; set; }
        public IAsyncCommand GoToDuraShopCmd { get; set; }
        public IAsyncCommand GoToDuraExpressCmd { get; set; }
        private IAuthenticationService _authService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand GoToSearchLoactionPopupCmd { get; set; }

        private GetProfileDetailsModel _profileDetails;
        public GetProfileDetailsModel ProfileDetails
        {
            get { return _profileDetails; }
            set
            {
                _profileDetails = value;
                OnPropertyChanged(nameof(ProfileDetails));
            }
        }

        public ILocationService locationService => DependencyService.Get<ILocationService>();
        public IPlatformSpecificLocationService platFormLocationService => DependencyService.Get<IPlatformSpecificLocationService>();

        private ObservableCollection<GetAddressModel> _addressList;
        public ObservableCollection<GetAddressModel> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; OnPropertyChanged(nameof(AddressList)); }
        }

        public IAsyncCommand NavigateToDuraEatsCommand { get; set; }
        private List<NewLocationDataResponse> locList;
        private ObservableCollection<NewLocationDataResponse> _allLocationList;
        public ObservableCollection<NewLocationDataResponse> AllLocationList
        {
            get { return _allLocationList; }
            set
            {
                SetProperty(ref _allLocationList, value);
            }
        }

        private CommonLatLong commonLatLong;
        public CommonLatLong CommonLatLong
        {
            get { return commonLatLong; }
            set
            {
                commonLatLong = value;
                OnPropertyChanged(nameof(CommonLatLong));
            }
        }

        private string _currentLocation;
        public string CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
            }
        }

        INavigationService _navigationService;
        private bool _isHavingFromSavedAddress;
        public bool IsHavingFromSAvedAddress
        {
            get { return _isHavingFromSavedAddress; }
            set
            {
                _isHavingFromSavedAddress = value;
                OnPropertyChanged(nameof(IsHavingFromSAvedAddress));
            }
        }

        private ObservableCollection<NewLocationDataResponse> _allLocationListCode;
        public ObservableCollection<NewLocationDataResponse> AllLocationListCode
        {
            get { return _allLocationListCode; }
            set
            {
                SetProperty(ref _allLocationListCode, value);
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public IAsyncCommand RefreshCommand { get; set; }
        public HomePageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            _userCoreService = userCoreService;
            GoToNotificationsCmd = new AsyncCommand(GoToNotificationsCmdExecute);
            GoToSearchLoactionPopupCmd = new AsyncCommand(GoToSearchLoactionPopupCmdExecute);
            GoToDuraShopCmd = new AsyncCommand(GoToDuraShopCmdExecute);
            GoToDuraExpressCmd = new AsyncCommand(GoToDuraExpressCmdExecute);
            NavigateToDuraEatsCommand = new AsyncCommand(NavigateToDuraEatsCommandExecute);
            RefreshCommand = new AsyncCommand(RefreshCommandExecute);
        }

        private async Task RefreshCommandExecute()
        {
            IsRefreshing = true;
            await GetCurrentLocation();
            await GetAddressList();
            IsRefreshing = false;
        }

        private async Task GoToSearchLoactionPopupCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(LocationSearchPopupViewModel))
            {
                await App.Locator.LocationSearchPopup.InitilizeData();
                await PopupNavigation.Instance.PushAsync(new LocationSearchPopup());
            }
        }

        public async Task GetAllLocation()
        {
            if (CheckConnection())
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        using (UserDialogs.Instance.Loading(AppResources.Please_wait))
                        {
                            try
                            {
                                locList = new List<NewLocationDataResponse>();
                                var result = await TryWithErrorAsync(_authService.GetAllLocationsNew(), true, false);
                                if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
                                {
                                    foreach (var item in result?.Data?.data)
                                    {
                                        locList.Add(item);
                                    }
                                }
                                else
                                {
                                    HideLoading();
                                    ShowAlert(result?.Data?.message);
                                }
                            }
                            catch (Exception ex)
                            {
                                HideLoading();
                            }
                            if (locList != null && locList.Count > 0)
                            {
                                AllLocationList = new ObservableCollection<NewLocationDataResponse>(locList);
                                AllLocationListCode = new ObservableCollection<NewLocationDataResponse>(locList.GroupBy(x => x.country_code).Select(y => y.First()));
                                int id = 0;
                                try
                                {
                                    id = Convert.ToInt32(Preferences.Get("LocationKey", ""));
                                }
                                catch (Exception ex) { id = 0; }
                                if (id != 0)
                                    App.Locator.EditProfileInfoPage.SelectedCountries = AllLocationList.Where(x => x.id == Convert.ToInt32(id)).FirstOrDefault();
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                }
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }

        private async Task NavigateToDuraEatsCommandExecute()
        {
            ShowToast(AppResources.We_are_coming_soon);
            //if (_navigationService.GetCurrentPageViewModel() != typeof(DuraEatsHomePageViewModel))
            //{
            //    await App.Locator.DuraEatsHomePage.InitilizeData();
            //    await _navigationService.NavigateToAsync<DuraEatsHomePageViewModel>();
            //}
        }

        private async Task GoToNotificationsCmdExecute()
        {
            await Shell.Current.GoToAsync("NotificatonPage");
            await App.Locator.NotificatonPage.InitializedData();
        }

        private async Task GoToDuraShopCmdExecute()
        {
            ShowToast(AppResources.We_are_coming_soon);
        }

        private async Task GoToDuraExpressCmdExecute()
        {
            await Shell.Current.GoToAsync("DuraExpress");
            await App.Locator.DuraExpress.InitilizeData();
            MessagingCenter.Send<HomePageViewModel>(this, "FromHomePage");
        }

        public async Task InitilizeData(LoginResponseModel loginResponseModel = null)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading(AppResources.Please_wait))
                    {
                        if (loginResponseModel != null)
                        {
                            SettingsExtension.Area = loginResponseModel?.area;
                            SettingsExtension.UserFullName = loginResponseModel?.full_name;
                            SettingsExtension.FirstName = loginResponseModel?.first_name;
                            SettingsExtension.LastName = loginResponseModel?.last_name;
                            SettingsExtension.Token = loginResponseModel?.data?.original?.token;
                            SettingsExtension.UserEmail = loginResponseModel?.email;
                            SettingsExtension.ProfileImageUrl = loginResponseModel?.profile_image;
                            SettingsExtension.UserId = loginResponseModel.user_id;
                            SettingsExtension.Phone = loginResponseModel?.phone;
                        }
                        await GetCurrentLocation();
                        await GetAddressList();
                    }
                });
            }
            catch (Exception ex)
            {
                var val = ex.Message;
            }
        }

        public async Task InitilizefbData(FacebookResponseModel loginResponseModel = null)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading(AppResources.Please_wait))
                    {
                        if (loginResponseModel != null)
                        {
                            SettingsExtension.UserFullName = loginResponseModel?.first_name + loginResponseModel?.last_name;
                            SettingsExtension.FirstName = loginResponseModel?.first_name;
                            SettingsExtension.LastName = loginResponseModel?.last_name;
                            SettingsExtension.Token = loginResponseModel?.data?.original?.token;
                            SettingsExtension.UserEmail = loginResponseModel?.email;
                            SettingsExtension.ProfileImageUrl = loginResponseModel?.profile_image;
                            SettingsExtension.UserId = loginResponseModel.user_id;
                            SettingsExtension.Phone = loginResponseModel?.phone;
                        }
                        await GetCurrentLocation();
                        await GetAddressList();
                    }
                });
            }
            catch (Exception ex)
            {
                var val = ex.Message;
            }
        }

        public async Task CheckAccountIsVerified()
        {
            if (CheckConnection())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading(AppResources.Please_wait))
                    {
                        try
                        {
                            CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel()
                            {
                                user_id = SettingsExtension.UserId
                            };
                            var result = await TryWithErrorAsync(_userCoreService.GetProfileDetails(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                            if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                            {
                                if (result?.Data?.data != null)
                                {
                                    if (result?.Data?.data?.is_verified == "0")
                                    {
                                        var confirmresult = await ShowConfirmationAlert(AppResources.Your_account_is_not_verified, AppResources.Verification, AppResources.Ok, "");
                                        if (confirmresult)
                                        {
                                            if (_navigationService.GetCurrentPageViewModel() != typeof(PhoneVerificationPopupPageViewModel))
                                            {
                                                await App.Locator.PhoneVerificationPopupPage.InitilizeData();
                                                await PopupNavigation.Instance.PushAsync(new PhoneVerificationPopupPage());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            HideLoading();
                        }
                    }
                });
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        public async Task GetCurrentLocation()
        {
            await GetCurrentLoc();
        }

        public async Task GetCurrentLoc()
        {
            if (CheckConnection())
            {
                var location = await LocationHelpers.getCurrentPosition();
                if (location != null)
                {
                    var address = await LocationHelpers.GetAddressBasedOnLatLong(location);
                    if (address != null)
                    {
                        CurrentLocation = $"{address?.FeatureName} {address?.SubAdminArea} {address?.Locality} {address?.CountryName} {address?.PostalCode}";
                        CommonLatLong commonLatLong = new CommonLatLong();
                        commonLatLong.latitude = location.Latitude;
                        commonLatLong.longitude = location.Longitude;
                        commonLatLong.FullAddress = CurrentLocation;
                        CommonLatLong = commonLatLong;
                    }
                }
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }

        public Task GetAddressList()
        {
            _ = Task.Run(async () =>
            {
                if (CheckConnection())
                {
                    try
                    {
                        CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel()
                        {
                            user_id = SettingsExtension.UserId,
                        };
                        AddressList = new ObservableCollection<GetAddressModel>();
                        var result = await TryWithErrorAsync(_userCoreService.GetAddress(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                        if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                        {
                            if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                            {
                                foreach (var item in result?.Data?.data)
                                {
                                    AddressList.Add(item);
                                }
                            }
                        }
                        else if (result?.ResultType == ResultType.Unauthorized)
                        {
                            await _navigationService.ClosePopupsAsync();
                            await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ShowAlert(CommonMessages.ServerError);
                    }
                }
                else
                    ShowToast(AppResources.NoInternet);
            });
            return Task.CompletedTask;
        }
    }
}
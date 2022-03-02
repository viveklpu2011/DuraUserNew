using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.Permissions.ViewModels;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class LandingPageViewModel : AppBaseViewModel
    {
        public IAsyncCommand SignupPageCommand { get; set; }
        INavigationService _navigationService;
        private IAuthenticationService _authService;

        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            set
            {
                _appVersion = value;
                OnPropertyChanged(nameof(AppVersion));
            }
        }

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

        public LandingPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            SignupPageCommand = new AsyncCommand(MoveToSignupCommandExecute, allowsMultipleExecutions: false);
            AppVersion = VersionTracking.CurrentVersion;
        }

        private async Task MoveToSignupCommandExecute()
        {
            if (SettingsExtension.IsPermissionEnabled)
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(LoginPageViewModels))
                {
                    ShowLoading();
                    await App.Locator.LoginPage.GetAllLocation();
                    HideLoading();
                    await _navigationService.NavigateToAsync<LoginPageViewModels>();
                }
            }
            else
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(PermissionPageViewModel))
                {
                    await _navigationService.NavigateToAsync<PermissionPageViewModel>();
                    await App.Locator.PermissionPage.InitilizeData();
                }
            }
        }

        public async Task GetAllLocation()
        {
            if (CheckConnection())
            {
                ShowLoading();
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
                        ////locList = new List<LocationDataResponse>();
                        //LocationDataResponse item = new LocationDataResponse { area = "philipins", country = "philipins" };
                        //locList.Add(item);
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                }
                if (locList != null && locList.Count > 0)
                {
                    AllLocationList = new ObservableCollection<NewLocationDataResponse>(locList);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

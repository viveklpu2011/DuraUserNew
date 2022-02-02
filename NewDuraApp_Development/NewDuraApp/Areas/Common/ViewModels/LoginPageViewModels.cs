using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.User.LocationInfo.Services;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Plugin.FacebookClient;
using Xamarin.Essentials;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class LoginPageViewModels : AppBaseViewModel
    {
        private string _buttonText = AppResources.LoginWithEmail;
        private bool _isVisibleEmailLayout;
        private bool _isVisiblePhoneLayout = true;
        public IAsyncCommand<object> SwitchToEmailLayout { get; set; }
        public IAsyncCommand NavigateToHomeCommand { get; set; }
        public IAsyncCommand NavigateToForgetPassword { get; set; }
        public IAsyncCommand GoToSignUpCmd { get; set; }
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        private List<NewLocationDataResponse> locList;
        private ObservableCollection<NewLocationDataResponse> _allLocationList;
        private bool _isAreaErrorVisible;
        private bool _emailNotValid;
        private bool _passwordNotValid;
        private bool _phoneNumberNotValid;
        private bool _isLoginButtoinEnabled;
        private IAuthenticationService _authService;

        private bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
        }

        private bool _isPhoneEmail;
        public bool IsPhoneEmail
        {
            get { return _isPhoneEmail; }
            set { _isPhoneEmail = value; OnPropertyChanged(nameof(IsPhoneEmail)); }
        }

        private string _mobileNumber;
        private string _email;
        private string _password;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                _mobileNumber = value;
                if (!string.IsNullOrEmpty(_mobileNumber))
                {
                    if (RegexUtilities.PhoneNumberValidation(_mobileNumber) && _mobileNumber.Length >= 10)
                    {
                        if (_mobileNumber.Contains("."))
                        {
                            IsPhoneErrorVisible = true;
                        }
                        else
                        {
                            IsPhoneErrorVisible = false;
                        }
                    }
                    else
                    {
                        IsPhoneErrorVisible = true;
                    }
                    CheckLoginValidation();
                }
                else
                {
                    IsPhoneErrorVisible = false;
                }
                this.OnPropertyChanged(nameof(MobileNumber));
            }
        }

        private LoginResponseModel _loginResponse;
        public LoginResponseModel LoginResponse
        {
            get { return _loginResponse; }
            set
            {
                _loginResponse = value;
                this.OnPropertyChanged(nameof(LoginResponse));
            }
        }

        private FacebookResponseModel _facebookResponse;
        public FacebookResponseModel FacebookResponse
        {
            get { return _facebookResponse; }
            set
            {
                _facebookResponse = value;
                this.OnPropertyChanged(nameof(FacebookResponse));
            }
        }

        private bool _isEmailErrorVisible;
        public bool IsEmailErrorVisible
        {
            get { return _isEmailErrorVisible; }
            set { _isEmailErrorVisible = value; OnPropertyChanged(nameof(IsEmailErrorVisible)); }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                if (!string.IsNullOrEmpty(_email))
                {
                    if (RegexUtilities.EmailValidation(_email))
                        IsEmailErrorVisible = false;
                    else
                        IsEmailErrorVisible = true;
                    CheckLoginValidation();
                }
                else
                {
                    IsEmailErrorVisible = false;
                }
                this.OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                CheckLoginValidation();
                this.OnPropertyChanged(nameof(Password));
            }
        }

        private string _countriesTitle;
        private NewLocationDataResponse _selectedCountries;
        public NewLocationDataResponse SelectedCountries
        {
            get { return _selectedCountries; }
            set
            {
                _selectedCountries = value;
                try
                {
                    this.OnPropertyChanged(nameof(SelectedCountries));
                }
                catch (Exception ex) { }
            }
        }

        private NewLocationDataResponse _selectedCountriesCode;
        public NewLocationDataResponse SelectedCountriesCode
        {
            get { return _selectedCountriesCode; }
            set
            {
                _selectedCountriesCode = value;
                try
                {
                    this.OnPropertyChanged(nameof(SelectedCountriesCode));
                    CountriesTitleCode = $"+{_selectedCountriesCode.country_code}";
                }
                catch (Exception ex) { }
            }
        }

        private bool _isPhoneNumberEmpty;
        public bool IsPhoneNumberEmpty
        {
            get { return _isPhoneNumberEmpty; }
            set
            {
                _isPhoneNumberEmpty = value;
                OnPropertyChanged(nameof(IsPhoneNumberEmpty));
            }
        }

        public string CountriesTitle
        {
            get { return _countriesTitle; }
            set
            {
                _countriesTitle = value;
                this.OnPropertyChanged("CountriesTitle");
            }
        }

        private string _countriesTitleCode;
        public string CountriesTitleCode
        {
            get { return _countriesTitleCode; }
            set
            {
                _countriesTitleCode = value;
                this.OnPropertyChanged("CountriesTitleCode");
            }
        }

        private ObservableCollection<Countries> _countriesList;
        public ObservableCollection<Countries> CountriesList
        {
            get { return _countriesList; }
            set
            {
                _countriesList = value;
                this.OnPropertyChanged(nameof(CountriesList));
            }
        }

        INavigationService _navigationService;
        public ObservableCollection<NewLocationDataResponse> AllLocationList
        {
            get { return _allLocationList; }
            set
            {
                SetProperty(ref _allLocationList, value);
            }
        }

        private NewLocationDataResponse _selectedLocation;
        public NewLocationDataResponse SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                if (_selectedLocation != null)
                {
                    if (RegexUtilities.EmptyString(_selectedLocation.country_name))
                    {
                        this.OnPropertyChanged(nameof(CountriesTitle));
                        CountriesTitle = $"+{_selectedLocation.country_code}";
                        IsAreaErrorVisible = false;
                    }
                    else
                        IsAreaErrorVisible = true;
                }
                CheckLoginValidation();
                this.OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private NewLocationDataResponse _selectedLocationTemp;
        public NewLocationDataResponse SelectedLocationTemp
        {
            get { return _selectedLocationTemp; }
            set
            {
                _selectedLocationTemp = value;
                this.OnPropertyChanged(nameof(SelectedLocationTemp));
            }
        }

        private bool CheckLoginValidation()
        {
            if (IsVisiblePhoneNumberLayout)
            {
                if (string.IsNullOrEmpty(MobileNumber) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SelectedLocation?.country_name))
                {
                    IsLoginButtoinEnabled = false;
                    return false;
                }
                else if (IsAreaErrorVisible || IsPhoneErrorVisible)
                {
                    IsLoginButtoinEnabled = false;
                    return false;
                }
                else
                {
                    IsLoginButtoinEnabled = true;
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SelectedLocation?.country_name))
                {
                    IsLoginButtoinEnabled = false;
                    return false;
                }
                else if (IsAreaErrorVisible || IsEmailErrorVisible)
                {
                    IsLoginButtoinEnabled = false;
                    return false;
                }
                else
                {
                    IsLoginButtoinEnabled = true;
                    return true;
                }
            }
        }

        public bool IsAreaErrorVisible
        {
            get { return _isAreaErrorVisible; }
            set { _isAreaErrorVisible = value; OnPropertyChanged(nameof(IsAreaErrorVisible)); }
        }

        public bool EmailNotValid
        {
            get { return _emailNotValid; }
            set { _emailNotValid = value; OnPropertyChanged(nameof(EmailNotValid)); }
        }

        public bool PasswordNotValid
        {
            get { return _passwordNotValid; }
            set { _passwordNotValid = value; OnPropertyChanged(nameof(PasswordNotValid)); }
        }

        public bool PhoneNumberNotValid
        {
            get { return _phoneNumberNotValid; }
            set { _phoneNumberNotValid = value; OnPropertyChanged(nameof(PhoneNumberNotValid)); }
        }

        public bool IsLoginButtoinEnabled
        {
            get { return _isLoginButtoinEnabled; }
            set { _isLoginButtoinEnabled = value; OnPropertyChanged(nameof(IsLoginButtoinEnabled)); }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(nameof(ButtonText)); }
        }

        public bool IsVisiblePhoneNumberLayout
        {
            get { return _isVisiblePhoneLayout; }
            set { _isVisiblePhoneLayout = value; OnPropertyChanged(nameof(IsVisiblePhoneNumberLayout)); }
        }

        public bool IsVisibleEmailLayout
        {
            get { return _isVisibleEmailLayout; }
            set { _isVisibleEmailLayout = value; OnPropertyChanged(nameof(IsVisibleEmailLayout)); }
        }

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

        private ObservableCollection<NewLocationDataResponse> _allLocationListCode;
        public ObservableCollection<NewLocationDataResponse> AllLocationListCode
        {
            get { return _allLocationListCode; }
            set
            {
                SetProperty(ref _allLocationListCode, value);
            }
        }

        public LoginPageViewModels(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            AppVersion = VersionTracking.CurrentVersion;
            SwitchToEmailLayout = new AsyncCommand<object>(SwitchCommandExecute);
            NavigateToHomeCommand = new AsyncCommand(LoginAndNavigateToHome);
            NavigateToForgetPassword = new AsyncCommand(NavigateToForgetPasswordPage);
            GoToSignUpCmd = new AsyncCommand(NavigateToSignup);
            IsPhoneEmail = true;
        }

        public async Task OnLoginWithGoogleCommandExecute(FacebookRequestModel arg)
        {
            try
            {
                string devicetype = string.Empty;
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.Android));
                }
                else
                {
                    devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.iOS));
                }
                var form = new MultipartFormDataContent();
                var phone = string.IsNullOrEmpty(arg?.phone) ? "" : arg?.phone;
                var firstName = string.IsNullOrEmpty(arg?.first_name) ? "" : arg?.first_name;
                var lastName = string.IsNullOrEmpty(arg?.last_name) ? "" : arg?.last_name;
                form.Add(new StringContent(arg.email), "email");
                form.Add(new StringContent(firstName), "first_name");
                form.Add(new StringContent(lastName), "last_name");
                form.Add(new StringContent(phone), "phone");
                form.Add(new StringContent(arg.login_type), "login_type");
                form.Add(new StringContent(arg.google_id), "google_id");
                form.Add(new StringContent(Convert.ToString(SelectedLocationTemp.id)), "country_id");
                form.Add(new StringContent(Convert.ToString(CountriesTitleCode)), "country_code");
                form.Add(new StringContent(Convert.ToString(App.DeviceToken)), "device_id");
                form.Add(new StringContent(Convert.ToString(devicetype)), "device_type");
                ShowLoading();
                var result = await TryWithErrorAsync(_authService.FbLogin(form), true, false);
                HideLoading();
                if (result.Data.status == 200)
                {
                    FacebookResponse = new FacebookResponseModel();
                    DuraApp.Core.Models.Auth.ResponseModel.Data data = new DuraApp.Core.Models.Auth.ResponseModel.Data();
                    Original original = new Original();
                    FbOriginal fbOriginal = new FbOriginal();
                    fbOriginal.token = result?.Data?.data?.original?.token;
                    original.token = result?.Data?.data?.original?.token;
                    FBData fBData = new FBData();
                    fBData.original = fbOriginal;
                    data.original = original;
                    FacebookResponse.data = fBData;
                    FacebookResponse.first_name = result?.Data?.first_name;
                    FacebookResponse.last_name = result?.Data?.last_name;
                    FacebookResponse.email = result?.Data?.email;
                    FacebookResponse.profile_image = result?.Data?.profile_image;
                    FacebookResponse.user_id = result.Data.user_id;
                    SettingsExtension.IsLoggedIn = true;
                    SettingsExtension.UserId = result.Data.user_id;
                    await App.Locator.HomePage.GetAllLocation();
                    await App.Locator.HomePage.InitilizefbData(FacebookResponse);
                    await App.Locator.MyProfile.GetProfileDetails();
                    await App.Locator.HomePage.GetAllLocation();
                    App.Locator.ProfilePage.GoogleLink = "https://www.google.com/" + arg.google_id;
                    await App.Locator.ProfilePage.UpdateSociaLink("Google");
                    await _navigationService.NavigateToAsync<AppShellViewModel>();
                    await App.Locator.HomePage.CheckAccountIsVerified();
                }
                else
                {
                    ShowAlert(result.Data.message, "Error", AppResources.Ok);
                    return;
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                ShowToast("Google server down." + ex.Message);
            }
        }

        public async Task OnLoginWithFacebookCommand(NetworkAuthData networkAuthData)
        {
            try
            {
                string devicetype = string.Empty;
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.Android));
                }
                else
                {
                    devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.iOS));
                }
                var form = new MultipartFormDataContent();
                var phone = string.IsNullOrEmpty(networkAuthData?.phone) ? "" : networkAuthData?.phone;
                var firstName = string.IsNullOrEmpty(networkAuthData?.first_name) ? "" : networkAuthData?.first_name;
                var lastName = string.IsNullOrEmpty(networkAuthData?.last_name) ? "" : networkAuthData?.last_name;
                form.Add(new StringContent(Convert.ToString(networkAuthData?.Email)), "email");
                form.Add(new StringContent(Convert.ToString(firstName)), "first_name");
                form.Add(new StringContent(Convert.ToString(lastName)), "last_name");
                form.Add(new StringContent(Convert.ToString(phone)), "phone");
                form.Add(new StringContent(Convert.ToString(networkAuthData?.login_type)), "login_type");
                form.Add(new StringContent(Convert.ToString(networkAuthData?.Id)), "facebook_id");
                form.Add(new StringContent(Convert.ToString(SelectedLocationTemp.id)), "country_id");
                form.Add(new StringContent(Convert.ToString(CountriesTitleCode)), "country_code");
                form.Add(new StringContent(Convert.ToString(App.DeviceToken)), "device_id");
                form.Add(new StringContent(Convert.ToString(devicetype)), "device_type");
                ShowLoading();
                var result = await TryWithErrorAsync(_authService.FbLogin(form), true, false);
                HideLoading();
                if (result.Data.status == 200)
                {
                    FacebookResponse = new FacebookResponseModel();
                    DuraApp.Core.Models.Auth.ResponseModel.Data data = new DuraApp.Core.Models.Auth.ResponseModel.Data();

                    Original original = new Original();
                    FbOriginal fbOriginal = new FbOriginal();
                    fbOriginal.token = result?.Data?.data?.original?.token;
                    original.token = result?.Data?.data?.original?.token;
                    FBData fBData = new FBData();
                    fBData.original = fbOriginal;
                    data.original = original;
                    FacebookResponse.data = fBData;
                    FacebookResponse.first_name = result?.Data?.first_name;
                    FacebookResponse.last_name = result?.Data?.last_name;
                    FacebookResponse.email = result?.Data?.email;
                    FacebookResponse.profile_image = result?.Data?.profile_image;
                    FacebookResponse.user_id = result.Data.user_id;
                    SettingsExtension.IsLoggedIn = true;
                    SettingsExtension.UserId = result.Data.user_id;
                    await App.Locator.HomePage.InitilizefbData(FacebookResponse);
                    await App.Locator.MyProfile.GetProfileDetails();
                    App.Locator.ProfilePage.FacebookLink = "https://www.facebook.com/" + networkAuthData.Id;
                    await App.Locator.ProfilePage.UpdateSociaLink("Facebook");
                    await App.Locator.HomePage.GetAllLocation();
                    await _navigationService.NavigateToAsync<AppShellViewModel>();
                    await App.Locator.HomePage.CheckAccountIsVerified();
                    HideLoading();
                    ShowToast(AppResources.Welcome);
                }
                else
                {
                    HideLoading();
                    ShowAlert(result.Data.message, "Error", AppResources.Ok);
                    return;
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                ShowToast("Facebook Server down." + ex.Message);
            }
        }

        private async Task NavigateToSignup()
        {
            await _navigationService.NavigateToAsync<SignUpPageViewModel>();
            await App.Locator.SignUpPage.InitilizeData();
        }

        private async Task NavigateToForgetPasswordPage()
        {
            await _navigationService.NavigateToAsync<ForgetPasswordPageViewModel>();
            await App.Locator.ForgetPasswordPage.InitilizeData();
        }

        private async Task SwitchCommandExecute(object arg)
        {
            var data = arg as LoginPageViewModels;
            if (data != null)
            {
                if (IsPhoneEmail)
                {
                    IsVisibleEmailLayout = true;
                    IsVisiblePhoneNumberLayout = false;
                    ButtonText = AppResources.LoginWithPhoneNumber;
                    IsPhoneEmail = false;
                }
                else
                {
                    IsVisibleEmailLayout = false;
                    IsVisiblePhoneNumberLayout = true;
                    ButtonText = AppResources.LoginWithEmail;
                    IsPhoneEmail = true;
                }
                CheckLoginValidation();
            }
        }

        private async Task LoginAndNavigateToHome()
        {
            if (SelectedCountries != null)
                Preferences.Set("LocationKey", Convert.ToString(SelectedCountries.id));
            if (CheckConnection())
            {
                if (IsVisiblePhoneNumberLayout)
                {
                    if (string.IsNullOrEmpty(MobileNumber))
                    {
                        ShowAlert(AppResources.Please_enter_valid_phone_number, AppResources.Alert, AppResources.Ok); return;
                    }
                    else if (MobileNumber.Contains("."))
                    {
                        ShowToast("Phone number should not contain .");
                        return;
                    }
                    else
                    {
                        Email = string.Empty;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Email))
                    {
                        ShowAlert(AppResources.Please_enter_valid_email_id, AppResources.Alert, AppResources.Ok); return;
                    }
                    else
                    {
                        CountriesTitle = string.Empty;
                        MobileNumber = string.Empty;
                    }
                }
                ShowLoading();
                try
                {
                    string devicetype = string.Empty;
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.Android));
                    }
                    else
                    {
                        devicetype = Convert.ToString(((int)DuraApp.Core.Helpers.Enums.DeviceType.iOS));
                    }
                    LoginRequestModel loginRequestModel = new LoginRequestModel
                    {
                        email = Email.Trim(),
                        password = Password.Trim(),
                        phone = MobileNumber.Trim(),
                        country_code = CountriesTitleCode.Trim(),
                        //country_id = Convert.ToString(SelectedCountries.id),
                        device_id = App.DeviceToken,
                        device_type = devicetype
                    };

                    LoginResponse = new LoginResponseModel();
                    var result = await TryWithErrorAsync(_authService.Login(loginRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        LoginResponse = new LoginResponseModel();
                        DuraApp.Core.Models.Auth.ResponseModel.Data data = new DuraApp.Core.Models.Auth.ResponseModel.Data();
                        Original original = new Original();
                        original.token = result?.Data?.data?.original?.token;
                        data.original = original;
                        LoginResponse.area = SelectedCountries?.country_name;
                        LoginResponse.full_name = $"{result?.Data?.first_name} {result?.Data?.last_name}";
                        LoginResponse.first_name = result?.Data?.first_name;
                        LoginResponse.last_name = result?.Data?.last_name;
                        LoginResponse.data = data;
                        LoginResponse.email = result?.Data?.email;
                        LoginResponse.profile_image = result?.Data?.profile_image;
                        LoginResponse.user_id = result.Data.user_id;
                        LoginResponse.country_code = CountriesTitleCode;
                        LoginResponse.phone = MobileNumber;
                        SettingsExtension.IsLoggedIn = true;
                        await App.Locator.HomePage.GetAllLocation();
                        await App.Locator.HomePage.InitilizeData(LoginResponse);
                        await _navigationService.NavigateToAsync<AppShellViewModel>();
                        ShowToast(AppResources.Welcome + " " + LoginResponse.first_name);
                        Email = Password = MobileNumber = string.Empty;
                        IsAreaErrorVisible = false;
                        IsPhoneErrorVisible = false;
                        CheckLoginValidation();
                    }
                    else
                    {
                        ShowAlert("Please recheck your credentials and selected location.");
                    }
                }
                catch (Exception ex)
                {
                    ShowToast("Please recheck your credentials and selected location.");
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        public async Task InitilizeData()
        {
            Email = Password = MobileNumber = string.Empty;
            IsAreaErrorVisible = false;
            IsPhoneErrorVisible = false;
            CheckLoginValidation();
        }

        public async Task GetAllLocation()
        {
            if (CheckConnection())
            {
                try
                {
                    locList = new List<NewLocationDataResponse>();
                    ShowLoading();
                    var result = await TryWithErrorAsync(_authService.GetAllLocationsNew(), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
                    {
                        foreach (var item in result?.Data?.data)
                        {
                            locList.Add(item);
                        }
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                }
                if (locList != null && locList.Count > 0)
                {
                    AllLocationList = new ObservableCollection<NewLocationDataResponse>(locList);
                    AllLocationListCode = new ObservableCollection<NewLocationDataResponse>(locList.GroupBy(x => x.country_code).Select(y => y.First()));
                    ShowLoading();
                    var country_code = await TrackingService.GetUserCountryCodeAsync();
                    int id = 0;
                    try
                    {
                        id = Convert.ToInt32(Preferences.Get("LocationKey", ""));
                    }
                    catch (Exception ex) { id = 0; }
                    if (id != 0)
                        SelectedCountries = AllLocationList.Where(x => x.id == Convert.ToInt32(id)).FirstOrDefault();
                    HideLoading();
                    if (country_code != null)
                    {
                        var countryData = AllLocationList.Where(x => x.iso == "PH").FirstOrDefault();
                        if (countryData != null)
                        {
                            SelectedLocation = countryData;
                            SelectedLocationTemp = countryData;
                            CountriesTitleCode = countryData != null ? $"+{countryData?.country_code}" : "";
                        }
                    }
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}
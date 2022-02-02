using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Behaviours;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class SignUpPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand BackToLoginCmd { get; set; }
        public IAsyncCommand GoToGetOTPCmd { get; set; }
        public IAsyncCommand GoToTermsCmd { get; set; }
        private IAuthenticationService _authService;
        private bool _passedValidation;
        private List<LocationDataResponse> locList;
        private ObservableCollection<NewLocationDataResponse> _allLocationList;
        private string _email, _password, _confirmPassword;
        public EmailValidatorBehavior _emailValidatorBehavior;
        public IAsyncCommand OnLoginWithFacebookCommand { get; set; }
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        private SignupRequestModel _signupRequestModelVM;
        private string selectedtext;
        private bool _isAreaErrorVisible;
        private bool _emailNotValid;
        private bool _passwordNotValid;
        private bool _confirmPasswordNotValid;
        private bool _isSignupButtoinEnabled;
        private bool _isAcceptTerms;
        private bool _isEmailErrorVisible;
        private bool _isPasswordErrorVisible;
        private bool _isConfirmPasswordErrorVisible;
        public bool IsEmailErrorVisible
        {
            get { return _isEmailErrorVisible; }
            set
            {
                _isEmailErrorVisible = value;
                OnPropertyChanged(nameof(IsEmailErrorVisible));
            }
        }

        public bool IsPasswordErrorVisible
        {
            get { return _isPasswordErrorVisible; }
            set
            {
                _isPasswordErrorVisible = value;
                OnPropertyChanged(nameof(IsPasswordErrorVisible));
            }
        }

        public bool IsConfirmPasswordErrorVisible
        {
            get { return _isConfirmPasswordErrorVisible; }
            set
            {
                _isConfirmPasswordErrorVisible = value;
                OnPropertyChanged(nameof(IsConfirmPasswordErrorVisible));
            }
        }

        private int _SelectedItemIndex;
        public int SelectedItemIndex
        {
            get { return _SelectedItemIndex; }
            set
            {
                SetProperty(ref _SelectedItemIndex, value);
            }
        }

        public bool IsAreaErrorVisible
        {
            get { return _isAreaErrorVisible; }
            set
            {
                _isAreaErrorVisible = value;
                OnPropertyChanged(nameof(IsAreaErrorVisible));
            }
        }
        public bool EmailNotValid
        {
            get { return _emailNotValid; }
            set { _emailNotValid = value; OnPropertyChanged(nameof(EmailNotValid)); }
        }

        public bool PasswordNotValid
        {
            get { return _passwordNotValid; }
            set
            {
                _passwordNotValid = value;
                OnPropertyChanged(nameof(PasswordNotValid));
            }
        }

        public bool ConfirmPasswordNotValid
        {
            get { return _confirmPasswordNotValid; }
            set
            {
                _confirmPasswordNotValid = value;
                OnPropertyChanged(nameof(ConfirmPasswordNotValid));
            }
        }

        public bool IsSignupButtoinEnabled
        {
            get { return _isSignupButtoinEnabled; }
            set
            {
                _isSignupButtoinEnabled = value;
                OnPropertyChanged(nameof(IsSignupButtoinEnabled));
            }
        }

        public bool IsAcceptTerms
        {
            set
            {
                _isAcceptTerms = value;
                CheckLoginValidation();
                OnPropertyChanged(nameof(IsAcceptTerms));
            }
            get => _isAcceptTerms;
        }

        public SignupRequestModel SignupRequestModelVM
        {
            set
            {
                _signupRequestModelVM = value;
                OnPropertyChanged(nameof(SignupRequestModelVM));
            }
            get => _signupRequestModelVM;
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
                OnPropertyChanged(nameof(Email));
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

        public bool CheckLoginValidation()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(SelectedLocation?.country_name) || !IsAcceptTerms)
            {
                IsSignupButtoinEnabled = false;
                return false;
            }
            else if (IsAreaErrorVisible || IsEmailErrorVisible || IsPasswordErrorVisible || IsConfirmPasswordErrorVisible || !IsAcceptTerms)
            {
                IsSignupButtoinEnabled = false;
                return false;
            }
            else
            {
                IsSignupButtoinEnabled = true;
                return true;
            }
        }

        public bool PassedValidation
        {
            get
            {
                return _emailValidatorBehavior.IsEmailValid;
            }
            set
            {
                _passedValidation = value;
                OnPropertyChanged(nameof(PassedValidation));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                if (!string.IsNullOrEmpty(_password))
                {
                    if (RegexUtilities.PasswordValidation(_password))
                        IsPasswordErrorVisible = false;
                    else
                        IsPasswordErrorVisible = true;
                    CheckLoginValidation();
                }
                else
                {
                    IsPasswordErrorVisible = false;
                }
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                if (!string.IsNullOrEmpty(_confirmPassword) && !string.IsNullOrEmpty(_password))
                {
                    if (RegexUtilities.ConfirmPasswordValidation(_password, _confirmPassword))
                        IsConfirmPasswordErrorVisible = false;
                    else
                        IsConfirmPasswordErrorVisible = true;
                    CheckLoginValidation();
                }
                else
                {
                    IsConfirmPasswordErrorVisible = false;
                }
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        string selectedColorName;
        public string SelectedColorName
        {
            get { return selectedColorName; }
            set
            {
                if (selectedColorName != value)
                {
                    selectedColorName = value;
                    OnPropertyChanged(nameof(SelectedColorName));
                    OnPropertyChanged(nameof(SelectedLocation));
                }
            }
        }

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
                        IsAreaErrorVisible = false;
                    else
                        IsAreaErrorVisible = true;
                }
                CheckLoginValidation();
                this.OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private FacebookResponseModel _facebookResponse;
        public FacebookResponseModel FacebookResponse
        {
            get { return _facebookResponse; }
            set { _facebookResponse = value; OnPropertyChanged(nameof(FacebookResponse)); }
        }

        public SignUpPageViewModel(INavigationService navigationService, IAuthenticationService authService, EmailValidatorBehavior emailValidatorBehavior)
        {
            _navigationService = navigationService;
            _authService = authService;
            _emailValidatorBehavior = emailValidatorBehavior;
            OnLoginWithFacebookCommand = new AsyncCommand(OnLoginWithFacebookCommandExecute);
            BackToLoginCmd = new AsyncCommand(BackToLogin);
            GoToGetOTPCmd = new AsyncCommand(NavigateToGetOTP);
            GoToTermsCmd = new AsyncCommand(NavigateToTerms);
        }

        private async Task OnLoginWithFacebookCommandExecute()
        {
            try
            {
                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }
                EventHandler<FBEventArgs<string>> userDataDelegate = null;
                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<Models.FacebookProfile>(e.Data));
                            var socialLoginData = new NetworkAuthData
                            {
                                Email = facebookProfile.Email,
                                Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}",
                                Id = facebookProfile.Id
                            };
                            break;
                        case FacebookActionStatus.Canceled:
                            break;
                    }
                    _facebookService.OnUserData -= userDataDelegate;
                };
                _facebookService.OnUserData += userDataDelegate;
                string[] fbRequestFields = { "email", "first_name", "gender", "last_name" };
                string[] fbPermissions = { "email" };
                var fb_result = await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermissions);
                if (fb_result.Data != null)
                {
                    var facebook_Profile = await Task.Run(() => JsonConvert.DeserializeObject<Models.FacebookProfile>(fb_result.Data));
                    FacebookRequestModel facebookRequest = new FacebookRequestModel
                    {
                        area = "area",
                        email = facebook_Profile.Email,
                        first_name = facebook_Profile.FirstName,
                        last_name = facebook_Profile.LastName,
                        login_type = "facebook",
                        facebook_id = facebook_Profile.Id
                    };
                    ShowLoading();
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

                    var phone = string.IsNullOrEmpty(facebookRequest?.phone) ? "" : facebookRequest?.phone;
                    var firstName = string.IsNullOrEmpty(facebookRequest?.first_name) ? "" : facebookRequest?.first_name;
                    var lastName = string.IsNullOrEmpty(facebookRequest?.last_name) ? "" : facebookRequest?.last_name;
                    form.Add(new StringContent(Convert.ToString(facebookRequest?.email)), "email");
                    form.Add(new StringContent(Convert.ToString(firstName)), "first_name");
                    form.Add(new StringContent(Convert.ToString(lastName)), "last_name");
                    form.Add(new StringContent(Convert.ToString(phone)), "phone");
                    form.Add(new StringContent(Convert.ToString(facebookRequest?.login_type)), "login_type");
                    form.Add(new StringContent(Convert.ToString(facebookRequest?.facebook_id)), "facebook_id");
                    form.Add(new StringContent(Convert.ToString(SelectedLocation.id)), "country_id");
                    form.Add(new StringContent(Convert.ToString($"+{SelectedLocation.country_code}")), "country_code");
                    form.Add(new StringContent(Convert.ToString(App.DeviceToken)), "device_id");
                    form.Add(new StringContent(Convert.ToString(devicetype)), "device_type");
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
                        await App.Locator.HomePage.InitilizefbData(FacebookResponse);
                        await _navigationService.NavigateToAsync<AppShellViewModel>();
                        await App.Locator.HomePage.CheckAccountIsVerified();
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                ShowToast(AppResources.Your_email_or_phone_is_already_registered);
            }
        }

        public async Task InitilizeData()
        {
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            IsAcceptTerms = false;
            IsConfirmPasswordErrorVisible = false;
            IsPasswordErrorVisible = false;
            IsEmailErrorVisible = false;
            CheckLoginValidation();
            AllLocationList = App.Locator.LoginPage.AllLocationList;
            var countryData = AllLocationList.Where(x => x.iso == "PH").FirstOrDefault();
            if (countryData != null)
            {
                SelectedLocation = countryData;
                SelectedIndex = AllLocationList.IndexOf(SelectedLocation);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        //private async Task GetAllLocation()
        //{
        //    if (CheckConnection())
        //    {
        //        ShowLoading();
        //        try
        //        {
        //            var result = await TryWithErrorAsync(_authService.GetAllLocations(), true, false);
        //            if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
        //            {
        //                locList = new List<LocationDataResponse>();
        //                foreach (var item in result?.Data?.data)
        //                {
        //                    locList.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                ShowAlert(result?.Data?.message);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowToast(CommonMessages.ServerError);
        //        }
        //        AllLocationList = new ObservableCollection<LocationDataResponse>(locList);
        //        HideLoading();
        //    }
        //    else
        //        ShowToast(CommonMessages.NoInternet);
        //}

        private async Task NavigateToGetOTP()
        {
            SignupRequestModel signupRequestModel = new SignupRequestModel
            {
                email = Email,
                password = Password,
                password_confirmation = ConfirmPassword,
                area = SelectedLocation?.country_name,
                country_id = Convert.ToString(SelectedLocation?.id)
            };
            SignupRequestModelVM = new SignupRequestModel();
            SignupRequestModelVM = signupRequestModel;
            if (_navigationService.GetCurrentPageViewModel() != typeof(SendOTPPageViewModel))
            {
                App.Locator.CurrentUser.SendWay = SendInvite.LOGIN_WAY.ToString();
                await _navigationService.NavigateToAsync<SendOTPPageViewModel>();
                await App.Locator.SendOTPPage.initializData();
                Email = Password = ConfirmPassword = string.Empty;
            }
        }

        private async Task NavigateToTerms()
        {
            await Browser.OpenAsync(AppConstant.TermsServicesUrl, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#211E66"),
                PreferredControlColor = Color.White
            });
        }

        private async Task BackToLogin()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(LoginPageViewModels))
            {
                await _navigationService.NavigateBackAsync();
                await App.Locator.LoginPage.InitilizeData();
            }
        }
    }
}
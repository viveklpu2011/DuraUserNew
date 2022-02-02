using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class ForgetPasswordPageViewModel : AppBaseViewModel
    {
        public IAsyncCommand GoToOTPVerifyCmd { get; set; }
        INavigationService _navigationService;
        private bool _emailNotValid;
        private bool _phoneNumberNotValid;
        private bool _isEmailErrorVisible;
        private bool _isEmailChecked = true;
        private bool _isPhoneChecked;
        private IAuthenticationService _authService;
        private ObservableCollection<NewLocationDataResponse> _countriesList;
        private string _countriesTitle;
        private NewLocationDataResponse _selectedCountries;
        private bool _isVisibleEmailView = true;
        private bool _isVisiblePhoneView;
        private string _mobileNumber;
        private string _email;
        private bool _isGetOTPButtoinEnabled;
        object _selectionRadio;
        private string _buttonText = AppResources.Get_OTP_on_Email;

        private SignupRequestModel _signupRequestModelVM;
        public bool IsEmailErrorVisible
        {
            get { return _isEmailErrorVisible; }
            set { _isEmailErrorVisible = value; OnPropertyChanged(nameof(IsEmailErrorVisible)); }
        }

        public object SelectionRadio
        {
            get => _selectionRadio;
            set
            {
                _selectionRadio = value;
                IsViewVisible(_selectionRadio as string);
                OnPropertyChanged(nameof(SelectionRadio));
            }
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

        public bool IsVisibleEmailView
        {
            get { return _isVisibleEmailView; }
            set
            {
                _isVisibleEmailView = value;
                OnPropertyChanged(nameof(IsVisibleEmailView));
            }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public bool IsVisiblePhoneView
        {
            get { return _isVisiblePhoneView; }
            set { _isVisiblePhoneView = value; OnPropertyChanged(nameof(IsVisiblePhoneView)); }
        }

        public bool EmailNotValid
        {
            get { return _emailNotValid; }
            set { _emailNotValid = value; OnPropertyChanged(nameof(EmailNotValid)); }
        }

        public bool IsEmailChecked
        {
            get { return _isEmailChecked; }
            set { _isEmailChecked = value; OnPropertyChanged(nameof(IsEmailChecked)); }
        }

        public bool IsPhoneChecked
        {
            get { return _isPhoneChecked; }
            set { _isPhoneChecked = value; OnPropertyChanged(nameof(IsPhoneChecked)); }
        }

        public bool PhoneNumberNotValid
        {
            get { return _phoneNumberNotValid; }
            set { _phoneNumberNotValid = value; OnPropertyChanged(nameof(PhoneNumberNotValid)); }
        }

        public bool IsGetOTPButtoinEnabled
        {
            get { return _isGetOTPButtoinEnabled; }
            set { _isGetOTPButtoinEnabled = value; OnPropertyChanged(nameof(IsGetOTPButtoinEnabled)); }
        }

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

        private void IsViewVisible(string value)
        {
            if (value != null)
            {
                if (value == "Email")
                {
                    IsVisibleEmailView = true;
                    IsVisiblePhoneView = false;
                    ButtonText = AppResources.Get_OTP_on_Email;
                }
                else
                {
                    IsVisibleEmailView = false;
                    IsVisiblePhoneView = true;
                    ButtonText = AppResources.Get_OTP_on_Phone;
                }
            }
            else
            {
                IsVisibleEmailView = true;
                IsVisiblePhoneView = false;
                ButtonText = AppResources.Get_OTP_on_Email;
            }
            CheckLoginValidation();
        }

        private bool CheckLoginValidation()
        {
            if (IsVisiblePhoneView)
            {
                if (string.IsNullOrEmpty(MobileNumber))
                {
                    IsGetOTPButtoinEnabled = false;
                    return false;
                }
                else if (IsPhoneErrorVisible)
                {
                    IsGetOTPButtoinEnabled = false;
                    return false;
                }
                else
                {
                    IsGetOTPButtoinEnabled = true;
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Email))
                {
                    IsGetOTPButtoinEnabled = false;
                    return false;
                }
                else if (IsEmailErrorVisible)
                {
                    IsGetOTPButtoinEnabled = false;
                    return false;
                }
                else
                {
                    IsGetOTPButtoinEnabled = true;
                    return true;
                }
            }
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

        public NewLocationDataResponse SelectedCountries
        {
            get { return _selectedCountries; }
            set
            {
                _selectedCountries = value;
                this.OnPropertyChanged(nameof(SelectedCountries));
                this.OnPropertyChanged(nameof(CountriesTitle));
                CountriesTitle = $"+{_selectedCountries.country_code}";
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

        private bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
        }

        public ObservableCollection<NewLocationDataResponse> CountriesList
        {
            get { return _countriesList; }
            set
            {
                _countriesList = value;
                this.OnPropertyChanged(nameof(CountriesList));
            }
        }

        public ForgetPasswordPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            GoToOTPVerifyCmd = new AsyncCommand(GoToOTPVerifyCmdExecute);
        }

        private async Task GoToOTPVerifyCmdExecute()
        {
            if (string.IsNullOrEmpty(CountriesTitle))
            {
                ShowToast(AppResources.Please_select_your_country_code);
                return;
            }
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    SignupRequestModelVM = new SignupRequestModel();
                    OTPRequestModel oTPRequestModel = new OTPRequestModel();
                    if (IsVisibleEmailView)
                    {
                        oTPRequestModel.phone = Email;
                        SignupRequestModelVM.phone = Email;
                    }
                    else
                    {
                        oTPRequestModel.country_code = CountriesTitle;
                        oTPRequestModel.phone = MobileNumber;
                        SignupRequestModelVM.country_code = CountriesTitle;
                        SignupRequestModelVM.phone = MobileNumber;
                        App.VerifyOTPWay = "CountryCodeWithPhone";
                    }
                    var result = await TryWithErrorAsync(_authService.SendOTP(oTPRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (_navigationService.GetCurrentPageViewModel() != typeof(VerifyOTPPageViewModel))
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(VerifyOTPPageViewModel))
                            {
                                await _navigationService.NavigateToAsync<VerifyOTPPageViewModel>();
                                await App.Locator.VerifyOTPPage.InitilizeData(result?.Data?.otp);
                            }
                        }
                    }
                    else
                    {
                        ShowAlert(result.Errors[0], "Error", AppResources.Ok);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        //public async Task GetCountryCode()
        //{
        //    ShowLoading();
        //    try
        //    {
        //        string jsonFileName = "countriescode.json";

        //        CountriesList = new ObservableCollection<Countries>();
        //        var assembly = typeof(ForgetPasswordPage).GetTypeInfo().Assembly;
        //        Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.CommonData.{jsonFileName}");
        //        using (var reader = new System.IO.StreamReader(stream))
        //        {
        //            var jsonString = reader.ReadToEnd();
        //            var countries = JsonConvert.DeserializeObject<ObservableCollection<Countries>>(jsonString);
        //            foreach (var item in countries)
        //            {
        //                CountriesList.Add(item);
        //            }
        //        } 
        //        var country_name = await TrackingService.GetUserCountryNameAsync();
        //        GetCountryBasedOnCurrentCountry(country_name);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowToast(CommonMessages.ServerError);
        //        return;
        //    }
        //    HideLoading();
        //}
        //private Countries GetCountryBasedOnCurrentCountry(string countryname)
        //{
        //    Countries countries = new Countries();

        //    if (!string.IsNullOrEmpty(countryname))
        //    {
        //        if (CountriesList != null && CountriesList.Count > 0)
        //        {
        //            countries = CountriesList.Where(x => x.name == countryname).FirstOrDefault();
        //            SelectedCountries = countries;
        //            CountriesTitle = SelectedCountries.dial_code;
        //        }
        //    }
        //    return countries;
        //}

        internal async Task InitilizeData()
        {
            MobileNumber = string.Empty;
            Email = string.Empty;
            IsVisibleEmailView = true;
            IsEmailChecked = true;
            ButtonText = AppResources.Get_OTP_on_Email;
            CountriesList = App.Locator.LoginPage.AllLocationListCode;
            var countryData = CountriesList.Where(x => x.iso == "PH").FirstOrDefault();
            if (countryData != null)
            {
                CountriesTitle = countryData != null ? $"+{countryData?.country_code}" : "";
            }
        }
    }
}

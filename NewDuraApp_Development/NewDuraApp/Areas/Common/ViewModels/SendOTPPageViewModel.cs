using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class SendOTPPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateOTPVerification { get; set; }
        private bool _phoneNumberNotValid;
        private bool _isVerifyButtonEnabled;
        private string _phoneNumber;
        private SignupRequestModel _signupRequestModelVM;
        private IAuthenticationService _authService;
        private string _phoneValidationString;

        private ObservableCollection<NewLocationDataResponse> _countriesList;
        public ObservableCollection<NewLocationDataResponse> CountriesList
        {
            get { return _countriesList; }
            set
            {
                _countriesList = value;
                this.OnPropertyChanged(nameof(CountriesList));
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

        public string PhoneValidationString
        {
            get { return _phoneValidationString; }
            set
            {
                _phoneValidationString = value;
                this.OnPropertyChanged(nameof(PhoneValidationString));
            }
        }

        public bool PhoneNumberNotValid
        {
            get { return _phoneNumberNotValid; }
            set
            {
                _phoneNumberNotValid = value;
                OnPropertyChanged(nameof(PhoneNumberNotValid));
            }
        }

        public bool IsVerifyButtonEnabled
        {
            get { return _isVerifyButtonEnabled; }
            set
            {
                _isVerifyButtonEnabled = value;
                OnPropertyChanged(nameof(IsVerifyButtonEnabled));
            }
        }

        private bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                if (!string.IsNullOrEmpty(_phoneNumber))
                {
                    if (RegexUtilities.PhoneNumberValidation(_phoneNumber) && _phoneNumber.Length >= 10)
                    {
                        if (_phoneNumber.Contains("."))
                        {
                            IsPhoneErrorVisible = true;
                            IsVerifyButtonEnabled = false;
                        }
                        else
                        {
                            IsPhoneErrorVisible = false;
                            IsVerifyButtonEnabled = true;
                        }
                    }
                    else
                    {
                        IsPhoneErrorVisible = true;
                        IsVerifyButtonEnabled = false;
                    }
                }
                else
                {
                    IsPhoneErrorVisible = false;
                }
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private NewLocationDataResponse _selectedCountries;
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

        private string _countriesTitle;
        public string CountriesTitle
        {
            get { return _countriesTitle; }
            set
            {
                _countriesTitle = value;
                this.OnPropertyChanged("CountriesTitle");
            }
        }

        public SendOTPPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            NavigateOTPVerification = new AsyncCommand(NavigateToVerifyOTP, allowsMultipleExecutions: false);
        }

        private async Task NavigateToVerifyOTP()
        {
            if (string.IsNullOrEmpty(CountriesTitle))
            {
                ShowToast(AppResources.Please_select_your_country_code);
                return;
            }
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                ShowToast(AppResources.Please_enter_valid_phone_number);
                return;
            }
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    OTPRequestModel oTPRequestModel = new OTPRequestModel
                    {
                        country_code = CountriesTitle,
                        phone = PhoneNumber
                    };
                    var result = await TryWithErrorAsync(_authService.SendOTP(oTPRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (_navigationService.GetCurrentPageViewModel() != typeof(VerifyOTPPageViewModel))
                        {
                            SignupRequestModelVM = new SignupRequestModel();
                            SignupRequestModelVM = App.Locator.SignUpPage.SignupRequestModelVM;
                            SignupRequestModelVM.country_code = CountriesTitle;
                            SignupRequestModelVM.phone = PhoneNumber;
                            App.VerifyOTPWay = "Signup";
                            await _navigationService.NavigateToAsync<VerifyOTPPageViewModel>();
                            await App.Locator.VerifyOTPPage.InitilizeData(result?.Data?.otp);
                        }
                    }
                    else
                    {
                        ShowAlert(AppResources.API_Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message);
                    return;
                }
                HideLoading();
            }
            else
                ShowToast(CommonMessages.NoInternet);
            return;
        }

        //public async Task GetCountryCode()
        //{
        //    ShowLoading();
        //    try
        //    {
        //        string jsonFileName = "countriescode.json"; 
        //        CountriesList = new ObservableCollection<Countries>();
        //        var assembly = typeof(SendOTPPage).GetTypeInfo().Assembly;
        //        Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.CommonData.{jsonFileName}");
        //        using (var reader = new System.IO.StreamReader(stream))
        //        {
        //            var jsonString = reader.ReadToEnd();
        //            CountriesList = JsonConvert.DeserializeObject<ObservableCollection<Countries>>(jsonString);
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
        internal async Task initializData()
        {
            CountriesList = App.Locator.LoginPage.AllLocationListCode;
            CountriesTitle = $"+{App.Locator.SignUpPage.SelectedLocation.country_code}";

            //var country_code = await TrackingService.GetUserCountryCodeAsync();
            //var countryData = CountriesList.Where(x => x.iso == country_code).FirstOrDefault();
            //CountriesTitle = countryData != null ? $"+{countryData?.country_code}" : "";

            PhoneNumber = string.Empty;
            //  IsPhoneErrorVisible = false;
            //CheckLoginValidation();
        }
    }
}

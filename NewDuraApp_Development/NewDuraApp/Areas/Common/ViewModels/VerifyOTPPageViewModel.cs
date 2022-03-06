using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class VerifyOTPPageViewModel : AppBaseViewModel
    {
        private string _stopTime;
        private string _otp;
        private string _otp1, _otp2, _otp3, _otp4;
        INavigationService _navigationService;
        private IAuthenticationService _authService;
        private SignupRequestModel _signupRequestModelVM;
        public IAsyncCommand VerifyOTPCommand { get; set; }
        public IAsyncCommand GoToResetPassCmd { get; set; }
        public IAsyncCommand RestartTimerCmd { get; set; }
        Stopwatch stopwatch = new Stopwatch();

        public delegate void PerformAddCartForTab();
        public event PerformAddCartForTab StartTimerEvent;

        public string OTP1
        {
            get { return _otp1; }
            set { _otp1 = value; OnPropertyChanged("OTP1"); }
        }

        public string OTP2
        {
            get { return _otp2; }
            set { _otp2 = value; OnPropertyChanged("OTP2"); }
        }

        public string OTP3
        {
            get { return _otp3; }
            set { _otp3 = value; OnPropertyChanged("OTP3"); }
        }

        public string OTP4
        {
            get { return _otp4; }
            set { _otp4 = value; OnPropertyChanged("OTP4"); }
        }

        private string _phoneNumber;
        private string _countryCode;
        private Timer time = new Timer();

        private bool _isEnable;
        public SignupRequestModel SignupRequestModelVM
        {
            set
            {
                _signupRequestModelVM = value;
                OnPropertyChanged(nameof(SignupRequestModelVM));
            }
            get => _signupRequestModelVM;
        }

        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; OnPropertyChanged(); }
        }

        private string _stopWatchMinutes;
        public string StopWatchMinutes
        {
            get { return _stopWatchMinutes; }
            set { _stopWatchMinutes = value; OnPropertyChanged("StopWatchMinutes"); }
        }

        public string OTP
        {
            get { return _otp; }
            set { _otp = value; OnPropertyChanged(nameof(OTP)); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; OnPropertyChanged(nameof(CountryCode)); }
        }

        private string _stopWatchSeconds;
        public string StopWatchSeconds
        {
            get { return _stopWatchSeconds; }
            set { _stopWatchSeconds = value; OnPropertyChanged("StopWatchSeconds"); }
        }

        public string StopTime
        {
            get { return _stopTime; }
            set { _stopTime = value; OnPropertyChanged(nameof(StopTime)); }
        }

        public VerifyOTPPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            VerifyOTPCommand = new AsyncCommand(VerifyOTPCommandExecute, allowsMultipleExecutions: false);
            GoToResetPassCmd = new AsyncCommand(GoToResetPassCmdExecute, allowsMultipleExecutions: false);
            RestartTimerCmd = new AsyncCommand(RestartTimerCmdExecute, allowsMultipleExecutions: false);
            GotoPersonalDetailsIsVisible = true;
            GotoResetPasswordIsVisible = false;
            MessagingCenter.Subscribe<ForgetPasswordPageViewModel>(this, "FromForgetPassPage", (sender) =>
            {
                GotoPersonalDetailsIsVisible = false;
                GotoResetPasswordIsVisible = true;
            });
            MessagingCenter.Subscribe<SendOTPPageViewModel>(this, "FromSendOTP", (sender) =>
            {
                GotoPersonalDetailsIsVisible = true;
                GotoResetPasswordIsVisible = false;
            });
        }

        private async Task RestartTimerCmdExecute()
        {
            await ResendOTP();
        }

        public async Task InitilizeData(string otp)
        {
            OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
            StartTimerEvent.Invoke();
            if (App.Locator.ForgetPasswordPage != null)
            {
                if (!App.Locator.ForgetPasswordPage.IsEmailChecked)
                {
                    //ShowToast($"Your OTP is {otp}");
                }
            }
            OTP = otp;
            if (App.VerifyOTPWay == "Signup")
            {
                var data = App.Locator.SendOTPPage.SignupRequestModelVM;
                CountryCode = data.country_code;
                PhoneNumber = data?.phone;
            }
            else
            {

                var data = App.Locator.ForgetPasswordPage.SignupRequestModelVM;
                if (App.VerifyOTPWay == "CountryCodeWithPhone")
                {
                    CountryCode = data.country_code;
                    PhoneNumber = data?.phone;
                }
                else
                {
                    PhoneNumber = data?.phone;
                    CountryCode = string.Empty;
                }
            }
        }

        private async Task VerifyOTPCommandExecute()
        {
            if (CheckConnection())
            {
                try
                {
                    if (string.IsNullOrEmpty(OTP1) || string.IsNullOrEmpty(OTP2) || string.IsNullOrEmpty(OTP3) || string.IsNullOrEmpty(OTP4))
                    {
                        ShowAlert(AppResources.Enter_the_otp, AppResources.Alert, AppResources.Ok);
                        return;
                    }
                    VerifyOTPRequestModel verifyOTPRequestModel = new VerifyOTPRequestModel
                    {
                        country_code = CountryCode,
                        phone = PhoneNumber,
                        otp = $"{OTP1}{OTP2}{OTP3}{OTP4}"
                    };
                    ShowLoading();
                    var result = await TryWithErrorAsync(_authService.VerifyOTP(verifyOTPRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (App.VerifyOTPWay == "Signup")
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(PersonalDetailsViewModel))
                            {
                                await _navigationService.NavigateToAsync<PersonalDetailsViewModel>();
                                await App.Locator.PersonalDetails.InitializedData();
                                OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
                            }
                        }
                        else
                        {
                            if (_navigationService.GetCurrentPageViewModel() != typeof(ResetPasswordViewModel))
                            {
                                await _navigationService.NavigateToAsync<ResetPasswordViewModel>();
                                OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
                            }
                        }
                        HideLoading();
                    }
                    else
                    {
                        HideLoading();
                        ShowAlert(AppResources.Your_OTP_is_not_matching_Please_resend_to_get_new_one, AppResources.Wrong_code);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    HideLoading();
                    ShowToast(AppResources.ServerError);
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        private async Task GoToResetPassCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ResetPasswordViewModel))
            {
                await _navigationService.NavigateToAsync<ResetPasswordViewModel>();
            }
        }

        private async Task ResendOTP()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    OTPRequestModel oTPRequestModel = new OTPRequestModel
                    {
                        country_code = CountryCode,
                        phone = PhoneNumber,
                    };
                    var result = await TryWithErrorAsync(_authService.SendOTP(oTPRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        StartTimerEvent.Invoke();
                        OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message, AppResources.Alert, AppResources.Ok);
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

        private void StartTimer()
        {
            stopwatch.Start();
            StopWatchMinutes = stopwatch.Elapsed.Minutes.ToString();
            StopWatchSeconds = stopwatch.Elapsed.Seconds.ToString();
            TimeSpan time = new TimeSpan(0, 4, 0);
            TimeSpan subtractTime = new TimeSpan(0, 0, 1);
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (time.Hours == 0 && time.Minutes == 0 && time.Seconds == 0)
                {
                    StopTime = time.ToString();
                    IsEnable = true;
                    return false;
                }
                else
                {
                    time = time - subtractTime;
                    StopTime = time.ToString();
                    return true;
                }
            });
        }

        private bool _gotoPersonalDetailsIsVisible;
        public bool GotoPersonalDetailsIsVisible
        {
            get { return _gotoPersonalDetailsIsVisible; }
            set { _gotoPersonalDetailsIsVisible = value; OnPropertyChanged(); }
        }

        private bool _gotoResetPasswordIsVisible;
        public bool GotoResetPasswordIsVisible
        {
            get { return _gotoResetPasswordIsVisible; }
            set { _gotoResetPasswordIsVisible = value; OnPropertyChanged(); }
        }
    }
}
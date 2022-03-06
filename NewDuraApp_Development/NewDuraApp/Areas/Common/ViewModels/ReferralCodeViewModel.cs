using System;
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class ReferralCodeViewModel : AppBaseViewModel
    {
        public IAsyncCommand GoToLoginCmd { get; set; }
        public IAsyncCommand SkipAndGoToLoginCmd { get; set; }
        INavigationService _navigationService;
        private bool _isReferalCodeErrorVisible;
        private bool _isContinueButtonEnabled;
        private IAuthenticationService _authService;
        private IUserCoreService _userCoreService;
        public ReferralCodeViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _authService = authenticationService;
            _userCoreService = userCoreService;
            GoToLoginCmd = new AsyncCommand(GoToLoginCmdExecute, allowsMultipleExecutions: false);
            SkipAndGoToLoginCmd = new AsyncCommand(SkipGoToLoginCmdExecute, allowsMultipleExecutions: false);
            var dr = App.Locator.PersonalDetails.SignupRequestModelVM;
        }

        public bool IsReferalCodeErrorVisible
        {
            get { return _isReferalCodeErrorVisible; }
            set { _isReferalCodeErrorVisible = value; OnPropertyChanged(nameof(IsReferalCodeErrorVisible)); }
        }

        public bool IsContinueButtonEnabled
        {
            get { return _isContinueButtonEnabled; }
            set { _isContinueButtonEnabled = value; OnPropertyChanged(nameof(IsContinueButtonEnabled)); }
        }

        private async Task SkipGoToLoginCmdExecute()
        {
            await UserRegistration();
        }

        private string _referalCode;
        public string ReferalCode
        {
            get { return _referalCode; }
            set
            {
                _referalCode = value;
                if (RegexUtilities.EmptyString(_referalCode))
                    IsReferalCodeErrorVisible = false;
                else
                    IsReferalCodeErrorVisible = true;
                CheckValidation();
                OnPropertyChanged(nameof(ReferalCode));
            }
        }

        private bool CheckValidation()
        {
            if (IsReferalCodeErrorVisible)
            {
                IsContinueButtonEnabled = false;
                return false;
            }
            else
            {
                IsContinueButtonEnabled = true;
                return true;
            }
        }

        internal async Task InitializedData()
        {
            ReferalCode = string.Empty;
            IsReferalCodeErrorVisible = false;
            CheckValidation();
        }

        private async Task UserRegistration(bool isSkipRefCode = false)
        {
            if (CheckConnection())
            {
                ShowLoading();
                var userData = App.Locator.PersonalDetails.SignupRequestModelVM;
                if (userData != null)
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
                        form.Add(new StringContent(userData?.email.Trim()), "email");
                        form.Add(new StringContent(userData?.country_id.Trim()), "country_id");
                        form.Add(new StringContent(userData?.password.Trim()), "password");
                        form.Add(new StringContent(userData?.password_confirmation.Trim()), "password_confirmation");
                        form.Add(new StringContent(userData?.area.Trim()), "area");
                        form.Add(new StringContent(userData?.first_name.Trim()), "first_name");
                        form.Add(new StringContent(userData?.last_name.Trim()), "last_name");
                        form.Add(new StringContent(userData?.phone.Trim()), "phone");
                        form.Add(new ByteArrayContent(userData?.userImage), "file", $"User_Profile_{CommonHelper.GenerateRandomId(5)}.jpg");
                        form.Add(new StringContent(userData.login_type), "login_type");
                        form.Add(new StringContent(Convert.ToString(App.DeviceToken)), "device_id");
                        form.Add(new StringContent(Convert.ToString(devicetype)), "device_type");
                        if (isSkipRefCode)
                        {
                            form.Add(new StringContent(ReferalCode.Trim()), "referral_code");
                        }
                        else
                        {
                            form.Add(new StringContent(""), "referral_code");
                        }
                        form.Add(new StringContent(userData?.country_code.Trim()), "country_code");
                        var result = await TryWithErrorAsync(_authService.UserSignup(form), true, false);
                        if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                        {
                            await App.Locator.LoginPage.GetAllLocation();
                            await _navigationService.NavigateToAsync<LoginPageViewModels>();
                            await App.Locator.LoginPage.InitilizeData();
                            ShowToast(result?.Data?.message);
                        }
                        else if (result?.ResultType == ResultType.Ok && result.Data.status == 404)
                        {
                            ShowAlert(AppResources.This_phone_number_is_already_registered, AppResources.Already_Exist, AppResources.Ok);
                        }
                        else
                        {
                            ShowAlert(result?.Data?.message + ".", AppResources.Alert, AppResources.Ok);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowAlert("Register API - ERROR" + ex.Message);
                    }
                    HideLoading();
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        private async Task GoToLoginCmdExecute()
        {
            if (CheckConnection())
            {
                try
                {
                    ReferCodeRequestModel referCodeRequestModel = new ReferCodeRequestModel()
                    {
                        refer_code = ReferalCode,
                    };
                    ShowLoading();
                    var result = await TryWithErrorAsync(_userCoreService.ReferCode(referCodeRequestModel), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null)
                        {
                            await UserRegistration(true);
                        }
                        else
                        {
                            var resultrefer = await App.Current.MainPage.DisplayAlert(AppResources.Alert, AppResources.Refer_code_does_not_match, AppResources.Yes, AppResources.No);
                            if (resultrefer)
                            {
                                await UserRegistration(false);
                            }
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
    }
}

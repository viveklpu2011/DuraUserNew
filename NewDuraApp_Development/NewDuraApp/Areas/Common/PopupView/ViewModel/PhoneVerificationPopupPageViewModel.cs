using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.PopupView.ViewModel
{
	public class PhoneVerificationPopupPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		public IAsyncCommand GoToOTPVerifyCmd { get; set; }
		private IAuthenticationService _authenticationService;
		private IUserCoreService _userCoreService;
		private ObservableCollection<NewLocationDataResponse> _allLocationListCode;
		public ObservableCollection<NewLocationDataResponse> AllLocationListCode {
			get { return _allLocationListCode; }
			set {
				try {
					SetProperty(ref _allLocationListCode, value);
				} catch (Exception ex) { }
			}
		}

		private string _buttonText = AppResources.Send_OTP;
		public string ButtonTextOTP {
			get { return _buttonText; }
			set {
				_buttonText = value;
				OnPropertyChanged(nameof(ButtonTextOTP));
			}
		}

		private bool _isOtpButtonEnabled;
		public bool IsOTPButtonEnabled {
			get { return _isOtpButtonEnabled; }
			set {
				_isOtpButtonEnabled = value;
				OnPropertyChanged(nameof(IsOTPButtonEnabled));
			}
		}

		private string _phoneNumber;
		public string PhoneNumber {
			get { return _phoneNumber; }
			set {
				_phoneNumber = value;
				if (!string.IsNullOrEmpty(_phoneNumber)) {
					if (RegexUtilities.PhoneNumberValidation(_phoneNumber) && _phoneNumber.Length >= 10) {
						if (_phoneNumber.Contains(".")) {
							IsPhoneErrorVisible = true;
						} else {
							IsPhoneErrorVisible = false;
						}
					} else {
						IsPhoneErrorVisible = true;
					}
					CheckLoginValidation();
				} else {
					IsPhoneErrorVisible = false;
				}

				OnPropertyChanged(nameof(PhoneNumber));
			}
		}
		private bool CheckLoginValidation()
		{
			if (string.IsNullOrEmpty(PhoneNumber)) {
				IsOTPButtonEnabled = false;
				return false;
			} else if (IsPhoneErrorVisible) {
				IsOTPButtonEnabled = false;
				return false;
			} else {
				IsOTPButtonEnabled = true;
				return true;
			}
		}
		public PhoneVerificationPopupPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_authenticationService = authenticationService;
			_userCoreService = userCoreService;
			GoToOTPVerifyCmd = new AsyncCommand(SendOTPMethodExecute, allowsMultipleExecutions: false);
		}

		private async Task SendOTPMethodExecute()
		{
			if (ButtonTextOTP == AppResources.Send_OTP) {
				await SendOTPExecute();
			}
		}

		private string _countriesTitle;
		private NewLocationDataResponse _selectedCountries;
		public NewLocationDataResponse SelectedCountries {
			get { return _selectedCountries; }
			set {
				_selectedCountries = value;
				this.OnPropertyChanged(nameof(SelectedCountries));
				this.OnPropertyChanged(nameof(CountriesTitle));
				CountriesTitle = $"+{_selectedCountries.country_code}";
			}
		}
		private bool _isPhoneErrorVisible;
		public bool IsPhoneErrorVisible {
			get { return _isPhoneErrorVisible; }
			set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
		}
		public string CountriesTitle {
			get { return _countriesTitle; }
			set {
				_countriesTitle = value;
				this.OnPropertyChanged("CountriesTitle");
			}
		}
		internal async Task InitilizeData()
		{
			AllLocationListCode = App.Locator.HomePage.AllLocationListCode;
			if (AllLocationListCode != null && AllLocationListCode.Count > 0) {
				CountriesTitle = $"+{AllLocationListCode.First().country_code}";
			}
			PhoneNumber = string.Empty;
			IsPhoneErrorVisible = false;
		}
		private async Task SendOTPExecute()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					var oTPRequestModel = new OTPRequestModel {
						country_code = CountriesTitle,
						phone = PhoneNumber
					};
					var result = await TryWithErrorAsync(_authenticationService.SendOTP(oTPRequestModel), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						App.Locator.CurrentUser.SendWay = SendInvite.VERIFICATION.ToString();
						App.UserPhone = $"{CountriesTitle}{PhoneNumber}";
						OTPPopupPageViewModel oTPPopupPageViewModel = new OTPPopupPageViewModel(_navigationService, _authenticationService, _userCoreService);
						await _navigationService.NavigateToPopupAsync<OTPPopupPageViewModel>(true, oTPPopupPageViewModel);

						await App.Locator.OTPPopupPage.InitilizeData();
					} else {
						ShowAlert( AppResources.Please_check_your_Phone_number_It_may_be_not_reachable);
					}
				} catch (Exception ex) {
					ShowToast(AppResources.ServerError);
				}
				HideLoading();
			} else
				ShowToast(AppResources.NoInternet);

		}
	}
}

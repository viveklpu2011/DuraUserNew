using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Profile.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;

namespace NewDuraApp.Areas.Profile.PopupViews.ViewModels
{
	public class OTPPopupPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		public IAsyncCommand NavigateToChangePhone { get; set; }
		public IAsyncCommand ResendOTPCommand { get; set; }
		public IAsyncCommand GetACallCommand { get; set; }
		private IAuthenticationService _authService;
		private IUserCoreService _userCoreService;
		private string _otp1, _otp2, _otp3, _otp4;
		public OTPPopupPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_authService = authenticationService;
			_userCoreService = userCoreService;
			ResendOTPCommand = new AsyncCommand(ResendOTPCommandExecute);
			GetACallCommand = new AsyncCommand(GetACallCommandExecute);
			NavigateToChangePhone = new AsyncCommand(NavigateToChangePhonePage);
			if (!string.IsNullOrEmpty(App.UserPhone)) {
				PhoneNumber = App.UserPhone;
			}
		}

		private async Task GetACallCommandExecute()
		{
			ShowToast("This functioanlity is in progress");
		}

		private async Task ResendOTPCommandExecute()
		{
			OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
			await ResendOTP();
		}

		public string OTP1 {
			get { return _otp1; }
			set { _otp1 = value; OnPropertyChanged("OTP1"); }
		}
		public string OTP2 {
			get { return _otp2; }
			set { _otp2 = value; OnPropertyChanged("OTP2"); }
		}
		public string OTP3 {
			get { return _otp3; }
			set { _otp3 = value; OnPropertyChanged("OTP3"); }
		}
		public string OTP4 {
			get { return _otp4; }
			set { _otp4 = value; OnPropertyChanged("OTP4"); }
		}
		private ChangePasswordRequestModel _changePasswordData;
		public ChangePasswordRequestModel ChangePasswordData {
			get { return _changePasswordData; }
			set {
				_changePasswordData = value;
				OnPropertyChanged(nameof(ChangePasswordData));
			}
		}
		private string _phoneNumber;
		public string PhoneNumber {
			get { return _phoneNumber; }
			set {
				_phoneNumber = value;
				OnPropertyChanged(nameof(PhoneNumber));
			}
		}

		private async Task NavigateToChangePhonePage()
		{
			await VerifyOTPCommandExecute();
		}

		internal async Task InitilizeData(ChangePasswordRequestModel args = null)
		{
			if (args != null) {
				OTP1 = OTP2 = OTP3 = OTP4 = string.Empty;
				PhoneNumber = SettingsExtension.Phone;
				ChangePasswordData = args;
			}

			//throw new NotImplementedException();
		}

		private async Task ResendOTP()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					OTPRequestModel oTPRequestModel;
					if (App.Locator.CurrentUser.SendWay == SendInvite.VERIFICATION.ToString()) {
						oTPRequestModel = new OTPRequestModel {
							country_code = App.Locator.PhoneVerificationPopupPage.CountriesTitle,
							phone = App.Locator.PhoneVerificationPopupPage.PhoneNumber,
						};
					} else {
						oTPRequestModel = new OTPRequestModel {
							country_code = App.Locator.ProfilePage.ProfileDetails.country_code,
							phone = App.Locator.ProfilePage.ProfileDetails.phone,
						};
					}

					var result = await TryWithErrorAsync(_authService.SendOTP(oTPRequestModel), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						//ShowToast($"Your OTP is {result?.Data?.otp}");
					} else {
						ShowAlert(result?.Data?.message);
					}
				} catch (Exception ex) {
					ShowToast(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);

		}

		private async Task VerifyOTPCommandExecute()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					VerifyOTPRequestModel verifyOTPRequestModel = new VerifyOTPRequestModel();

					if (App.Locator.CurrentUser.SendWay == SendInvite.VERIFICATION.ToString()) {
						verifyOTPRequestModel = new VerifyOTPRequestModel {
							country_code = App.Locator.PhoneVerificationPopupPage.CountriesTitle,
							phone = App.Locator.PhoneVerificationPopupPage.PhoneNumber,
							otp = $"{OTP1}{OTP2}{OTP3}{OTP4}"
						};
					} else {
						verifyOTPRequestModel = new VerifyOTPRequestModel {
							country_code = App.Locator.ProfilePage.ProfileDetails.country_code,
							phone = App.Locator.ProfilePage.ProfileDetails.phone,
							otp = $"{OTP1}{OTP2}{OTP3}{OTP4}"
						};
					}


					var result = await TryWithErrorAsync(_authService.VerifyOTP(verifyOTPRequestModel), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						if (App.Locator.CurrentUser.SendWay == SendInvite.OTP.ToString()) {
							ShowToast("OTP verified");
							await PopupNavigation.Instance.PopAsync();
							await Task.Delay(1000);
							App.Locator.EditProfileInfoPage.ButtonTextOTP = "Save Phone";
						} else if (App.Locator.CurrentUser.SendWay == SendInvite.VERIFICATION.ToString()) {
							ShowToast("OTP verified");
							if (CheckConnection()) {
								ShowLoading();
								try {
									var verifyPhoneRequestModel = new VerifyPhoneRequestModel {
										country_code = App.Locator.PhoneVerificationPopupPage.CountriesTitle,
										phone = App.Locator.PhoneVerificationPopupPage.PhoneNumber,
										user_id = SettingsExtension.UserId
									};
									var resultphoneverification = await TryWithErrorAsync(_userCoreService.VerifyPhoneNumber(verifyPhoneRequestModel, SettingsExtension.Token), true, false);
									if (resultphoneverification?.ResultType == ResultType.Ok && resultphoneverification?.Data?.status == 200) {
										ShowToast("Phone number verified successfully.");
										await PopupNavigation.Instance.PopAsync();
										await PopupNavigation.Instance.PopAsync();
										ShowAlert("Your Default Location is Metro Manila. You can update it from profile section.");

									} else {
										await PopupNavigation.Instance.PopAsync();
										ShowAlert("Phone Number already present in our dura database.Please use different one.");
										App.Locator.PhoneVerificationPopupPage.PhoneNumber = string.Empty;
									}
								} catch (Exception ex) {
									ShowToast(CommonMessages.ServerError);
								}
								HideLoading();
							} else
								ShowToast(CommonMessages.NoInternet);
						} else {
							if (CheckConnection()) {
								ShowLoading();
								try {
									var resultpassword = await TryWithErrorAsync(_userCoreService.ChangePassword(ChangePasswordData, SettingsExtension.Token), true, false);
									if (resultpassword?.ResultType == ResultType.Ok && resultpassword?.Data?.status == 200) {
										ShowToast("OTP verified");
										await PopupNavigation.Instance.PopAsync();
										await _navigationService.NavigateBackAsync();
										ShowToast("Password updated successfully.");
									} else {

										ShowToast("Password not updated! Server Error");
										await PopupNavigation.Instance.PopAsync();
									}
								} catch (Exception ex) {
									ShowToast(CommonMessages.ServerError);
								}
								HideLoading();
							} else
								ShowToast(CommonMessages.NoInternet);
						}

					} else {
						ShowAlert(result?.Data?.message);
					}
					//ShowAlert(result.Data.message);
				} catch (Exception ex) {
					//ShowToast(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);

		}
	}
}

using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Areas.Profile.PopupViews.Views;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Profile.ViewModels
{
	public class ChangePasswordPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private IUserCoreService _userCoreService;
		private IAuthenticationService _authService;
		public IAsyncCommand GoToOTPVerifyCmd { get; set; }
		private bool _isButtonEnabled;
		public bool IsButtonEnabled {
			get { return _isButtonEnabled; }
			set {
				_isButtonEnabled = value;
				OnPropertyChanged(nameof(IsButtonEnabled));
			}
		}
		private bool _isNewPasswordErrorVisible;
		private bool _isConfirmPasswordErrorVisible;
		private ChangePasswordRequestModel _changePasswordData;
		public ChangePasswordRequestModel ChangePasswordData {
			get { return _changePasswordData; }
			set {
				_changePasswordData = value;
				OnPropertyChanged(nameof(ChangePasswordData));
			}
		}
		public bool IsNewPasswordErrorVisible {
			get { return _isNewPasswordErrorVisible; }
			set {
				_isNewPasswordErrorVisible = value;
				OnPropertyChanged(nameof(IsNewPasswordErrorVisible));
			}
		}

		public bool IsConfirmPasswordErrorVisible {
			get { return _isConfirmPasswordErrorVisible; }
			set {
				_isConfirmPasswordErrorVisible = value;
				OnPropertyChanged(nameof(IsConfirmPasswordErrorVisible));
			}
		}
		private string _oldPassword;
		public string OldPassword {
			get { return _oldPassword; }
			set {
				_oldPassword = value;
				OnPropertyChanged(nameof(OldPassword));
			}
		}

		private string _newPassword;
		public string NewPassword {
			get { return _newPassword; }
			set {
				_newPassword = value;
				if (!string.IsNullOrEmpty(_newPassword)) {
					if (RegexUtilities.PasswordValidation(_newPassword))
						IsNewPasswordErrorVisible = false;
					else
						IsNewPasswordErrorVisible = true;
				} else {
					IsNewPasswordErrorVisible = false;
				}
				CheckPasswordValidation();
				OnPropertyChanged(nameof(NewPassword));
			}
		}

		private string _confirmPassword;
		public string ConfirmPassword {
			get { return _confirmPassword; }
			set {
				_confirmPassword = value;
				if (!string.IsNullOrEmpty(_confirmPassword)) {
					if (RegexUtilities.ConfirmPasswordValidation(_newPassword, _confirmPassword))
						IsConfirmPasswordErrorVisible = false;
					else
						IsConfirmPasswordErrorVisible = true;
				} else {
					IsConfirmPasswordErrorVisible = false;
				}
				CheckPasswordValidation();
				OnPropertyChanged(nameof(ConfirmPassword));
			}
		}


		public ChangePasswordPageViewModel(INavigationService navigationService, IUserCoreService userCoreService, IAuthenticationService authenticationService)
		{
			_navigationService = navigationService;
			_authService = authenticationService;
			_userCoreService = userCoreService;
			GoToOTPVerifyCmd = new AsyncCommand(GoToOTPVerifyCmdExecute, allowsMultipleExecutions: false);
		}

		private async Task GoToOTPVerifyCmdExecute()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {

					OTPRequestModel oTPRequestModel = new OTPRequestModel {
						country_code = App.Locator.ProfilePage.ProfileDetails.country_code,
						phone = App.Locator.ProfilePage.ProfileDetails.phone
					};

					var result = await TryWithErrorAsync(_authService.SendOTP(oTPRequestModel), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						// ShowToast($"Your OTP is {result?.Data?.otp}");
						ChangePasswordData = new ChangePasswordRequestModel {
							user_id = SettingsExtension.UserId,
							old_password = OldPassword,
							password = NewPassword,
							password_confirmation = ConfirmPassword
						};


						if (_navigationService.GetCurrentPageViewModel() != typeof(OTPPopupPageViewModel)) {
							await PopupNavigation.Instance.PushAsync(new OTPPopupPage());
							await App.Locator.OTPPopupPage.InitilizeData(ChangePasswordData);
						}
					} else {
						ShowAlert(AppResources.Sending_OTP_Server_Error);
					}
					//ShowAlert(result.Data.message);
				} catch (Exception ex) {
					ShowToast(AppResources.ServerError);
				}
				HideLoading();
			} else
				ShowToast(AppResources.NoInternet);
		}

		public bool CheckPasswordValidation()
		{
			if (IsConfirmPasswordErrorVisible || IsNewPasswordErrorVisible) {
				IsButtonEnabled = false;
				return false;
			} else if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(NewPassword)) {
				IsButtonEnabled = false;
				return false;
			} else {
				IsButtonEnabled = true;
				return true;
			}
		}
		internal async Task InitilizeData()
		{
			NewPassword = OldPassword = ConfirmPassword = string.Empty;
			IsNewPasswordErrorVisible = IsConfirmPasswordErrorVisible = false;
			CheckPasswordValidation();
		}
	}
}

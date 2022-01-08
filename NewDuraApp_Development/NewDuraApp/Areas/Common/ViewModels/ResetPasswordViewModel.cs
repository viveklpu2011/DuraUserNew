using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;

namespace NewDuraApp.Areas.Common.ViewModels
{
	public class ResetPasswordViewModel : AppBaseViewModel
	{
		public IAsyncCommand SuccessPopupCmd { get; set; }
		INavigationService _navigationService;
		private bool _passwordNotValid;
		private bool _confirmPasswordNotValid;
		private bool _isSuccessButtoinEnabled;
		private bool _isPasswordErrorVisible;
		private bool _isConfirmPasswordErrorVisible;
		public bool IsPasswordErrorVisible {
			get { return _isPasswordErrorVisible; }
			set {
				_isPasswordErrorVisible = value;
				OnPropertyChanged(nameof(IsPasswordErrorVisible));
			}
		}
		public bool IsConfirmPasswordErrorVisible {
			get { return _isConfirmPasswordErrorVisible; }
			set {
				_isConfirmPasswordErrorVisible = value;
				OnPropertyChanged(nameof(IsConfirmPasswordErrorVisible));
			}
		}
		private IAuthenticationService _authService;
		public ResetPasswordViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
		{
			_navigationService = navigationService;
			_authService = authenticationService;
			SuccessPopupCmd = new AsyncCommand(SuccessPopupCmdExecute);
		}

		private async Task SuccessPopupCmdExecute()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					ResetPasswordRequestModel resetPasswordRequestModel = new ResetPasswordRequestModel {
						country_code = App.Locator.ForgetPasswordPage.SignupRequestModelVM.country_code,
						phone = App.Locator.ForgetPasswordPage.SignupRequestModelVM.phone,
						password = Password
					};
					var result = await TryWithErrorAsync(_authService.ResetPassword(resetPasswordRequestModel), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						if (_navigationService.GetCurrentPageViewModel() != typeof(VerifyOTPPageViewModel)) {
							if (_navigationService.GetCurrentPageViewModel() != typeof(ResetSuccessfullyPopup)) {
								await PopupNavigation.Instance.PushAsync(new ResetSuccessfullyPopup());
								await App.Locator.LoginPage.GetAllLocation();
								await PopupNavigation.Instance.PopAsync();
								await _navigationService.NavigateToAsync<LoginPageViewModels>();
								await App.Locator.LoginPage.InitilizeData();
								Password = string.Empty;
								ConfirmPassword = string.Empty;
							}
						}
					} else if (result?.Data?.status == 201) {
						ShowAlert(result?.Data?.message);
						return;
					}
				} catch (Exception ex) {
					ShowToast(AppResources.ServerError);
				}
				HideLoading();
			} else
				ShowToast(AppResources.NoInternet);
		}
		public bool PasswordNotValid {
			get { return _passwordNotValid; }
			set {
				_passwordNotValid = value;
				OnPropertyChanged(nameof(PasswordNotValid));
			}
		}
		public bool ConfirmPasswordNotValid {
			get { return _confirmPasswordNotValid; }
			set {
				_confirmPasswordNotValid = value;
				OnPropertyChanged(nameof(ConfirmPasswordNotValid));
			}
		}
		public bool IsSuccessButtoinEnabled {
			get { return _isSuccessButtoinEnabled; }
			set {
				_isSuccessButtoinEnabled = value;
				OnPropertyChanged(nameof(IsSuccessButtoinEnabled));
			}
		}
		private string _password;

		public string Password {
			get { return _password; }
			set {
				_password = value;

				if (!string.IsNullOrEmpty(_password)) {
					if (RegexUtilities.PasswordValidation(_password))
						IsPasswordErrorVisible = false;
					else
						IsPasswordErrorVisible = true;
					CheckLoginValidation();
				} else {
					IsPasswordErrorVisible = false;
				}
				OnPropertyChanged(nameof(Password));
			}
		}
		private string _confirmPassword;

		public string ConfirmPassword {
			get { return _confirmPassword; }
			set {
				_confirmPassword = value;
				if (!string.IsNullOrEmpty(_confirmPassword) && !string.IsNullOrEmpty(_password)) {
					if (RegexUtilities.ConfirmPasswordValidation(_password, _confirmPassword))
						IsConfirmPasswordErrorVisible = false;
					else
						IsConfirmPasswordErrorVisible = true;
					CheckLoginValidation();

				} else {
					IsConfirmPasswordErrorVisible = false;
				}
				OnPropertyChanged(nameof(ConfirmPassword));
			}
		}
		public bool CheckLoginValidation()
		{
			if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword)) {
				IsSuccessButtoinEnabled = false;
				return false;
			} else if (IsPasswordErrorVisible || IsConfirmPasswordErrorVisible) {
				IsSuccessButtoinEnabled = false;
				return false;
			} else {
				IsSuccessButtoinEnabled = true;
				return true;
			}
		}
	}
}
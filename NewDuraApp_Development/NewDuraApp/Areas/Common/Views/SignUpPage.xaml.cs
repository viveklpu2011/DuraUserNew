using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using NewDuraApp.Models;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Xamarin.Auth;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.Views
{
	public partial class SignUpPage : ContentPage
	{
		Account account;
		IGoogleClientManager _googleService;
		IFacebookClient _facebookService;
		public SignUpPage()
		{
			InitializeComponent();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			//App.Locator.SignUpPage.SelectedLocation = null;
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			_googleService = CrossGoogleClient.Current;
			_facebookService = CrossFacebookClient.Current;
			_googleService.Logout();
			_facebookService.Logout();
			await Task.CompletedTask;
		}
		private FacebookResponseModel _facebookResponse;
		public FacebookResponseModel FacebookResponse {
			get { return _facebookResponse; }
			set { _facebookResponse = value; OnPropertyChanged(nameof(FacebookResponse)); }
		}

		#region SocialLogin
		async void googlebtn_Clicked(System.Object sender, System.EventArgs e)
		{
			AuthNetwork authNetwork = new AuthNetwork() {
				Name = "Google",
				Icon = "ic_google",
				Foreground = "#000000",
				Background = "#F8F8F8"
			};

			await LoginGoogleAsync(authNetwork);
		}

		async void facebookbtn_Clicked(System.Object sender, System.EventArgs e)
		{
			AuthNetwork authNetwork = new AuthNetwork() {
				Name = "Facebook",
				Icon = "ic_ig",
				Foreground = "#FFFFFF",
				Background = "#DD2A7B"
			};

			await LoginFacebookAsync(authNetwork);
		}

		async Task LoginGoogleAsync(AuthNetwork authNetwork)
		{
			try {
				if (!string.IsNullOrEmpty(_googleService.AccessToken)) {
					//Always require user authentication
					_googleService.Logout();
				}

				EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
				userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) => {
					switch (e.Status) {
						case GoogleActionStatus.Completed:
#if DEBUG
							var googleUserString = JsonConvert.SerializeObject(e.Data);
							Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
#endif

							var socialLoginData = new NetworkAuthData {
								Id = e.Data.Id,
								Logo = authNetwork.Icon,
								Foreground = authNetwork.Foreground,
								Background = authNetwork.Background,
								Picture = e.Data.Picture.AbsoluteUri,
								Name = e.Data.Name,
								Email = e.Data.Email,

							};

							GoogleLogin(socialLoginData);
							break;
						case GoogleActionStatus.Canceled:
							//await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
							break;
						case GoogleActionStatus.Error:
							//await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
							break;
						case GoogleActionStatus.Unauthorized:
							//await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
							break;
					}

					_googleService.OnLogin -= userLoginDelegate;
				};

				_googleService.OnLogin += userLoginDelegate;

				await _googleService.LoginAsync();
			} catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
		}

		async void GoogleLogin(NetworkAuthData socialLoginData)
		{
			if (socialLoginData != null) {

				FacebookRequestModel GmailRequest = new FacebookRequestModel {
					area = "area",
					email = socialLoginData.Email,
					facebook_id = "",
					last_name = "",
					phone = "",
					referral_code = "",
					first_name = socialLoginData.Name,
					login_type = "google",
					google_id = socialLoginData.Id
				};
				await App.Locator.LoginPage.OnLoginWithGoogleCommandExecute(GmailRequest);
			} else {
				await DisplayAlert("Error", "Invalid phone number or email id", "Ok");
				//App.Current.MainPage = new NavigationPage(new MyDashBoardPage());
				return;

			}
		}

		async Task LoginFacebookAsync(AuthNetwork authNetwork)
		{
			try {

				if (_facebookService.IsLoggedIn) {
					_facebookService.Logout();
				}

				EventHandler<FBEventArgs<string>> userDataDelegate = null;

				userDataDelegate = async (object sender, FBEventArgs<string> e) => {
					switch (e.Status) {
						case FacebookActionStatus.Completed:
							var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
							var socialLoginData = new NetworkAuthData {
								Id = facebookProfile.Id,
								Logo = authNetwork.Icon,
								Foreground = authNetwork.Foreground,
								Background = authNetwork.Background,
								Picture = facebookProfile.Picture.Data.Url,
								first_name = facebookProfile.FirstName,
								last_name = facebookProfile.LastName,
								login_type = "facebook",
								Email = facebookProfile.Email,
								Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}",
							};
							await App.Locator.LoginPage.OnLoginWithFacebookCommand(socialLoginData);
							break;
						case FacebookActionStatus.Canceled:
							//await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
							break;
						case FacebookActionStatus.Error:
							//await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
							break;
						case FacebookActionStatus.Unauthorized:
							//await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
							break;
					}

					_facebookService.OnUserData -= userDataDelegate;
				};

				_facebookService.OnUserData += userDataDelegate;

				string[] fbRequestFields = { "email", "first_name", "picture", "gender", "last_name" };
				string[] fbPermisions = { "email" };
				await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
			} catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}
		}

		#endregion
		void pickerLocation_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			App.Locator.SignUpPage.SelectedLocation = null;
			int selectedIndex = pickerLocation.SelectedIndex;
			if (selectedIndex != -1) {
				App.Locator.SignUpPage.SelectedLocation = (NewLocationDataResponse)pickerLocation.ItemsSource[selectedIndex];
			}
		}
		protected override bool OnBackButtonPressed()
		{
			App.Locator.LoginPage.InitilizeData();
			return base.OnBackButtonPressed();
		}
	}
}

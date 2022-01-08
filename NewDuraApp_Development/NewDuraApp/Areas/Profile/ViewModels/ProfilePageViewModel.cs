using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.ViewModels
{
    public class ProfilePageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand NavigateToChangePassword { get; set; }
        public IAsyncCommand NavigateToEditProfileInfo { get; set; }
        public IAsyncCommand NavigateToPersonalUploadDocument { get; set; }
        public IAsyncCommand NavigateToBusinessUploadDocument { get; set; }
        public IAsyncCommand LinkCommand { get; set; }
        public IAsyncCommand GoogleCommand { get; set; }
        private GetProfileDetailsModel _profileDetails;
        public IAsyncCommand ToggleIsBusinessCommand { get; set; }
        IFacebookClient _facebookService;
        IGoogleClientManager _googleService;
        private Color _switchThumbColor = Color.Red;
        public Color SwitchThumbColor
        {
            get { return _switchThumbColor; }
            set
            {
                _switchThumbColor = value;
                OnPropertyChanged(nameof(SwitchThumbColor));
            }
        }

        public GetProfileDetailsModel ProfileDetails
        {
            get { return _profileDetails; }
            set
            {
                _profileDetails = value;
                OnPropertyChanged(nameof(ProfileDetails));
            }
        }

        private string _facebookLink;
        public string FacebookLink
        {
            get { return _facebookLink; }
            set
            {
                _facebookLink = value;
                OnPropertyChanged(nameof(FacebookLink));
            }
        }

        private string _googleLink;
        public string GoogleLink
        {
            get { return _googleLink; }
            set
            {
                _googleLink = value;
                OnPropertyChanged(nameof(GoogleLink));
            }
        }

        private bool _isVisibleBusinessView;
        public bool IsBusinessVisible
        {
            get { return _isVisibleBusinessView; }
            set
            {
                _isVisibleBusinessView = value;
                OnPropertyChanged(nameof(IsBusinessVisible));
            }
        }

        private string _linkText;
        public string LinkText
        {
            get { return _linkText; }
            set
            {
                _linkText = value;
                OnPropertyChanged(nameof(LinkText));
            }
        }
        private string _linkTextGoogle;
        public string LinkTextGoogle
        {
            get { return _linkTextGoogle; }
            set
            {
                _linkTextGoogle = value;
                OnPropertyChanged(nameof(LinkTextGoogle));
            }
        }

        private byte[] _productImage;
        public byte[] ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
        }
        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }

        private bool _isbusineestoggled;
        public bool IsBusinessToggeld
        {
            get { return _isbusineestoggled; }
            set
            {
                _isbusineestoggled = value;
                OnPropertyChanged(nameof(IsBusinessToggeld));
            }
        }

        public ProfilePageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            NavigateToChangePassword = new AsyncCommand(NavigateToChangePasswordPage, allowsMultipleExecutions: false);
            NavigateToEditProfileInfo = new AsyncCommand(NavigateToEditProfileInfoPage, allowsMultipleExecutions: false);
            NavigateToPersonalUploadDocument = new AsyncCommand(NavigateToPersonalUploadDocumentPage, allowsMultipleExecutions: false);
            NavigateToBusinessUploadDocument = new AsyncCommand(NavigateToBusinessUploadDocumentPage, allowsMultipleExecutions: false);
            LinkCommand = new AsyncCommand(LinkCommandExecute, allowsMultipleExecutions: false);
            GoogleCommand = new AsyncCommand(GoogleCommandExecute, allowsMultipleExecutions: false);
            ToggleIsBusinessCommand = new AsyncCommand(ToggleIsBusinessCommandExecute, allowsMultipleExecutions: false);
            _googleService = CrossGoogleClient.Current;
            _facebookService = CrossFacebookClient.Current;
            _googleService.Logout();
            _facebookService.Logout();
            FacebookLink = AppResources.Not_Linked;
            LinkText = AppResources.Not_Linked;
        }

        private async Task ToggleIsBusinessCommandExecute()
        {
            SettingsExtension.IsVisibleBusinessDocument = IsBusinessToggeld;
            if (IsBusinessToggeld)
            {
                SwitchThumbColor = Color.Green;
                IsBusinessVisible = true;
            }
            else
            {
                SwitchThumbColor = Color.Red;
                IsBusinessVisible = false;
            }
        }

        private async Task GoogleCommandExecute()
        {
            if (ProfileDetails != null)
            {
                AuthNetwork authNetwork = new AuthNetwork()
                {
                    Name = "Google",
                    Icon = "ic_google",
                    Foreground = "#000000",
                    Background = "#F8F8F8"
                };
                await ConnectGoogleAsync(authNetwork);
            }
        }
        private async Task LinkCommandExecute()
        {
            if (ProfileDetails != null)
            {
                AuthNetwork authNetwork = new AuthNetwork()
                {
                    Name = "Facebook",
                    Icon = "ic_ig",
                    Foreground = "#FFFFFF",
                    Background = "#DD2A7B"
                };
                await ConnectFacebookAsync(authNetwork);

            }
        }
        private async Task NavigateToBusinessUploadDocumentPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(BusinessDocumentUploadPageViewModel))
            {
                await App.Locator.BusinessDocumentUploadPage.InitilizeData();
                await _navigationService.NavigateToAsync<BusinessDocumentUploadPageViewModel>();

            }
        }

        private async Task NavigateToPersonalUploadDocumentPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(PersonalDocumentUploadPageViewModel))
            {
                await _navigationService.NavigateToAsync<PersonalDocumentUploadPageViewModel>();
                await App.Locator.PersonalDocumentUploadPage.InitilizeData();
            }
        }

        private async Task NavigateToEditProfileInfoPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(EditProfileInfoPageViewModel))
            {
                //await App.Locator.EditProfileInfoPage.GetAllLocation();
                await App.Locator.EditProfileInfoPage.InitilizeData(ProfileDetails);
                await _navigationService.NavigateToAsync<EditProfileInfoPageViewModel>();

            }
        }

        private async Task NavigateToChangePasswordPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ChangePasswordPageViewModel))
            {
                await App.Locator.ChangePasswordPage.InitilizeData();
                await _navigationService.NavigateToAsync<ChangePasswordPageViewModel>();

            }
        }

        public async Task InitilizeData(GetProfileDetailsModel getProfileDetailsModel = null)
        {

            _profileDetails = getProfileDetailsModel;
            if (SettingsExtension.IsVisibleBusinessDocument)
            {
                SwitchThumbColor = Color.Green;
                IsBusinessVisible = true;
            }
            else
            {
                IsBusinessVisible = false;
                SwitchThumbColor = Color.Red;
            }

            if (!string.IsNullOrEmpty(getProfileDetailsModel?.google))
            {
                GoogleLink = getProfileDetailsModel.google;
                LinkTextGoogle = AppResources.Linked;
            }
            else
            {
                GoogleLink = "";
                LinkTextGoogle = "Link";

            }
            if (!string.IsNullOrEmpty(getProfileDetailsModel?.facebook))
            {
                FacebookLink = getProfileDetailsModel.facebook;
                LinkText = AppResources.Linked;
            }
            else
            {
                FacebookLink = "";
                LinkText = "Link";

            }

        }
        public async Task UpdateSociaLink(string type)
        {

            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    UpdateSocialLinkRequestModel updateSocialLinkRequestModel = new UpdateSocialLinkRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                        google = GoogleLink,
                        facebook = FacebookLink
                    };
                    var result = await TryWithErrorAsync(_userCoreService.UpdateSocialLink(updateSocialLinkRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        ShowToast($"{type} link updated");
                        if (type == "Facebook")
                        {
                            LinkText = AppResources.Linked;
                        }
                        if (type == "Google")
                        {
                            LinkTextGoogle = AppResources.Linked;
                        }
                        await App.Locator.MyProfile.InitilizeData();
                    }
                    //ShowAlert(result?.Data?.message, result?.Data?.message);
                }
                catch (Exception ex)
                {
                    // ShowAlert(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }

        async Task ConnectFacebookAsync(AuthNetwork authNetwork)
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
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            var socialLoginData = new NetworkAuthData
                            {
                                Id = facebookProfile.Id,
                            };
                            FacebookLink = "https://www.facebook.com/" + socialLoginData.Id;
                            await UpdateSociaLink("Facebook");
                            //await App.Locator.LoginPage.OnLoginWithFacebookCommand(socialLoginData);
                            break;
                        case FacebookActionStatus.Canceled:
                            //FacebookLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Facebook Auth", "Canceled", "Ok");
                            break;
                        case FacebookActionStatus.Error:
                            //FacebookLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Facebook Auth", "Error", "Ok");
                            break;
                        case FacebookActionStatus.Unauthorized:
                            //FacebookLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Facebook Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "picture", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task ConnectGoogleAsync(AuthNetwork authNetwork)
        {
            try
            {
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
#if DEBUG
                            var googleUserString = JsonConvert.SerializeObject(e.Data);
                            Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
#endif

                            var socialLoginData = new NetworkAuthData
                            {
                                Id = e.Data.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = e.Data.Picture.AbsoluteUri,
                                Name = e.Data.Name,
                                Email = e.Data.Email,

                            };
                            GoogleLink = "https://www.google.com/" + socialLoginData.Id;
                            await UpdateSociaLink("Google");
                            //GoogleLogin(socialLoginData);
                            break;
                        case GoogleActionStatus.Canceled:
                            //GoogleLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            //GoogleLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            //GoogleLink = string.Empty;
                            //await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}

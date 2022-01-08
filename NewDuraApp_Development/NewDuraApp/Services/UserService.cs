using Acr.UserDialogs;
using DuraApp.Core.Helpers;
using NewDuraApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewDuraApp.Services
{
    public class UserService : BindableBase, IUserService
    {
        protected IUserDialogs _userDialogs => UserDialogs.Instance;
        private INavigationService _navigationService;
        public UserService()
        {

        }
        public bool LoggedIn => IsUserLoggedIn();

        public bool IsUserLoggedIn()
        {
            //if (SettingsExtension.AccessToken != null)
            //    if (!string.IsNullOrEmpty(SettingsExtension.AccessToken.access_token))
            //        return true;
            return false;
        }
        public async Task SignOutUser()
        {
            //App.Locator.CurrentUser.AccessToken = null;
            //App.Locator.CurrentUser.UserProfileData = null;
            //SettingsExtension.AccessToken = null;
            _navigationService = App.Container.Resolve<INavigationService>();
            // await _navigationService.NavigateToAsync<AppShellViewModel>();
        }
    }
}

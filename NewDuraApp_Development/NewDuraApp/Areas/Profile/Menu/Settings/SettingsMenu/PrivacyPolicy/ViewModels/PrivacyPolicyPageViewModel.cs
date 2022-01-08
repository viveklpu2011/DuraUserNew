using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.PrivacyPolicy.ViewModels
{
    public class PrivacyPolicyPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;

        public PrivacyPolicyPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async Task InitilizeData()
        {

        }
    }
}

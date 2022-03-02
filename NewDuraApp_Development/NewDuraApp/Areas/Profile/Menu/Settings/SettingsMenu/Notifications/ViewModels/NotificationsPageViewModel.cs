using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.ViewModels
{
    public class NotificationsPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        
        public NotificationsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public async Task InitilizeData()
        {

        }
    }
}

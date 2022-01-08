using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.StandardRates.ViewModels
{
    public class StandardRatesPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public StandardRatesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task InitilizeData()
        {

        }
    }
}

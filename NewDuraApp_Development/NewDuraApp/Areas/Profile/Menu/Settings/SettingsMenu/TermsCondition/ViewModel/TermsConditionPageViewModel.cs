using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.TermsCondition.ViewModel
{
    public class TermsConditionPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;

        public TermsConditionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async Task InitilizeData()
        {

        }
    }
}

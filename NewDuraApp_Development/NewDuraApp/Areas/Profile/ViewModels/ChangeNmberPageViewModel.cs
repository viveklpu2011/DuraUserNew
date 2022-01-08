using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Profile.ViewModels
{
    public class ChangeNmberPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateBackToProfileHome { get; set; }
        public ChangeNmberPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateBackToProfileHome = new AsyncCommand(NavigateBackToProfileHomePage, allowsMultipleExecutions: false);
        }

        private async Task NavigateBackToProfileHomePage()
        {
            await _navigationService.NavigateBackAsync();
            await _navigationService.NavigateBackAsync();
        }

        internal async Task InitilizeData()
        {
            //throw new NotImplementedException();
        }
    }
}

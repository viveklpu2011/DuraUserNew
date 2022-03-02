using System;
using System.Threading.Tasks;
using NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels
{
    public class ChangeLocationPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigaeteToSavedAddressPopup { get; set; }
        public ChangeLocationPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigaeteToSavedAddressPopup = new AsyncCommand(NavigaeteToSavedAddressPopupPage, allowsMultipleExecutions: false);
        }

        private async Task NavigaeteToSavedAddressPopupPage()
        {
           // await App.Locator.MySavedAddressPopupPage.InitilizeData();
            MySavedAddressPopupPageViewModel savedAddressPopup = new MySavedAddressPopupPageViewModel(_navigationService);
            await _navigationService.NavigateToPopupAsync<MySavedAddressPopupPageViewModel>(true, savedAddressPopup);
        }

        public async Task InitilizeData()
        {

        }
    }
}

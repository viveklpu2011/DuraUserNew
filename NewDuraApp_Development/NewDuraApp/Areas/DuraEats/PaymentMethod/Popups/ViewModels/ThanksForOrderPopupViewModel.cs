using System;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels
{
    public class ThanksForOrderPopupViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToTrackOrder { get; set; }
        public ThanksForOrderPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToTrackOrder = new AsyncCommand(NavigateToTrackOrderCommand);
        }

        internal async Task InitilizeData()
        {
           // throw new NotImplementedException();
        }

        private async Task NavigateToTrackOrderCommand()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(TrackOrderPageViewModel))
            {
                await _navigationService.ClosePopupsAsync();
                await App.Locator.TrackOrderPage.InitilizeData();
                await _navigationService.NavigateToAsync<TrackOrderPageViewModel>();
            }
        }
    }
}

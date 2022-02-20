using System;
using System.Threading.Tasks;
using NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels
{
    public class ThanksForOrderPopupViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToTrackOrder { get; set; }
        public ThanksForOrderPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToTrackOrder = new AsyncCommand(NavigateToTrackOrderCommand, allowsMultipleExecutions: false);
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

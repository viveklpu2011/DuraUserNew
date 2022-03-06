using System;
using System.Threading.Tasks;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using NewDuraApp.Areas.DuraEats.OrderDetails.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels
{
    public class TrackOrderPageViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToOrderDetails { get; set; }
        public IAsyncCommand NavigateToChatHelp { get; set; }
        public TrackOrderPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToOrderDetails = new AsyncCommand(NavigateToOrderDetailsPage, allowsMultipleExecutions: false);
            NavigateToChatHelp = new AsyncCommand(NavigateToChatHelpPage, allowsMultipleExecutions: false);
        }

        private async Task NavigateToChatHelpPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(DuraEatsHelpPageViewModel))
            {
                await App.Locator.DuraEatsHelpPage.InitilizeData();
                await _navigationService.NavigateToAsync<DuraEatsHelpPageViewModel>();
            }
        }

        private async Task NavigateToOrderDetailsPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetailsPageViewModel))
            {
                await App.Locator.OrderDetailsPage.InitilizeData();
                await _navigationService.NavigateToAsync<OrderDetailsPageViewModel>();
            }
        }

        internal async Task InitilizeData()
        {
            // throw new NotImplementedException();
        }
    }
}

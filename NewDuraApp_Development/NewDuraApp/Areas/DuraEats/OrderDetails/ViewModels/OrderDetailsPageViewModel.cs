using System;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.OrderDetails.ViewModels
{
    public class OrderDetailsPageViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToHomeCmd { get; set; }

        public OrderDetailsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToHomeCmd = new AsyncCommand(GoToHomeCmdExecute);
        }
        private async Task GoToHomeCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(HomePageViewModel))
            {
                TabNavigationHelper.ForceFullyRedirectingTab(0);
            }
        }
        internal async Task InitilizeData()
        {
            // throw new NotImplementedException();
        }
    }
}

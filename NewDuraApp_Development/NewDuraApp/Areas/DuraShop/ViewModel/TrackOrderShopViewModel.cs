
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraShop.Popup.ViewModel;
using NewDuraApp.Areas.Orders.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraShop.ViewModel
{
   public class TrackOrderShopViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToMyOrderDuraShop { get; set; }
        public TrackOrderShopViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToMyOrderDuraShop = new AsyncCommand(GoToMyOrderDuraShopExecute, allowsMultipleExecutions: false);
            GoToMyOrderIsVisible = true;
            MessagingCenter.Subscribe<MyOrdersViewModel>(this, "FromMyOrderShop", (sender) =>
            {
                GoToMyOrderIsVisible = false;
            });
            MessagingCenter.Subscribe<SuccessOrderPopupViewModel>(this, "FromSuccessPopup", (sender) =>
            {
                GoToMyOrderIsVisible = true;
            });
        }

        private async Task GoToMyOrderDuraShopExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(MyOrdersViewModel))
            {
                // await _navigationService.NavigateToAsync<MyOrdersViewModel>();
                // await Shell.Current.GoToAsync("MyOrders");             

                // Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[1];
                TabNavigationHelper.ForceFullyRedirectingTab(1);
            }
        }
        private bool _goToMyOrderIsVisible;

        public bool GoToMyOrderIsVisible
        {
            get { return _goToMyOrderIsVisible; }
            set { _goToMyOrderIsVisible = value; OnPropertyChanged(); }
        }

    }
}

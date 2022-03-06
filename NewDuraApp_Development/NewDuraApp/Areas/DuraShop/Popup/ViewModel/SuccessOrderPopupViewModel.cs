
using NewDuraApp.Areas.DuraShop.ViewModel;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraShop.Popup.ViewModel
{
   public class SuccessOrderPopupViewModel: AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToTrackOrderCmd { get; set; }
        public SuccessOrderPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToTrackOrderCmd = new AsyncCommand(GoToTrackOrderCmdExecute, allowsMultipleExecutions: false);
        }

        private async Task GoToTrackOrderCmdExecute()
        {
           if(_navigationService.GetCurrentPageViewModel()!= typeof(TrackOrderShopViewModel))
            {
                await _navigationService.NavigateToAsync<TrackOrderShopViewModel>();
                MessagingCenter.Send<SuccessOrderPopupViewModel>(this, "FromSuccessPopup");
                await PopupNavigation.PopAsync();
                
            }
        }
      
    }
}

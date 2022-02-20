
using NewDuraApp.Areas.DuraShop.Popup.View;
using NewDuraApp.Areas.DuraShop.Popup.ViewModel;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.PopupView.ViewModel
{
    public class CashOnDeliveryPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToSuccessPopupCmd { get; set; }
        public CashOnDeliveryPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToSuccessPopupCmd = new AsyncCommand(GoToSuccessPopupCmdExecute, allowsMultipleExecutions: false);
        }

        private async Task GoToSuccessPopupCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(SuccessOrderPopupViewModel))
            {
                await _navigationService.ClosePopupsAsync();
                await PopupNavigation.Instance.PushAsync(new SuccessOrderPopup());
            }
        }
    }
}

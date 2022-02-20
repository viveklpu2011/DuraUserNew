using System;
using System.Threading.Tasks;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraEats.PaymentMethod.ViewModels
{
    public class PaymentModePageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand NavigateToSelectAmount { get; set; }
        public IAsyncCommand NavigateToThanksForOrder { get; set; }
        public IAsyncCommand GoToCashOnDeliveryPopup { get; set; }
        public PaymentModePageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            NavigateToSelectAmount = new AsyncCommand(NavigateToSelectAmountPage, allowsMultipleExecutions: false);
            NavigateToThanksForOrder = new AsyncCommand(GetFoodDetails, allowsMultipleExecutions: false);
            GoToCashOnDeliveryPopup = new AsyncCommand(GoToCashOnDeliveryPopupExecute, allowsMultipleExecutions: false);
        }
        private async Task NavigateToSelectAmountPage()
        {
            // await App.Locator.TopupAmuntPopupPage.InitilizeData();
            TopupAmuntPopupPageViewModel topupAmuntPopupPageViewModel = new TopupAmuntPopupPageViewModel(_navigationService, _userCoreService);
            await _navigationService.NavigateToPopupAsync<TopupAmuntPopupPageViewModel>(true, topupAmuntPopupPageViewModel);
        }
        private async Task GoToCashOnDeliveryPopupExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(CashOnDeliveryPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new CashOnDeliveryPopup());
            }
        }
        internal async Task InitilizeData()
        {
            //throw new NotImplementedException();
        }

        public async Task GetFoodDetails()
        {
            await App.Locator.ThanksForOrderPopup.InitilizeData();
            ThanksForOrderPopupViewModel thanksPopup = new ThanksForOrderPopupViewModel(_navigationService);
            await _navigationService.NavigateToPopupAsync<ThanksForOrderPopupViewModel>(true, thanksPopup);
        }
    }
}

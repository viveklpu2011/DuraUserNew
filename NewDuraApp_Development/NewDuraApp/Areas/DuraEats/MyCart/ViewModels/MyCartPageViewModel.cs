using System;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;

using NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels;

using NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels;

using NewDuraApp.Areas.DuraEats.PaymentMethod.ViewModels;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.MyCart.ViewModels
{
    public class MyCartPageViewModel:AppBaseViewModel
    {
        public INavigationService _navigationService;
        public IAsyncCommand NavigateToPaymentMode { get; set; }
        public IAsyncCommand NavigateToPromoCode { get; set; }
        public IAsyncCommand NavigateToPromoCode1 { get; set; }
        public IAsyncCommand GoToAddItemCmd { get; set; }
        public IAsyncCommand NavigateToRateAndReviews { get; set; }
        public IAsyncCommand NavigateToChangeLocation { get; set; }
        public IAsyncCommand EditCartItems { get; set; }
        public MyCartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToPaymentMode = new AsyncCommand(NavigateToPaymentModePage);
            NavigateToPromoCode = new AsyncCommand(NavigateToPromoCodePage);

            NavigateToPromoCode1 = new AsyncCommand(NavigateToPromoCode1Execute);
            GoToAddItemCmd = new AsyncCommand(GoToAddItemCmdExecute);
           // NavigateToRateAndReviews = new AsyncCommand(NavigateToRateAndReviewsPage);

           // NavigateToRateAndReviews = new AsyncCommand(NavigateToRateAndReviewsPage);
            NavigateToChangeLocation = new AsyncCommand(NavigateToChangeLocationPage);
            EditCartItems = new AsyncCommand(EditCartItemsPage);
        }

        private async Task EditCartItemsPage()
        {
            await _navigationService.NavigateBackAsync();
            await App.Locator.ItemsDetailAddCartPopup.InitilizeData();
            ItemsDetailAddCartPopupViewModel CartPopupp = new ItemsDetailAddCartPopupViewModel(_navigationService);
            await _navigationService.NavigateToPopupAsync<ItemsDetailAddCartPopupViewModel>(true, CartPopupp);
        }

        private async Task NavigateToChangeLocationPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ChangeLocationPageViewModel))
            {
                await App.Locator.ChangeLocationPage.InitilizeData();
                await _navigationService.NavigateToAsync<ChangeLocationPageViewModel>();
            }

        }

        //private async Task NavigateToRateAndReviewsPage()
        //{
        //    if (_navigationService.GetCurrentPageViewModel() != typeof(RatingAndReviewsPageViewModel))
        //    {
        //        await App.Locator.ApplyPromoCodePage.InitilizeData();
        //        await _navigationService.NavigateToAsync<RatingAndReviewsPageViewModel>();
        //    }
        //}

        private async Task NavigateToPromoCode1Execute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OfferAndPromocodePageViewModel))
            {
                await App.Locator.OfferAndPromocodePage.InitilizeData();
                await _navigationService.NavigateToAsync<OfferAndPromocodePageViewModel>();
            }
        }
        private async Task GoToAddItemCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(DuraEatsItemDetailsPageViewModel))
            {
                await App.Locator.DuraEatsItemDetailsPage.InitilizeData();
                await _navigationService.NavigateToAsync<DuraEatsItemDetailsPageViewModel>();
            }
        }
        private async Task NavigateToPromoCodePage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ApplyPromoCodePageViewModel))
            {
                await App.Locator.ApplyPromoCodePage.InitilizeData();
                await _navigationService.NavigateToAsync<ApplyPromoCodePageViewModel>();
            }
        }

        internal async Task InitilizeData()
        {
            //throw new NotImplementedException();
        }

        private async Task NavigateToPaymentModePage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(PaymentModePageViewModel))
            {
                await App.Locator.MyCartPage.InitilizeData();
                await _navigationService.NavigateToAsync<PaymentModePageViewModel>();
            }
        }
    }
}

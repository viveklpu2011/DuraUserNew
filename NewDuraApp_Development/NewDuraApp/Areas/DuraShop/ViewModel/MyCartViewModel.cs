
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraShop.ViewModel
{
  public class MyCartViewModel: AppBaseViewModel
    {
        INavigationService _navigationService;

        public IAsyncCommand GoToPaymentCmd { get; set; }
        public IAsyncCommand GoToOfferCmd { get; set; }
        public IAsyncCommand GoToEditAddressCmd { get; set; }
        private ObservableCollection<MyCartModel> _cartList;

        public ObservableCollection<MyCartModel> CartList
        {
            get { return _cartList; }
            set { _cartList = value; OnPropertyChanged(); }
        }
        public MyCartViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToPaymentCmd = new AsyncCommand(GoToPaymentCmdExecute, allowsMultipleExecutions: false);
            GoToOfferCmd = new AsyncCommand(GoToOfferCmdExecute, allowsMultipleExecutions: false);
            GoToEditAddressCmd = new AsyncCommand(GoToEditAddressCmdExecute, allowsMultipleExecutions: false);
            CartList = GetCartList();
            //Quantity = 0;
        }

        private async Task GoToEditAddressCmdExecute()
        {
            if(_navigationService.GetCurrentPageViewModel() != typeof(ChangeLocationPageViewModel))
            {
                //await App.Locator.OfferAndPromocodePage.InitilizeData();
                await _navigationService.NavigateToAsync<ChangeLocationPageViewModel>();
               
            }
        }
        private async Task GoToOfferCmdExecute()
        {
            if(_navigationService.GetCurrentPageViewModel() != typeof(OfferAndPromocodePageViewModel))
            {
                await App.Locator.OfferAndPromocodePage.InitilizeData();
                await _navigationService.NavigateToAsync<OfferAndPromocodePageViewModel>();
               
            }
        }
        private async Task   GoToPaymentCmdExecute()
        {
            if(_navigationService.GetCurrentPageViewModel() != typeof(PaymentViewModel))
            {
                await _navigationService.NavigateToAsync<PaymentViewModel>();
                MessagingCenter.Send<MyCartViewModel>(this, "FromMyCart");
            }
        }

        private ObservableCollection<MyCartModel> GetCartList()
        {
            return new ObservableCollection<MyCartModel>()
            {
                new MyCartModel(){ProductDescription="Men's Fit T-Shirt",Quantity=0}
            };
        }


        //public ICommand GoToPaymentCmd => new Command(async (obj) =>
        //{
        //    await RichNavigation.PushAsync(new Payment(), typeof(Payment));
        //    MessagingCenter.Send<MyCartViewModel>(this, "FromMyCart");
        //});
    }
}

using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.ResponseModels;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels
{
    public class RechargeSuccessfullPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToWallet { get; set; }
        private GetSaveWalletAmountResponseModel _getSaveWalletAmount;

        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(nameof(Amount)); }
        }

        public GetSaveWalletAmountResponseModel GetSaveWalletAmount
        {
            get { return _getSaveWalletAmount; }
            set { _getSaveWalletAmount = value; OnPropertyChanged(nameof(GetSaveWalletAmount)); }
        }

        public RechargeSuccessfullPopupPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToWallet = new AsyncCommand(NavigateToWalletPage);
            InitilizeData();
        }

        private async Task InitilizeData()
        {
            GetSaveWalletAmount = new GetSaveWalletAmountResponseModel();
            Amount = App.Locator.RechargeToppupPopupPage.WalletAmount.AmountWithCurrency;//.GetSaveWalletAmount;
            var data1 = App.Locator.TopupAmuntPopupPage.WalletAmount;
            var data = GetSaveWalletAmount;
        }

        private async Task NavigateToWalletPage()
        {
            await _navigationService.ClosePopupsAsync();
            if (App.Locator.CurrentUser.SendWay == SendInvite.MENU.ToString())
            {
                await _navigationService.RemoveLastFromBackStackAsyncForResetPassword();
            }
            else
            {
                await _navigationService.NavigateBackAsync();
                await _navigationService.NavigateBackAsync();
            }
            await App.Locator.Payment.InitilizeData();
        }
    }
}

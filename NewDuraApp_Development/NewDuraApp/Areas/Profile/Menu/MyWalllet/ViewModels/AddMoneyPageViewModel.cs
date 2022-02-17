using System.Threading.Tasks;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.GCash.ViewModel;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels
{
    public class AddMoneyPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToRechargeTopup { get; set; }
        private IUserCoreService _userCoreService;

        private WalletAmountModel _walletAmount;
        public WalletAmountModel WalletAmount
        {
            get { return _walletAmount; }
            set { _walletAmount = value; OnPropertyChanged(nameof(WalletAmount)); }
        }

        public AddMoneyPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            NavigateToRechargeTopup = new AsyncCommand(NavigateToRechargeTopupPage);
        }

        private async Task NavigateToRechargeTopupPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(GCashPaymentPageViewModel))
            {
                await _navigationService.NavigateToAsync<GCashPaymentPageViewModel>();
                await App.Locator.GCashPaymentPage.InitilizeData();
            }
        }

        public async Task InitilizeData()
        {
            App.Locator.RechargeToppupPopupPage.WalletAmount = WalletAmount;
        }
    }
}


using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.GCash.ViewModel
{
    public class PaymentPageWebviewViewModel : AppBaseViewModel
    {
        private INavigationService _navigationService;
        public IAsyncCommand BackCommand { get; set; }
        private string _url;
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged(nameof(URL));
            }
        }

        public PaymentPageWebviewViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new AsyncCommand(BackCommandExecute, allowsMultipleExecutions: false);
        }
        private async Task BackCommandExecute()
        {
            await _navigationService.NavigateBackAsync();
            //await App.Locator.GCashPaymentPage.GcashMakePayment();
        }
        internal async Task InitilizeData(string url = "")
        {
            //ShowToast("Please use Back button of app , not hardware back for your successfully transfer");
            URL = url;
        }

    }
}

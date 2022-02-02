using System.Threading.Tasks;
using DuraApp.Core.Models.ResponseModels;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.Popup.ViewModel
{
    public class FoundDriverPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToOrderDetailsCmd { get; set; }
        public IAsyncCommand CancelRideCommand { get; set; }
        private GetDriverDetailsModel driverDetails;
        public GetDriverDetailsModel DriverDetails
        {
            get { return driverDetails; }
            set
            {
                driverDetails = value;
                OnPropertyChanged(nameof(DriverDetails));
            }
        }

        private byte[] _productImage;
        public byte[] ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }

        public FoundDriverPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToOrderDetailsCmd = new AsyncCommand(GoToOrderDetailsCmdExecute, allowsMultipleExecutions: false);
            CancelRideCommand = new AsyncCommand(CancelRideCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task CancelRideCommandExecute()
        {
            await PopupNavigation.PopAsync();
            await App.Locator.CancelRideReasonPopup.InitilizeData(DriverDetails);
            await PopupNavigation.Instance.PushAsync(new CancelRideReasonPopup());
        }

        private async Task GoToOrderDetailsCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetailsViewModel))
            {
                App.Locator.FindingDriver.SearchText = AppResources.Search_for_the_driver;
                App.Locator.FindingDriver.IsVisibleSearchImage = false;
                App.Locator.FindingDriver.CancelButtonText = AppResources.Search;
                await PopupNavigation.PopAsync();
                await _navigationService.NavigateToAsync<OrderDetailsViewModel>();
                await App.Locator.OrderDetails.InitilizeData();
            }
        }

        internal async Task InitilizeData(GetDriverDetailsModel getDriverDetailsModel = null)
        {
            if (getDriverDetailsModel != null)
            {
                DriverDetails = getDriverDetailsModel;
            }
        }
    }
}

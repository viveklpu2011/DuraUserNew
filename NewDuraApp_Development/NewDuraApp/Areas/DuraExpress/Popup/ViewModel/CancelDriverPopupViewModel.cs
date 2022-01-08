using System.Threading.Tasks;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.DuraExpress.Popup.ViewModel
{
    public class CancelDriverPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToDuraExpressCmd { get; set; }
        public CancelDriverPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToDuraExpressCmd = new AsyncCommand(GoToDuraExpressCmdExecute, allowsMultipleExecutions: false);

        }

        private async Task GoToDuraExpressCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(DuraExpresViewModel))
            {
                await _navigationService.NavigateToAsync<DuraExpresViewModel>();
                await PopupNavigation.PopAsync();
            }
        }

        //public ICommand GoToDuraExpressCmd => new Command(async (obj) =>
        //{
        //    await RichNavigation.PushAsync(new DuraExpress(), typeof(DuraExpress));
        //    await PopupNavigation.PopAsync();
        //});
    }
}

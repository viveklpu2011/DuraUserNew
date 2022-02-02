using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class GetStartedPageViewModel : AppBaseViewModel
    {
        private ObservableCollection<GetStartedCaroselModel> _welcomeGoThroughList;
        public IAsyncCommand SkipCommand { get; set; }
        INavigationService _navigationService;

        public ObservableCollection<GetStartedCaroselModel> WelcomeGoThroughList
        {
            get { return _welcomeGoThroughList; }
            set { _welcomeGoThroughList = value; OnPropertyChanged(nameof(WelcomeGoThroughList)); }
        }

        public GetStartedPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SkipCommand = new AsyncCommand(SkipCommandExecute);
        }

        private async Task SkipCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(LandingPageViewModel))
            {
                await _navigationService.NavigateToAsync<LandingPageViewModel>();
            }
        }

        public async Task InitilizeData()
        {
            WelcomeGoThroughList = new ObservableCollection<GetStartedCaroselModel>
            {
                new GetStartedCaroselModel
                {
                   Id=0,
                   Descripption=AppResources.DuraExpressDesc,
                   Heading=AppResources.DuraExpressHeading,
                   ImageName=ImageHelper.GetImageNameFromResource("dura_express_drive.png")
                },
                new GetStartedCaroselModel
                {
                   Id=1,
                   Descripption=AppResources.DuraEatsDesc,
                   Heading=AppResources.DuraEatsHeading,
                   ImageName=ImageHelper.GetImageNameFromResource("dura_eats_graphic.png")
                }
                ,
                new GetStartedCaroselModel
                {
                   Id=2,
                   Descripption=AppResources.DuraShopDesc,
                   Heading=AppResources.DuraShopHeading,
                   ImageName=ImageHelper.GetImageNameFromResource("dura_shop_graphic.png")
                }
            };
        }
    }
}

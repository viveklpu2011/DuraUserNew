using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.Home.ViewModels
{
    public class DuraEatsHomePageViewModel : AppBaseViewModel
    {
        private ObservableCollection<DuraEatsBannerModel> _duraEatsBannerList;
        private ObservableCollection<DuraEatsMenuModel> _menuTypeList;
        private ObservableCollection<DuraEatsHomePageRestaurentModel> _restaurentList;
        public IAsyncCommand SkipCommand { get; set; }
        public IAsyncCommand GoToMyCartCmd { get; set; }
        INavigationService _navigationService;
        public IAsyncCommand<DuraEatsHomePageRestaurentModel> RestaurentDetails { get; set; }
        public ObservableCollection<DuraEatsBannerModel> DuraEatsBannerList
        {
            get { return _duraEatsBannerList; }
            set { _duraEatsBannerList = value; OnPropertyChanged(nameof(DuraEatsBannerList)); }
        }
        public ObservableCollection<DuraEatsHomePageRestaurentModel> RestaurentList
        {
            get { return _restaurentList; }
            set { _restaurentList = value; OnPropertyChanged(nameof(RestaurentList)); }
        }
        public ObservableCollection<DuraEatsMenuModel> MenuTypeList
        {
            get { return _menuTypeList; }
            set { _menuTypeList = value; OnPropertyChanged(nameof(MenuTypeList)); }
        }
        public DuraEatsHomePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SkipCommand = new AsyncCommand(SkipCommandExecute);
            GoToMyCartCmd = new AsyncCommand(GoToMyCartCmdExecute);
            RestaurentDetails = new AsyncCommand<DuraEatsHomePageRestaurentModel>(SelectedRestaurent);
        }

        private async Task SelectedRestaurent(DuraEatsHomePageRestaurentModel arg)
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(DuraEatsItemDetailsPageViewModel))
            {
                App.Locator.DuraEatsItemDetailsPage.RestaurentDetails = arg;
                await App.Locator.DuraEatsItemDetailsPage.InitilizeData();
                await _navigationService.NavigateToAsync<DuraEatsItemDetailsPageViewModel>();
            }
        }
        private async Task GoToMyCartCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(MyCartPageViewModel))
            {                
                await _navigationService.NavigateToAsync<MyCartPageViewModel>();
            }
        }

        public async Task InitilizeData()
        {
            DuraEatsBannerList = new ObservableCollection<DuraEatsBannerModel>
            {
                new DuraEatsBannerModel
                {
                   Id=0,
                   ImageName=ImageHelper.GetImageNameFromResource("banner_two.png")
                },
                new DuraEatsBannerModel
                {
                   Id=1,
                   ImageName=ImageHelper.GetImageNameFromResource("banner_two.png")
                }
                ,
                new DuraEatsBannerModel
                {
                   Id=2,
                   ImageName=ImageHelper.GetImageNameFromResource("banner_two.png")
                }
            };

            MenuTypeList = new ObservableCollection<DuraEatsMenuModel>
            {
                new DuraEatsMenuModel
                {
                   Id=0,
                   MenuImage=ImageHelper.GetImageNameFromResource("mexican.png"),
                   MenuName="Maxican"
                },
                new DuraEatsMenuModel
                {
                   Id=1,
                   MenuImage=ImageHelper.GetImageNameFromResource("pizza.png"),
                   MenuName="Pizza"
                },
                new DuraEatsMenuModel
                {
                   Id=2,
                   MenuImage=ImageHelper.GetImageNameFromResource("chinese.png"),
                   MenuName="Chinese"
                },
                new DuraEatsMenuModel
                {
                   Id=0,
                   MenuImage=ImageHelper.GetImageNameFromResource("indian.png"),
                   MenuName="Indian"
                },
                new DuraEatsMenuModel
                {
                   Id=1,
                   MenuImage=ImageHelper.GetImageNameFromResource("burger.png"),
                   MenuName="Burger"
                }
                ,
                new DuraEatsMenuModel
                {
                   Id=2,
                   MenuImage=ImageHelper.GetImageNameFromResource("pizza.png"),
                   MenuName="Pizza"
                }
            };

            RestaurentList = new ObservableCollection<DuraEatsHomePageRestaurentModel>
            {
                new DuraEatsHomePageRestaurentModel
                {
                   Id=0,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_one.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                },
                new DuraEatsHomePageRestaurentModel
                {
                   Id=1,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_two.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                }
                ,
                new DuraEatsHomePageRestaurentModel
                {
                   Id=2,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_three.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                },
                new DuraEatsHomePageRestaurentModel
                {
                   Id=3,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_four.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                },
                new DuraEatsHomePageRestaurentModel
                {
                   Id=4,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_one.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                },
                new DuraEatsHomePageRestaurentModel
                {
                   Id=5,
                   RestarentImage=ImageHelper.GetImageNameFromResource("resto_two.png"),
                   RestaurentName="Le Paris Cafe",
                   RestaurentLocation="1976 Capt. M. Reyes, Makati",
                   RestaurentDistance="3.5 km away"
                }
            };
        }
        private async Task SkipCommandExecute()
        {
            //throw new NotImplementedException();
        }
    }
}

using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NewDuraApp.Areas.DuraShop.ViewModel
{
    public class DuraShopViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToFeaturedCmd { get; set; }
        public IAsyncCommand GoToMyCartCmd { get; set; }
        public IAsyncCommand GoToItemDetailsCmd { get; set; }
        public IAsyncCommand GoToSearchCmd { get; set; }

        private ObservableCollection<DuraShopModel> _bannerList;
        public ObservableCollection<DuraShopModel> BannerList
        {
            get { return _bannerList; }
            set { _bannerList = value; OnPropertyChanged(); }
        }

        public DuraShopViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToFeaturedCmd = new AsyncCommand(GoToFeaturedCmdExecute);
            GoToMyCartCmd = new AsyncCommand(GoToMyCartCmdExecute);
            GoToItemDetailsCmd = new AsyncCommand(GoToItemDetailsCmdExecute);
            GoToSearchCmd = new AsyncCommand(GoToSearchCmdExecute);
            BannerList = GetBannerList();
            ProductList = GetProductList();
            NewProductList = NewGetProductList();
        }

        private async Task GoToSearchCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(SearchScreenViewModel))
            {
                await _navigationService.NavigateToAsync<SearchScreenViewModel>();
            }
        }

        private async Task GoToItemDetailsCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ItemDetailsViewModel))
            {
                await _navigationService.NavigateToAsync<ItemDetailsViewModel>();
            }
        }

        private async Task GoToMyCartCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(MyCartViewModel))
            {
                await _navigationService.NavigateToAsync<MyCartViewModel>();
            }
        }

        private async Task GoToFeaturedCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(FeaturedViewModel))
            {
                await _navigationService.NavigateToAsync<FeaturedViewModel>();
            }
        }

        private ObservableCollection<DuraShopModel> GetBannerList()
        {
            return new ObservableCollection<DuraShopModel>()
            {
                new DuraShopModel(){BannerImage=ImageHelper.GetImageNameFromResource("Rectangle_2654.png")},
                new DuraShopModel(){BannerImage=ImageHelper.GetImageNameFromResource("banner_two.png")},
                new DuraShopModel(){BannerImage=ImageHelper.GetImageNameFromResource("Rectangle_2654.png")},
            };
        }

        //public ObservableCollection<DuraShopModel> CalendarList { get; set; } = new ObservableCollection<DuraShopModel>
        //{
        //    new DuraShopModel{DayName="essential_goods", Date="Essentials Goods", ColorSelected=Color.WhiteSmoke,TextColor=Color.Black,},
        //    new DuraShopModel{DayName="appliances", Date="Appliances", ColorSelected=Color.White,TextColor=Color.FromHex("#211E66")},
        //    new DuraShopModel{DayName="gadgets", Date="Gadgets", ColorSelected=Color.White,TextColor=Color.FromHex("#211E66")},

        //};
        //public ICommand SelectCategoryCmd1 => new Command((obj) =>
        //{
        //    try
        //    {
        //        var item = obj as DuraShopModel;
        //        var items = CalendarList.Where(x => x.ColorSelected == Color.WhiteSmoke);
        //        foreach (var i in items)
        //        {
        //            i.ColorSelected = Color.White;
        //            i.TextColor = Color.FromHex("#211E66");
        //        }
        //        item.ColorSelected = Color.WhiteSmoke;
        //        item.TextColor = Color.Black;
        //    }
        //    catch (Exception ex)
        //    {
        //        UserDialogs.Instance.Alert(ex.Message);
        //    }
        //});

        private ObservableCollection<DuraShopModel> _newproductList;
        public ObservableCollection<DuraShopModel> NewProductList
        {
            get { return _newproductList; }
            set { _newproductList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DuraShopModel> NewGetProductList()
        {
            return new ObservableCollection<DuraShopModel>()
            {
                new DuraShopModel(){NewProductImage=ImageHelper.GetImageNameFromResource("watch.png"), NewProductName="Men's &amp; Boy's Watch",NewProductPrice="₱ 60"},
                new DuraShopModel(){NewProductImage=ImageHelper.GetImageNameFromResource("refrigerator.png"), NewProductName="Single Door Refrigerter",NewProductPrice="₱ 3500"},
                new DuraShopModel(){NewProductImage=ImageHelper.GetImageNameFromResource("microwave.png"), NewProductName="Grill Microwave Overs",NewProductPrice="₱ 1260"},
                new DuraShopModel(){NewProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png"), NewProductName="Men's Fit T-Shirt",NewProductPrice="₱ 200"},
            };
        }

        private ObservableCollection<DuraShopModel> _productList;
        public ObservableCollection<DuraShopModel> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DuraShopModel> GetProductList()
        {
            return new ObservableCollection<DuraShopModel>()
            {
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png"), ProductName="Men's Fit T-shirt",ProductPrice="₱ 200"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("microwave.png"), ProductName="Grill Microwave Overs",ProductPrice="₱ 1260"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("refrigerator.png"), ProductName="Dishwashed",ProductPrice="₱ 160"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png"), ProductName="Men's Fit T-Shirt",ProductPrice="₱ 200"},
            };
        }


        //public ICommand GoToFeaturedCmd => new Command(async (obj) =>
        //{
        //    await RichNavigation.PushAsync(new Featured(), typeof(Featured));
        //});
    }
}

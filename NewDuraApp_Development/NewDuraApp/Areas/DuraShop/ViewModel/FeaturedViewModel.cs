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
    public class FeaturedViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand ItemDetailsCmd { get; set; }
        public IAsyncCommand GoToSearchCmd { get; set; }

        public FeaturedViewModel(INavigationService navigationService)
        {
            ItemDetailsCmd = new AsyncCommand(ItemDetailsCmdExecute);
            GoToSearchCmd = new AsyncCommand(GoToSearchCmdExecute);
            _navigationService = navigationService;
            ProductList = GetProductList();
        }

        private async Task ItemDetailsCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ItemDetailsViewModel))
            {
                await _navigationService.NavigateToAsync<ItemDetailsViewModel>();
            }
        }

        private async Task GoToSearchCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(SearchScreenViewModel))
            {
                await _navigationService.NavigateToAsync<SearchScreenViewModel>();
            }
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
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("watch.png"), ProductName="Men's &amp; Boy's Watch",ProductPrice="₱ 200"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png"), ProductName="Men's Fit T-shirt",ProductPrice="₱ 200"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("microwave.png"), ProductName="Grill Microwave Overs",ProductPrice="₱ 1260"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("refrigerator.png"), ProductName="Dishwashed",ProductPrice="₱ 160"},
                new DuraShopModel(){ProductImage=ImageHelper.GetImageNameFromResource("watch.png"), ProductName="Men's & Boys Watch",ProductPrice="₱ 60"},
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels
{
    public class ItemsDetailAddCartPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand ClosePopup { get; set; }
        public IAsyncCommand NavigateToCart { get; set; }
        private DuraEatsItemDetailsFoodList _duraEatsFoodMenuDetails;
        private ObservableCollection<ItemDetailsAddCartAddOnsModel> _itemDetailsAddCartAddOnsList;
        public ItemsDetailAddCartPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ClosePopup = new AsyncCommand(ClosePopUpcommand);
            NavigateToCart = new AsyncCommand(NavigateToCartPage);
            InitilizeData();
        }

        private async Task NavigateToCartPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(MyCartPageViewModel))
            {
                // App.Locator.DuraEatsItemDetailsPage.RestaurentDetails = arg;
                await _navigationService.ClosePopupsAsync();
                await App.Locator.MyCartPage.InitilizeData();
                await _navigationService.NavigateToAsync<MyCartPageViewModel>();
            }
        }

        private async Task ClosePopUpcommand()
        {
            await _navigationService.ClosePopupsAsync();
        }

        public DuraEatsItemDetailsFoodList DuraEatsFoodMenuDetails
        {
            get { return _duraEatsFoodMenuDetails; }
            set { _duraEatsFoodMenuDetails = value; OnPropertyChanged(nameof(DuraEatsFoodMenuDetails)); }
        }
        public ObservableCollection<ItemDetailsAddCartAddOnsModel> ItemDetailsAddCartAddOnsList
        {
            get { return _itemDetailsAddCartAddOnsList; }
            set { _itemDetailsAddCartAddOnsList = value; OnPropertyChanged(nameof(ItemDetailsAddCartAddOnsList)); }
        }
        public async Task InitilizeData()
        {
            var data = DuraEatsFoodMenuDetails;

            ItemDetailsAddCartAddOnsList = new ObservableCollection<ItemDetailsAddCartAddOnsModel>
            {
                new ItemDetailsAddCartAddOnsModel()
                {
                    AddOnsItemId=0,
                    AddOnsItemHeading="Toppings",
                    AddOnsItemList=new List<AddOnsItems>()
                    {
                        new AddOnsItems()
                        {
                            AddOnsId=0,
                            AddOnsItemName="Extra Mint Sauce",
                            AddOnsPrice="P 12"
                        },
                        new AddOnsItems()
                        {
                            AddOnsId=1,
                            AddOnsItemName="Extra Viniger Onion",
                            AddOnsPrice="P 13"
                        },
                        new AddOnsItems()
                        {
                            AddOnsId=2,
                            AddOnsItemName="Extra Mayo",
                            AddOnsPrice="P 23"
                        }
                    }
                },
                new ItemDetailsAddCartAddOnsModel()
                {
                    AddOnsItemId=0,
                    AddOnsItemHeading="Size",
                    AddOnsItemList=new List<AddOnsItems>()
                    {
                        new AddOnsItems()
                        {
                            AddOnsId=0,
                            AddOnsItemName="Half",
                            AddOnsPrice="P 12"
                        },
                        new AddOnsItems()
                        {
                            AddOnsId=1,
                            AddOnsItemName="Full",
                            AddOnsPrice="P 13"
                        }
                    }
                }
            };
        }
    }
}

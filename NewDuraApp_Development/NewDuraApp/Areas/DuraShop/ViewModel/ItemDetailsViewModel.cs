using Acr.UserDialogs;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraShop.ViewModel
{
   public class ItemDetailsViewModel: AppBaseViewModel 
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToMyCartCmd { get; set; }
        private ObservableCollection<ProductItemModel> _productList;
        
        public ObservableCollection<ProductItemModel> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }
        public ItemDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToMyCartCmd = new AsyncCommand(GoToMyCartCmdExecute, allowsMultipleExecutions: false);
            ProductList = GetProductList();
            SelectColor = GetSelectColor();
            SelectSize = GetSelectSize();
        }

        private async Task GoToMyCartCmdExecute()
        {
            if(_navigationService.GetCurrentPageViewModel() != typeof(MyCartViewModel))
            {
                await _navigationService.NavigateToAsync<MyCartViewModel>();
            }
        }

        private ObservableCollection<ProductItemModel> GetProductList()
        {
            return new ObservableCollection<ProductItemModel>()
            {
                new ProductItemModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png")},
                new ProductItemModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png")},
                new ProductItemModel(){ProductImage=ImageHelper.GetImageNameFromResource("t_shirt.png")},
            };
        }
        private ObservableCollection<ProductItemModel> _selectColor;

        public ObservableCollection<ProductItemModel> SelectColor
        {
            get { return _selectColor; }
            set { _selectColor = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductItemModel> GetSelectColor()
        {
            return new ObservableCollection<ProductItemModel>()
            {
                new ProductItemModel(){ColorSelected=Color.FromHex("#211E66"),TextColor=Color.FromHex("#FF4646")},
                new ProductItemModel(){ColorSelected=Color.White,TextColor=Color.FromHex("#302F2F")},
                new ProductItemModel(){ColorSelected=Color.White,TextColor=Color.FromHex("#0044FF")},
            };
        }
        public ICommand SelectCategoryCmd1 => new MvvmHelpers.Commands.Command((obj) =>
        {
            try
            {
                var item = obj as ProductItemModel;
                var items = SelectColor.Where(x => x.ColorSelected == Color.FromHex("#211E66"));
                foreach (var i in items)
                {
                    i.ColorSelected = Color.White;
                    // i.TextColor = Color.FromHex("#211E66");
                }
                item.ColorSelected = Color.FromHex("#211E66");
                // item.TextColor = Color.Black;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        });



        private ObservableCollection<ProductItemModel> GetSelectSize()
        {
            return new ObservableCollection<ProductItemModel>()
            {
                new ProductItemModel(){ColorSelected=Color.FromHex("#211E66"),SizeText="S"},
                new ProductItemModel(){ColorSelected=Color.White,SizeText="M"},
                new ProductItemModel(){ColorSelected=Color.White,SizeText="L"},
            };
        }


        private ObservableCollection<ProductItemModel> _selectSize;

        public ObservableCollection<ProductItemModel> SelectSize
        {
            get { return _selectSize; }
            set { _selectSize = value; OnPropertyChanged(); }
        }

        public ICommand SelectSizeCmd => new MvvmHelpers.Commands.Command((obj) =>
        {
            try
            {
                var item = obj as ProductItemModel;
                var items = SelectSize.Where(x => x.ColorSelected == Color.FromHex("#211E66"));
                foreach (var i in items)
                {
                    i.ColorSelected = Color.White;
                    // i.TextColor = Color.FromHex("#211E66");
                }
                item.ColorSelected = Color.FromHex("#211E66");
                // item.TextColor = Color.Black;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
        });
        //public ICommand GoToMyCartCmd => new Command(async (obj) =>
        //{
        //    await RichNavigation.PushAsync(new MyCart(), typeof(MyCart));
        //});
    }
}

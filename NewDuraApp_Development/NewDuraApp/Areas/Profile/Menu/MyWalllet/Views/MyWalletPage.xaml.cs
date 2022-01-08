using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.Views
{
    public partial class MyWalletPage : ContentPage
    {
        public MyWalletPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await App.Locator.MyWalletPage.InitilizeData();
        }
    }
}

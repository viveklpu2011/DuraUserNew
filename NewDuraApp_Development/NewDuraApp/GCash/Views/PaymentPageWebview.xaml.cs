using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.GCash.Views
{
    public partial class PaymentPageWebview : ContentPage
    {
        public PaymentPageWebview()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await progress.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }
    }
}

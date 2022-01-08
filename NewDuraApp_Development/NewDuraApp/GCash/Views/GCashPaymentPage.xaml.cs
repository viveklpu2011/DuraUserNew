using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.GCash.Views
{
    public partial class GCashPaymentPage : ContentPage
    {
        public GCashPaymentPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.Locator.GCashPaymentPage.IsAppearing)
            {
                App.Locator.GCashPaymentPage.IsAppearing = false;
                await App.Locator.GCashPaymentPage.GcashMakePayment();
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

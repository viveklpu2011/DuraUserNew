using System;
using System.Collections.Generic;
using DuraApp.Core.Helpers;
using NewDuraApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NewDuraApp.Areas.Common.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //await App.Locator.HomePage.CheckAccountIsVerified();
            App.Locator.DuraExpress.PickupLocationTextVisible = false;
            App.Locator.DuraExpress.PickupLocationText = string.Empty;
            SettingsExtension.PickupAddress = string.Empty;
            App.Locator.PickupSchedule.PickupScheduleRequest = null;
            App.Locator.PickupLocation.PickupScheduleRequest = null;
            App.Locator.WhereTo.PickupScheduleRequest = null;
            App.Locator.AddStopLocation.PickupScheduleRequest = null;
        }
    }
}

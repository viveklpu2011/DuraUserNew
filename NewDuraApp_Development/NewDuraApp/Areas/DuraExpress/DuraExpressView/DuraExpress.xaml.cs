using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuraExpress : ContentPage
    {
        public DuraExpress()
        {
            InitializeComponent();
           
            //lblPickupLocation.IsVisible = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (lblPickupLocation.IsVisible)
            {
                lblPickupLocation.Text = SettingsExtension.PickupAddress;
            }
            App.Locator.PickupLocation.UpdateLabelEvent += (value) =>
            {
                lblPickupLocation.IsVisible = true;
                lblPickupLocation.Text = Convert.ToString(value);
            };
        }
        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    lblPickupLocation.IsVisible = false;
        //    SettingsExtension.PickupAddress = string.Empty;

        //}
    }
}
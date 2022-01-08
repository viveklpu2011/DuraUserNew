
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.Popup.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupSchedulePopUp : PopupPage
    {
        public PickupSchedulePopUp()
        {
            InitializeComponent();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //PickDateLater.Focus();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Locator.PickupSchedule.ishowtoast = false;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Locator.PickupSchedule.ishowtoast = false;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}
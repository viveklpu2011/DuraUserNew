using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using NewDuraApp.SocialLogin.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMoreDetails : ContentPage
    {
        bool firstTime = true;
        static double tipamount = 0;
        public AddMoreDetails()
        {
            InitializeComponent();
            // AccoutType.SelectedIndex = 0;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            tipamount = 0;

        }
        private async void btnTake1_Clicked(object sender, EventArgs e)
        {
            //if (!CrossMedia.Current.IsPickPhotoSupported)
            //{
            //    await DisplayAlert("no upload", "picking a photo is not supported", "ok");
            //    return;
            //}

            //var file = await CrossMedia.Current.PickPhotoAsync();
            //if (file == null)
            //    return;

            //img_placeholder.Source = ImageSource.FromStream(() => file.GetStream());
        }

        void radioTen_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            string price = button?.Content.ToString().Remove(0, 1);

            App.Locator.AddMoreDetails.TipAmount = price;
            if (tipamount != Convert.ToDouble(price))
            {
                tipamount = Convert.ToDouble(price);
                var amount = App.Locator.SelectVehicle.TotalFare;
                if (App.Locator.AddMoreDetails.VerifyPromoCode != null)
                {
                    var discount = App.Locator.AddMoreDetails.VerifyPromoCode?.discount;
                    amount = Math.Round(App.Locator.SelectVehicle.TotalFare - (App.Locator.SelectVehicle.TotalFare * (Convert.ToDouble(discount) / 100)));
                }
                App.Locator.AddMoreDetails.TotalFare = amount + Convert.ToDouble(price);
                App.Locator.AddMoreDetails.TotalFinalFare = App.Locator.AddMoreDetails.TotalFare.ToString("N2");
                AppConstant.TipAmount = Convert.ToDouble(App.Locator.AddMoreDetails.TipAmount);
                AppConstant.TotalFinalFare = App.Locator.AddMoreDetails.TotalFinalFare;
            }
        }

        void CustomEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                if (e.NewTextValue.Contains(".") || e.NewTextValue.Contains("-") || e.NewTextValue.Contains(","))
                    return;

                var amount = App.Locator.SelectVehicle.TotalFare;
                if (App.Locator.AddMoreDetails.VerifyPromoCode != null)
                {
                    var discount = App.Locator.AddMoreDetails.VerifyPromoCode?.discount;
                    amount = Math.Round(App.Locator.SelectVehicle.TotalFare - (App.Locator.SelectVehicle.TotalFare * (Convert.ToDouble(discount) / 100)));
                }
                App.Locator.AddMoreDetails.TotalFare = amount + Convert.ToDouble(e.NewTextValue);
                App.Locator.AddMoreDetails.TotalFinalFare = App.Locator.AddMoreDetails.TotalFare.ToString("N2");
            }
            else
            {
                var amount = App.Locator.SelectVehicle.TotalFare;
                if (App.Locator.AddMoreDetails.VerifyPromoCode != null)
                {
                    var discount = App.Locator.AddMoreDetails.VerifyPromoCode?.discount;
                    amount = Math.Round(App.Locator.SelectVehicle.TotalFare - (App.Locator.SelectVehicle.TotalFare * (Convert.ToDouble(discount) / 100)));
                }

                App.Locator.AddMoreDetails.TotalFare = amount;
                App.Locator.AddMoreDetails.TotalFinalFare = App.Locator.AddMoreDetails.TotalFare.ToString("N2");
            }
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                AccoutType.Focus();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
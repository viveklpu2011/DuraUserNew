using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.PopupView.View
{
    public partial class PhoneVerificationPopupPage : PopupPage
    {
        public PhoneVerificationPopupPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                pickerCountryCode.Focus();
            }
            catch (Exception ex)
            {

            }

        }

    }
}

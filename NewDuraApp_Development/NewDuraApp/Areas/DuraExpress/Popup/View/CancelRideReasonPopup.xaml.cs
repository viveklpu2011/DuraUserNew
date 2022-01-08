using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.Popup.View
{
    public partial class CancelRideReasonPopup : PopupPage
    {
        public CancelRideReasonPopup()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.PopupViews.Views
{
    public partial class OTPPopupPage : PopupPage
    {
        public OTPPopupPage()
        {
            InitializeComponent();
        }
        void VerifyEntryText_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (sender != null)
            {
                var value = e.NewTextValue;
                var selectedEntry = (Entry)sender;
                switch (selectedEntry.AutomationId)
                {
                    case "1":
                        if (!string.IsNullOrEmpty(value))
                        {
                            //Pin1.Unfocus();
                            Pin2.Focus();
                            //Pin3.Unfocus();
                            //Pin4.Unfocus();
                        }
                        break;
                    case "2":
                        if (!string.IsNullOrEmpty(value))
                        {
                            // Pin1.Unfocus();
                            // Pin2.Unfocus();
                            Pin3.Focus();
                            // Pin4.Unfocus();
                        }
                        break;
                    case "3":
                        if (!string.IsNullOrEmpty(value))
                        {
                            //Pin1.Unfocus();
                            // Pin2.Unfocus();
                            // Pin3.Unfocus();
                            Pin4.Focus();
                        }
                        break;
                    case "4":
                        if (!string.IsNullOrEmpty(value))
                        {
                            Pin1.Unfocus();
                            Pin2.Unfocus();
                            Pin3.Unfocus();
                            Pin4.Unfocus();
                            //  NewPin1.Focus();
                        }
                        break;
                }

            }
        }
    }
}

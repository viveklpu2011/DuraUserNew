﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.Views
{
    public partial class ForgetPasswordPage : ContentPage
    {
        public ForgetPasswordPage()
        {
            InitializeComponent();
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

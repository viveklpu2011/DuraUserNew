﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.Common.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TermsConditionsPage : ContentPage
	{
		public TermsConditionsPage()
		{
			InitializeComponent();
			webView.Source = "https://duradriver.com/duradrive/terms-and-conditions";
		}
	}
}
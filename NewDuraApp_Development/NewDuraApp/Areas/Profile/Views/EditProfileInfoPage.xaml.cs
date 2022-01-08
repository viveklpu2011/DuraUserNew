using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Views
{
	public partial class EditProfileInfoPage : ContentPage
	{
		public EditProfileInfoPage()
		{
			InitializeComponent();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			App.Locator.EditProfileInfoPage.SelectedLocation = null;
		}
		void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
		{
			try {
				pickerCountryCode.Focus();
			} catch (Exception ex) {

			}

		}

		void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
		{
			try {
				pickLocation.Focus();
			} catch (Exception ex) { }
		}
	}
}

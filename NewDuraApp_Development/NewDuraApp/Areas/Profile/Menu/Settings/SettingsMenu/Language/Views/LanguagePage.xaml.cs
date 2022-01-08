using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.Views
{
	public partial class LanguagePage : ContentPage
	{
		public LanguagePage()
		{

			InitializeComponent();
		}

		void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
		{
		}

		void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
		{
			try {
				pickerLanguage.Focus();
			} catch (Exception ex) {

			}
		}
	}
}

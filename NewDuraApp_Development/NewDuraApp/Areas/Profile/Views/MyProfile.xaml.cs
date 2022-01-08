using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.Profile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyProfile : ContentPage
	{
		public MyProfile()
		{
			InitializeComponent();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await App.Locator.MyProfile.RefreshCommandExecute();

		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Models.ResponseModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectVehicle : ContentPage
	{
		public SelectVehicle()
		{
			InitializeComponent();
		}

		async void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
		{
			var cb = (CheckBox)sender;
			var item = (AdditionalServices)cb.BindingContext;
			await App.Locator.SelectVehicle.AdditionalServicesCommandExecute(item);
		}
		protected override void OnAppearing()
		{
			//AppConstant.ServiceList = new List<ServicesModel>();
		}
	}
}
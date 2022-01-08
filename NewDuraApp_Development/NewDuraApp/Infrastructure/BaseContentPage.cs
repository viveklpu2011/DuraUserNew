using NewDuraApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NewDuraApp.Infrastructure
{
	public class BaseContentPage : ContentPage
	{
		public BaseContentPage()
		{
			this.On<iOS>().SetUseSafeArea(true);
			Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
		}
		protected async override void OnAppearing()
		{
			await (this.BindingContext as AppBaseViewModel).OnPageAppearing();
			base.OnAppearing();
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}

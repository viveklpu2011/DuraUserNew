using Rg.Plugins.Popup.Pages;

namespace NewDuraApp.Areas.NoConnection.View
{
	public partial class NoInternetPopupPage : PopupPage
	{
		public NoInternetPopupPage()
		{
			InitializeComponent();
		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}

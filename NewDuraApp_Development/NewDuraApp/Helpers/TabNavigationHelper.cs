using Xamarin.Forms;

namespace NewDuraApp.Helpers
{
	public class TabNavigationHelper
	{
		public static void ForceFullyRedirectingTab(int tabIndex)
		{
			try {
				var navigationStack = (Application.Current.MainPage as Shell).Navigation.NavigationStack;

				for (var i = navigationStack.Count - 1; i > 0; i--) {
					var page = (Application.Current.MainPage as Shell).Navigation.NavigationStack[i];
					(Application.Current.MainPage as Shell).Navigation.RemovePage(page);
				}
				(Application.Current.MainPage as Shell).CurrentItem = (Application.Current.MainPage as Shell).CurrentItem.Items[tabIndex];
			} catch (System.Exception ex) {

			}
		}

	}

}

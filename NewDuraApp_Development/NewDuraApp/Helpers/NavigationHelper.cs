
using System;
using System.Linq;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Helpers
{
	public class NavigationHelper : AppBaseViewModel
	{
		public static bool CheckType(Type type)
		{
			if (App.Current.MainPage.Navigation.NavigationStack.Count > 0)
				return App.Current.MainPage.Navigation.NavigationStack.Last().GetType() != type;
			else
				return true;
		}
	}
}

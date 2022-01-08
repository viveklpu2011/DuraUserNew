using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderDetails : ContentPage
	{
		public OrderDetails()
		{
			NavigationPage.SetHasBackButton(this, false);
			InitializeComponent();

		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.Common.PopupView.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationSearchPopup : PopupPage
    {
        public LocationSearchPopup()
        {
            InitializeComponent();
        }


        async void MySearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!App.Locator.LocationSearchPopup.IsReadOnlyText)
            {
                if (!string.IsNullOrWhiteSpace(App.Locator.LocationSearchPopup.AddressText))
                {
                    await App.Locator.LocationSearchPopup.GetPlacesPredictionsAsync();
                }
                else
                {
                    App.Locator.LocationSearchPopup.IsVisibleListView = false;
                }
            }

        }
    }
}
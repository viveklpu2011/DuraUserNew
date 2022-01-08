using System;
using System.Collections.Generic;
using NewDuraApp.Models;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraEats.ItemDetails.Views
{
    public partial class DuraEatsItemDetailsPage : ContentPage
    {
        public DuraEatsItemDetailsPage()
        {
            InitializeComponent();
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            Frame button = (Frame)sender;
            StackLayout listViewItem = (StackLayout)button.Parent;
           // var label = listViewItem.Children[0] as DuraEatsItemDetailsFoodList;

            var selectedProduct = ((Frame)sender).BindingContext as DuraEatsItemDetailsFoodList;
            if (selectedProduct!=null)
            {
                await App.Locator.DuraEatsItemDetailsPage.GetFoodDetails(selectedProduct);
            }
            // String text = label.Text;

            //list.Remove(text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.ResponseModels;
using NewDuraApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.Orders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrders : ContentPage
    {
        public MyOrders()
        {
            InitializeComponent();
            App.Locator.MyOrders.IsOnLoad = true;

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Picker1.SelectedIndex = 0;
            await App.Locator.MyOrders.InitilizeDataMyOrder();
        }
        private async void BorderlessPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!App.Locator.MyOrders.IsOnLoad)
            {
                if (Picker1.SelectedIndex == 0)
                {
                    App.Locator.MyOrders.PageNo = 1;
                    App.Locator.MyOrders.OrderStatus = OrderStatusType.Pending;
                    await App.Locator.MyOrders.GetDuraExpressOrderNew(App.Locator.MyOrders.OrderStatus);
                }
                else if (Picker1.SelectedIndex == 1)
                {
                    App.Locator.MyOrders.PageNo = 1;
                    App.Locator.MyOrders.OrderStatus = OrderStatusType.Ongoing;
                    await App.Locator.MyOrders.GetDuraExpressOrderNew(App.Locator.MyOrders.OrderStatus);
                }
                else if (Picker1.SelectedIndex == 2)
                {
                    App.Locator.MyOrders.PageNo = 1;
                    App.Locator.MyOrders.OrderStatus = OrderStatusType.Completed;
                    await App.Locator.MyOrders.GetDuraExpressOrderNew(App.Locator.MyOrders.OrderStatus);
                }
                else
                {
                    App.Locator.MyOrders.PageNo = 1;
                    App.Locator.MyOrders.OrderStatus = OrderStatusType.Cancelled;
                    await App.Locator.MyOrders.GetDuraExpressOrderNew(App.Locator.MyOrders.OrderStatus);
                }
            }

        }
        //async void CompletedFrame_RemainingItemsThresholdReached(System.Object sender, System.EventArgs e)
        //{
        //	if (IsBusy) return;
        //	if (Picker1.SelectedIndex == 0) {
        //		await App.Locator.MyOrders.GetDuraExpressOrder(OrderStatusType.Pending, ++App.Locator.MyOrders.PageNo);
        //	} else if (Picker1.SelectedIndex == 1) {
        //		await App.Locator.MyOrders.GetDuraExpressOrder(OrderStatusType.Ongoing, ++App.Locator.MyOrders.PageNo);
        //	} else if (Picker1.SelectedIndex == 2) {
        //		await App.Locator.MyOrders.GetDuraExpressOrder(OrderStatusType.Completed, ++App.Locator.MyOrders.PageNo);
        //	} else {
        //		await App.Locator.MyOrders.GetDuraExpressOrder(OrderStatusType.Cancelled, ++App.Locator.MyOrders.PageNo);
        //		IsBusy = true;
        //	}
        //}

    }
}
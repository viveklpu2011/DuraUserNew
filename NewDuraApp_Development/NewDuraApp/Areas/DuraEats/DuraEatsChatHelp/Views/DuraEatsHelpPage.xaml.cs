using System;
using System.Collections.Generic;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.Views
{
    public partial class DuraEatsHelpPage : ContentPage
    {
        public DuraEatsHelpPage()
        {
            InitializeComponent();
        }
        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as DuraEatsHelpPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //while (vm.DelayedMessages.Count > 0)
                        //{
                        //    vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        //}
                        //vm.ShowScrollTap = false;
                        //vm.LastMessageVisible = true;
                        //vm.PendingMessageCount = 0;
                        //ChatList?.ScrollToFirst();
                    });


                }

            }
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            //chatInput.UnFocusEntry();
        }
    }
}

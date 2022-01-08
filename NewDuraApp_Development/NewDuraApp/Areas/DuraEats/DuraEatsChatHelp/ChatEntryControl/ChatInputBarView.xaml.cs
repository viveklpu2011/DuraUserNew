using System;
using System.Collections.Generic;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ChatEntryControl
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            (this.Parent.Parent.BindingContext as DuraEatsHelpPageViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }

    }
}

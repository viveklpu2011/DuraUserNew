using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel
{
    public class DuraEatsHelpPageViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;

        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }

        public Queue<Message> DelayedMessages { get; set; } = new Queue<Message>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public IAsyncCommand OnSendCommand { get; set; }
        public IAsyncCommand<Message> MessageAppearingCommand { get; set; }
        public IAsyncCommand<Message> MessageDisappearingCommand { get; set; }

        public DuraEatsHelpPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        internal async Task InitilizeData()
        {
            Messages.Insert(0, new Message() { Text = "Hi" });
            Messages.Insert(0, new Message() { Text = "How are you?", User = App.User });
            Messages.Insert(0, new Message() { Text = "What's new?" });
            Messages.Insert(0, new Message() { Text = "How is your family", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your dog?", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your cat?", User = App.User });
            Messages.Insert(0, new Message() { Text = "How is your sister?" });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem", User = App.User });
            Messages.Insert(0, new Message() { Text = "Hugs and Kisses", User = App.User });
            Messages.Insert(0, new Message() { Text = "When we are going to meet?" });
            Messages.Insert(0, new Message() { Text = "I want to buy a laptop" });
            Messages.Insert(0, new Message() { Text = "Where I can find a good one?" });
            Messages.Insert(0, new Message() { Text = "Also I'm testing this chat" });
            Messages.Insert(0, new Message() { Text = "Oh My God!" });
            Messages.Insert(0, new Message() { Text = " No Problem" });
            Messages.Insert(0, new Message() { Text = "Hugs and KissesHugs and KissesHugs and KissesHugs and Kisses Hugs and Kisses Hugs and Kisses" });

            MessageAppearingCommand = new AsyncCommand<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new AsyncCommand<Message>(OnMessageDisappearing);

            //OnSendCommand = new Command(() =>
            //{
            //    if (!string.IsNullOrEmpty(TextToSend))
            //    {
            //        Messages.Insert(0, new Message() { Text = TextToSend, User = App.User });
            //        TextToSend = string.Empty;
            //    }

            //});

            //Code to simulate reveing a new message procces
            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    if (LastMessageVisible)
            //    {
            //        Messages.Insert(0, new Message() { Text = "New message test", User = "Mario" });
            //    }
            //    else
            //    {
            //        DelayedMessages.Enqueue(new Message() { Text = "New message test", User = "Mario" });
            //        PendingMessageCount++;
            //    }
            //    return true;
            //});
        }

        async Task OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        async Task OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }


    }
}

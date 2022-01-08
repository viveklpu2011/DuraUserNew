using System;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewCells.Views;
using NewDuraApp.Models;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewCells.TemplateSeletors
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncommingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;


            return (messageVm.User == App.User) ? incomingDataTemplate : outgoingDataTemplate;
        }

    }
}

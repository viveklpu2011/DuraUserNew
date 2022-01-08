using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.Popup.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoundDriverPopup : PopupPage
    {
        public FoundDriverPopup()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
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
    public partial class TrackOrder : ContentPage
    {
        public TrackOrder()
        {
            // NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

    }
}
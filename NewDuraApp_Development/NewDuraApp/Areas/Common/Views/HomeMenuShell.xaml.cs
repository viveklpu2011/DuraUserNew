using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMenuShell : Shell
    {
        private bool v;
        public static new HomeMenuShell Current;
        public HomeMenuShell()
        {
            InitializeComponent();

            Current = this;
            BindingContext = new AppBaseViewModel();
            //RegisterRoutes();
            //this.
        }
        public HomeMenuShell(bool from)
        {
            InitializeComponent();
            //RegisterRoutes(); 
            MyTabbar.CurrentItem = MyTabbar.Items[4];
        }

        public HomeMenuShell(bool from, bool v) : this(from)
        {
            InitializeComponent();
            BindingContext = new AppBaseViewModel();
            this.v = v;
            Shell.SetTabBarIsVisible(this, false);
        }
    }
}
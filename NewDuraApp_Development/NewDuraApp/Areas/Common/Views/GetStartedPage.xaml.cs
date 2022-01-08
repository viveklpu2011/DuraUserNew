using System;
using System.Collections.Generic;
using NewDuraApp.Interfaces;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.Views
{
    public partial class GetStartedPage  : IRootView, IMainView
    {
        public GetStartedPage()
        {
            InitializeComponent();
            //App.Locator.GetStartedPage.InitilizeData();
        }
    }

    
}

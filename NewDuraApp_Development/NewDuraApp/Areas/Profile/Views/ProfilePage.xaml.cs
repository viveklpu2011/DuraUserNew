using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        void btnname_Clicked(System.Object sender, System.EventArgs e)
        {
            if (sender == btnname)
            {
                App.Locator.EditProfileInfoPage.SetStackVisibility(btnname.ClassId);
            }
            else if (sender == btnPhone)
            {
                App.Locator.EditProfileInfoPage.SetStackVisibility(btnPhone.ClassId);
            }
            else if (sender == btncity)
            {
                App.Locator.EditProfileInfoPage.SetStackVisibility(btncity.ClassId);
            }
        }
        protected async override void OnAppearing()
        {
            await App.Locator.MyProfile.InitilizeData();
        }


        void stackprofile_Tapped(System.Object sender, System.EventArgs e)
        {
            App.Locator.EditProfileInfoPage.SetStackVisibility("name");
        }

        void stackphone_Tapped(System.Object sender, System.EventArgs e)
        {
            App.Locator.EditProfileInfoPage.SetStackVisibility("phone");
        }

        void stackcountry_Tapped(System.Object sender, System.EventArgs e)
        {
            App.Locator.EditProfileInfoPage.SetStackVisibility("city");
        }
    }
}

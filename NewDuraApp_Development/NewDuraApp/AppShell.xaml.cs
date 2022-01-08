using System;
using System.Collections.Generic;
using NewDuraApp.Areas.Common.Views;
using NewDuraApp.Areas.DuraExpress.DuraExpressView;
using NewDuraApp.Areas.DuraShop.View;
using NewDuraApp.Areas.Orders.Views;
using NewDuraApp.Interfaces;
using Xamarin.Forms;

namespace NewDuraApp
{
    public partial class AppShell : Shell, IRootView, IMainView, IShelll
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }
        public AppShell()
        {
            RegisterRoutes();
            InitializeComponent();
        }
        void RegisterRoutes()
        {
            routes.Add("NotificatonPage", typeof(NotificatonPage));
            routes.Add("DuraExpress", typeof(DuraExpress));
            routes.Add("DuraShop", typeof(DuraShop));
            routes.Add("AppShell", typeof(AppShell));
            routes.Add("MyOrders", typeof(MyOrders));
            routes.Add("HomePage", typeof(HomePage));
            routes.Add("OrderDetails2", typeof(OrderDetails2));
            //routes.Add(AppConst.SelectedTripNavPage, typeof(TripDetailPage));
            //routes.Add(AppConst.HomeNavPage, typeof(HomePage));
            //routes.Add(AppConst.BrowserPageNavUrl, typeof(BrowseAdventurePage));
            //routes.Add(AppConst.SignUpPageNavUrl, typeof(SignUpPage));
            //routes.Add(AppConst.SearchResultPageNavUrl, typeof(SearchResultPage));
            //routes.Add(AppConst.ChangeEmailPageNavUrl, typeof(ChangeEmailPage));
            //routes.Add(AppConst.ChangePasswordPageNavUrl, typeof(ChangePasswordPage));
            //routes.Add(AppConst.EditProfilePageNavUrl, typeof(EditProfilePage));
            //routes.Add(AppConst.ClinicationSignUpPageNavUrl, typeof(ClinicationSignUpPage));
            //routes.Add(AppConst.VerificationPageNavUrl, typeof(VerificationPage));
            //routes.Add(AppConst.SpecialtyInfoPageNavUrl, typeof(SpecialtyInfoPage));
            //routes.Add(AppConst.TermsAndConditionNavUrl, typeof(TermsAndCondition));
            //routes.Add(AppConst.SearchFilterPageNavUrl, typeof(SearchFilterPage));
            //routes.Add(AppConst.SignUpVerificationPageNavUrl, typeof(SignUpVerificationPage));
            //routes.Add(AppConst.AdvancedSearchPageNavUrl, typeof(AdvancedSearchPage));
            //routes.Add(AppConst.ConcentPageNavUrl, typeof(ConcentPage));
            //routes.Add(AppConst.BookingNotesPageNavUrl, typeof(BookingNotesPage));
            //routes.Add(AppConst.ClinicianDetailPageNavUrl, typeof(ClinicianDetailPage));
            //routes.Add(AppConst.ClinicDetailPageNavUrl, typeof(ClinicDetailPage));
            //routes.Add(AppConst.ResetPasswordPageNavUrl, typeof(ResetPasswordPage));
            //routes.Add(AppConst.ResetPasswordEmailPageNavUrl, typeof(ResetPasswordEmailPage));
            //routes.Add(AppConst.ResetPasswordVerificationPageNavUrl, typeof(ResetPasswordVerificationPage));
            //routes.Add(AppConst.YourAppointmentPageNavUrl, typeof(YourAppointmentPageViewModel));
            //routes.Add(AppConst.SearchFilterNearMePageNavUrl, typeof(SearchFilterNearMePage));
            //routes.Add(AppConst.SearchResultMapViewPageNavUrl, typeof(SearchResultMapViewPage));
            //routes.Add(AppConst.ClinisPageNavUrl, typeof(ClinicsPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
        protected override void OnTabIndexPropertyChanged(int oldValue, int newValue)
        {
            base.OnTabIndexPropertyChanged(oldValue, newValue);
        }
    }
}

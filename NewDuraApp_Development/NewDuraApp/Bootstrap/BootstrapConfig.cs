
using System;
using System.Diagnostics;
using FreshMvvm;
using System.Net.Http;
using NewDuraApp.Services.Interfaces;
using DuraApp.Core.Services;
using DuraApp.Core.Services.Interfaces;
using ShriekSecurity.Core.Services;
using NewDuraApp.Services;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.Profile.ViewModels;
using NewDuraApp.Areas.Orders.ViewModels;
using NewDuraApp.ViewModels;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Areas.DuraShop.ViewModel;

using NewDuraApp.Areas.DuraEats.Home.ViewModels;
using NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels;
using NewDuraApp.Areas.DuraEats.OrderDetails.ViewModels;

using NewDuraApp.Areas.DuraShop.Popup.ViewModel;

using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Reviews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.ReferFriend.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.PrivacyPolicy.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.TermsCondition.ViewModel;
using NewDuraApp.Areas.DuraExpress.Common.MapPopups.ViewModel;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.Areas.NoConnection.ViewModel;
using NewDuraApp.GCash.ViewModel;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.StandardRates.ViewModels;
using NewDuraApp.Areas.Common.Permissions.ViewModels;

namespace NewDuraApp.Bootstrap
{
    public class BootstrapConfig
    {
        public BootstrapConfig()
        {

        }
        public static void RegisterService()
        {
            try
            {
                FreshIOC.Container.Register<HttpClient>(new HttpClient());
            }
            catch (Exception e)
            {
                Debug.WriteLine($"already initiated ={e.Message}");
            }

            FreshIOC.Container.Register<IUserService, UserService>();
            FreshIOC.Container.Register<INavigationService, NavigationService>();
            FreshIOC.Container.Register<HttpService, HttpService>();
            FreshIOC.Container.Register<IAuthenticationService, AuthenticationService>();
            FreshIOC.Container.Register<IUserCoreService, UserCoreService>();
        }
        public static void RegisterViewModel()
        {
            FreshIOC.Container.Register<CurrentUser, CurrentUser>().AsSingleton();
            FreshIOC.Container.Register<HomePageViewModel, HomePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<LoginPageViewModels, LoginPageViewModels>().AsSingleton();
            FreshIOC.Container.Register<GetStartedPageViewModel, GetStartedPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<LandingPageViewModel, LandingPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<SignUpPageViewModel, SignUpPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ForgetPasswordPageViewModel, ForgetPasswordPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<SendOTPPageViewModel, SendOTPPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<VerifyOTPPageViewModel, VerifyOTPPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyProfileViewModel, MyProfileViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyOrdersViewModel, MyOrdersViewModel>().AsSingleton();
            FreshIOC.Container.Register<NotificatonPageViewModel, NotificatonPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<AppShellViewModel, AppShellViewModel>().AsSingleton();
            FreshIOC.Container.Register<AppShellViewModel, AppShellViewModel>().AsSingleton();
            FreshIOC.Container.Register<TermsConditionsPageViewModel, TermsConditionsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PersonalDetailsViewModel, PersonalDetailsViewModel>().AsSingleton();
            FreshIOC.Container.Register<ReferralCodeViewModel, ReferralCodeViewModel>().AsSingleton();
            FreshIOC.Container.Register<ResetPasswordViewModel, ResetPasswordViewModel>().AsSingleton();
            FreshIOC.Container.Register<DuraExpresViewModel, DuraExpresViewModel>().AsSingleton();
            FreshIOC.Container.Register<PickupScheduleViewModel, PickupScheduleViewModel>().AsSingleton();
            FreshIOC.Container.Register<PickupLocationViewModel, PickupLocationViewModel>().AsSingleton();
            FreshIOC.Container.Register<WhereToViewModel, WhereToViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddStopLocationViewModel, AddStopLocationViewModel>().AsSingleton();
            FreshIOC.Container.Register<TrackOrderViewModel, TrackOrderViewModel>().AsSingleton();
            FreshIOC.Container.Register<SelectVehicleViewModel, SelectVehicleViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddMoreDetailsViewModel, AddMoreDetailsViewModel>().AsSingleton();
            FreshIOC.Container.Register<PaymentViewModel, PaymentViewModel>().AsSingleton();
            FreshIOC.Container.Register<FindingDriverViewModel, FindingDriverViewModel>().AsSingleton();
            FreshIOC.Container.Register<OrderDetailsViewModel, OrderDetailsViewModel>().AsSingleton();
            FreshIOC.Container.Register<OrderDetails2ViewModel, OrderDetails2ViewModel>().AsSingleton();
            FreshIOC.Container.Register<DuraShopViewModel, DuraShopViewModel>().AsSingleton();
            FreshIOC.Container.Register<FeaturedViewModel, FeaturedViewModel>().AsSingleton();
            FreshIOC.Container.Register<ItemDetailsViewModel, ItemDetailsViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyCartViewModel, MyCartViewModel>().AsSingleton();
            FreshIOC.Container.Register<TrackOrderShopViewModel, TrackOrderShopViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyOrdersTabViewModel, MyOrdersTabViewModel>().AsSingleton();
            FreshIOC.Container.Register<RateandReviewViewModel, RateandReviewViewModel>().AsSingleton();
            FreshIOC.Container.Register<SearchScreenViewModel, SearchScreenViewModel>().AsSingleton();
            FreshIOC.Container.Register<AutoCompleteMapPageViewModel, AutoCompleteMapPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<GCashPaymentPageViewModel, GCashPaymentPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PaymentPageWebviewViewModel, PaymentPageWebviewViewModel>().AsSingleton();
            FreshIOC.Container.Register<OrderTrackerPageViewModel, OrderTrackerPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PermissionPageViewModel, PermissionPageViewModel>().AsSingleton();
            //popup
            FreshIOC.Container.Register<LocationSearchPopupViewModel, LocationSearchPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<FoundDriverPopupViewModel, FoundDriverPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<CancelDriverPopupViewModel, CancelDriverPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<FeeBreakdownPopupViewModel, FeeBreakdownPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<SuccessOrderPopupViewModel, SuccessOrderPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<CashOnDeliveryPopupViewModel, CashOnDeliveryPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<CameraGalleryPopupPageViewModel, CameraGalleryPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PhoneVerificationPopupPageViewModel, PhoneVerificationPopupPageViewModel>().AsSingleton();

            FreshIOC.Container.Register<DuraEatsHomePageViewModel, DuraEatsHomePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<DuraEatsItemDetailsPageViewModel, DuraEatsItemDetailsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyCartPageViewModel, MyCartPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PaymentModePageViewModel, PaymentModePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<TrackOrderPageViewModel, TrackOrderPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<OrderDetailsPageViewModel, OrderDetailsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<DuraEatsHelpPageViewModel, DuraEatsHelpPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ApplyPromoCodePageViewModel, ApplyPromoCodePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<RatingAndReviewsPageViewModel, RatingAndReviewsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ProfilePageViewModel, ProfilePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ChangePasswordPageViewModel, ChangePasswordPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<EditProfileInfoPageViewModel, EditProfileInfoPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ChangeNmberPageViewModel, ChangeNmberPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PersonalDocumentUploadPageViewModel, PersonalDocumentUploadPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<BusinessDocumentUploadPageViewModel, BusinessDocumentUploadPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyWalletPageViewModel, MyWalletPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddMoneyPageViewModel, AddMoneyPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<SavedCardsListPageViewModel, SavedCardsListPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddNewCardPageViewModel, AddNewCardPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<OfferAndPromocodePageViewModel, OfferAndPromocodePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyAddressPageViewModel, MyAddressPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddNewAddressPageViewModel, AddNewAddressPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MyReviewsPageViewModel, MyReviewsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ReferAFriendPageViewModel, ReferAFriendPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<SettingsPageViewModel, SettingsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<NotificationsPageViewModel, NotificationsPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<LanguagePageViewModel, LanguagePageViewModel>().AsSingleton();
            FreshIOC.Container.Register<PrivacyPolicyPageViewModel, PrivacyPolicyPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<AboutPageViewModel, AboutPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<ChangeLocationPageViewModel, ChangeLocationPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<TermsConditionPageViewModel, TermsConditionPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<StandardRatesPageViewModel, StandardRatesPageViewModel>().AsSingleton();
            // Pop ups
            FreshIOC.Container.Register<ItemsDetailAddCartPopupViewModel, ItemsDetailAddCartPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<ThanksForOrderPopupViewModel, ThanksForOrderPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<OTPPopupPageViewModel, OTPPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<TopupAmuntPopupPageViewModel, TopupAmuntPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<RechargeToppupPopupPageViewModel, RechargeToppupPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<RechargeSuccessfullPopupPageViewModel, RechargeSuccessfullPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MySavedAddressPopupPageViewModel, MySavedAddressPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<MapPopupViewModel, MapPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<NoInternetPopupPageViewModel, NoInternetPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<CancelRideReasonPopupViewModel, CancelRideReasonPopupViewModel>().AsSingleton();
            FreshIOC.Container.Register<AddRatingPopupPageViewModel, AddRatingPopupPageViewModel>().AsSingleton();
            FreshIOC.Container.Register<LogoutPopupPageViewModel, LogoutPopupPageViewModel>().AsSingleton();
        }
    }
}

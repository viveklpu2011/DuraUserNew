
using FreshMvvm;
using NewDuraApp.Areas.Common.Permissions.ViewModels;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using NewDuraApp.Areas.DuraEats.Home.ViewModels;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Areas.DuraEats.OrderDetails.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.ViewModels;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels;
using NewDuraApp.Areas.DuraExpress.Common.MapPopups.ViewModel;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Areas.DuraShop.Popup.ViewModel;
using NewDuraApp.Areas.DuraShop.ViewModel;
using NewDuraApp.Areas.NoConnection.ViewModel;
using NewDuraApp.Areas.Orders.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Areas.Profile.Menu.ReferFriend.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Reviews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.PrivacyPolicy.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.StandardRates.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.TermsCondition.ViewModel;
using NewDuraApp.Areas.Profile.Menu.Settings.ViewModels;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Areas.Profile.ViewModels;
using NewDuraApp.GCash.ViewModel;
using NewDuraApp.Services;

namespace NewDuraApp.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }
        static ViewModelLocator()
        {

        }
        public CurrentUser CurrentUser => FreshIOC.Container.Resolve<CurrentUser>();
        public AppShellViewModel AppShell => FreshIOC.Container.Resolve<AppShellViewModel>();
        public HomePageViewModel HomePage => FreshIOC.Container.Resolve<HomePageViewModel>();
        public LoginPageViewModels LoginPage => FreshIOC.Container.Resolve<LoginPageViewModels>();
        public GetStartedPageViewModel GetStartedPage => FreshIOC.Container.Resolve<GetStartedPageViewModel>();
        public LandingPageViewModel LandingPage => FreshIOC.Container.Resolve<LandingPageViewModel>();
        public SignUpPageViewModel SignUpPage => FreshIOC.Container.Resolve<SignUpPageViewModel>();
        public ForgetPasswordPageViewModel ForgetPasswordPage => FreshIOC.Container.Resolve<ForgetPasswordPageViewModel>();
        public SendOTPPageViewModel SendOTPPage => FreshIOC.Container.Resolve<SendOTPPageViewModel>();
        public VerifyOTPPageViewModel VerifyOTPPage => FreshIOC.Container.Resolve<VerifyOTPPageViewModel>();
        public MyProfileViewModel MyProfile => FreshIOC.Container.Resolve<MyProfileViewModel>();
        public MyOrdersViewModel MyOrders => FreshIOC.Container.Resolve<MyOrdersViewModel>();
        public NotificatonPageViewModel NotificatonPage => FreshIOC.Container.Resolve<NotificatonPageViewModel>();
        public TermsConditionsPageViewModel TermsConditionsPage => FreshIOC.Container.Resolve<TermsConditionsPageViewModel>();
        public PersonalDetailsViewModel PersonalDetails => FreshIOC.Container.Resolve<PersonalDetailsViewModel>();
        public ReferralCodeViewModel ReferralCodeView => FreshIOC.Container.Resolve<ReferralCodeViewModel>();
        public ResetPasswordViewModel ResetPassword => FreshIOC.Container.Resolve<ResetPasswordViewModel>();
        public DuraExpresViewModel DuraExpress => FreshIOC.Container.Resolve<DuraExpresViewModel>();
        public PickupScheduleViewModel PickupSchedule => FreshIOC.Container.Resolve<PickupScheduleViewModel>();
        public PickupLocationViewModel PickupLocation => FreshIOC.Container.Resolve<PickupLocationViewModel>();
        public WhereToViewModel WhereTo => FreshIOC.Container.Resolve<WhereToViewModel>();
        public AddStopLocationViewModel AddStopLocation => FreshIOC.Container.Resolve<AddStopLocationViewModel>();
        public TrackOrderViewModel TrackOrder => FreshIOC.Container.Resolve<TrackOrderViewModel>();
        public SelectVehicleViewModel SelectVehicle => FreshIOC.Container.Resolve<SelectVehicleViewModel>();
        public AddMoreDetailsViewModel AddMoreDetails => FreshIOC.Container.Resolve<AddMoreDetailsViewModel>();
        public PaymentViewModel Payment => FreshIOC.Container.Resolve<PaymentViewModel>();
        public FindingDriverViewModel FindingDriver => FreshIOC.Container.Resolve<FindingDriverViewModel>();
        public OrderDetailsViewModel OrderDetails => FreshIOC.Container.Resolve<OrderDetailsViewModel>();
        public OrderDetails2ViewModel OrderDetails2 => FreshIOC.Container.Resolve<OrderDetails2ViewModel>();
        public DuraShopViewModel DuraShop => FreshIOC.Container.Resolve<DuraShopViewModel>();
        public FeaturedViewModel Featured => FreshIOC.Container.Resolve<FeaturedViewModel>();
        public ItemDetailsViewModel ItemDetails => FreshIOC.Container.Resolve<ItemDetailsViewModel>();
        public MyCartViewModel MyCart => FreshIOC.Container.Resolve<MyCartViewModel>();
        public TrackOrderShopViewModel TrackOrderShop => FreshIOC.Container.Resolve<TrackOrderShopViewModel>();
        public MyOrdersTabViewModel MyOrdersTab => FreshIOC.Container.Resolve<MyOrdersTabViewModel>();
        public RateandReviewViewModel RateandReview => FreshIOC.Container.Resolve<RateandReviewViewModel>();
        public SearchScreenViewModel SearchScreen => FreshIOC.Container.Resolve<SearchScreenViewModel>();
        public StandardRatesPageViewModel StandardRatesPage => FreshIOC.Container.Resolve<StandardRatesPageViewModel>();
        public OrderTrackerPageViewModel OrderTrackerPage => FreshIOC.Container.Resolve<OrderTrackerPageViewModel>();
        public PermissionPageViewModel PermissionPage => FreshIOC.Container.Resolve<PermissionPageViewModel>();
        //Popup
        public LocationSearchPopupViewModel LocationSearchPopup => FreshIOC.Container.Resolve<LocationSearchPopupViewModel>();
        public FoundDriverPopupViewModel FoundDriverPopup => FreshIOC.Container.Resolve<FoundDriverPopupViewModel>();
        public CancelDriverPopupViewModel CancelDriverPopup => FreshIOC.Container.Resolve<CancelDriverPopupViewModel>();
        public FeeBreakdownPopupViewModel FeeBreakdownPopup => FreshIOC.Container.Resolve<FeeBreakdownPopupViewModel>();
        public SuccessOrderPopupViewModel SuccessOrderPopup => FreshIOC.Container.Resolve<SuccessOrderPopupViewModel>();
        public CameraGalleryPopupPageViewModel CameraGalleryPopupPage => FreshIOC.Container.Resolve<CameraGalleryPopupPageViewModel>();
        public PhoneVerificationPopupPageViewModel PhoneVerificationPopupPage => FreshIOC.Container.Resolve<PhoneVerificationPopupPageViewModel>();

        public DuraEatsHomePageViewModel DuraEatsHomePage => FreshIOC.Container.Resolve<DuraEatsHomePageViewModel>();
        public DuraEatsItemDetailsPageViewModel DuraEatsItemDetailsPage => FreshIOC.Container.Resolve<DuraEatsItemDetailsPageViewModel>();
        public MyCartPageViewModel MyCartPage => FreshIOC.Container.Resolve<MyCartPageViewModel>();
        public PaymentModePageViewModel PaymentModePage => FreshIOC.Container.Resolve<PaymentModePageViewModel>();
        public TrackOrderPageViewModel TrackOrderPage => FreshIOC.Container.Resolve<TrackOrderPageViewModel>();
        public OrderDetailsPageViewModel OrderDetailsPage => FreshIOC.Container.Resolve<OrderDetailsPageViewModel>();
        public DuraEatsHelpPageViewModel DuraEatsHelpPage => FreshIOC.Container.Resolve<DuraEatsHelpPageViewModel>();
        public ApplyPromoCodePageViewModel ApplyPromoCodePage => FreshIOC.Container.Resolve<ApplyPromoCodePageViewModel>();
        public RatingAndReviewsPageViewModel RatingAndReviewsPage => FreshIOC.Container.Resolve<RatingAndReviewsPageViewModel>();
        public ProfilePageViewModel ProfilePage => FreshIOC.Container.Resolve<ProfilePageViewModel>();
        public ChangePasswordPageViewModel ChangePasswordPage => FreshIOC.Container.Resolve<ChangePasswordPageViewModel>();
        public EditProfileInfoPageViewModel EditProfileInfoPage => FreshIOC.Container.Resolve<EditProfileInfoPageViewModel>();
        public ChangeNmberPageViewModel ChangeNmberPage => FreshIOC.Container.Resolve<ChangeNmberPageViewModel>();
        public PersonalDocumentUploadPageViewModel PersonalDocumentUploadPage => FreshIOC.Container.Resolve<PersonalDocumentUploadPageViewModel>();
        public BusinessDocumentUploadPageViewModel BusinessDocumentUploadPage => FreshIOC.Container.Resolve<BusinessDocumentUploadPageViewModel>();
        public MyWalletPageViewModel MyWalletPage => FreshIOC.Container.Resolve<MyWalletPageViewModel>();
        public AddMoneyPageViewModel AddMoneyPage => FreshIOC.Container.Resolve<AddMoneyPageViewModel>();
        public SavedCardsListPageViewModel SavedCardsListPage => FreshIOC.Container.Resolve<SavedCardsListPageViewModel>();
        public AddNewCardPageViewModel AddNewCardPage => FreshIOC.Container.Resolve<AddNewCardPageViewModel>();
        public OfferAndPromocodePageViewModel OfferAndPromocodePage => FreshIOC.Container.Resolve<OfferAndPromocodePageViewModel>();
        public MyAddressPageViewModel MyAddressPage => FreshIOC.Container.Resolve<MyAddressPageViewModel>();
        public AddNewAddressPageViewModel AddNewAddressPage => FreshIOC.Container.Resolve<AddNewAddressPageViewModel>();
        public MyReviewsPageViewModel MyReviewsPage => FreshIOC.Container.Resolve<MyReviewsPageViewModel>();
        public ReferAFriendPageViewModel ReferAFriendPage => FreshIOC.Container.Resolve<ReferAFriendPageViewModel>();
        public SettingsPageViewModel SettingsPage => FreshIOC.Container.Resolve<SettingsPageViewModel>();
        public NotificationsPageViewModel NotificationsPage => FreshIOC.Container.Resolve<NotificationsPageViewModel>();
        public LanguagePageViewModel LanguagePage => FreshIOC.Container.Resolve<LanguagePageViewModel>();
        public PrivacyPolicyPageViewModel PrivacyPolicyPage => FreshIOC.Container.Resolve<PrivacyPolicyPageViewModel>();
        public AboutPageViewModel AboutPage => FreshIOC.Container.Resolve<AboutPageViewModel>();
        public ChangeLocationPageViewModel ChangeLocationPage => FreshIOC.Container.Resolve<ChangeLocationPageViewModel>();
        public TermsConditionPageViewModel TermsConditionPage => FreshIOC.Container.Resolve<TermsConditionPageViewModel>();
        public AutoCompleteMapPageViewModel AutoCompleteMapPage => FreshIOC.Container.Resolve<AutoCompleteMapPageViewModel>();
        public GCashPaymentPageViewModel GCashPaymentPage => FreshIOC.Container.Resolve<GCashPaymentPageViewModel>();
        public PaymentPageWebviewViewModel PaymentPageWebview => FreshIOC.Container.Resolve<PaymentPageWebviewViewModel>();
        //pop Ups

        public ItemsDetailAddCartPopupViewModel ItemsDetailAddCartPopup => FreshIOC.Container.Resolve<ItemsDetailAddCartPopupViewModel>();
        public ThanksForOrderPopupViewModel ThanksForOrderPopup => FreshIOC.Container.Resolve<ThanksForOrderPopupViewModel>();
        public OTPPopupPageViewModel OTPPopupPage => FreshIOC.Container.Resolve<OTPPopupPageViewModel>();
        public TopupAmuntPopupPageViewModel TopupAmuntPopupPage => FreshIOC.Container.Resolve<TopupAmuntPopupPageViewModel>();
        public RechargeToppupPopupPageViewModel RechargeToppupPopupPage => FreshIOC.Container.Resolve<RechargeToppupPopupPageViewModel>();
        public RechargeSuccessfullPopupPageViewModel RechargeSuccessfullPopupPage => FreshIOC.Container.Resolve<RechargeSuccessfullPopupPageViewModel>();
        public MySavedAddressPopupPageViewModel MySavedAddressPopupPage => FreshIOC.Container.Resolve<MySavedAddressPopupPageViewModel>();
        public CashOnDeliveryPopupViewModel CashOnDeliveryPopup => FreshIOC.Container.Resolve<CashOnDeliveryPopupViewModel>();
        public MapPopupViewModel MapPopup => FreshIOC.Container.Resolve<MapPopupViewModel>();
        public NoInternetPopupPageViewModel NoInternetPopupPage => FreshIOC.Container.Resolve<NoInternetPopupPageViewModel>();
        public CancelRideReasonPopupViewModel CancelRideReasonPopup => FreshIOC.Container.Resolve<CancelRideReasonPopupViewModel>();
        public AddRatingPopupPageViewModel AddRatingPopupPage => FreshIOC.Container.Resolve<AddRatingPopupPageViewModel>();
        public LogoutPopupPageViewModel LogoutPopupPage => FreshIOC.Container.Resolve<LogoutPopupPageViewModel>();
    }
}

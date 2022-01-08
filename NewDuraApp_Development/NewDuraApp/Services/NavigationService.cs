using NewDuraApp;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.Common.Views;
using NewDuraApp.Areas.DuraExpress.DuraExpressView;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Areas.DuraShop.View;
using NewDuraApp.Areas.DuraShop.ViewModel;

using NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.Views;
using NewDuraApp.Areas.DuraEats.ChangeLocation.ViewModels;
using NewDuraApp.Areas.DuraEats.ChangeLocation.Views;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.Views;
using NewDuraApp.Areas.DuraEats.Home.ViewModels;
using NewDuraApp.Areas.DuraEats.Home.Views;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.Views;
using NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels;
using NewDuraApp.Areas.DuraEats.ItemDetails.Views;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Areas.DuraEats.MyCart.Views;
using NewDuraApp.Areas.DuraEats.OrderDetails.ViewModels;
using NewDuraApp.Areas.DuraEats.OrderDetails.Views;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Popups.Views;
using NewDuraApp.Areas.DuraEats.PaymentMethod.ViewModels;
using NewDuraApp.Areas.DuraEats.PaymentMethod.Views;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Areas.DuraEats.RatingReviews.Views;
using NewDuraApp.Areas.DuraEats.TrackOrder.ViewModels;
using NewDuraApp.Areas.DuraEats.TrackOrder.Views;
using NewDuraApp.Areas.Orders.ViewModels;
using NewDuraApp.Areas.Orders.Views;
using NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyAddress.Views;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.Views;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Views;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.Views;
using NewDuraApp.Areas.Profile.Menu.ReferFriend.ViewModels;
using NewDuraApp.Areas.Profile.Menu.ReferFriend.Views;
using NewDuraApp.Areas.Profile.Menu.Reviews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Reviews.Views;
using NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels;
using NewDuraApp.Areas.Profile.Menu.SavedCards.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.PrivacyPolicy.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.PrivacyPolicy.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.Views;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Areas.Profile.PopupViews.Views;
using NewDuraApp.Areas.Profile.ViewModels;
using NewDuraApp.Areas.Profile.Views;
using NewDuraApp.Interfaces;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using NewDuraApp.Areas.DuraShop.Popup.View;
using NewDuraApp.Areas.DuraShop.Popup.ViewModel;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.TermsCondition.View;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.TermsCondition.ViewModel;
using DuraApp.Core.Helpers;
using NewDuraApp.Areas.DuraExpress.Common.MapPopups.ViewModel;
using NewDuraApp.Areas.DuraExpress.Common.MapPopups.Views;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.Areas.NoConnection.ViewModel;
using NewDuraApp.Areas.NoConnection.View;
using NewDuraApp.GCash.ViewModel;
using NewDuraApp.GCash.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.StandardRates.Views;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.StandardRates.ViewModels;
using NewDuraApp.Areas;
using NewDuraApp.Areas.Common.Permissions.ViewModels;
using NewDuraApp.Areas.Common.Permissions.Views;
using Plugin.Multilingual;
using NewDuraApp.Resources;
using System.Globalization;

namespace NewDuraApp.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly IList<Tuple<Type, Type, bool>> _mappings;

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public IList<Tuple<Type, Type, bool>> _mappingsRead { get => _mappings; }

        //Re-Exposed Navigation events for hooking into page changes
        public EventHandler ModalPopped;
        public EventHandler ModalPushed;
        private readonly IUserService _userService;

        public NavigationService(IUserService userService)
        {
            _userService = userService;
            CurrentApplication.ModalPopped += (sender, e) => ModalPopped?.Invoke(sender, e);
            CurrentApplication.ModalPushed += (sender, e) => ModalPushed?.Invoke(sender, e);

            _mappings = new List<Tuple<Type, Type, bool>>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (SettingsExtension.SelectedLanguage)
                {
                    CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("fil");
                    AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                }
                else
                {
                    CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("en");
                    AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                }
                if (SettingsExtension.IsLoggedIn)
                {
                    await NavigateToAsync<AppShellViewModel>();
                    await App.Locator.HomePage.GetAllLocation();
                    await App.Locator.HomePage.InitilizeData();
                    await App.Locator.HomePage.CheckAccountIsVerified();
                }
                else
                {
                    await NavigateToAsync<GetStartedPageViewModel>();
                    await App.Locator.GetStartedPage.InitilizeData();
                }
                
                //await App.Locator.TimerPage.InitilizeData();
                //await NavigateToAsync<TimerPageViewModel>();
                //await App.Locator.TimerPage.UpdateTextColor(0);
                //await App.Locator.TimerPage.UpdateHoursTextColor(1);
            }
            catch (Exception ex)
            {

            }
        }

        public Task NavigateToAsync<TViewModel>(TViewModel viewModel) where TViewModel : AppBaseViewModel
        {

            return InternalNavigateToAsync(typeof(TViewModel), viewModel, null);

        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : AppBaseViewModel
        {

            return InternalNavigateToAsync(typeof(TViewModel), null, null);

        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : AppBaseViewModel
        {

            return InternalNavigateToAsync(typeof(TViewModel), null, parameter);

        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, null, parameter);
        }

        public async Task ShellGoToAsync(string route)
        {
            await Shell.Current.GoToAsync(route, animate: true);
        }

        public async Task ShellNavigationPushAsync(Page page)
        {
            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task ShellGoBackAsync()
        {
            Shell.Current.SendBackButtonPressed();
        }

        public async Task ShellNavigationPopAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async Task ShellNavigationPopToRootAsync()
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public Type GetCurrentPageViewModel()
        {
            if (CurrentApplication.MainPage != null)
            {
                Page currentPage = CurrentApplication.MainPage.Navigation.NavigationStack.Last();
                if (currentPage?.BindingContext != null)
                    return currentPage.BindingContext.GetType();
            }
            return null;
        }

        public bool SetCurrentPageTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Page currentPage = CurrentApplication.MainPage.Navigation.NavigationStack.Last();
                if (currentPage != null)
                {
                    currentPage.Title = title;
                    return true;
                }
            }
            return false;
        }

        public async virtual Task RemoveLastFromBackStackAsync()
        {
            //var mainPage = CurrentApplication.MainPage as Page;

            //mainPage.Navigation.RemovePage(
            //    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count-6]);

            var mainPage = CurrentApplication.MainPage as Page;
            for (int i = 0; i < mainPage.Navigation.NavigationStack.ToList().Count - 1; i++)
            {
                mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            await mainPage.Navigation.PopAsync();

            //return Task.FromResult(true);
        }
        public virtual Task ClearNavigationStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as Page;
            for (int i = 0; i < mainPage.Navigation.NavigationStack.ToList().Count - 1; i++)
            {
                mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[i]);
            }
            return Task.FromResult(true);
        }

        public Task NavigateToPopupAsync<TViewModel>(bool animate, TViewModel viewModel) where TViewModel : AppBaseViewModel
        {
            return NavigateToPopupAsync(null, animate, viewModel);
        }

        public async Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate, TViewModel vieModel) where TViewModel : AppBaseViewModel
        {
            var page = CreateAndBindPage(typeof(TViewModel), vieModel, parameter, true);

            await (page.BindingContext as AppBaseViewModel).InitializeAsync(parameter);

            if (page is PopupPage)
            {
                await PopupNavigation.Instance.PushAsync(page as PopupPage, animate);
            }
            else
            {
                throw new ArgumentException($"The type ${typeof(TViewModel)} its not a PopupPage type");
            }
        }

        public async Task CloseAllPopupsAsync()
        {
            await PopupNavigation.Instance.PopAllAsync(true);
        }

        public async Task ClosePopupsAsync()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object viewModel, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, viewModel, parameter, false);
            if (page != null)
            {
                if (page is IRootView)
                {
                    //TODO prob need to check the navigation stack at this point to ensure there are no pages on top??
                    if (page is IMainView)
                    {
                        if (page is IShelll)
                        {
                            object bindingContext = page.BindingContext;
                            page = new AppShell();
                            page.BindingContext = bindingContext;
                        }
                        else
                        {
                            object bindingContext = page.BindingContext;
                            page = new NavigationPage(page) { BarBackgroundColor = Color.FromHex("#ffffff"), BarTextColor = Color.Black };
                            page.BindingContext = bindingContext;
                        }
                    }
                    CurrentApplication.MainPage = page;
                }
                else if (CurrentApplication != null && CurrentApplication.MainPage != null)
                {
                    if (CurrentApplication.MainPage is IMainView || CurrentApplication.MainPage is NavigationPage) // Implemented as an interface so we can have different main views on different platforms
                    {

                        var mainPage = CurrentApplication.MainPage as Page;

                        if (mainPage.GetType() != page.GetType())
                        {
                            var transitionNavigationPage = CurrentApplication.MainPage as NavigationPage;
                            if (transitionNavigationPage != null)
                            {
                                await mainPage.Navigation.PushAsync(page);
                            }
                            else
                            {
                                await mainPage.Navigation.PushAsync(page);
                            }

                        }
                    }
                }

            }

            //await (page.BindingContext as BasePageViewModel).InitializeAsync(parameter);
        }


        protected Type GetPageTypeForViewModel(Type viewModelType, bool isPopup)
        {
            Type pageType = null;
            if (!isPopup)
                pageType = _mappings.FirstOrDefault(_ => (_.Item1 == viewModelType) && !_.Item3).Item2;
            else
                pageType = _mappings.FirstOrDefault(_ => (_.Item1 == viewModelType) && _.Item3).Item2;

            if (pageType == null)
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");

            return pageType;
        }

        protected Page CreateAndBindPage(Type viewModelType, object viewModelObj, object parameter, bool isPopup)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType, isPopup);
            Page page = null;
            if (pageType == null)
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            try
            {
                page = Activator.CreateInstance(pageType) as Page;
                AppBaseViewModel viewModel;

                if (viewModelObj != null)
                    viewModel = viewModelObj as AppBaseViewModel;
                else
                    viewModel = App.Container.Resolve(viewModelType) as AppBaseViewModel;
                if (page is PopupPage)
                    page.BindingContext = viewModel;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Code result's in {e.Message}");
            }
            return page;
        }

        private void CreatePageViewModelMappings()
        {
            //Pages
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(LoginPageViewModels), typeof(LoginPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(HomePageViewModel), typeof(HomePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(GetStartedPageViewModel), typeof(GetStartedPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(LandingPageViewModel), typeof(LandingPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SignUpPageViewModel), typeof(SignUpPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ForgetPasswordPageViewModel), typeof(ForgetPasswordPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SendOTPPageViewModel), typeof(SendOTPPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(VerifyOTPPageViewModel), typeof(VerifyOTPPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyProfileViewModel), typeof(MyProfile), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyOrdersViewModel), typeof(MyOrders), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(NotificatonPageViewModel), typeof(NotificatonPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AppShellViewModel), typeof(AppShell), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TermsConditionsPageViewModel), typeof(TermsConditionsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PersonalDetailsViewModel), typeof(PersonalDetails), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ReferralCodeViewModel), typeof(ReferralCodeView), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ResetPasswordViewModel), typeof(ResetPassword), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(DuraExpresViewModel), typeof(DuraExpress), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PickupScheduleViewModel), typeof(PickupSchedule), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PickupLocationViewModel), typeof(PickupLocation), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(WhereToViewModel), typeof(WhereTo), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddStopLocationViewModel), typeof(AddStopLocation), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TrackOrderViewModel), typeof(TrackOrder), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SelectVehicleViewModel), typeof(SelectVehicle), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddMoreDetailsViewModel), typeof(AddMoreDetails), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PaymentViewModel), typeof(Payment), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(FindingDriverViewModel), typeof(FindingDriver), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OrderDetailsViewModel), typeof(OrderDetails), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OrderDetails2ViewModel), typeof(OrderDetails2), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(DuraShopViewModel), typeof(DuraShop), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(FeaturedViewModel), typeof(Featured), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ItemDetailsViewModel), typeof(ItemDetails), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyCartViewModel), typeof(MyCart), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TrackOrderShopViewModel), typeof(TrackOrderShop), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyOrdersTabViewModel), typeof(MyOrdersTab), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(RateandReviewViewModel), typeof(RateandReview), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SearchScreenViewModel), typeof(SearchScreen), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AutoCompleteMapPageViewModel), typeof(AutoCompleteMapPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(GCashPaymentPageViewModel), typeof(GCashPaymentPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PaymentPageWebviewViewModel), typeof(PaymentPageWebview), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(StandardRatesPageViewModel), typeof(StandardRatesPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OrderTrackerPageViewModel), typeof(OrderTrackerPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PermissionPageViewModel), typeof(PermissionPage), false));
            //PopupViews
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ResetSuccessfullyPopupViewModel), typeof(ResetSuccessfullyPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(LocationSearchPopupViewModel), typeof(LocationSearchPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(FoundDriverPopupViewModel), typeof(FoundDriverPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(CancelDriverPopupViewModel), typeof(CancelDriverPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(FeeBreakdownPopupViewModel), typeof(FeeBreakdownPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SuccessOrderPopupViewModel), typeof(SuccessOrderPopup), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(CameraGalleryPopupPageViewModel), typeof(CameraGalleryPopupPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PhoneVerificationPopupPageViewModel), typeof(PhoneVerificationPopupPage), false));

            // _mappings.Add(new Tuple<Type, Type, bool>(typeof(ShopMoreDetailsPageViewModel), typeof(ShopMoreDetailsPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(DuraEatsHomePageViewModel), typeof(DuraEatsHomePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(DuraEatsItemDetailsPageViewModel), typeof(DuraEatsItemDetailsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyCartPageViewModel), typeof(MyCartPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PaymentModePageViewModel), typeof(PaymentModePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TrackOrderPageViewModel), typeof(TrackOrderPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OrderDetailsPageViewModel), typeof(OrderDetailsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(DuraEatsHelpPageViewModel), typeof(DuraEatsHelpPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ApplyPromoCodePageViewModel), typeof(ApplyPromoCodePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(RatingAndReviewsPageViewModel), typeof(RatingAndReviewsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ProfilePageViewModel), typeof(ProfilePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ChangePasswordPageViewModel), typeof(ChangePasswordPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(EditProfileInfoPageViewModel), typeof(EditProfileInfoPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ChangeNmberPageViewModel), typeof(ChangeNmberPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PersonalDocumentUploadPageViewModel), typeof(PersonalDocumentUploadPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(BusinessDocumentUploadPageViewModel), typeof(BusinessDocumentUploadPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyWalletPageViewModel), typeof(MyWalletPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddMoneyPageViewModel), typeof(AddMoneyPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SavedCardsListPageViewModel), typeof(SavedCardsListPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddNewCardPageViewModel), typeof(AddNewCardPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OfferAndPromocodePageViewModel), typeof(OfferAndPromocodePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyAddressPageViewModel), typeof(MyAddressPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddNewAddressPageViewModel), typeof(AddNewAddressPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MyReviewsPageViewModel), typeof(MyReviewsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ReferAFriendPageViewModel), typeof(ReferAFriendPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(SettingsPageViewModel), typeof(SettingsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(NotificationsPageViewModel), typeof(NotificationsPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(LanguagePageViewModel), typeof(LanguagePage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(PrivacyPolicyPageViewModel), typeof(PrivacyPolicyPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AboutPageViewModel), typeof(AboutPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ChangeLocationPageViewModel), typeof(ChangeLocationPage), false));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TermsConditionPageViewModel), typeof(TermsConditionPage), false));
            //PopupViews
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ItemsDetailAddCartPopupViewModel), typeof(ItemsDetailAddCartPopup), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ThanksForOrderPopupViewModel), typeof(ThanksForOrderPopup), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(CashOnDeliveryPopupViewModel), typeof(CashOnDeliveryPopup), true));

            _mappings.Add(new Tuple<Type, Type, bool>(typeof(OTPPopupPageViewModel), typeof(OTPPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(TopupAmuntPopupPageViewModel), typeof(TopupAmuntPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(RechargeToppupPopupPageViewModel), typeof(RechargeToppupPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(RechargeSuccessfullPopupPageViewModel), typeof(RechargeSuccessfullPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MySavedAddressPopupPageViewModel), typeof(MySavedAddressPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(MapPopupViewModel), typeof(MapPopup), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(NoInternetPopupPageViewModel), typeof(NoInternetPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(CancelRideReasonPopupViewModel), typeof(CancelRideReasonPopup), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(AddRatingPopupPageViewModel), typeof(AddRatingPopupPage), true));
            _mappings.Add(new Tuple<Type, Type, bool>(typeof(LogoutPopupPageViewModel), typeof(LogoutPopupPage), true));

            _mappings.Add(new Tuple<Type, Type, bool>(typeof(ChatViewModel), typeof(ChatPage), false));
        }

        public Task ShellNavigationPushAsync<TViewModel>() where TViewModel : AppBaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, null);
        }

        public async Task RemoveLastFromBackStackAsyncForResetPassword()
        {
            var mainPage = CurrentApplication.MainPage as Page;
            for (int i = 0; i < mainPage.Navigation.NavigationStack.ToList().Count - 2; i++)
            {
                mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            await mainPage.Navigation.PopAsync();
        }

        public async Task RemoveLastFromBackStackAsyncForDuraExpress(int pagenumber)
        {
            var mainPage = CurrentApplication.MainPage as Page;
            for (int i = 0; i < pagenumber; i++)
            {
                mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            await mainPage.Navigation.PopAsync();
            //for (int i = 0; i < mainPage.Navigation.NavigationStack.ToList().Count - 2; i++)
            //{
            //    mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            //}
            //await mainPage.Navigation.PopAsync();
        }
    }
}

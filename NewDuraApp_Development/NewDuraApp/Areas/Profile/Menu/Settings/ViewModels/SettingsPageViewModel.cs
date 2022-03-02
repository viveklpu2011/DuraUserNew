using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Notifications.ViewModels;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Menu.Settings.ViewModels
{
    public class SettingsPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateToNotifications { get; set; }
        public IAsyncCommand NavigateToLanguage { get; set; }
        public IAsyncCommand NavigateToAboutUs { get; set; }
        public IAsyncCommand NavigateToPrivacyPoliccy { get; set; }
        public IAsyncCommand NavigateToTermsCondition { get; set; }
        public IAsyncCommand LogoutCommand { get; set; }
        public SettingsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateToNotifications = new AsyncCommand(NavigateToNotificationsPage, allowsMultipleExecutions: false);
            NavigateToLanguage = new AsyncCommand(NavigateToLanguagePage, allowsMultipleExecutions: false);
            NavigateToAboutUs = new AsyncCommand(NavigateToAboutUsPage, allowsMultipleExecutions: false);
            NavigateToPrivacyPoliccy = new AsyncCommand(NavigateToPrivacyPoliccyPage, allowsMultipleExecutions: false);
            NavigateToTermsCondition = new AsyncCommand(NavigateToTermsConditionExecute, allowsMultipleExecutions: false);
            LogoutCommand = new AsyncCommand(LogoutCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task LogoutCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(LogoutPopupPageViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new LogoutPopupPage());
            }

            //var result = await ShowConfirmationAlert("Are you sure you want to logout?", "Question");
            //if (result)
            //{
            //    App.Locator.PickupSchedule.IsVisibleLaterView = false;
            //    App.Locator.PickupSchedule.AsapIsChecked = true;
            //    App.Locator.PickupSchedule.LaterIsCheck = false;
            //    App.Locator.PickupSchedule.IsButtonEnabled = true;
            //    App.Locator.PickupSchedule.DuraAddressCommon = null;
            //    App.Locator.PickupSchedule.PickupScheduleRequest = null;
            //    App.Locator.PickupLocation.PickupScheduleRequest = null;
            //    App.Locator.WhereTo.PickupScheduleRequest = null;
            //    App.Locator.AddStopLocation.PickupScheduleRequest = null;
            //    App.Locator.DuraExpress.PickupLocationText = string.Empty;
            //    App.Locator.DuraExpress.PickupLocationTextVisible = false;
            //    SettingsExtension.PickupAddress = string.Empty;
            //    App.Locator.DuraExpress.PickupScheduleLocText = string.Empty;
            //    App.Locator.DuraExpress.PickupScheduleLocTextVisible = false;
            //    App.Locator.DuraExpress.PickupWhereToText = string.Empty;
            //    App.Locator.DuraExpress.PickupWhereToTextVisible = false;
            //    App.Locator.AddMoreDetails.VehicleListSelectedData = null;
            //    App.Locator.SelectVehicle.VehicleListSelectedData = null;
            //    SettingsExtension.IsLoggedIn = false;
            //    App.Locator.PickupLocation.Address1 = string.Empty;
            //    App.Locator.PickupLocation.Address2 = string.Empty;
            //    App.Locator.WhereTo.Address1 = string.Empty;
            //    App.Locator.WhereTo.Address2 = string.Empty;
            //    App.Locator.AddStopLocation.Address1 = string.Empty;
            //    App.Locator.AddStopLocation.Address2 = string.Empty;
            //    // await App.Locator.LoginPage.GetAllLocation();
            //    App.Current.MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
            //    await App.Locator.LoginPage.InitilizeData();
            //}
        }

        private async Task NavigateToTermsConditionExecute()
        {
            await Browser.OpenAsync(AppConstant.TermsServicesUrl, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#211E66"),
                PreferredControlColor = Color.White
            });
        }

        private async Task NavigateToPrivacyPoliccyPage()
        {
            await Browser.OpenAsync(AppConstant.PrivacyPolicyUrl, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#211E66"),
                PreferredControlColor = Color.White,
            });
        }

        private async Task NavigateToAboutUsPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AboutPageViewModel))
            {
                await App.Locator.AboutPage.InitilizeData();
                await _navigationService.NavigateToAsync<AboutPageViewModel>();
            }
        }

        private async Task NavigateToNotificationsPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(NotificationsPageViewModel))
            {
                ShowAlert(AppResources.We_are_Working_on_it_It_is_in_Progress, "DuraDrive", AppResources.Ok);
                await App.Locator.NotificationsPage.InitilizeData();
                await _navigationService.NavigateToAsync<NotificationsPageViewModel>();
            }
        }
        private async Task NavigateToLanguagePage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(LanguagePageViewModel))
            {
                await App.Locator.LanguagePage.InitilizeData();
                await _navigationService.NavigateToAsync<LanguagePageViewModel>();
            }
        }

        public async Task InitilizeData()
        {

        }
    }
}

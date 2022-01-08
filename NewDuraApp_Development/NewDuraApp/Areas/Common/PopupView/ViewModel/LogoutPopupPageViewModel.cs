using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.Views;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.PopupView.ViewModel
{
    public class LogoutPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand LogoutCommand { get; set; }
        public IAsyncCommand CancelCommand { get; set; }

        public LogoutPopupPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            LogoutCommand = new AsyncCommand(LogoutCommandExecute);
            CancelCommand = new AsyncCommand(CancelCommandExecute);
        }

        private async Task CancelCommandExecute()
        {
            await _navigationService.ClosePopupsAsync();
        }

        private async Task LogoutCommandExecute()
        {
            ShowLoading();
            App.Locator.AddMoreDetails.LstServices = new List<VehicleServicesRequest>();
            App.Locator.AddMoreDetails.LstServices.Clear();
            App.Locator.AddMoreDetails.TipAmount = "0";
            App.Locator.AddMoreDetails.VerifyPromoCode = null;
            App.Locator.AddStopLocation.StopAddressList.Clear();
            App.Locator.SelectVehicle.LstServices = new List<VehicleServicesRequest>();
            App.Locator.SelectVehicle.LstServices.Clear();
            App.Locator.AddStopLocation.stopid = 0;
            App.Locator.AddStopLocation.StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.PickupSchedule.IsVisibleLaterView = false;
            App.Locator.PickupSchedule.AsapIsChecked = true;
            App.Locator.PickupSchedule.LaterIsCheck = false;
            App.Locator.PickupSchedule.IsButtonEnabled = true;
            App.Locator.PickupSchedule.DuraAddressCommon = null;
            App.Locator.PickupSchedule.PickupScheduleRequest = null;
            App.Locator.PickupLocation.PickupScheduleRequest = null;
            App.Locator.WhereTo.PickupScheduleRequest = null;
            App.Locator.AddStopLocation.PickupScheduleRequest = null;
            App.Locator.DuraExpress.PickupLocationText = string.Empty;
            App.Locator.DuraExpress.PickupLocationTextVisible = false;
            SettingsExtension.PickupAddress = string.Empty;
            App.Locator.DuraExpress.PickupScheduleLocText = string.Empty;
            App.Locator.DuraExpress.PickupScheduleLocTextVisible = false;
            App.Locator.DuraExpress.PickupWhereToText = string.Empty;
            App.Locator.DuraExpress.PickupWhereToTextVisible = false;
            App.Locator.AddMoreDetails.VehicleListSelectedData = null;
            App.Locator.SelectVehicle.VehicleListSelectedData = null;
            SettingsExtension.IsLoggedIn = false;
            App.Locator.PickupLocation.Address1 = string.Empty;
            App.Locator.PickupLocation.Address2 = string.Empty;
            App.Locator.WhereTo.Address1 = string.Empty;
            App.Locator.WhereTo.Address2 = string.Empty;
            App.Locator.AddStopLocation.Address1 = string.Empty;
            App.Locator.AddStopLocation.Address2 = string.Empty;

            App.Current.MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
            await App.Locator.LoginPage.GetAllLocation();
            await _navigationService.ClosePopupsAsync();

            HideLoading();
        }
    }
}

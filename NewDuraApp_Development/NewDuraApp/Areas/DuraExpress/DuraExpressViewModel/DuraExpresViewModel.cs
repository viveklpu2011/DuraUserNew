using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.Services.LocationService;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class DuraExpresViewModel : AppBaseViewModel
    {
        public IAsyncCommand GoToPickupScheduleCmd { get; set; }
        public IAsyncCommand GoToPickupLocationCmd { get; set; }
        public IAsyncCommand GoToAddStopLocationCmd { get; set; }
        public IAsyncCommand GoToSelectVehicleCmd { get; set; }
        public IAsyncCommand GoToWhereCmd { get; set; }
        INavigationService _navigationService;
        public ILocationService locationService => DependencyService.Get<ILocationService>();
        public IPlatformSpecificLocationService platFormLocationService => DependencyService.Get<IPlatformSpecificLocationService>();

        private PickupScheduleResponseModel _pickupScheduleResponse;
        public PickupScheduleResponseModel PickupScheduleResponse
        {
            get { return _pickupScheduleResponse; }
            set { _pickupScheduleResponse = value; OnPropertyChanged(nameof(PickupScheduleResponse)); }
        }
        private IUserCoreService _userCoreService;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        public PickupScheduleRequestModel PickupScheduleRequest
        {
            get { return _pickupScheduleRequest; }
            set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
        }
        private bool _addStopIsVisible;
        private bool _pickupScheduleLocTextVisible;
        private bool _pickupLocationTextVisible;
        private bool _pickupWhereToTextVisible;

        private string _pickupScheduleLocText;
        private string _pickupLocationText;
        private string _pickupWhereToText;

        public bool AddStopIsVisible
        {
            get { return _addStopIsVisible; }
            set { _addStopIsVisible = value; OnPropertyChanged(); }
        }
        public bool PickupScheduleLocTextVisible
        {
            get { return _pickupScheduleLocTextVisible; }
            set { _pickupScheduleLocTextVisible = value; OnPropertyChanged(nameof(PickupScheduleLocTextVisible)); }
        }
        public bool PickupLocationTextVisible
        {
            get { return _pickupLocationTextVisible; }
            set { _pickupLocationTextVisible = value; OnPropertyChanged(nameof(PickupLocationTextVisible)); }
        }
        public bool PickupWhereToTextVisible
        {
            get { return _pickupWhereToTextVisible; }
            set { _pickupWhereToTextVisible = value; OnPropertyChanged(nameof(PickupWhereToTextVisible)); }
        }

        public string PickupScheduleLocText
        {
            get { return _pickupScheduleLocText; }
            set { _pickupScheduleLocText = value; OnPropertyChanged(nameof(PickupScheduleLocText)); }
        }

        public string PickupLocationText
        {
            get { return _pickupLocationText; }
            set { _pickupLocationText = value; OnPropertyChanged(nameof(PickupLocationText)); }
        }
        public string PickupWhereToText
        {
            get { return _pickupWhereToText; }
            set { _pickupWhereToText = value; OnPropertyChanged(nameof(PickupWhereToText)); }
        }

        public DuraExpresViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToPickupScheduleCmd = new AsyncCommand(GoToPickupScheduleCmdExecute, allowsMultipleExecutions: false);
            GoToPickupLocationCmd = new AsyncCommand(GoToPickupLocationCmdExecute, allowsMultipleExecutions: false);
            GoToAddStopLocationCmd = new AsyncCommand(GoToAddStopLocationCmdExecute, allowsMultipleExecutions: false);
            GoToSelectVehicleCmd = new AsyncCommand(GoToSelectVehicleCmdExecute, allowsMultipleExecutions: false);
            GoToWhereCmd = new AsyncCommand(GoToWhereCmdExecute, allowsMultipleExecutions: false);
            MessagingCenter.Subscribe<HomePageViewModel>(this, "FromHomePage", (sender) =>
            {
                AddStopIsVisible = false;
            });
            MessagingCenter.Subscribe<WhereToViewModel>(this, "FromWhereTo", (sender) =>
            {
                AddStopIsVisible = true;
            });
        }

        internal async Task InitilizeData()
        {
            PickupLocationText = string.Empty;
            PickupScheduleLocText = string.Empty;
            PickupWhereToText = string.Empty;

            //await getCurrentPosition();
        }
        public async Task<Location> getCurrentPosition()
        {
            Location location1 = new Location();
            try
            {
                location1 = await Geolocation.GetLastKnownLocationAsync();

                if (location1 != null)
                {
                    Console.WriteLine($"Latitude: {location1.Latitude}, Longitude: {location1.Longitude}, Altitude: {location1.Altitude}");

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //return null;
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                //return null;
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                //return null;
                // Handle permission exception
            }
            catch (Exception ex)
            {
                //return null;
                // Unable to get location
            }
            return location1;
        }
        private async Task GoToAddStopLocationCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddStopLocationViewModel))
            {
                App.DuraExpressTrackOrderWay = "AddStop";
                await _navigationService.NavigateToAsync<AddStopLocationViewModel>();
                await App.Locator.AddStopLocation.InitilizeData();
            }
        }
        private async Task GoToSelectVehicleCmdExecute()
        {
            if (CheckValidation())
            {
                await NavigateSelectVehicleCmdExecute();
            }
            //if (!CheckValidation())
            //{
            //    await NavigateSelectVehicleCmdExecute();
            //}

        }
        private async Task GoToWhereCmdExecute()
        {
            //if (platFormLocationService != null)
            //{
            //    var statusLocationService = platFormLocationService.IsLocationServiceEnabled();
            //    if (!statusLocationService)
            //    {
            //        ShowSettingToast("Location service not enabled", "Settings");
            //        return;
            //    }
            //}
            //var status = await locationService.CheckAndRequestLocationPermission();
            //if (status == PermissionStatus.Granted)
            //{
            //    ShowLoading();
            //    if (!App.Locator.HomePage.IsHavingFromSAvedAddress)
            //    {
            //        await App.Locator.HomePage.GetCurrentLoc();
            //    }
            //    if (_navigationService.GetCurrentPageViewModel() != typeof(WhereToViewModel))
            //    {
            App.DuraExpressTrackOrderWay = "WhereTo";
            await _navigationService.NavigateToAsync<WhereToViewModel>();
            await App.Locator.WhereTo.InitilizeData();
            //    }
            //    HideLoading();
            //}
            //else
            //{
            //    ShowToast("Permission not granted");
            //}


        }
        private async Task GoToPickupScheduleCmdExecute()
        {
            //if (_navigationService.GetCurrentPageViewModel() != typeof(PickupScheduleViewModel))
            //{
            //    await _navigationService.NavigateToAsync<PickupScheduleViewModel>();
            //    await App.Locator.PickupSchedule.InitilizeData();
            //}
            if (_navigationService.GetCurrentPageViewModel() != typeof(PickupSchedulePopUp))
            {
                await PopupNavigation.Instance.PushAsync(new PickupSchedulePopUp());
                await App.Locator.PickupSchedule.InitilizeData();
            }
        }
        private async Task GoToPickupLocationCmdExecute()
        {
            //if (platFormLocationService != null)
            //{
            //    var statusLocationService = platFormLocationService.IsLocationServiceEnabled();
            //    if (!statusLocationService)
            //    {
            //        ShowSettingToast("Location service not enabled", "Settings");
            //        return;
            //    }
            //}
            //var status = await locationService.CheckAndRequestLocationPermission();
            //if (status == PermissionStatus.Granted)
            //{
            //    ShowLoading();
            //    if (!App.Locator.HomePage.IsHavingFromSAvedAddress)
            //    {
            //        await App.Locator.HomePage.GetCurrentLoc();
            //    }
            //if (_navigationService.GetCurrentPageViewModel() != typeof(PickupLocationViewModel))
            //{
            App.DuraExpressTrackOrderWay = "PickupLocation";
            await _navigationService.NavigateToAsync<PickupLocationViewModel>();
            await App.Locator.PickupLocation.InitilizeData();
            //    HideLoading();
            //}

            //}
            //else
            //{
            //    ShowToast("Permission not granted");
            //}
        }
        private bool CheckValidation()
        {
            bool res = true;
            string error = string.Empty;
            if (!PickupScheduleLocTextVisible)
            {
                error = AppResources.Please_select_Pickup_Schedule;
                res = false;
            }
            else if (!PickupLocationTextVisible)
            {
                error = AppResources.Please_select_Pickup_location;
                res = false;
            }
            else if (!PickupWhereToTextVisible)
            {
                error = AppResources.Please_select_desctination_where_to_Final_Drop_location;
                res = false;
            }
            if (!string.IsNullOrEmpty(error))
            {
                ShowToast(error);
            }

            return res;
        }

        private async Task NavigateSelectVehicleCmdExecute()
        {
            if (CheckConnection())
            {
                //if (_navigationService.GetCurrentPageViewModel() != typeof(PaymentViewModel))
                //{

                //    await _navigationService.NavigateToAsync<PaymentViewModel>();
                //    await App.Locator.Payment.InitilizeData();
                //    //MessagingCenter.Send<AddMoreDetailsViewModel>(null, "FromAddMoreDetails");
                //}
                ShowLoading();
                try
                {
                    PickupScheduleReqModel pickupScheduleRequestModel = new PickupScheduleReqModel
                    {
                        destinationlat = PickupScheduleRequest.destinationlat,
                        destinationlon = PickupScheduleRequest.destinationlon,
                        destination_address1 = PickupScheduleRequest.destination_address1,
                        destination_address2 = PickupScheduleRequest.destination_address2,
                        destination_mobile = PickupScheduleRequest.destination_mobile,
                        destination_name = PickupScheduleRequest.destination_name,
                        pdate = PickupScheduleRequest.pdate,
                        pickuplat = PickupScheduleRequest.pickuplat,
                        pickuplon = PickupScheduleRequest.pickuplon,
                        pickup_address1 = PickupScheduleRequest.pickup_address1,
                        pickup_address2 = PickupScheduleRequest.pickup_address2,
                        pickup_mobile = PickupScheduleRequest.pickup_mobile,
                        pickup_name = PickupScheduleRequest.pickup_name,

                        type = PickupScheduleRequest.type,
                        user_id = SettingsExtension.UserId
                    };
                    //stoplat = PickupScheduleRequest.stoplat,
                    //    stoplon = PickupScheduleRequest.stoplon,
                    //    stop_address1 = PickupScheduleRequest.stop_address1,
                    //    stop_address2 = PickupScheduleRequest.stop_address2,
                    //    stop_mobile = PickupScheduleRequest.stop_mobile,
                    //    stop_name = PickupScheduleRequest.stop_name,
                    var result = await TryWithErrorAsync(_userCoreService.PickupSchedule(pickupScheduleRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        App.IsSelectVehicleFromExpress = true;
                        if (_navigationService.GetCurrentPageViewModel() != typeof(SelectVehicleViewModel))
                        {
                            PickupScheduleResponse = new PickupScheduleResponseModel();
                            PickupScheduleResponse = result?.Data;
                            App.Locator.SelectVehicle.InitilizeData();
                            await _navigationService.NavigateToAsync<SelectVehicleViewModel>();

                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                    //ShowAlert(result.Data.message);
                }
                catch (Exception ex)
                {
                    ShowToast(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
    }
}

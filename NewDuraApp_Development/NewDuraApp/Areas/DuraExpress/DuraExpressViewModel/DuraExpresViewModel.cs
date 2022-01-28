using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public IAsyncCommand<PickupScheduleRequestStopModel> HideAddStopView { get; set; }
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

        private bool _isVisibleMapMarkerDestination = true;
        public bool IsVisibleMapMarkerDestination
        {
            get { return _isVisibleMapMarkerDestination; }
            set
            {
                _isVisibleMapMarkerDestination = value;
                OnPropertyChanged(nameof(IsVisibleMapMarkerDestination));
            }
        }

        private bool _addStopIsVisible;
        private bool _pickupScheduleLocTextVisible;
        private bool _pickupLocationTextVisible;
        private bool _pickupWhereToTextVisible;

        private string _pickupScheduleLocText;
        private string _pickupLocationText;
        private string _pickupWhereToText;

        private ObservableCollection<PickupScheduleRequestStopModel> _stopAddressListTemp = new ObservableCollection<PickupScheduleRequestStopModel>();
        public ObservableCollection<PickupScheduleRequestStopModel> StopAddressListTemp
        {
            get { return _stopAddressListTemp; }
            set
            {
                _stopAddressListTemp = value;
                OnPropertyChanged(nameof(StopAddressListTemp));
            }
        }

        private ObservableCollection<PickupScheduleRequestStopModel> _stopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
        public ObservableCollection<PickupScheduleRequestStopModel> StopAddressList
        {
            get { return _stopAddressList; }
            set
            {
                _stopAddressList = value;
                OnPropertyChanged(nameof(StopAddressList));
            }
        }

        private bool _isVisibleAddStopView;
        public bool IsVisibleAddStopView
        {
            get { return _isVisibleAddStopView; }
            set { _isVisibleAddStopView = value; OnPropertyChanged(nameof(IsVisibleAddStopView)); }
        }

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
            HideAddStopView = new AsyncCommand<PickupScheduleRequestStopModel>(HideAddStopViewCommandExecute, allowsMultipleExecutions: false);
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
            IsVisibleMapMarkerDestination = true;
            StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
            StopAddressListTemp = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.AddStopLocation.StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.AddStopLocation.StopAddressListTemp = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.AddStopLocation.stopid = 0;
        }

        internal void CheckStopLocation()
        {
            if (App.Locator.AddStopLocation.PickupScheduleRequest.IsAvailableAddStopLocationLocation)
            {
                StopAddressListTemp = new ObservableCollection<PickupScheduleRequestStopModel>(App.Locator.AddStopLocation.StopAddressList);
                if (StopAddressListTemp != null && StopAddressListTemp.Count > 0)
                {
                    StopAddressListTemp.Where(c => c.IsVisibleLastBottomLine == false).Select(c => { c.IsVisibleLastBottomLine = true; return c; }).ToList();
                    StopAddressListTemp.Last().IsVisibleLastBottomLine = false;
                    StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>(StopAddressListTemp);
                    IsVisibleMapMarkerDestination = false;
                    IsVisibleAddStopView = true;
                }
                else
                    IsVisibleAddStopView = false;
            }
            else
            {
                IsVisibleAddStopView = false;
            }
        }

        private async Task HideAddStopViewCommandExecute(PickupScheduleRequestStopModel arg)
        {
            ShowLoading();
            try
            {
                int id = 0;
                if (arg != null)
                {
                    StopAddressList.RemoveAt(StopAddressList.IndexOf(arg));
                    if (StopAddressList.Count > 0)
                    {
                        StopAddressListTemp = new ObservableCollection<PickupScheduleRequestStopModel>(StopAddressList);
                        StopAddressListTemp.Where(c => c.IsVisibleLastBottomLine == false).Select(c => { c.IsVisibleLastBottomLine = true; return c; }).ToList();
                        StopAddressListTemp.Last().IsVisibleLastBottomLine = false;
                        var finalList = StopAddressList;
                        if (finalList != null && finalList.Count > 0)
                        {
                            var lst = new List<PickupScheduleRequestStopModel>();
                            foreach (var item in finalList)
                            {
                                id = id + 1;
                                item.StopId = id;
                                item.StopText = $"Stop Location {id}";
                                lst.Add(item);
                            }
                            if (lst != null && lst.Count > 0)
                            {
                                StopAddressList.Clear();
                                StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>(lst);
                                App.Locator.AddStopLocation.stopid = id;
                                App.Locator.AddStopLocation.StopAddressList = StopAddressList;
                            }
                        }
                    }
                    else
                    {
                        StopAddressList.Clear();
                        App.Locator.AddStopLocation.stopid = id;
                        App.Locator.AddStopLocation.StopAddressList = StopAddressList;
                        IsVisibleMapMarkerDestination = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            HideLoading();
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
            if (PickupWhereToTextVisible && !string.IsNullOrEmpty(PickupWhereToText))
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(AddStopLocationViewModel))
                {
                    App.DuraExpressTrackOrderWay = "AddStop";
                    await _navigationService.NavigateToAsync<AddStopLocationViewModel>();
                    await App.Locator.AddStopLocation.InitilizeData();
                }
            }
            else
            {
                ShowToast("Please select destination address");
            }
        }

        private async Task GoToSelectVehicleCmdExecute()
        {
            if (CheckValidation())
            {
                await NavigateSelectVehicleCmdExecute();
            }
        }

        private async Task GoToWhereCmdExecute()
        {
            if (PickupLocationTextVisible && !string.IsNullOrEmpty(PickupLocationText))
            {
                App.DuraExpressTrackOrderWay = "WhereTo";
                await _navigationService.NavigateToAsync<WhereToViewModel>();
                await App.Locator.WhereTo.InitilizeData();
            }
            else
            {
                ShowToast("Please select pickup location");
            }
        }

        private async Task GoToPickupScheduleCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(PickupSchedulePopUp))
            {
                await PopupNavigation.Instance.PushAsync(new PickupSchedulePopUp());
                await App.Locator.PickupSchedule.InitilizeData();
            }
        }

        private async Task GoToPickupLocationCmdExecute()
        {
            if (PickupScheduleLocTextVisible && !string.IsNullOrEmpty(PickupScheduleLocText))
            {
                App.DuraExpressTrackOrderWay = "PickupLocation";
                await _navigationService.NavigateToAsync<PickupLocationViewModel>();
                await App.Locator.PickupLocation.InitilizeData();
            }
            else
            {
                ShowToast("Please select pickup schedule first");
            }
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
                ShowLoading();
                try
                {
                    List<PickupScheduleRequestStopModelWithoutId> lstStopAddress = new List<PickupScheduleRequestStopModelWithoutId>();
                    PickupScheduleRequestStopModelWithoutId pickupScheduleRequestStopModelWithoutId = new PickupScheduleRequestStopModelWithoutId();
                    if (StopAddressList != null && StopAddressList.Count > 0)
                    {
                        foreach (var item in StopAddressList)
                        {
                            pickupScheduleRequestStopModelWithoutId = new PickupScheduleRequestStopModelWithoutId();
                            pickupScheduleRequestStopModelWithoutId.stoplat = item.stoplat;
                            pickupScheduleRequestStopModelWithoutId.stoplon = item.stoplon;
                            pickupScheduleRequestStopModelWithoutId.stop_address1 = item.stop_address1;
                            pickupScheduleRequestStopModelWithoutId.stop_address2 = item.stop_address2;
                            pickupScheduleRequestStopModelWithoutId.stop_name = item.stop_name;
                            pickupScheduleRequestStopModelWithoutId.stop_mobile = item.stop_mobile;
                            lstStopAddress.Add(pickupScheduleRequestStopModelWithoutId);
                        }
                    }
                    else
                    {
                        lstStopAddress = null;
                    }
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
                        user_id = SettingsExtension.UserId,
                        stopData = lstStopAddress
                    };
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

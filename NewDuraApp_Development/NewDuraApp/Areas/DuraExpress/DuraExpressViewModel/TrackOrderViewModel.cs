using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class TrackOrderViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToAddStopAddrsCmd { get; set; }
        public IAsyncCommand GoToAddPickupLocationCmd { get; set; }
        public IAsyncCommand GoToAddWhereToCmd { get; set; }
        public IAsyncCommand GoToSelectVehicleCmd { get; set; }
        public IAsyncCommand GoBackCommand { get; set; }
        public IAsyncCommand GoToPickupScheduleCmd { get; set; }
        public IAsyncCommand<PickupScheduleRequestStopModel> HideAddStopView { get; set; }
        private IUserCoreService _userCoreService;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        private DuraAddressCommonModel _duraAddressCommon;
        private PickupScheduleResponseModel _pickupScheduleResponse;
        public PickupScheduleResponseModel PickupScheduleResponse
        {
            get { return _pickupScheduleResponse; }
            set { _pickupScheduleResponse = value; OnPropertyChanged(nameof(PickupScheduleResponse)); }
        }
        public DuraAddressCommonModel DuraAddressCommon
        {
            get { return _duraAddressCommon; }
            set { _duraAddressCommon = value; OnPropertyChanged(nameof(DuraAddressCommon)); }
        }
        public PickupScheduleRequestModel PickupScheduleRequest
        {
            get { return _pickupScheduleRequest; }
            set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
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
        private bool _pickupScheduleLocTextVisible;

        private bool _isVisiblePickupLocationView;
        private bool _isVisibleWhereToView;
        private bool _isEnabledSelectVehicleButton;
        private string _pickupScheduleLocText = "Pick up schedule is missing*";
        private string _pickupScheduleLocTextFontFamily = "ProximaRegular";
        private Color _pickupScheduleLocTextColor = Color.Red;
        private bool _isVisibleAddStopView;
        public bool IsVisibleAddStopView
        {
            get { return _isVisibleAddStopView; }
            set { _isVisibleAddStopView = value; OnPropertyChanged(nameof(IsVisibleAddStopView)); }
        }
        public bool IsEnabledSelectVehicleButton
        {
            get { return _isEnabledSelectVehicleButton; }
            set { _isEnabledSelectVehicleButton = value; OnPropertyChanged(nameof(IsEnabledSelectVehicleButton)); }
        }
        public bool IsVisiblePickupLocationView
        {
            get { return _isVisiblePickupLocationView; }
            set { _isVisiblePickupLocationView = value; OnPropertyChanged(nameof(IsVisiblePickupLocationView)); }
        }
        public bool IsVisibleWhereToView
        {
            get { return _isVisibleWhereToView; }
            set { _isVisibleWhereToView = value; OnPropertyChanged(nameof(IsVisibleWhereToView)); }
        }
        public bool PickupScheduleLocTextVisible
        {
            get { return _pickupScheduleLocTextVisible; }
            set { _pickupScheduleLocTextVisible = value; OnPropertyChanged(nameof(PickupScheduleLocTextVisible)); }
        }

        public string PickupScheduleLocText
        {
            get { return _pickupScheduleLocText; }
            set { _pickupScheduleLocText = value; OnPropertyChanged(nameof(PickupScheduleLocText)); }
        }
        public string PickupScheduleLocTextFontFamily
        {
            get { return _pickupScheduleLocTextFontFamily; }
            set { _pickupScheduleLocTextFontFamily = value; OnPropertyChanged(nameof(PickupScheduleLocTextFontFamily)); }
        }
        public Color PickupScheduleLocTextColor
        {
            get { return _pickupScheduleLocTextColor; }
            set { _pickupScheduleLocTextColor = value; OnPropertyChanged(nameof(PickupScheduleLocTextColor)); }
        }


        public TrackOrderViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToAddStopAddrsCmd = new AsyncCommand(GoToAddStopAddrsCmdExecute, allowsMultipleExecutions: false);
            GoToSelectVehicleCmd = new AsyncCommand(GoToSelectVehicleCmdExecute, allowsMultipleExecutions: false);
            GoToPickupScheduleCmd = new AsyncCommand(GoToPickupScheduleCmdExecute, allowsMultipleExecutions: false);
            GoBackCommand = new AsyncCommand(GoBackCommandExecute, allowsMultipleExecutions: false);
            HideAddStopView = new AsyncCommand<PickupScheduleRequestStopModel>(HideAddStopViewCommandExecute, allowsMultipleExecutions: false);
            GoToAddPickupLocationCmd = new AsyncCommand(GoToAddPickupLocationCmdExecute, allowsMultipleExecutions: false);
            GoToAddWhereToCmd = new AsyncCommand(GoToAddWhereToCmdExecute, allowsMultipleExecutions: false);
        }



        private async Task GoToAddWhereToCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(WhereToViewModel))
            {
                await _navigationService.NavigateToAsync<WhereToViewModel>();
            }
        }

        private async Task GoToAddPickupLocationCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(PickupLocationViewModel))
            {
                await _navigationService.NavigateToAsync<PickupLocationViewModel>();
            }
        }

        private async Task HideAddStopViewCommandExecute(PickupScheduleRequestStopModel arg)
        {
            #region Commented code
            //IsVisibleAddStopView = false;
            //if (!IsVisibleAddStopView)
            //{
            //    PickupScheduleRequest.stoplat = 0;
            //    PickupScheduleRequest.stoplon = 0;
            //    PickupScheduleRequest.stop_address1 = string.Empty;
            //    PickupScheduleRequest.stop_address2 = string.Empty;
            //    PickupScheduleRequest.stop_mobile = string.Empty;
            //    PickupScheduleRequest.stop_name = string.Empty;

            //}
            //CheckButtonEnaled();
            #endregion
            ShowLoading();
            try
            {
                int id = 0;
                if (arg != null)
                {
                    StopAddressList.RemoveAt(StopAddressList.IndexOf(arg));
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
            }
            catch (Exception ex)
            {

            }

            HideLoading();
        }

        internal async Task InitilizeData()
        {
            if (App.Locator.PickupSchedule.DuraAddressCommon != null)
            {
                DuraAddressCommon = new DuraAddressCommonModel();
                DuraAddressCommon = App.Locator.PickupSchedule.DuraAddressCommon;
                PickupScheduleLocTextFontFamily = "ProximaSemiBold";
                PickupScheduleLocTextColor = Color.Black;
                PickupScheduleLocTextVisible = true;
            }
            else
            {
                PickupScheduleLocTextFontFamily = "ProximaRegular";
                PickupScheduleLocTextColor = Color.Red;
                PickupScheduleLocTextVisible = true;
            }
            PickupScheduleRequest = new PickupScheduleRequestModel();
            PickupScheduleRequest = App.Locator.WhereTo.PickupScheduleRequest;
            if (PickupScheduleRequest != null)
            {
                if (PickupScheduleRequest.IsAvailablePickupLocation)
                {
                    IsVisiblePickupLocationView = true;
                }
                else
                {
                    IsVisiblePickupLocationView = false;
                }

                if (PickupScheduleRequest.IsAvailableAddStopLocationLocation)
                {
                    IsVisibleAddStopView = true;
                    StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>(App.Locator.AddStopLocation.StopAddressList);
                }
                else
                {
                    IsVisibleAddStopView = false;
                }

                if (PickupScheduleRequest.IsAvailableWhereToLocation)
                {
                    IsVisibleWhereToView = true;
                }
                else
                {
                    IsVisibleWhereToView = false;
                }

            }

            CheckButtonEnaled();



        }

        private void CheckButtonEnaled()
        {
            if (IsVisiblePickupLocationView && IsVisibleWhereToView && PickupScheduleLocTextColor == Color.Black)
            {
                IsEnabledSelectVehicleButton = true;
            }
            else
            {
                IsEnabledSelectVehicleButton = false;
            }
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
        private async Task GoBackCommandExecute()
        {
            if (App.DuraExpressTrackOrderWay == "PickupLocation")
            {
                await _navigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                await _navigationService.RemoveLastFromBackStackAsyncForResetPassword();
            }
        }

        private async Task GoToAddStopAddrsCmdExecute()
        {
            if (StopAddressList != null && StopAddressList.Count >= 3)
            {
                ShowToast(AppResources.You_can_add_maximum_three_stop_location);
                return;
            }
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddStopLocationViewModel))
            {

                await _navigationService.NavigateToAsync<AddStopLocationViewModel>();
                await App.Locator.AddStopLocation.InitilizeData();
            }
        }
        private async Task GoToSelectVehicleCmdExecute()
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
                        pdate = DuraAddressCommon.PickupDate,
                        pickuplat = PickupScheduleRequest.pickuplat,
                        pickuplon = PickupScheduleRequest.pickuplon,
                        pickup_address1 = PickupScheduleRequest.pickup_address1,
                        pickup_address2 = PickupScheduleRequest.pickup_address2,
                        pickup_mobile = PickupScheduleRequest.pickup_mobile,
                        pickup_name = PickupScheduleRequest.pickup_name,
                        type = DuraAddressCommon.PickupType.ToLower(),
                        user_id = SettingsExtension.UserId,
                        stopData = lstStopAddress
                    };

                    var result = await TryWithErrorAsync(_userCoreService.PickupSchedule(pickupScheduleRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (_navigationService.GetCurrentPageViewModel() != typeof(SelectVehicleViewModel))
                        {
                            PickupScheduleResponse = new PickupScheduleResponseModel();
                            PickupScheduleResponse = result?.Data;
                            await _navigationService.NavigateToAsync<SelectVehicleViewModel>();
                            App.Locator.SelectVehicle.InitilizeData();
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
                    //ShowToast(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
    }
}

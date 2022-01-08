using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Areas.DuraExpress.Popup.ViewModel;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class OrderDetailsViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand ShareCmd { get; set; }
        private IUserCoreService _userCoreService;
        public IAsyncCommand GoToOrederDetails2Cmd { get; set; }
        public IAsyncCommand GoToFeeBreakdownPopup { get; set; }

        public IAsyncCommand DuraHelpCommand { get; set; }
        public IAsyncCommand CallNowCommand { get; set; }
        public IAsyncCommand SMSCommand { get; set; }
        public IAsyncCommand BackButtonPressed { get; set; }
        public IAsyncCommand LocationLinkTapCommand { get; set; }
        private byte[] _productImage;
        public byte[] ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
        }
        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }

        private bool _isVisibleDriverView = true;
        public bool IsVisibleDriverView
        {
            get { return _isVisibleDriverView; }
            set
            {
                _isVisibleDriverView = value;
                OnPropertyChanged(nameof(IsVisibleDriverView));
            }
        }

        private bool _isVisibleCouponView;
        public bool IsVisibleCouponView
        {
            get { return _isVisibleCouponView; }
            set
            {
                _isVisibleCouponView = value;
                OnPropertyChanged(nameof(IsVisibleCouponView));
            }
        }

        private string _sharingLine = "https://wwww.google.com";
        public string SharingLink
        {
            get { return _sharingLine; }
            set
            {
                _sharingLine = value;
                OnPropertyChanged(nameof(SharingLink));
            }
        }


        private GetpickupDetailsModel _getpickupData;
        public GetpickupDetailsModel GetPickupData
        {
            get { return _getpickupData; }
            set
            {
                _getpickupData = value;
                OnPropertyChanged(nameof(GetPickupData));
            }
        }
        public OrderDetailsViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            LocationLinkTapCommand = new AsyncCommand(LocationLinkTapCommandExecute, allowsMultipleExecutions: false);
            GoToOrederDetails2Cmd = new AsyncCommand(GoToOrederDetails2CmdExecute, allowsMultipleExecutions: false);
            CallNowCommand = new AsyncCommand(CallNowCommandExecute, allowsMultipleExecutions: false);
            SMSCommand = new AsyncCommand(SMSCommandExecute, allowsMultipleExecutions: false);
            BackButtonPressed = new AsyncCommand(BackButtonPressedExecute, allowsMultipleExecutions: false);
            GoToFeeBreakdownPopup = new AsyncCommand(GoToFeeBreakdownPopupExecute, allowsMultipleExecutions: false);
            ShareCmd = new AsyncCommand(ShareCmdExecute, allowsMultipleExecutions: false);
            DuraHelpCommand = new AsyncCommand(DuraHelp, allowsMultipleExecutions: false);
        }

        private async Task DuraHelp()
        {
            //ShowAlert("We are Working on it.It is in Progress.", "DuraDrive", "Ok");
            await _navigationService.NavigateToAsync<ChatViewModel>();
        }

        private async Task LocationLinkTapCommandExecute()
        {
            ShowAlert(AppResources.We_are_working_with_google_for_tracking_link_Backend_Team_work_is_inProgress);
            await Browser.OpenAsync(GetPickupData.driverlocationlink, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#211E66"),
                PreferredControlColor = Color.White
            });
        }

        private async Task ShareCmdExecute()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = GetPickupData?.driverlocationlink,
                Title = AppResources.Track_Dura_Express_Order
            });
        }

        private async Task BackButtonPressedExecute()
        {
            App.Locator.AddStopLocation.StopAddressList.Clear();
            App.Locator.AddStopLocation.stopid = 0;
            App.Locator.AddStopLocation.StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
            App.Locator.AddMoreDetails.LstServices = new List<VehicleServicesRequest>();
            App.Locator.AddMoreDetails.LstServices.Clear();
            App.Locator.SelectVehicle.LstServices = new List<VehicleServicesRequest>();
            App.Locator.SelectVehicle.LstServices.Clear();
            App.Locator.AddMoreDetails.TipAmount = "0";
            App.Locator.TrackOrder.StopAddressList.Clear();
            App.Locator.AddMoreDetails.VehicleListSelectedData = null;
            App.Locator.AddMoreDetails.VerifyPromoCode = null;
            App.Locator.SelectVehicle.VehicleListSelectedData = null;
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
            App.Locator.PickupLocation.Address2 = string.Empty;
            App.Locator.WhereTo.Address2 = string.Empty;
            App.Locator.AddStopLocation.Address2 = string.Empty;
            if (_navigationService.GetCurrentPageViewModel() != typeof(HomePageViewModel))
            {
                TabNavigationHelper.ForceFullyRedirectingTab(0);
            }

        }

        private async Task SMSCommandExecute()
        {
            await PhoneHelper.SendSms($"Hi this is {GetPickupData?.username}, have booked your {GetPickupData?.vehicle}", GetPickupData?.drivermobile);
        }

        private async Task CallNowCommandExecute()
        {
            PhoneHelper.PlacePhoneCall(GetPickupData?.drivermobile);
        }

        private async Task GoToOrederDetails2CmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetails2ViewModel))
            {
                await App.Locator.OrderDetails2.InitilizeData(GetPickupData);
                await _navigationService.NavigateToAsync<OrderDetails2ViewModel>();
            }
        }
        private async Task GoToFeeBreakdownPopupExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(FeeBreakdownPopupViewModel))
            {
                await PopupNavigation.Instance.PushAsync(new FeeBreakdownPopup());
                await App.Locator.FeeBreakdownPopup.InitilizeData(GetPickupData);
            }
        }
        internal async Task InitilizeData()
        {
            await GetPickupDetails();
        }
        private async Task GetPickupDetails()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel
                    {
                        user_id = SettingsExtension.UserId
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetPickupDetails(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        if (result?.Data?.data != null)
                        {
                            GetPickupData = result?.Data?.data;

                            if (GetPickupData?.status == "1")
                            {
                                GetPickupData.displaystatustextcolor = Color.Red;
                                GetPickupData.displaystatus = AppResources.Pending_OrderNotPickedYet;
                            }
                            if (GetPickupData?.status == "2")
                            {
                                GetPickupData.displaystatustextcolor = Color.Green;
                                GetPickupData.displaystatus = AppResources.Ongoing_OrderPicked;
                            }
                            if (GetPickupData?.status == "3")
                            {
                                GetPickupData.displaystatustextcolor = Color.Red;
                                GetPickupData.displaystatus = AppResources.Completed_OrderDelivered;
                            }
                            if (GetPickupData?.status == "4")
                            {
                                GetPickupData.displaystatustextcolor = Color.Red;
                                GetPickupData.displaystatus = AppResources.Cancelled_OrderCancelled;
                            }

                            if (result?.Data?.data?.coupon != null)
                            {
                                IsVisibleCouponView = true;
                            }
                            else
                            {
                                IsVisibleCouponView = false;
                            }
                            if (!App.Locator.PickupSchedule.DuraAddressCommon.PickupType.ToLower().Contains("later"))
                            {
                                IsVisibleDriverView = true;
                                //ProductImage = string.IsNullOrEmpty(GetPickupData?.driverphoto) ? await ImageHelper.GetStreamFormResource("camera.png") : await ImageHelper.GetImageFromUrl(GetPickupData?.driverphoto);
                                //ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
                            }
                            else
                            {
                                IsVisibleDriverView = false;
                            }
                            ShowAlert("Your Code for Sharing Driver :" + GetPickupData.verification_code, "Driver Code", "OK");
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        ShowToast(AppResources.No_Data_Found);
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
        ////public ICommand CallNowCommand => new Command(async (obj) =>
        ////{
        ////    try
        ////    {
        ////        PhoneDialer.Open(Phone);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        UserDialogs.Instance.Alert(ex.Message);
        ////    }
        ////});
        //private string _phone;
        //public string Phone
        //{
        //    get { return _phone; }
        //    set { _phone = value; OnPropertyChanged(); }
        //}
        //public OrderDetailsViewModel()
        //{
        //    Phone = "323234234";
        //}
        //public ICommand GoToOrederDetails2Cmd => new Command(async (obj) =>
        //{
        //    await RichNavigation.PushAsync(new OrderDetails2(), typeof(OrderDetails2));
        //});
        //public ICommand GoToFeeBreakdownPopup => new Command(async (obj) =>
        //{
        //    await PopupNavigation.PushAsync(new FeeBreakdownPopup());
        //});
        //async void btnShare_Clicked(object sender, System.EventArgs e)
        //{
        //    await ShareText(txtText.Text);
        //}

        //public ICommand ShareCmd => new Command(async (obj) =>
        //{
        //    await Share.RequestAsync(new ShareTextRequest
        //    {
        //        Subject = "This is a test message",
        //        Text = "This is my URL",
        //        Title = "Title test message",
        //        Uri = "https://askxammy.com"
        //    });
        //});


    }
}

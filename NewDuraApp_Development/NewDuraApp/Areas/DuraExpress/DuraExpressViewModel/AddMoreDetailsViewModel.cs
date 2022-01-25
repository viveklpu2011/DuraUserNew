using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class AddMoreDetailsViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand GoToPaymentCmd { get; set; }
        public IAsyncCommand GoToOfferCmd { get; set; }
        public IAsyncCommand GoToApplyCmd { get; set; }
        private VerifyPromoCodeData _verifyPromoCode;
        private double _totalFare;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        private VehicleListData _vehicleListSelectedData;
        private PickupScheduleResponseModel _pickupScheduleResponse;
        public IAsyncCommand PickImageCommand { get; set; }
        public IAsyncCommand RemovePromoCodeCommand { get; set; }
        private bool _isVisiblePhotoButton = true;
        private DuraAddressCommonModel _duraAddressCommon;
        private string _tipAmount = "0";
        public IAsyncCommand ManualEntryCommand { get; set; }
        private ObservableCollection<TipPriceModel> _tipPriceList;
        public ObservableCollection<TipPriceModel> TipPriceList
        {
            get { return _tipPriceList; }
            set
            {
                _tipPriceList = value;
                OnPropertyChanged(nameof(TipPriceList));
            }
        }
        private List<VehicleServicesRequest> _lstServices = new List<VehicleServicesRequest>();
        public List<VehicleServicesRequest> LstServices
        {
            get { return _lstServices; }
            set
            {
                _lstServices = value;
                OnPropertyChanged(nameof(LstServices));
            }
        }
        private List<PriceBreakUpList> _PriceBreakUpList = new List<PriceBreakUpList>();
        public List<PriceBreakUpList> PriceBreakUpList
        {
            get { return _PriceBreakUpList; }
            set
            {
                _PriceBreakUpList = value;
                OnPropertyChanged(nameof(PriceBreakUpList));
            }
        }
        private string _addNote = string.Empty;
        private string _pType;

        private bool _isVisiblePromoCodeStack = true;
        public bool IsVisiblePromoCodeStack
        {
            get { return _isVisiblePromoCodeStack; }
            set { _isVisiblePromoCodeStack = value; OnPropertyChanged(nameof(IsVisiblePromoCodeStack)); }
        }
        private string _totalFinalFare;
        public string TotalFinalFare
        {
            get { return _totalFinalFare; }
            set { _totalFinalFare = value; OnPropertyChanged(nameof(TotalFinalFare)); }
        }
        public string PType
        {
            get { return _pType; }
            set { _pType = value; OnPropertyChanged(nameof(PType)); }
        }
        public string AddNote
        {
            get { return _addNote; }
            set { _addNote = value; OnPropertyChanged(nameof(AddNote)); }
        }

        public DuraAddressCommonModel DuraAddressCommon
        {
            get { return _duraAddressCommon; }
            set { _duraAddressCommon = value; OnPropertyChanged(nameof(DuraAddressCommon)); }
        }
        public string TipAmount
        {
            get { return _tipAmount; }
            set
            {
                _tipAmount = value;
                OnPropertyChanged(nameof(TipAmount));
            }
        }
        public bool IsVisiblePhotoButton
        {
            get { return _isVisiblePhotoButton; }
            set { _isVisiblePhotoButton = value; OnPropertyChanged(nameof(IsVisiblePhotoButton)); }
        }

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

        public double TotalFare
        {
            get { return _totalFare; }
            set { _totalFare = value; OnPropertyChanged(nameof(TotalFare)); }
        }
        public VerifyPromoCodeData VerifyPromoCode
        {
            get { return _verifyPromoCode; }
            set { _verifyPromoCode = value; OnPropertyChanged(nameof(VerifyPromoCode)); }
        }
        public VehicleListData VehicleListSelectedData
        {
            get { return _vehicleListSelectedData; }
            set { _vehicleListSelectedData = value; OnPropertyChanged(nameof(VehicleListSelectedData)); }
        }

        public PickupScheduleRequestModel PickupScheduleRequest
        {
            get { return _pickupScheduleRequest; }
            set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
        }
        public PickupScheduleResponseModel PickupScheduleResponse
        {
            get { return _pickupScheduleResponse; }
            set { _pickupScheduleResponse = value; OnPropertyChanged(nameof(PickupScheduleResponse)); }
        }
        private ObservableCollection<AddMoreDetailsModel> _accountType;

        public ObservableCollection<AddMoreDetailsModel> AccountType
        {
            get { return _accountType; }
            set { _accountType = value; OnPropertyChanged(); }
        }
        private AddMoreDetailsModel _selectedAccount;

        public AddMoreDetailsModel SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;

                OnPropertyChanged(nameof(SelectedAccount));
            }
        }
        private int _index = 0;

        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged(nameof(Index)); }
        }

        private bool _isVisibleManualTipEntry;
        public bool IsVisibleManualTipEntry
        {
            get { return _isVisibleManualTipEntry; }
            set
            {
                _isVisibleManualTipEntry = value;
                OnPropertyChanged(nameof(IsVisibleManualTipEntry));
            }
        }

        public AddMoreDetailsViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            ManualEntryCommand = new AsyncCommand(ManualEntryCommandExecute, allowsMultipleExecutions: false);
            GoToPaymentCmd = new AsyncCommand(GoToPaymentCmdExecute, allowsMultipleExecutions: false);
            GoToOfferCmd = new AsyncCommand(GoToOfferCmdExecute, allowsMultipleExecutions: false);
            GoToApplyCmd = new AsyncCommand(GoToApplyCmdExecute, allowsMultipleExecutions: false);
            RemovePromoCodeCommand = new AsyncCommand(RemovePromoCodeCommandExecute, allowsMultipleExecutions: false);
            PickImageCommand = new AsyncCommand(PickImage, allowsMultipleExecutions: false);
            _tipAmount = "0";
        }

        private async Task ManualEntryCommandExecute()
        {
            IsVisibleManualTipEntry = !IsVisibleManualTipEntry;
            await Task.CompletedTask;
        }

        private async Task RemovePromoCodeCommandExecute()
        {
            TotalFare = Math.Round(App.Locator.SelectVehicle.TotalFare);
            TotalFinalFare = Convert.ToString(Math.Round(Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare) + Convert.ToDouble(TipAmount)));
            AppConstant.Discount = 0;
            VerifyPromoCode = null;
            AppConstant.TotalFinalFare = TotalFinalFare;
            IsVisiblePromoCodeStack = true;
            await Task.CompletedTask;
        }

        internal async Task InitilizeData()
        {

            var promoData = VerifyPromoCode;
            if (VerifyPromoCode != null)
            {
                IsVisiblePromoCodeStack = false;
                var discount = VerifyPromoCode?.discount;
                var totalDiscount = App.Locator.SelectVehicle.TotalFare - (App.Locator.SelectVehicle.TotalFare * (Convert.ToDouble(discount) / 100));
                TotalFare = totalDiscount;
                TotalFinalFare = Math.Round(TotalFare + Convert.ToDouble(TipAmount)).ToString("N2");
                AppConstant.Discount = totalDiscount;
                AppConstant.TotalFinalFare = Convert.ToString(Math.Round(Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare)));
            }
            //else
            //{
            //    //AccountType = GetAccountType();
            //    //Index = 0;
            //    //AddNote = string.Empty;
            //    //IsVisiblePhotoButton = true;
            //    //ProductImage = null;
            //    //PickupScheduleRequest = App.Locator.SelectVehicle.PickupScheduleRequest;
            //    //PickupScheduleResponse = App.Locator.SelectVehicle.PickupScheduleResponse;
            //    //VehicleListSelectedData = App.Locator.SelectVehicle.VehicleListSelectedData;
            //    //LstServices = App.Locator.SelectVehicle.LstServices;
            //    //TotalFare = Math.Round(App.Locator.SelectVehicle.TotalFare);
            //    //TotalFinalFare = Convert.ToString(Math.Round(Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare)));
            //    //string defaultImage = "img_placeholder.png";
            //    //ProductImage = await ImageHelper.GetStreamFormResource(defaultImage);
            //    //ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
            //    //PType = App.Locator.TrackOrder.DuraAddressCommon.PickupType;
            //    //await GetTipPrice();
            //}

        }
        internal async Task InitializeDataNew()
        {
            App.Locator.AddMoreDetails.RemovePromoCodeCommand.Execute(null);
            _tipAmount = "0";
            AccountType = GetAccountType();
            Index = 0;
            AddNote = string.Empty;
            IsVisiblePhotoButton = true;
            ProductImage = null;
            PickupScheduleRequest = App.Locator.SelectVehicle.PickupScheduleRequest;
            PickupScheduleResponse = App.Locator.SelectVehicle.PickupScheduleResponse;
            VehicleListSelectedData = App.Locator.SelectVehicle.VehicleListSelectedData;
            LstServices = App.Locator.SelectVehicle.LstServices;
            TotalFare = Math.Round(App.Locator.SelectVehicle.TotalFare);
            TotalFinalFare = Convert.ToString(Math.Round(Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare)));
            string defaultImage = "img_placeholder.png";
            ProductImage = await ImageHelper.GetStreamFormResource(defaultImage);
            ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
            PType = App.Locator.TrackOrder.DuraAddressCommon.PickupType;
            await GetTipPrice();
        }
        private async Task GetTipPrice()
        {
            if (CheckConnection())
            {

                try
                {
                    ShowLoading();
                    var result = await TryWithErrorAsync(_userCoreService.TipPrice(SettingsExtension.Token), true, false);
                    HideLoading();
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            TipPriceList = new ObservableCollection<TipPriceModel>();
                            foreach (var item in result?.Data?.data)
                            {
                                TipPriceList.Add(item);
                            }
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    //ShowAlert(result?.Data?.message, result?.Data?.message);
                }
                catch (Exception ex)
                {
                    ShowAlert(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
        private async Task PickImage()
        {
            //var res = await ShowCameraActionSheet();
            var res = await ShowCameraPopup();
            if (res != null)
            {
                if (res.Item1 == ProfilePicSelectionType.Camera)
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        ShowAlert("No Camera", ":( No camera avaialble.", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                        Directory = "Dura",
                        CompressionQuality = 92,
                        Name = "test.jpg"
                    });

                    if (file == null)
                    {
                        IsVisiblePhotoButton = true;
                        return;
                    }
                    else
                    {
                        IsVisiblePhotoButton = false;
                    }

                    ProfileImage = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });
                    ProductImage = ImageHelper.ReadToEnd(file.GetStream());
                }
                else if (res.Item1 == ProfilePicSelectionType.Gallery)
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        ShowAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                        return;
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                            CompressionQuality = 82
                        });
                        if (file == null)
                        {
                            IsVisiblePhotoButton = true;
                            return;
                        }
                        else
                        {
                            IsVisiblePhotoButton = false;
                        }
                        ProfileImage = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImage = ImageHelper.ReadToEnd(file.GetStream());
                    });

                }
            }

            //   var result = await ShowConfirmationAlert("Please select below one", "Edit Photo", "Pick from gallery", "Pick from camera");
            //if (result) {

            //} else {

            //}
        }

        private async Task GoToApplyCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ApplyPromoCodePageViewModel))
            {
                await _navigationService.NavigateToAsync<ApplyPromoCodePageViewModel>();
            }
        }
        private async Task GoToPaymentCmdExecute()
        {
            await AddMoreDetailsCommandExecute();
        }
        private async Task GoToOfferCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(OfferAndPromocodePageViewModel))
            {
                App.IsVisiblePromoEntry = true;
                await App.Locator.OfferAndPromocodePage.InitilizeData();
                await _navigationService.NavigateToAsync<OfferAndPromocodePageViewModel>();
            }
        }
        private ObservableCollection<AddMoreDetailsModel> GetAccountType()
        {
            return new ObservableCollection<AddMoreDetailsModel>()
            {
                new AddMoreDetailsModel(){Account="Personal",Index=0},
                new AddMoreDetailsModel(){Account="Corporate",Index=1}
            };
            Index = 0;
        }
        private async Task AddMoreDetailsCommandExecute()
        {
            if (CheckConnection())
            {
                if (TipAmount == string.Empty)
                    TipAmount = "0";
                var amount = Convert.ToString(TipAmount);
                AppConstant.TotalFinalFare = TotalFare.ToString();

                AppConstant.TipAmount = Convert.ToDouble(TipAmount);
                if (!string.IsNullOrEmpty(amount) && amount != "0")
                {
                    if (!Regex.Match(amount, @"^0|[1-9]\d*$").Success)
                    {
                        ShowToast(AppResources.Please_enter_valid_amount);
                        return;
                    }
                }

                try
                {
                    string services = string.Empty;
                    if (LstServices != null && LstServices.Count > 0)
                    {
                        services = JsonConvert.SerializeObject(LstServices);
                    }
                    else
                    {
                        services = "no";
                    }
                    var form = new MultipartFormDataContent();

                    if (ProductImage != null)
                    {
                        form.Add(new ByteArrayContent(ProductImage), "itemphoto", $"ProductImage_{CommonHelper.GenerateRandomId(5)}.jpg");
                    }
                    else
                    {
                        form.Add(new StringContent(""), "itemphoto", $"no.jpg");
                    }

                    form.Add(new StringContent(Convert.ToString(VehicleListSelectedData?.id)), "vehicle_id");
                    form.Add(new StringContent(Convert.ToString(App.Locator.SelectVehicle.TotalFare)), "price");
                    if (!string.IsNullOrEmpty(AddNote))
                    {
                        form.Add(new StringContent(AddNote.Trim()), "drivernote");
                    }
                    else
                    {
                        form.Add(new StringContent(""), "drivernote");
                    }
                    form.Add(new StringContent(Convert.ToString(TipAmount)), "tip");
                    form.Add(new StringContent(PType.Trim()), "itemtype");
                    if (VerifyPromoCode != null)
                    {
                        form.Add(new StringContent(VerifyPromoCode?.name.Trim()), "coupon");
                        form.Add(new StringContent(VerifyPromoCode?.discount.Trim()), "discount");
                        var total = $"{TotalFare + Convert.ToDouble(TipAmount)}";
                        form.Add(new StringContent(Convert.ToString(Math.Round(Convert.ToDouble(total)))), "finalprice");
                    }
                    else
                    {
                        form.Add(new StringContent(""), "coupon");
                        form.Add(new StringContent(""), "discount");
                        form.Add(new StringContent(Convert.ToString(TotalFare)), "finalprice");
                    }

                    var lator = App.Locator.PickupSchedule.DuraAddressCommon.PickupType.ToLower();
                    form.Add(new StringContent(lator), "type");
                    form.Add(new StringContent(Convert.ToString(PickupScheduleResponse.data)), "pickup_id");
                    form.Add(new StringContent(services), "services");
                    if (SelectedAccount != null)
                    {
                        form.Add(new StringContent(SelectedAccount?.Account), "acctype");
                    }
                    else
                    {
                        form.Add(new StringContent(""), "acctype");
                    }
                    PriceBreakUpList = new List<PriceBreakUpList>();
                    var fare = Convert.ToDouble(App.Locator.SelectVehicle.TotalFinalFare) - (Convert.ToDouble(AppConstant.SelectedVehicle.basefare) + LstServices.Sum(x => Convert.ToInt32(x.ServicesFee)));
                    var distance = Convert.ToString(fare / Convert.ToDouble(AppConstant.SelectedVehicle.kmfare));
                    PriceBreakUpList.Add(new PriceBreakUpList() { base_fare= App.Locator.SelectVehicle.VehicleListSelectedData.basefare,tip= TipAmount,discount= VerifyPromoCode?.discount.Trim(),distance=distance,finalprice= Convert.ToString(TotalFare),price= Convert.ToString(fare),km_fare= AppConstant.SelectedVehicle.kmfare });
                    
                    form.Add(new StringContent(JsonConvert.SerializeObject(PriceBreakUpList)), "price_breakup");
                    ShowLoading();
                    var result = await TryWithErrorAsync(_userCoreService.AddMoreDetailsPickup(form, SettingsExtension.Token), true, false);

                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        form.Dispose();
                        if (_navigationService.GetCurrentPageViewModel() != typeof(PaymentViewModel))
                        {
                            if (VerifyPromoCode != null)
                            {
                                if (CheckConnection())
                                {

                                    try
                                    {
                                        VerifyPromoCodeRequestModel verifyPromoCodeRequestModel = new VerifyPromoCodeRequestModel
                                        {
                                            promocode = VerifyPromoCode?.name.Trim().Trim(),
                                            user_id = SettingsExtension.UserId
                                        };
                                        var resultpromo = await TryWithErrorAsync(_userCoreService.VerifyPromoCode(verifyPromoCodeRequestModel, SettingsExtension.Token), true, false);
                                        if (resultpromo?.ResultType == ResultType.Ok && resultpromo?.Data?.status == 200)
                                        {
                                            ShowToast(AppResources.Promo_code_applied_successfully);
                                        }
                                        else
                                        {
                                            ShowAlert(resultpromo?.Data?.message);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //ShowToast(CommonMessages.ServerError);
                                    }

                                }
                                else
                                    ShowToast(CommonMessages.NoInternet);
                            }
                            await _navigationService.NavigateToAsync<PaymentViewModel>();
                            await App.Locator.Payment.InitilizeData();
                            MessagingCenter.Send<AddMoreDetailsViewModel>(this, "FromAddMoreDetails");
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        ShowAlert(AppResources.Your_order_location_is_not_selected_right_Please_recheck_your_order_confirm_location_pickup_dropup_again, "DruaDrive", AppResources.Ok);
                    }

                    //ShowAlert(result?.Data?.message, result?.Data?.message);
                }
                catch (Exception ex)
                {
                    ShowAlert(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
    }
}

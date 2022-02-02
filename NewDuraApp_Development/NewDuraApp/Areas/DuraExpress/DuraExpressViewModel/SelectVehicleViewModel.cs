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
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class SelectVehicleViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        List<ServicesModel> objServices = new List<ServicesModel>();
        private IUserCoreService _userCoreService;
        public IAsyncCommand GoToAddMoreDetailsCmd { get; set; }

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

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }

        public IAsyncCommand<VehicleListData> VehicleDetailsCommand { get; set; }
        public IAsyncCommand<AdditionalServices> AdditionalServiceDetailsCommand { get; set; }
        private AdditionalServices _additionalServicesData;
        private bool _isVisibleStack, _isVisibleLoader;
        List<AdditionalServices> _listServices = new List<AdditionalServices>();
        private bool _isVisibleInfoLabel = true;
        private double _totalFare;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        private VehicleListData _vehicleListSelectedData;
        private PickupScheduleResponseModel _pickupScheduleResponse;

        private string _totalFinalFare;
        public string TotalFinalFare
        {
            get { return _totalFinalFare; }
            set { _totalFinalFare = value; OnPropertyChanged(nameof(TotalFinalFare)); }
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

        public double TotalFare
        {
            get { return _totalFare; }
            set { _totalFare = value; OnPropertyChanged(nameof(TotalFare)); }
        }

        public bool IsVisibleStack
        {
            get { return _isVisibleStack; }
            set { _isVisibleStack = value; OnPropertyChanged(nameof(IsVisibleStack)); }
        }

        public AdditionalServices AdditionalServicesData
        {
            get { return _additionalServicesData; }
            set { _additionalServicesData = value; OnPropertyChanged(nameof(AdditionalServicesData)); }
        }

        public bool IsVisibleLoader
        {
            get { return _isVisibleLoader; }
            set { _isVisibleLoader = value; OnPropertyChanged(nameof(IsVisibleLoader)); }
        }

        public VehicleListData VehicleListSelectedData
        {
            get { return _vehicleListSelectedData; }
            set { _vehicleListSelectedData = value; OnPropertyChanged(nameof(VehicleListSelectedData)); }
        }

        public bool IsVisibleInfoLabel
        {
            get { return _isVisibleInfoLabel; }
            set { _isVisibleInfoLabel = value; OnPropertyChanged(nameof(IsVisibleInfoLabel)); }
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

        private ObservableCollection<VehicleListData> _vehicleList;
        public ObservableCollection<VehicleListData> VehicleList
        {
            get { return _vehicleList; }
            set { _vehicleList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<VehicleListData> _vehicleListTem = new ObservableCollection<VehicleListData>();
        public ObservableCollection<VehicleListData> VehicleListTemp
        {
            get { return _vehicleListTem; }
            set { _vehicleListTem = value; OnPropertyChanged(); }
        }

        public SelectVehicleViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            objServices = new List<ServicesModel>();
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToAddMoreDetailsCmd = new AsyncCommand(GoToAddMoreDetailsCmdExecute, allowsMultipleExecutions: false);
            VehicleDetailsCommand = new AsyncCommand<VehicleListData>(VehicleDetailsCommandExecute, allowsMultipleExecutions: false);
            AdditionalServiceDetailsCommand = new AsyncCommand<AdditionalServices>(AdditionalServicesCommandExecute, allowsMultipleExecutions: false);
        }

        public async Task AdditionalServicesCommandExecute(AdditionalServices arg)
        {
            if (arg != null)
            {
                if (_listServices != null)
                {
                    var isExist = _listServices.Where(x => x.id == arg.id).FirstOrDefault();
                    if (isExist != null)
                    {
                        _listServices.Remove(arg);
                        var isSidExist = LstServices.Where(x => x.service_id == arg.id).FirstOrDefault();
                        if (isSidExist != null)
                        {
                            LstServices.Remove(isSidExist);
                            AppConstant.ServiceList.Remove(AppConstant.ServiceList?.Where(x => x.id == arg.id).FirstOrDefault());
                        }
                        TotalFare -= Convert.ToDouble(arg?.price);
                        TotalFinalFare = TotalFare.ToString("N2");
                    }
                    else
                    {
                        _listServices.Add(arg);
                        LstServices.Add(new VehicleServicesRequest() { service_id = arg.id, services = arg.service, services_fee = arg.price.ToString() });
                        objServices.Add(new ServicesModel { id = arg.id, services = arg.service, servicefee = arg.price.ToString() });
                        AppConstant.ServiceList = objServices;
                        TotalFare += Convert.ToDouble(arg?.price);
                        TotalFinalFare = Math.Round(TotalFare).ToString("N2");
                        AppConstant.TotalFinalFare = TotalFinalFare;
                    }
                }
            }
        }

        private async Task VehicleDetailsCommandExecute(VehicleListData arg)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                IsVisibleLoader = true;
                if (arg != null)
                {
                    ShowLoading();
                    VehicleListSelectedData = new VehicleListData();
                    _listServices = new List<AdditionalServices>();
                    IsVisibleInfoLabel = false;
                    IsVisibleStack = true;
                    VehicleListSelectedData = arg;
                    IsVisibleLoader = false;
                    TotalFare = 0;
                    Currency = arg?.currency;
                    TotalFare = Convert.ToDouble(arg.totalfare);
                    TotalFinalFare = Math.Round(TotalFare).ToString("N2");
                    AppConstant.SelectedVehicle = arg;
                    var tempListData = VehicleListTemp.ToList();
                    tempListData.RemoveAt(tempListData.IndexOf(arg));
                    VehicleList = new ObservableCollection<VehicleListData>(tempListData);
                    HideLoading();
                }
            });
        }

        private async Task GoToAddMoreDetailsCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddMoreDetailsViewModel))
            {
                await _navigationService.NavigateToAsync<AddMoreDetailsViewModel>();
                await App.Locator.AddMoreDetails.InitializeDataNew();
                await App.Locator.AddMoreDetails.InitilizeData();
            }
        }

        internal async void InitilizeData()
        {
            if (App.Locator.TrackOrder.PickupScheduleRequest != null)
            {
                PickupScheduleRequest = App.Locator.TrackOrder.PickupScheduleRequest;
            }
            else
            {
                PickupScheduleRequest = App.Locator.DuraExpress.PickupScheduleRequest;
            }

            if (App.Locator.TrackOrder.PickupScheduleResponse != null)
            {
                PickupScheduleResponse = App.Locator.TrackOrder.PickupScheduleResponse;
            }
            else
            {
                PickupScheduleResponse = App.Locator.DuraExpress.PickupScheduleResponse;
            }
            IsVisibleInfoLabel = true;
            IsVisibleStack = false;
            IsVisibleLoader = false;
            TotalFare = 0;
            objServices = new List<ServicesModel>();
            TotalFinalFare = Math.Round(TotalFare).ToString("N2");
            await GetVehicleList();
        }

        private async Task GetVehicleList()
        {
            if (CheckConnection())
            {
                try
                {
                    objServices = new List<ServicesModel>();
                    VehicleListRequestModel vehicleListRequestModel = new VehicleListRequestModel
                    {
                        destinationlat = PickupScheduleRequest.destinationlat,
                        destinationlon = PickupScheduleRequest.destinationlon,
                        pickuplat = PickupScheduleRequest.pickuplat,
                        pickuplon = PickupScheduleRequest.pickuplon,
                        pickup_id = PickupScheduleResponse.data,
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetVehicleList(vehicleListRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        var vehList = new List<VehicleListData>();
                        VehicleListData vehicleListData = new VehicleListData();
                        AdditionalServices additionalServices = new AdditionalServices();
                        List<AdditionalServices> lstAdditionalService = new List<AdditionalServices>();
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            ShowLoading();
                            foreach (var item in result?.Data?.data)
                            {
                                vehicleListData = new VehicleListData();
                                lstAdditionalService = new List<AdditionalServices>();
                                if (item?.services.Count > 0)
                                {
                                    foreach (var itemservices in item?.services)
                                    {
                                        additionalServices = new AdditionalServices();
                                        additionalServices.id = itemservices.id;
                                        additionalServices.price = itemservices.price;
                                        additionalServices.service = itemservices.service;
                                        additionalServices.currency = item?.currency;
                                        lstAdditionalService.Add(additionalServices);
                                    }
                                    vehicleListData.services = lstAdditionalService;
                                }
                                vehicleListData.currency = item?.currency;
                                vehicleListData.basefare = item?.basefare;
                                vehicleListData.city = item?.city;
                                vehicleListData.created_at = item?.created_at;
                                vehicleListData.description = item?.description;
                                vehicleListData.id = item.id;
                                vehicleListData.image = item?.image;
                                vehicleListData.kmfare = item?.kmfare;
                                vehicleListData.priceafterbasefare = item?.priceafterbasefare;
                                vehicleListData.priceby = item?.priceby;
                                vehicleListData.service = item?.service;
                                vehicleListData.size_limit = item?.size_limit;
                                vehicleListData.stopprice = item?.stopprice;
                                vehicleListData.totalfare = item.totalfare;
                                vehicleListData.vehicle_type = item.vehicle_type.Contains("\r\n") ? item?.vehicle_type.Replace("\r\n", "") : item?.vehicle_type;
                                vehicleListData.weight_limit = item?.weight_limit;
                                vehicleListData.VehicleImage = item?.image;
                                vehList.Add(vehicleListData);
                            }
                            VehicleList = new ObservableCollection<VehicleListData>(vehList);
                            VehicleListTemp = new ObservableCollection<VehicleListData>(vehList);
                            HideLoading();
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
                    HideLoading();
                    ShowToast(AppResources.ServerError);
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

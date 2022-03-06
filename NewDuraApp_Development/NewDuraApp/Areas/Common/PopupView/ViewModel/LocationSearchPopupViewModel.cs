using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.PopupView.ViewModel
{
    public class LocationSearchPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GetCurrentLocationCommand { get; set; }
        private IUserCoreService _userCoreService;
        public IAsyncCommand<AddressInfo> AddressDetailsCommand { get; set; }
        public IAsyncCommand TextClick { get; set; }
        private ObservableCollection<GetAddressModel> _addressList;
        public ObservableCollection<GetAddressModel> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; OnPropertyChanged(nameof(AddressList)); }
        }

        private ObservableCollection<LocationModel> _addressLocationList;
        public ObservableCollection<LocationModel> AddressLocationList
        {
            get { return _addressLocationList; }
            set { _addressLocationList = value; OnPropertyChanged(); }
        }

        private bool _isReadOnlyText = true;
        public bool IsReadOnlyText
        {
            get { return _isReadOnlyText; }
            set
            {
                _isReadOnlyText = value;
                OnPropertyChanged(nameof(IsReadOnlyText));
            }
        }

        private static HttpClient _httpClientInstance;
        public static HttpClient HttpClientInstance => _httpClientInstance ?? (_httpClientInstance = new HttpClient());
        private string _addressText;
        public string AddressText
        {
            get { return _addressText; }
            set
            {
                _addressText = value;
                OnPropertyChanged(nameof(AddressText));
            }
        }

        private double _rowHeight = 50;
        public double RowHeight
        {
            get { return _rowHeight; }
            set
            {
                _rowHeight = value;
                OnPropertyChanged(nameof(RowHeight));
            }
        }

        private GetAddressModel _addressData;
        public GetAddressModel AddressData
        {
            get { return _addressData; }
            set
            {
                _addressData = value;
                OnPropertyChanged(nameof(AddressData));
            }
        }

        public IAsyncCommand<GetAddressModel> AddressDetails { get; set; }
        private bool _isVisibleListView;
        public bool IsVisibleListView
        {
            get { return _isVisibleListView; }
            set
            {
                _isVisibleListView = value;
                OnPropertyChanged(nameof(IsVisibleListView));
            }
        }

        private ObservableCollection<AddressInfo> _addresses;
        public ObservableCollection<AddressInfo> Addresses
        {
            get => _addresses ?? (_addresses = new ObservableCollection<AddressInfo>());
            set
            {
                if (_addresses != value)
                {
                    _addresses = value;
                    OnPropertyChanged(nameof(Addresses));
                }
            }
        }

        private CommonLatLong commonLatLong;
        public CommonLatLong CommonLatLong
        {
            get { return commonLatLong; }
            set
            {
                commonLatLong = value;
                OnPropertyChanged(nameof(CommonLatLong));
            }
        }

        public LocationSearchPopupViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            AddressDetails = new AsyncCommand<GetAddressModel>(SavedAddressDetailsCommand, allowsMultipleExecutions: false);
            AddressText = string.Empty;
            AddressDetailsCommand = new AsyncCommand<AddressInfo>(AddressDetailsCommandExecute, allowsMultipleExecutions: false);
            TextClick = new AsyncCommand(TextClickExecute);
            GetCurrentLocationCommand = new AsyncCommand(GetCurrentLocationCommandExecute, allowsMultipleExecutions: false);
            AddressLocationList = GetAddressLocationList();
        }

        private async Task GetCurrentLocationCommandExecute()
        {
            await App.Locator.HomePage.GetCurrentLocation();
            await _navigationService.ClosePopupsAsync();
        }

        private async Task TextClickExecute()
        {
            IsReadOnlyText = false;
        }

        private async Task AddressDetailsCommandExecute(AddressInfo arg)
        {
            ShowLoading();
            IsReadOnlyText = true;
            AddressText = arg?.Address;
            Addresses.Clear();
            Addresses = null;
            IsVisibleListView = false;

            App.Locator.HomePage.CurrentLocation = arg.Address;
            App.Locator.HomePage.IsHavingFromSAvedAddress = true;
            var latlon = await LocationHelpers.GetLatLongBasedOnAddressonHome(arg?.Address);
            if (latlon != null)
            {
                CommonLatLong commonLatLong = new CommonLatLong();
                commonLatLong.latitude = latlon.Latitude;
                commonLatLong.longitude = latlon.Longitude;
                commonLatLong.FullAddress = arg.Address;
                CommonLatLong = commonLatLong;
                App.Locator.HomePage.CommonLatLong = CommonLatLong;
            }
            HideLoading();
            await _navigationService.ClosePopupsAsync();
        }

        private ObservableCollection<LocationModel> GetAddressLocationList()
        {
            return new ObservableCollection<LocationModel>()
            {
                new LocationModel(){AddressLocation="Location 1"},
                new LocationModel(){AddressLocation="Location 2"},
                new LocationModel(){AddressLocation="Location 3"},
                new LocationModel(){AddressLocation="Location 4"},
            };
        }

        public async Task InitilizeData()
        {
            AddressList = App.Locator.HomePage.AddressList;
            IsReadOnlyText = true;
            AddressText = string.Empty;
        }

        private async Task SavedAddressDetailsCommand(GetAddressModel arg)
        {
            if (arg != null)
            {
                AddressData = arg;
                CommonLatLong commonLatLong = new CommonLatLong();
                commonLatLong.latitude = Convert.ToDouble(arg.latitude);
                commonLatLong.longitude = Convert.ToDouble(arg.longitude);
                commonLatLong.FullAddress = arg.FullAddress;
                commonLatLong.address1 = arg.address1;
                commonLatLong.address2 = arg.address2;
                commonLatLong.contactperson = arg.contactperson;
                commonLatLong.mobile = arg.mobile;
                commonLatLong.type = arg.type;
                commonLatLong.isdefault = arg.isdefault;
                CommonLatLong = commonLatLong;
                App.Locator.HomePage.CommonLatLong = CommonLatLong;
                App.Locator.HomePage.CurrentLocation = arg.FullAddress;
                App.Locator.HomePage.IsHavingFromSAvedAddress = true;
                await _navigationService.ClosePopupsAsync();
            }
        }

        private async Task GetAddressList()
        {
            if (CheckConnection())
            {
                try
                {
                    CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                    };
                    AddressList = new ObservableCollection<GetAddressModel>();
                    var result = await TryWithErrorAsync(_userCoreService.GetAddress(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            foreach (var item in result?.Data?.data)
                            {
                                AddressList.Add(item);
                            }
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        await _navigationService.ClosePopupsAsync();
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        public async Task GetPlacesPredictionsAsync()
        {
            try
            {
                CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token;
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(AppConstant.GooglePlaceUrl, AppConstant.GooglePlaceKey, WebUtility.UrlEncode(_addressText))))
                {
                    using (HttpResponseMessage message = await HttpClientInstance.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false))
                    {
                        if (message.IsSuccessStatusCode)
                        {
                            string json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                            PlacesLocationPredictions predictionList = await Task.Run(() => JsonConvert.DeserializeObject<PlacesLocationPredictions>(json)).ConfigureAwait(false);
                            if (predictionList.Status == "OK")
                            {
                                Addresses.Clear();
                                Addresses = new ObservableCollection<AddressInfo>();
                                if (predictionList.Predictions.Count > 0)
                                {
                                    AddressInfo addressInfo = new AddressInfo();
                                    IsVisibleListView = true;
                                    RowHeight = RowHeight * predictionList.Predictions.Count;
                                    foreach (Prediction prediction in predictionList.Predictions)
                                    {
                                        addressInfo.Address = prediction?.Description;
                                        Addresses.Add(addressInfo);
                                    }
                                }
                            }
                            else
                            {
                                IsVisibleListView = false;
                                ShowToast(AppResources.Location_not_found);
                            }
                        }
                        else
                        {
                            IsVisibleListView = false;
                            ShowToast(AppResources.Location_not_found);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

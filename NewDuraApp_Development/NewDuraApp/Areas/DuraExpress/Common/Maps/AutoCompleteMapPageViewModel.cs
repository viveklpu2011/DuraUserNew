using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.ResponseModels;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NewDuraApp.Areas.DuraExpress.Common.Maps
{
    public class AutoCompleteMapPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private static HttpClient _httpClientInstance;
        public static HttpClient HttpClientInstance => _httpClientInstance ?? (_httpClientInstance = new HttpClient());
        private ObservableCollection<GetAddressModel> _addressList;
        public ObservableCollection<GetAddressModel> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; OnPropertyChanged(nameof(AddressList)); }
        }
        public IAsyncCommand<Countries> CountryDetailsCommand { get; set; }
        public IAsyncCommand BackCommand { get; set; }
        public IAsyncCommand ContinueButtonCommand { get; set; }
        public IAsyncCommand<AddressInfo> AddressDetailsCommand { get; set; }
        public IAsyncCommand TextClick { get; set; }
        private ObservableCollection<Countries> _countriesList;
        public ObservableCollection<Countries> CountriesList
        {
            get { return _countriesList; }
            set
            {
                _countriesList = value;
                this.OnPropertyChanged(nameof(CountriesList));
            }
        }


        private string _headerTitle;
        public string HeaderTitle
        {
            get { return _headerTitle; }
            set
            {
                _headerTitle = value;
                OnPropertyChanged(nameof(HeaderTitle));
            }
        }

        private bool _isbuttonEnabled;
        public bool IsButtonEnabled
        {
            get { return _isbuttonEnabled; }
            set
            {
                _isbuttonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private ObservableCollection<Countries> _countriesNameList;
        public ObservableCollection<Countries> CountriesNameList
        {
            get { return _countriesNameList; }
            set
            {
                _countriesNameList = value;
                this.OnPropertyChanged(nameof(CountriesNameList));
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
        private bool _isVisibleCountryListView;
        public bool IsVisibleCountryListView
        {
            get { return _isVisibleCountryListView; }
            set
            {
                _isVisibleCountryListView = value;
                OnPropertyChanged(nameof(IsVisibleCountryListView));
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

        private ExpressType _pageType;
        public ExpressType PageType
        {
            get { return _pageType; }
            set
            {
                _pageType = value;
                OnPropertyChanged(nameof(PageType));
            }
        }


        private Position position;
        public Position Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public IAsyncCommand<GetAddressModel> AddressDetails { get; set; }
        public Xamarin.Forms.Command ExecuteSetPosition { get; set; }
        public Xamarin.Forms.Command<Position> ExecuteSetAddress { get; set; }
        public IAsyncCommand AddressReturnCommand { get; set; }

        private string _address123;
        public string Address123
        {
            get { return _address123; }
            set
            {
                _address123 = value;

                OnPropertyChanged(nameof(Address123));
            }
        }

        private string _address2;
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;

                OnPropertyChanged(nameof(Address2));
            }
        }
        public AutoCompleteMapPageViewModel(INavigationService navigationService)
        {

            _navigationService = navigationService;
            AddressText = string.Empty;
            Address2 = string.Empty;
            AddressDetailsCommand = new AsyncCommand<AddressInfo>(AddressDetailsCommandExecute, allowsMultipleExecutions: false);
            AddressDetails = new AsyncCommand<GetAddressModel>(AddressDetailsCommands, allowsMultipleExecutions: false);
            TextClick = new AsyncCommand(TextClickExecute, allowsMultipleExecutions: false);
            CountryDetailsCommand = new AsyncCommand<Countries>(CountryDetailsCommandExecute, allowsMultipleExecutions: false);
            AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetAddress = new Xamarin.Forms.Command<Position>
                (async (position) =>
                {
                    //App.Locator.CurrentUser.SendWay = SendInvite.MAPLOCATION.ToString();
                    await SetAddress(position);
                }
                );
            ExecuteSetPosition = new Xamarin.Forms.Command(async () => await SetPosition(this.AddressText));
            ContinueButtonCommand = new AsyncCommand(ContinueButtonCommandExecute, allowsMultipleExecutions: false);
            BackCommand = new AsyncCommand(BackCommandExecute, allowsMultipleExecutions: false);

        }
        private async Task AddressDetailsCommands(GetAddressModel arg)
        {
            if (arg != null)
            {
                Position p = new Position(Convert.ToDouble(arg.latitude), Convert.ToDouble(arg.longitude));
                try
                {
                    if (PageType == ExpressType.PickupLocation)
                    {

                        await App.Locator.PickupLocation.SetAddress(p);
                        App.Locator.PickupLocation.Address1 = arg?.FullAddress;
                        App.Locator.PickupLocation.Address2 = arg?.address2;
                        App.Locator.PickupLocation.Name = arg?.contactperson;
                        App.Locator.PickupLocation.Number = arg?.mobile;
                        await _navigationService.NavigateBackAsync();
                    }
                    else if (PageType == ExpressType.WhereToLocation)
                    {
                        await App.Locator.WhereTo.SetAddress(p);
                        App.Locator.WhereTo.Address1 = arg?.FullAddress;
                        App.Locator.WhereTo.Address2 = arg?.address2;
                        App.Locator.WhereTo.Name = arg?.contactperson;
                        App.Locator.WhereTo.Number = arg?.mobile;
                        await _navigationService.NavigateBackAsync();
                    }
                    else if (PageType == ExpressType.StopLocation)
                    {
                        await App.Locator.AddStopLocation.SetAddress(p);
                        App.Locator.AddStopLocation.Address1 = arg?.FullAddress;
                        App.Locator.AddStopLocation.Address2 = arg?.address2;
                        App.Locator.AddStopLocation.Name = arg?.contactperson;
                        App.Locator.AddStopLocation.Number = arg?.mobile;
                        await _navigationService.NavigateBackAsync();
                    }
                    else
                    {
                        App.Locator.AddNewAddressPage.HomeAddress = AddressText;
                        await _navigationService.NavigateBackAsync();
                    }
                }
                catch (Exception ex) { }
            }

        }
        private async Task TextClickExecute()
        {
            IsReadOnlyText = false;
        }

        private async Task BackCommandExecute()
        {
            await _navigationService.NavigateBackAsync();
        }

        private async Task ContinueButtonCommandExecute()
        {
            if (PageType == ExpressType.PickupLocation)
            {
                App.Locator.PickupLocation.Address1 = AddressText;
                //App.Locator.PickupLocation.Address2 = Address2;
                //await App.Locator.PickupLocation.SetPosition(AddressText);
                await _navigationService.NavigateBackAsync();
            }
            else if (PageType == ExpressType.WhereToLocation)
            {
                App.Locator.WhereTo.Address1 = AddressText;
                //App.Locator.WhereTo.Address2 = Address2;
                //await App.Locator.WhereTo.SetPosition(AddressText);
                await _navigationService.NavigateBackAsync();
            }
            else if (PageType == ExpressType.StopLocation)
            {
                App.Locator.AddStopLocation.Address1 = AddressText;
                //App.Locator.AddStopLocation.Address2 = Address2;
                //await App.Locator.AddStopLocation.SetPosition(AddressText);
                await _navigationService.NavigateBackAsync();
            }
            else
            {
                App.Locator.AddNewAddressPage.HomeAddress = AddressText;
                await _navigationService.NavigateBackAsync();
            }
        }

        private async Task CountryDetailsCommandExecute(Countries arg)
        {
            Address2 = arg?.name;
            CountriesList.Clear();
            CountriesList = null;
            IsVisibleCountryListView = false;
        }

        private async Task AddressDetailsCommandExecute(AddressInfo arg)
        {
            //App.Locator.CurrentUser.SendWay = SendInvite.MAPLOCATION.ToString();
            IsReadOnlyText = true;
            AddressText = arg?.Address;
            Addresses.Clear();
            Addresses = null;
            IsVisibleListView = false;
            await SetPosition(arg?.Address);
        }

        public async Task AddressReturnCommandExecute()
        {
            await SetPosition(this.AddressText);
        }

        private async Task SetAddress(Position p)
        {
            IsButtonEnabled = true;
            IsReadOnlyText = true;

            try
            {
                if (p == Position)
                {
                    p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                    //var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    //Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                    this.AddressText = $"{App.Locator.HomePage.CommonLatLong.FullAddress}";
                }
                else
                {
                    //p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    this.AddressText = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";

                }

                //if (App.Locator.CurrentUser.SendWay == SendInvite.SELECTEDLOCATION.ToString())
                //{
                //    p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                //    //var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                //    this.AddressText = $"{App.Locator.HomePage.CurrentLocation}";
                //}
                //else
                //{
                //    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                //    this.AddressText = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                //}

                if (PageType == ExpressType.PickupLocation)
                {
                    await App.Locator.PickupLocation.SetAddress(p);
                }
                else if (PageType == ExpressType.WhereToLocation)
                {
                    await App.Locator.WhereTo.SetAddress(p);
                }
                else if (PageType == ExpressType.StopLocation)
                {
                    await App.Locator.AddStopLocation.SetAddress(p);
                }
                else
                {
                    await App.Locator.AddNewAddressPage.SetAddress(p);
                }
                Position = p;
            }
            catch (Exception ex)
            {

            }

            //this.Address2 = $"{addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";

            Position = p;
        }

        private async Task SetPosition(string street)
        {
            try
            {
                var location = (await Geocoding.GetLocationsAsync($"{street}")).FirstOrDefault();

                if (location == null) return;

                Position = new Position(location.Latitude, location.Longitude);

                IsVisibleListView = false;
                IsButtonEnabled = true;
                //App.Locator.CurrentUser.SendWay = SendInvite.MAPLOCATION.ToString();

                if (PageType == ExpressType.PickupLocation)
                {
                    await App.Locator.PickupLocation.SetAddress(Position);
                }
                else if (PageType == ExpressType.WhereToLocation)
                {
                    await App.Locator.WhereTo.SetAddress(Position);
                }
                else if (PageType == ExpressType.StopLocation)
                {
                    await App.Locator.AddStopLocation.SetAddress(Position);
                }
                else
                {
                    await App.Locator.AddNewAddressPage.SetAddress(Position);
                }
            }
            catch (Exception ex)
            {

            }

        }


        internal async Task InitilizeData(ExpressType pagetype)
        {
            IsReadOnlyText = true;
            PageType = pagetype;
            if (pagetype == ExpressType.PickupLocation)
            {
                HeaderTitle =AppResources.Enter_the_pickup_address;
            }
            else if (pagetype == ExpressType.StopLocation)
            {
                HeaderTitle =AppResources.Enter_the_stop_address;
            }
            else if (pagetype == ExpressType.WhereToLocation)
            {
                HeaderTitle =AppResources.Enter_your_drop_address;
            }
            else
            {
                HeaderTitle =AppResources.Enter_the_address;
            }
            AddressList = App.Locator.HomePage.AddressList;
            //await GetCountryName();
            //IsVisibleCountryListView = false;
            IsVisibleListView = false;
            AddressText = string.Empty;
            Address2 = string.Empty;
            Addresses = new ObservableCollection<AddressInfo>();
            Geolocation.GetLastKnownLocationAsync()
                                 .ToObservable()
                                 .Catch(Observable.Return(new Location()))
                                 .SubscribeOn(RxApp.MainThreadScheduler)
                                 .Subscribe(async location =>
                                 {
                                     var position = new Position(location.Latitude, location.Longitude);
                                     Position = position;
                                     await SetAddress(position);
                                 });
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
                                        // addressInfo.Latitude=prediction?.
                                        addressInfo = new AddressInfo();
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

        public async Task GetCountryName()
        {
            ShowLoading();
            try
            {
                string jsonFileName = "countriescode.json";
                CountriesList = new ObservableCollection<Countries>();
                var assembly = typeof(AutoCompleteMapPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.CommonData.{jsonFileName}");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();
                    var countries = JsonConvert.DeserializeObject<ObservableCollection<Countries>>(jsonString);
                    foreach (var item in countries)
                    {
                        CountriesList.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            HideLoading();
        }

        public async Task GetCountryCodeAutocomplete(string predictions)
        {
            CountriesNameList = new ObservableCollection<Countries>();
            CountriesNameList.Clear();

            if (CountriesList != null && CountriesList.Count > 0)
            {
                //IsVisibleListView = true;
                //RowHeight = RowHeight * predictionList.Predictions.Count;
                var selectedcounrtylist = CountriesList.Where(x => x.name.ToLower() == predictions.ToLower()).ToList();
                if (selectedcounrtylist != null && selectedcounrtylist.Count > 0)
                {
                    IsVisibleCountryListView = true;
                    foreach (var prediction in selectedcounrtylist)
                    {
                        CountriesNameList.Add(new Countries
                        {
                            name = prediction.name
                        });
                    }
                }
                else
                {
                    IsVisibleCountryListView = false;
                }

            }
        }
    }
}

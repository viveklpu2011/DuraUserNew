using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.FontIcons;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class AddStopLocationViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToTrackorderCmd { get; set; }
        public IAsyncCommand SelectContactCommand { get; set; }
        private Map _mapAddStopLocation;
        private Location _addStopLocation;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        private bool _isAddress1ErrorVisible;
        private bool _isAddress2ErrorVisible;
        private bool _isNameErrorVisible;

        private bool _isButtonEnabled;
        public IAsyncCommand SkipCommand { get; set; }
        public IAsyncCommand OpenMapCommand { get; set; }
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

        public Xamarin.Forms.Command ExecuteSetPosition { get; set; }
        public Xamarin.Forms.Command<Position> ExecuteSetAddress { get; set; }
        public IAsyncCommand AddressReturnCommand { get; set; }

        public bool IsAddress1ErrorVisible
        {
            get { return _isAddress1ErrorVisible; }
            set { _isAddress1ErrorVisible = value; OnPropertyChanged(nameof(IsAddress1ErrorVisible)); }
        }
        public bool IsAddress2ErrorVisible
        {
            get { return _isAddress2ErrorVisible; }
            set { _isAddress2ErrorVisible = value; OnPropertyChanged(nameof(IsAddress2ErrorVisible)); }
        }
        public bool IsNameErrorVisible
        {
            get { return _isNameErrorVisible; }
            set { _isNameErrorVisible = value; OnPropertyChanged(nameof(IsNameErrorVisible)); }
        }
        private bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
        }
        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set { _isButtonEnabled = value; OnPropertyChanged(nameof(IsButtonEnabled)); }
        }
        public PickupScheduleRequestModel PickupScheduleRequest
        {
            get { return _pickupScheduleRequest; }
            set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
        }
        public Location AddStopLocation
        {
            get { return _addStopLocation; }
            set { _addStopLocation = value; OnPropertyChanged(nameof(AddStopLocation)); }
        }

        private Placemark _placemarkAddress;
        public Placemark PlacemarkAddress
        {
            get { return _placemarkAddress; }
            set { _placemarkAddress = value; OnPropertyChanged(nameof(PlacemarkAddress)); }
        }

        public Map MapAddStopLocation
        {
            get { return _mapAddStopLocation; }
            set { _mapAddStopLocation = value; OnPropertyChanged(nameof(MapAddStopLocation)); }
        }

        private string _address1;
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                if (!string.IsNullOrEmpty(_address1))
                {
                    if (RegexUtilities.EmptyString(_address1))
                    {
                        IsAddress1ErrorVisible = false;
                    }
                    else
                    {
                        IsAddress1ErrorVisible = true;
                    }
                }
                else
                {
                    IsAddress1ErrorVisible = false;
                }
                CheckLoginValidation();
                OnPropertyChanged(nameof(Address1));
            }
        }

        private string _address2;
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                if (!string.IsNullOrEmpty(_address2))
                {
                    if (RegexUtilities.EmptyString(_address2))
                    {
                        IsAddress2ErrorVisible = false;
                    }
                    else
                    {
                        IsAddress2ErrorVisible = true;
                    }
                }
                else
                {
                    IsAddress2ErrorVisible = false;
                }
                CheckLoginValidation();
                OnPropertyChanged(nameof(Address2));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (!string.IsNullOrEmpty(_name))
                {
                    if (RegexUtilities.AlphanumericNameValidation(_name))
                    {
                        IsNameErrorVisible = false;
                    }
                    else
                    {
                        IsNameErrorVisible = true;
                    }
                }
                else
                {
                    IsNameErrorVisible = false;
                }
                CheckLoginValidation();
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                if (!string.IsNullOrEmpty(_number))
                {
                    if (RegexUtilities.PhoneNumberValidation(_number) && _number.Length >= 10)
                    {
                        IsPhoneErrorVisible = false;
                    }
                    else
                    {
                        IsPhoneErrorVisible = true;
                    }
                }
                else
                {
                    IsPhoneErrorVisible = false;
                }

                CheckLoginValidation();
                OnPropertyChanged(nameof(Number));
            }
        }

        private bool CheckLoginValidation()
        {
            if (IsAddress1ErrorVisible || IsNameErrorVisible || IsPhoneErrorVisible)
            {
                IsButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(Address1) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Number))
            {
                IsButtonEnabled = false;
                return false;
            }
            else
            {
                IsButtonEnabled = true;
                return true;
            }

        }

        public int stopid = 0;
        public AddStopLocationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectContactCommand = new AsyncCommand(SelectContactCommandExecute, allowsMultipleExecutions: false);
            GoToTrackorderCmd = new AsyncCommand(GoToTrackorderCmdExecute, allowsMultipleExecutions: false);
            SkipCommand = new AsyncCommand(SkipCommandExecute, allowsMultipleExecutions: false);
            OpenMapCommand = new AsyncCommand(OpenMapCommandExecute, allowsMultipleExecutions: false);
            Address1 = "";
            Address2 = "";
            Name = "";
            Number = "";
            stopid = 0;
            AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetAddress = new Xamarin.Forms.Command<Position>(async (position) => await SetAddress(position));
            ExecuteSetPosition = new Xamarin.Forms.Command(async () => await SetPosition(Address1));
        }

        private async Task SkipCommandExecute()
        {
            PickupScheduleRequest = new PickupScheduleRequestModel();
            if (App.Locator.PickupLocation.PickupScheduleRequest != null)
            {
                PickupScheduleRequest.pickuplat = App.Locator.PickupLocation.PickupScheduleRequest.pickuplat;
                PickupScheduleRequest.pickuplon = App.Locator.PickupLocation.PickupScheduleRequest.pickuplon;
                PickupScheduleRequest.pickup_address1 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address1;
                PickupScheduleRequest.pickup_address2 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address2;
                PickupScheduleRequest.pickup_mobile = App.Locator.PickupLocation.PickupScheduleRequest.pickup_mobile;
                PickupScheduleRequest.pickup_name = App.Locator.PickupLocation.PickupScheduleRequest.pickup_name;
                PickupScheduleRequest.type = App.Locator.PickupLocation.PickupScheduleRequest.type;
                PickupScheduleRequest.pdate = App.Locator.PickupLocation.PickupScheduleRequest.pdate;
                PickupScheduleRequest.IsAvailablePickupLocation = App.Locator.PickupLocation.PickupScheduleRequest.IsAvailablePickupLocation;
            }
            await _navigationService.NavigateToAsync<WhereToViewModel>();
        }

        public async Task AddressReturnCommandExecute()
        {
            await SetPosition(Address1);
        }

        public async Task SetAddress(Position p)
        {
            try
            {
                if (p == Position)
                {
                    if (App.Locator.HomePage.CommonLatLong != null)
                    {
                        p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                        Address1 = $"{App.Locator.HomePage.CommonLatLong.FullAddress}";
                        Address2 = App.Locator.HomePage.CommonLatLong.address2;
                        Name = App.Locator.HomePage.CommonLatLong.contactperson;
                        Number = App.Locator.HomePage.CommonLatLong.mobile;
                        Helpers.CommonHelper.CurrentLat = p.Latitude;
                        Helpers.CommonHelper.CurrentLong = p.Longitude;
                        Helpers.CommonHelper.CurrentLatStop = p.Latitude;
                        Helpers.CommonHelper.CurrentLongStop = p.Longitude;
                    }
                    else
                    {
                        var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                        Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                        Helpers.CommonHelper.CurrentLat = p.Latitude;
                        Helpers.CommonHelper.CurrentLong = p.Longitude;
                        Helpers.CommonHelper.CurrentLatStop = p.Latitude;
                        Helpers.CommonHelper.CurrentLongStop = p.Longitude;
                    }
                }
                else
                {
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                    Helpers.CommonHelper.CurrentLat = p.Latitude;
                    Helpers.CommonHelper.CurrentLong = p.Longitude;
                    Helpers.CommonHelper.CurrentLatStop = p.Latitude;
                    Helpers.CommonHelper.CurrentLongStop = p.Longitude;
                }

                Position = p;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SetPosition(string street)
        {
            try
            {
                Location location = null;
                Position = new Position(Helpers.CommonHelper.CurrentLatStop, Helpers.CommonHelper.CurrentLongStop);
                HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task OpenMapCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AutoCompleteMapPageViewModel))
            {
                await _navigationService.NavigateToAsync<AutoCompleteMapPageViewModel>();
                await App.Locator.AutoCompleteMapPage.InitilizeData(ExpressType.StopLocation);
            }
        }
        private async Task GoToTrackorderCmdExecute()
        {
            try
            {
                var address2 = Address2 == null ? "" : Address2.Trim();
                ShowLoading();
                AddStopLocation = await LocationHelpers.GetLatLongBasedOnAddress($"{Address1} {address2}");
                HideLoading();
                if (AddStopLocation.Latitude <= 0 && AddStopLocation.Longitude <= 0)
                {
                    ShowToast(AppResources.Google_map_is_not_able_to_pick_address_currently);
                    return;
                }
                if (string.IsNullOrEmpty(Address1))
                {
                    ShowToast(AppResources.Address_are_mandatory);
                    return;
                }
                if (_navigationService.GetCurrentPageViewModel() != typeof(WhereToViewModel))
                {
                    PickupScheduleRequest = new PickupScheduleRequestModel();
                    if (App.Locator.PickupLocation.PickupScheduleRequest != null)
                    {
                        PickupScheduleRequest.pickuplat = App.Locator.PickupLocation.PickupScheduleRequest.pickuplat;
                        PickupScheduleRequest.pickuplon = App.Locator.PickupLocation.PickupScheduleRequest.pickuplon;
                        PickupScheduleRequest.pickup_address1 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address1.Trim();
                        PickupScheduleRequest.pickup_address2 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address2.Trim();
                        PickupScheduleRequest.pickup_mobile = App.Locator.PickupLocation.PickupScheduleRequest.pickup_mobile.Trim();
                        PickupScheduleRequest.pickup_name = App.Locator.PickupLocation.PickupScheduleRequest.pickup_name.Trim();
                        PickupScheduleRequest.type = App.Locator.PickupLocation.PickupScheduleRequest.type;
                        PickupScheduleRequest.pdate = App.Locator.PickupLocation.PickupScheduleRequest.pdate;
                        PickupScheduleRequest.IsAvailablePickupLocation = App.Locator.PickupLocation.PickupScheduleRequest.IsAvailablePickupLocation;
                    }

                    if (App.Locator.WhereTo.PickupScheduleRequest != null &&
                        App.Locator.PickupLocation.PickupScheduleRequest != null)
                    {
                        PickupScheduleRequest.pickuplat = App.Locator.PickupLocation.PickupScheduleRequest.pickuplat;
                        PickupScheduleRequest.pickuplon = App.Locator.PickupLocation.PickupScheduleRequest.pickuplon;
                        PickupScheduleRequest.pickup_address1 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address1.Trim();
                        PickupScheduleRequest.pickup_address2 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address2.Trim();
                        PickupScheduleRequest.pickup_mobile = App.Locator.PickupLocation.PickupScheduleRequest.pickup_mobile.Trim();
                        PickupScheduleRequest.pickup_name = App.Locator.PickupLocation.PickupScheduleRequest.pickup_name.Trim();
                        PickupScheduleRequest.IsAvailablePickupLocation = App.Locator.PickupLocation.PickupScheduleRequest.IsAvailablePickupLocation;
                        PickupScheduleRequest.type = App.Locator.PickupLocation.PickupScheduleRequest.type;
                        PickupScheduleRequest.pdate = App.Locator.PickupLocation.PickupScheduleRequest.pdate;
                        PickupScheduleRequest.destinationlat = App.Locator.WhereTo.PickupScheduleRequest.destinationlat;
                        PickupScheduleRequest.destinationlon = App.Locator.WhereTo.PickupScheduleRequest.destinationlon;
                        PickupScheduleRequest.destination_address1 = App.Locator.WhereTo.PickupScheduleRequest.destination_address1.Trim();
                        PickupScheduleRequest.destination_address2 = App.Locator.WhereTo.PickupScheduleRequest.destination_address2.Trim();
                        PickupScheduleRequest.destination_name = App.Locator.WhereTo.PickupScheduleRequest.destination_name.Trim();
                        PickupScheduleRequest.destination_mobile = App.Locator.WhereTo.PickupScheduleRequest.destination_mobile.Trim();
                        PickupScheduleRequest.IsAvailableWhereToLocation = App.Locator.WhereTo.PickupScheduleRequest.IsAvailableWhereToLocation;

                    }

                    PickupScheduleRequest.stoplat = AddStopLocation.Latitude;
                    PickupScheduleRequest.stoplon = AddStopLocation.Longitude;
                    PickupScheduleRequest.stop_address1 = Address1.Trim();
                    PickupScheduleRequest.stop_address2 = address2;
                    PickupScheduleRequest.stop_name = Name;
                    PickupScheduleRequest.stop_mobile = Number.Replace(" ", "").Trim();
                    PickupScheduleRequest.IsAvailableAddStopLocationLocation = true;
                    App.Locator.WhereTo.PickupScheduleRequest = PickupScheduleRequest;
                    stopid = stopid + 1;
                    PickupScheduleRequestStopModel pickupScheduleRequestStopModel = new PickupScheduleRequestStopModel
                    {
                        StopId = stopid,
                        StopText = $"Stop Location {stopid}",
                        IsAvailableAddStopLocationLocation = PickupScheduleRequest.IsAvailableAddStopLocationLocation,
                        stoplat = AddStopLocation.Latitude,
                        stoplon = AddStopLocation.Longitude,
                        stop_address1 = Address1.Trim(),
                        stop_address2 = address2,
                        stop_mobile = Number.Replace(" ", "").Trim(),
                        stop_name = Name.Trim(),
                        ImageName = FontIconsClass.CheckboxBlankCircleOutline,
                        IsVisibleLastBottomLine = true
                    };
                    StopAddressList.Add(pickupScheduleRequestStopModel);
                    await _navigationService.NavigateBackAsync();
                    await App.Locator.TrackOrder.InitilizeData();
                    App.Locator.DuraExpress.CheckStopLocation();
                }
            }
            catch (Exception ex)
            {

            }
        }
        internal async Task InitilizeData()
        {
            Name = string.Empty;
            Number = string.Empty;
            Address2 = string.Empty;

            IsAddress1ErrorVisible = IsAddress2ErrorVisible = IsNameErrorVisible = IsPhoneErrorVisible = false;
            CheckLoginValidation();
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

        private async Task SelectContactCommandExecute()
        {
            try
            {
                ShowLoading();
                var userContact = await CommonHelper.GetContact();
                HideLoading();
                if (userContact != null)
                {
                    Name = userContact?.DisplayName;
                    if (userContact?.Phones != null && userContact?.Phones.Count > 0)
                    {
                        Number = userContact?.Phones[0].PhoneNumber.Trim();
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}

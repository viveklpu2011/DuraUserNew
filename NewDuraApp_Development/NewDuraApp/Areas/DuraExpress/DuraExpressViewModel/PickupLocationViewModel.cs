using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class PickupLocationViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public delegate void PerformUpDateLable(string val);
        public event PerformUpDateLable UpdateLabelEvent;
        public IAsyncCommand SelectContactCommand { get; set; }
        public IAsyncCommand GoToWhereCmd { get; set; }
        private Map _mapPickupLocation;
        private string _address1;


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

        public Command ExecuteSetPosition { get; set; }
        public Command<Position> ExecuteSetAddress { get; set; }
        public IAsyncCommand AddressReturnCommand { get; set; }


        private PickupScheduleRequestModel _pickupScheduleRequest;

        private bool _isAddress1ErrorVisible;
        private bool _isAddress2ErrorVisible;
        private bool _isNameErrorVisible;
        private bool _isPhoneErrorVisible;
        private bool _isButtonEnabled;
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
        private Location _pickupLocation;
        public Location PickupLocation
        {
            get { return _pickupLocation; }
            set { _pickupLocation = value; OnPropertyChanged(nameof(PickupLocation)); }
        }
        private Placemark _placemarkAddress;
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;

                OnPropertyChanged(nameof(Address1));
            }
        }
        public Placemark PlacemarkAddress
        {
            get { return _placemarkAddress; }
            set { _placemarkAddress = value; OnPropertyChanged(nameof(PlacemarkAddress)); }
        }
        public Map MapPickupLocation
        {
            get { return _mapPickupLocation; }
            set { _mapPickupLocation = value; OnPropertyChanged(nameof(MapPickupLocation)); }
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
            if (IsNameErrorVisible || IsPhoneErrorVisible)
            {
                IsButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Number))
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
        public PickupLocationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectContactCommand = new AsyncCommand(SelectContactCommandExecute, allowsMultipleExecutions: false);
            GoToWhereCmd = new AsyncCommand(GoToWhereCmdExecute, allowsMultipleExecutions: false);
            Address1 = "";
            Address2 = "";
            Name = "";
            Number = "";
            OpenMapCommand = new AsyncCommand(OpenMapCommandExecute, allowsMultipleExecutions: false);
            AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetAddress = new Command<Position>(async (position) => await SetAddress(position));

            ExecuteSetPosition = new Command(async () => await SetPosition(Address1));
            InitilizeData();

        }

        private async Task OpenMapCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AutoCompleteMapPageViewModel))
            {
                //App.Locator.CurrentUser.SendWay = SendInvite.SELECTEDLOCATION.ToString();
                await _navigationService.NavigateToAsync<AutoCompleteMapPageViewModel>();
                await App.Locator.AutoCompleteMapPage.InitilizeData(ExpressType.PickupLocation);
            }
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
                        //var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                        //Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                        Helpers.CommonHelper.CurrentLat = p.Latitude;
                        Helpers.CommonHelper.CurrentLong = p.Longitude;
                        Helpers.CommonHelper.CurrentLatPick = p.Latitude;
                        Helpers.CommonHelper.CurrentLongPick = p.Longitude;
                        Address1 = $"{App.Locator.HomePage.CommonLatLong.FullAddress}";
                        Address2 = App.Locator.HomePage.CommonLatLong.address2;
                        Name = App.Locator.HomePage.CommonLatLong.contactperson;
                        Number = App.Locator.HomePage.CommonLatLong.mobile;
                    }
                    else
                    {
                        var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                        Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                        Helpers.CommonHelper.CurrentLat = p.Latitude;
                        Helpers.CommonHelper.CurrentLong = p.Longitude;

                        Helpers.CommonHelper.CurrentLatPick = p.Latitude;
                        Helpers.CommonHelper.CurrentLongPick = p.Longitude;


                    }
                }
                else
                {
                    //p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    Helpers.CommonHelper.CurrentLat = p.Latitude;
                    Helpers.CommonHelper.CurrentLong = p.Longitude;
                    Helpers.CommonHelper.CurrentLatPick = p.Latitude;
                    Helpers.CommonHelper.CurrentLongPick = p.Longitude;
                    Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";

                }
                //if (App.Locator.CurrentUser.SendWay == SendInvite.SELECTEDLOCATION.ToString())
                //{
                //    p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                //    //var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                //    Address1 = $"{App.Locator.HomePage.CurrentLocation}";
                //}
                //else
                //{
                //    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                //    Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                //}

                Position = p;
                HideLoading();
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
                //if (App.Locator.CurrentUser.SendWay == SendInvite.SELECTEDLOCATION.ToString())
                //{
                //    location = (await Geocoding.GetLocationsAsync($"{App.Locator.HomePage.CurrentLocation}")).FirstOrDefault();
                //}
                //else
                //{
                //location = (await Geocoding.GetLocationsAsync($"{street}")).FirstOrDefault();

                //}
                //Position = new Position(location.Latitude, location.Longitude);
                Position = new Position(Helpers.CommonHelper.CurrentLatPick, Helpers.CommonHelper.CurrentLongPick);
                HideLoading();
            }
            catch (Exception ex)
            {

            }
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

        private async Task GoToWhereCmdExecute()
        {
            var address2 = Address2 == null ? "" : Address2.Trim();
            ShowLoading();
            PickupLocation = await LocationHelpers.GetLatLongBasedOnAddress($"{Address1} {address2}");
            HideLoading();
            if (PickupLocation.Latitude <= 0 && PickupLocation.Longitude <= 0)
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

                //App.Locator.CurrentUser.SendWay = SendInvite.SELECTEDLOCATION.ToString();
                PickupScheduleRequest = new PickupScheduleRequestModel();
                PickupScheduleRequest.pickuplat = PickupLocation.Latitude;
                PickupScheduleRequest.pickuplon = PickupLocation.Longitude;
                PickupScheduleRequest.pickup_address1 = Address1.Trim();
                PickupScheduleRequest.pickup_address2 = address2;
                PickupScheduleRequest.pickup_mobile = Number.Replace(" ", "").Trim();
                PickupScheduleRequest.pickup_name = Name;
                PickupScheduleRequest.IsAvailablePickupLocation = true;
                App.Locator.DuraExpress.PickupLocationTextVisible = true;
                App.Locator.DuraExpress.PickupLocationText = $"{Name.Trim()} - {Address1.Trim()} {address2}";
                SettingsExtension.PickupAddress = $"{Name} - {Address1} {address2}";
                //UpdateLabelEvent?.Invoke($"{Name} - {Address1} {Address2}");
                if (App.Locator.PickupSchedule.DuraAddressCommon != null)
                {
                    PickupScheduleRequest.type = App.Locator.PickupSchedule.DuraAddressCommon.PickupType;
                    PickupScheduleRequest.pdate = App.Locator.PickupSchedule.DuraAddressCommon.PickupDate;

                }
                App.Locator.DuraExpress.PickupScheduleRequest = PickupScheduleRequest;
                await _navigationService.NavigateToAsync<WhereToViewModel>();
                await App.Locator.WhereTo.InitilizeData();
            }
        }
        internal async Task InitilizeData()
        {
            if (PickupScheduleRequest != null)
            {
                Address1 = PickupScheduleRequest.pickup_address1;
                Address2 = PickupScheduleRequest.pickup_address2;
                Number = PickupScheduleRequest.pickup_mobile.Replace(" ", "").Trim();
                Name = PickupScheduleRequest.pickup_name;
                ShowLoading();
                await SetPosition(Address1);
                HideLoading();
            }
            else
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
                            ShowLoading();
                            await SetAddress(position);
                            HideLoading();
                        });
            }

        }

    }
}


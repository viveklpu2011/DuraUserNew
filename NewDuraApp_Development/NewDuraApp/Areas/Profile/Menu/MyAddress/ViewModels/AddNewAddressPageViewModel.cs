using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.DuraExpress.Common.Maps;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels
{
    public class AddNewAddressPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand SaveAddress { get; set; }
        private IUserCoreService _userCoreService;
        public IAsyncCommand OpenMapCommand { get; set; }
        public IAsyncCommand SelectContactCommand { get; set; }

        private int _selectedTypeIndex = -1;
        public int SelectedTypeIndex
        {
            get { return _selectedTypeIndex; }
            set
            {
                _selectedTypeIndex = value;
                OnPropertyChanged(nameof(SelectedTypeIndex));
            }
        }

        private bool _isCheckedDefault;
        public bool IsCheckedDefault
        {
            get { return _isCheckedDefault; }
            set
            {
                _isCheckedDefault = value;
                OnPropertyChanged(nameof(IsCheckedDefault));
            }
        }

        private string _addressType;
        public string AddressType
        {
            get { return _addressType; }
            set
            {
                _addressType = value;
                if (RegexUtilities.EmptyString(_addressType))
                {
                    IsAddressTypeErrorVisible = false;
                }
                else
                {
                    IsAddressTypeErrorVisible = true;
                }
                CheckAddressValidation();
                OnPropertyChanged(nameof(AddressType));
            }
        }

        private string _homeAddress;
        public string HomeAddress
        {
            get { return _homeAddress; }
            set
            {
                _homeAddress = value;
                if (RegexUtilities.EmptyString(_homeAddress))
                {
                    IsHomeAddressErrorVisible = false;
                }
                else
                {
                    IsHomeAddressErrorVisible = true;
                }
                CheckAddressValidation();
                OnPropertyChanged(nameof(HomeAddress));
            }
        }

        private string _blockAddress;
        public string BlockAddress
        {
            get { return _blockAddress; }
            set
            {
                _blockAddress = value;
                if (RegexUtilities.EmptyString(_blockAddress))
                {
                    IsBlockAddressErrorVisible = false;
                }
                else
                {
                    IsBlockAddressErrorVisible = true;
                }
                CheckAddressValidation();
                OnPropertyChanged(nameof(BlockAddress));
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                if (!string.IsNullOrEmpty(_city))
                {
                    if (RegexUtilities.EmptyString(_city))
                    {
                        IsCityErrorVisible = false;
                    }
                    else
                    {
                        IsCityErrorVisible = true;
                    }

                }
                else
                {
                    IsCityErrorVisible = false;
                }
                CheckAddressValidation();
                OnPropertyChanged(nameof(City));
            }
        }

        private string _contactPerson;
        public string ContactPerson
        {
            get { return _contactPerson; }
            set
            {
                _contactPerson = value;
                if (!string.IsNullOrEmpty(_contactPerson))
                {
                    if (RegexUtilities.AlphanumericNameValidation(_contactPerson))
                    {
                        IsContactPersonErrorVisible = false;
                    }
                    else
                    {
                        IsContactPersonErrorVisible = true;
                    }
                }
                else
                {
                    IsContactPersonErrorVisible = false;
                }
                CheckAddressValidation();
                OnPropertyChanged(nameof(ContactPerson));
            }
        }

        private string _contactNumber;
        public string ContactNumber
        {
            get { return _contactNumber; }
            set
            {
                _contactNumber = value;
                if (!string.IsNullOrEmpty(_contactNumber))
                {
                    if (RegexUtilities.PhoneNumberValidation(_contactNumber) && _contactNumber.Length >= 10)
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
                CheckAddressValidation();
                OnPropertyChanged(nameof(ContactNumber));
            }
        }

        private string _default = "0";
        public string Default
        {
            get { return _default; }
            set
            {
                _default = value;
                OnPropertyChanged(nameof(Default));
            }
        }

        private string _headerText = AppResources.Add_New_Address;
        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        private string _buttonText = AppResources.Add;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        private bool _isHomeAddressErrorVisible;
        public bool IsHomeAddressErrorVisible
        {
            get { return _isHomeAddressErrorVisible; }
            set
            {
                _isHomeAddressErrorVisible = value;
                OnPropertyChanged(nameof(IsHomeAddressErrorVisible));
            }
        }

        private bool _isBlockAddressErrorVisible;
        public bool IsBlockAddressErrorVisible
        {
            get { return _isBlockAddressErrorVisible; }
            set
            {
                _isBlockAddressErrorVisible = value;
                OnPropertyChanged(nameof(IsBlockAddressErrorVisible));
            }
        }

        private bool _isCityErrorVisible;
        public bool IsCityErrorVisible
        {
            get { return _isCityErrorVisible; }
            set
            {
                _isCityErrorVisible = value;
                OnPropertyChanged(nameof(IsCityErrorVisible));
            }
        }

        private bool _isContactPersonErrorVisible;
        public bool IsContactPersonErrorVisible
        {
            get { return _isContactPersonErrorVisible; }
            set
            {
                _isContactPersonErrorVisible = value;
                OnPropertyChanged(nameof(IsContactPersonErrorVisible));
            }
        }

        private bool _isPhoneErrorVisible;
        public bool IsPhoneErrorVisible
        {
            get { return _isPhoneErrorVisible; }
            set
            {
                _isPhoneErrorVisible = value;
                OnPropertyChanged(nameof(IsPhoneErrorVisible));
            }
        }

        private bool _isAddressTypeErrorVisible;
        public bool IsAddressTypeErrorVisible
        {
            get { return _isAddressTypeErrorVisible; }
            set
            {
                _isAddressTypeErrorVisible = value;
                OnPropertyChanged(nameof(IsAddressTypeErrorVisible));
            }
        }

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
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

        public Xamarin.Forms.Command ExecuteSetPosition { get; set; }
        public Xamarin.Forms.Command<Position> ExecuteSetAddress { get; set; }
        public IAsyncCommand AddressReturnCommand { get; set; }

        public AddNewAddressPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            SaveAddress = new AsyncCommand(SaveAddressCommand, allowsMultipleExecutions: false);
            SelectContactCommand = new AsyncCommand(SelectContactCommandExecute, allowsMultipleExecutions: false);
            AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetAddress = new Xamarin.Forms.Command<Position>(async (position) => await SetAddress(position));
            OpenMapCommand = new AsyncCommand(OpenMapCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetPosition = new Xamarin.Forms.Command(async () => await SetPosition(HomeAddress));
        }

        private async Task SelectContactCommandExecute()
        {
            var userContact = await CommonHelper.GetContact();
            if (userContact != null)
            {

                ContactPerson = userContact?.DisplayName;
                if (userContact?.Phones != null && userContact?.Phones.Count > 0)
                {
                    ContactNumber = userContact?.Phones[0].PhoneNumber.Trim();
                }
            }
        }

        public async Task AddressReturnCommandExecute()
        {
            await SetPosition(HomeAddress);
        }

        private async Task OpenMapCommandExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AutoCompleteMapPageViewModel))
            {
                await _navigationService.NavigateToAsync<AutoCompleteMapPageViewModel>();
                await App.Locator.AutoCompleteMapPage.InitilizeData(ExpressType.AddAddress);
            }
        }

        public async Task SetAddress(Position p)
        {
            try
            {
                if (p == Position)
                {
                    p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    HomeAddress = $"{App.Locator.HomePage.CommonLatLong.FullAddress}";
                }
                else
                {
                    var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
                    HomeAddress = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
                }

                Position = p;
                HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task SetPosition(string street)
        {
            try
            {
                Location location = null;
                location = (await Geocoding.GetLocationsAsync($"{street}")).FirstOrDefault();
                Position = new Position(location.Latitude, location.Longitude);
                HideLoading();
            }
            catch (Exception ex)
            {

            }
        }


        private async Task SaveAddressCommand()
        {
            if (ButtonText == "Update")
            {
                if (CheckConnection())
                {
                    ShowLoading();
                    try
                    {
                        UpdateAddressRequestModel updateAddressRequestModel = new UpdateAddressRequestModel()
                        {
                            user_id = SettingsExtension.UserId,
                            address1 = HomeAddress,
                            address2 = BlockAddress,
                            contactperson = ContactPerson,
                            mobile = ContactNumber,
                            isdefault = Default,
                            latitude = Position.Latitude,
                            longitude = Position.Longitude,
                            addressid = AddressData.id
                        };
                        var result = await TryWithErrorAsync(_userCoreService.UpdateAddress(updateAddressRequestModel, SettingsExtension.Token), true, false);
                        if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                        {
                            App.Locator.CurrentUser.SendWay = SendInvite.SELECTEDLOCATION.ToString();
                            await App.Locator.HomePage.GetAddressList();
                            await _navigationService.NavigateBackAsync();
                            await App.Locator.MyAddressPage.InitilizeData();
                            ShowToast(AppResources.Address_Updated);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    HideLoading();
                }
                else
                    ShowToast(CommonMessages.NoInternet);
            }
            else
            {
                if (CheckConnection())
                {
                    ShowLoading();
                    try
                    {
                        AddAddressRequestModel addAddressRequestModel = new AddAddressRequestModel()
                        {
                            user_id = SettingsExtension.UserId,
                            address1 = HomeAddress,
                            address2 = BlockAddress,
                            contactperson = ContactPerson,
                            mobile = ContactNumber,
                            isdefault = Default,
                            latitude = Position.Latitude,
                            longitude = Position.Longitude

                        };
                        var result = await TryWithErrorAsync(_userCoreService.AddAddress(addAddressRequestModel, SettingsExtension.Token), true, false);
                        if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                        {
                            App.Locator.CurrentUser.SendWay = SendInvite.SELECTEDLOCATION.ToString();
                            await App.Locator.HomePage.GetAddressList();
                            await _navigationService.NavigateBackAsync();
                            await App.Locator.MyAddressPage.InitilizeData();
                            ShowToast(AppResources.New_Address_Added);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    HideLoading();
                }
                else
                    ShowToast(AppResources.NoInternet);
            }
        }

        public async Task InitilizeData(GetAddressModel getAddressModel = null)
        {
            if (getAddressModel != null)
            {
                AddressData = getAddressModel;
                ButtonText = "Update";
                HeaderText = "Update Address";
                HomeAddress = AddressData?.address1;
                BlockAddress = AddressData?.address2;
                ContactPerson = AddressData?.contactperson;
                ContactNumber = AddressData?.mobile;
                Position = new Position(Convert.ToDouble(AddressData?.latitude), Convert.ToDouble(AddressData?.longitude));

                if (AddressData?.isdefault == "1")
                {
                    IsCheckedDefault = true;
                }
                else
                {
                    IsCheckedDefault = false;
                }
                await SetPosition($"{AddressData?.address1} {AddressData?.address2} {AddressData?.city}");
            }
            else
            {
                ButtonText = AppResources.Add;
                HeaderText = AppResources.Add_New_Address;
                HomeAddress = BlockAddress = City = ContactPerson = ContactNumber = AddressType = string.Empty;
                IsCheckedDefault = false;
                IsHomeAddressErrorVisible = IsBlockAddressErrorVisible = IsContactPersonErrorVisible = IsPhoneErrorVisible = false;
                CheckAddressValidation();
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
        }

        private bool CheckAddressValidation()
        {
            if (IsHomeAddressErrorVisible ||
                IsBlockAddressErrorVisible ||
                IsContactPersonErrorVisible ||
                IsPhoneErrorVisible)
            {
                IsButtonEnabled = false;
                return false;
            }
            else if (string.IsNullOrEmpty(HomeAddress) ||
                string.IsNullOrEmpty(BlockAddress) ||
                string.IsNullOrEmpty(ContactPerson) ||
                string.IsNullOrEmpty(ContactNumber))
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
    }
}

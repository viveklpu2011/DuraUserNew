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
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Xamarin.CommunityToolkit.ObjectModel;
using Map = Xamarin.Forms.Maps.Map;
using NewDuraApp.Resources;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
	public class WhereToViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		public IAsyncCommand GoToAddStopLocationCmd { get; set; }
		public IAsyncCommand SelectContactCommand { get; set; }
		private Map _mapWhereToLocation;
		private Placemark _placemarkAddress;
		public IAsyncCommand OpenMapCommand { get; set; }



		private Position position;
		public Position Position {
			get { return position; }
			set {
				position = value;
				OnPropertyChanged(nameof(Position));
			}
		}

		public Xamarin.Forms.Command ExecuteSetPosition { get; set; }
		public Xamarin.Forms.Command<Position> ExecuteSetAddress { get; set; }
		public IAsyncCommand AddressReturnCommand { get; set; }


		private bool _isAddress1ErrorVisible;
		private bool _isAddress2ErrorVisible;
		private bool _isNameErrorVisible;
		private bool _isPhoneErrorVisible;
		private bool _isButtonEnabled;
		public bool IsAddress1ErrorVisible {
			get { return _isAddress1ErrorVisible; }
			set { _isAddress1ErrorVisible = value; OnPropertyChanged(nameof(IsAddress1ErrorVisible)); }
		}
		public bool IsAddress2ErrorVisible {
			get { return _isAddress2ErrorVisible; }
			set { _isAddress2ErrorVisible = value; OnPropertyChanged(nameof(IsAddress2ErrorVisible)); }
		}
		public bool IsNameErrorVisible {
			get { return _isNameErrorVisible; }
			set { _isNameErrorVisible = value; OnPropertyChanged(nameof(IsNameErrorVisible)); }
		}
		public bool IsPhoneErrorVisible {
			get { return _isPhoneErrorVisible; }
			set { _isPhoneErrorVisible = value; OnPropertyChanged(nameof(IsPhoneErrorVisible)); }
		}
		public bool IsButtonEnabled {
			get { return _isButtonEnabled; }
			set { _isButtonEnabled = value; OnPropertyChanged(nameof(IsButtonEnabled)); }
		}
		private Location _whereToLocation;
		private PickupScheduleRequestModel _pickupScheduleRequest;

		public PickupScheduleRequestModel PickupScheduleRequest {
			get { return _pickupScheduleRequest; }
			set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
		}
		public Location WhereToLocation {
			get { return _whereToLocation; }
			set { _whereToLocation = value; OnPropertyChanged(nameof(WhereToLocation)); }
		}

		public Placemark PlacemarkAddress {
			get { return _placemarkAddress; }
			set { _placemarkAddress = value; OnPropertyChanged(nameof(PlacemarkAddress)); }
		}
		public Map MapWhereToLocation {
			get { return _mapWhereToLocation; }
			set { _mapWhereToLocation = value; OnPropertyChanged(nameof(MapWhereToLocation)); }
		}


		public WhereToViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SelectContactCommand = new AsyncCommand(SelectContactCommandExecute, allowsMultipleExecutions: false);
			GoToAddStopLocationCmd = new AsyncCommand(GoToAddStopLocationCmdExecute, allowsMultipleExecutions: false);
			OpenMapCommand = new AsyncCommand(OpenMapCommandExecute, allowsMultipleExecutions: false);

			AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
			ExecuteSetAddress = new Xamarin.Forms.Command<Position>(async (position) => await SetAddress(position));

			ExecuteSetPosition = new Xamarin.Forms.Command(async () => await SetPosition(Address1));

		}

		public async Task AddressReturnCommandExecute()
		{
			await SetPosition(Address1);
		}
		private async Task OpenMapCommandExecute()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(AutoCompleteMapPageViewModel)) {
				await _navigationService.NavigateToAsync<AutoCompleteMapPageViewModel>();
				await App.Locator.AutoCompleteMapPage.InitilizeData(ExpressType.WhereToLocation);
			}
		}
		public async Task SetAddress(Position p)
		{
			try {
				if (p == Position) {
					if (App.Locator.HomePage.CommonLatLong != null) {
						p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
						//var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
						//Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
						Address1 = $"{App.Locator.HomePage.CommonLatLong.FullAddress}";
						Address2 = App.Locator.HomePage.CommonLatLong.address2;
						Name = App.Locator.HomePage.CommonLatLong.contactperson;
						Number = App.Locator.HomePage.CommonLatLong.mobile;
						Helpers.CommonHelper.CurrentLat = p.Latitude;
						Helpers.CommonHelper.CurrentLong = p.Longitude;

						Helpers.CommonHelper.CurrentLatDrop = p.Latitude;
						Helpers.CommonHelper.CurrentLongStop = p.Longitude;
					} else {
						var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
						Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
						Helpers.CommonHelper.CurrentLat = p.Latitude;
						Helpers.CommonHelper.CurrentLong = p.Longitude;
						Helpers.CommonHelper.CurrentLatDrop = p.Latitude;
						Helpers.CommonHelper.CurrentLongStop = p.Longitude;

					}
				} else {
					//p = new Position(App.Locator.HomePage.CommonLatLong.latitude, App.Locator.HomePage.CommonLatLong.longitude);
					var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
					Address1 = $"{addrs.SubLocality} {addrs.SubAdminArea} {addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
					Helpers.CommonHelper.CurrentLat = p.Latitude;
					Helpers.CommonHelper.CurrentLong = p.Longitude;
					Helpers.CommonHelper.CurrentLatDrop = p.Latitude;
					Helpers.CommonHelper.CurrentLongStop = p.Longitude;

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
			} catch (Exception ex) {

			}

		}

		public async Task SetPosition(string street)
		{
			try {
				try {
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
					Position = new Position(Helpers.CommonHelper.CurrentLatDrop, Helpers.CommonHelper.CurrentLongStop);
					HideLoading();
				} catch (Exception ex) {

				}

				//Location location = null;
				//if (App.Locator.CurrentUser.SendWay == SendInvite.SELECTEDLOCATION.ToString())
				//{
				//    location = (await Geocoding.GetLocationsAsync($"{App.Locator.HomePage.CurrentLocation}")).FirstOrDefault();
				//}
				//else
				//{
				//    location = (await Geocoding.GetLocationsAsync($"{street}")).FirstOrDefault();

				//}
				//Position = new Position(location.Latitude, location.Longitude);
			} catch (Exception ex) {

			}
		}


		private async Task GoToAddStopLocationCmdExecute()
		{
			var address2 = Address2 == null ? "" : Address2.Trim();
			ShowLoading();
			WhereToLocation = await LocationHelpers.GetLatLongBasedOnAddress($"{Address1} {address2}");
			HideLoading();
			if (WhereToLocation.Latitude <= 0 && WhereToLocation.Longitude <= 0) {
				if (WhereToLocation.Latitude <= 0 && WhereToLocation.Longitude <= 0) {
					ShowToast(AppResources.Google_map_is_not_able_to_pick_address_currently);
					return;
				}
			}
			if (_navigationService.GetCurrentPageViewModel() != typeof(TrackOrderViewModel)) {

				//App.Locator.CurrentUser.SendWay = SendInvite.SELECTEDLOCATION.ToString();
				App.Locator.DuraExpress.PickupWhereToTextVisible = true;
				App.Locator.DuraExpress.PickupWhereToText = $"{Name} - {Address1} {address2}";


				PickupScheduleRequest = new PickupScheduleRequestModel();
				if (App.Locator.AddStopLocation.PickupScheduleRequest != null) {
					PickupScheduleRequest.pickuplat = App.Locator.AddStopLocation.PickupScheduleRequest.pickuplat;
					PickupScheduleRequest.pickuplon = App.Locator.AddStopLocation.PickupScheduleRequest.pickuplon;
					PickupScheduleRequest.pickup_address1 = App.Locator.AddStopLocation.PickupScheduleRequest.pickup_address1.Trim();
					PickupScheduleRequest.pickup_address2 = App.Locator.AddStopLocation.PickupScheduleRequest.pickup_address2.Trim();
					PickupScheduleRequest.pickup_mobile = App.Locator.AddStopLocation.PickupScheduleRequest.pickup_mobile.Trim();
					PickupScheduleRequest.pickup_name = App.Locator.AddStopLocation.PickupScheduleRequest.pickup_name.Trim();
					PickupScheduleRequest.type = App.Locator.AddStopLocation.PickupScheduleRequest.type;
					PickupScheduleRequest.pdate = App.Locator.AddStopLocation.PickupScheduleRequest.pdate;

					PickupScheduleRequest.stoplat = App.Locator.AddStopLocation.PickupScheduleRequest.stoplat;
					PickupScheduleRequest.stoplon = App.Locator.AddStopLocation.PickupScheduleRequest.stoplon;
					PickupScheduleRequest.stop_address1 = App.Locator.AddStopLocation.PickupScheduleRequest.stop_address1.Trim();
					PickupScheduleRequest.stop_address2 = App.Locator.AddStopLocation.PickupScheduleRequest.stop_address2.Trim();
					PickupScheduleRequest.stop_name = App.Locator.AddStopLocation.PickupScheduleRequest.stop_name.Trim();
					PickupScheduleRequest.stop_mobile = App.Locator.AddStopLocation.PickupScheduleRequest.stop_mobile.Trim();
					PickupScheduleRequest.IsAvailableAddStopLocationLocation = App.Locator.AddStopLocation.PickupScheduleRequest.IsAvailableAddStopLocationLocation;
					PickupScheduleRequest.IsAvailablePickupLocation = App.Locator.AddStopLocation.PickupScheduleRequest.IsAvailablePickupLocation;
				} else if (App.Locator.PickupLocation.PickupScheduleRequest != null) {
					PickupScheduleRequest.pickuplat = App.Locator.PickupLocation.PickupScheduleRequest.pickuplat;
					PickupScheduleRequest.pickuplon = App.Locator.PickupLocation.PickupScheduleRequest.pickuplon;
					PickupScheduleRequest.pickup_address1 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address1.Trim();
					PickupScheduleRequest.pickup_address2 = App.Locator.PickupLocation.PickupScheduleRequest.pickup_address2.Trim();
					PickupScheduleRequest.pickup_mobile = App.Locator.PickupLocation.PickupScheduleRequest.pickup_mobile.Trim();
					PickupScheduleRequest.pickup_name = App.Locator.PickupLocation.PickupScheduleRequest.pickup_name.Trim();
					PickupScheduleRequest.IsAvailablePickupLocation = App.Locator.PickupLocation.PickupScheduleRequest.IsAvailablePickupLocation;
					PickupScheduleRequest.type = App.Locator.PickupLocation.PickupScheduleRequest.type;
					PickupScheduleRequest.pdate = App.Locator.PickupLocation.PickupScheduleRequest.pdate;
				}
				PickupScheduleRequest.destinationlat = WhereToLocation.Latitude;
				PickupScheduleRequest.destinationlon = WhereToLocation.Longitude;
				PickupScheduleRequest.destination_address1 = Address1.Trim();
				PickupScheduleRequest.destination_address2 = address2;
				PickupScheduleRequest.destination_name = Name.Trim();
				PickupScheduleRequest.destination_mobile = Number.Replace(" ", "").Trim();
				PickupScheduleRequest.IsAvailableWhereToLocation = true;
				App.Locator.DuraExpress.PickupScheduleRequest = PickupScheduleRequest;
				await _navigationService.NavigateToAsync<TrackOrderViewModel>();
				await App.Locator.TrackOrder.InitilizeData();

			}
		}
		private string _address1;

		public string Address1 {
			get { return _address1; }
			set {
				_address1 = value;

				OnPropertyChanged(nameof(Address1));
			}
		}
		private string _address2;

		public string Address2 {
			get { return _address2; }
			set {
				_address2 = value;

				OnPropertyChanged(nameof(Address2));
			}
		}
		private string _name;

		public string Name {
			get { return _name; }
			set {
				_name = value;
				if (!string.IsNullOrEmpty(_name)) {
					if (RegexUtilities.AlphanumericNameValidation(_name)) {
						IsNameErrorVisible = false;
					} else {
						IsNameErrorVisible = true;
					}


				} else {
					IsNameErrorVisible = false;
				}
				CheckLoginValidation();
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _number;

		public string Number {
			get { return _number; }
			set {
				_number = value;
				if (!string.IsNullOrEmpty(_number)) {
					if (RegexUtilities.PhoneNumberValidation(_number) && _number.Length >= 10) {
						IsPhoneErrorVisible = false;
					} else {
						IsPhoneErrorVisible = true;
					}
				} else {
					IsPhoneErrorVisible = false;
				}

				CheckLoginValidation();
				OnPropertyChanged(nameof(Number));
			}
		}
		private bool CheckLoginValidation()
		{
			if (IsAddress1ErrorVisible || IsNameErrorVisible || IsPhoneErrorVisible) {
				IsButtonEnabled = false;
				return false;
			} else if (string.IsNullOrEmpty(Address1) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Number)) {
				IsButtonEnabled = false;
				return false;
			} else {
				IsButtonEnabled = true;
				return true;
			}

		}
		internal async Task InitilizeData()
		{
			if (PickupScheduleRequest != null) {
				Address1 = PickupScheduleRequest.destination_address1;
				Address2 = PickupScheduleRequest.destination_address2;
				Number = PickupScheduleRequest.destination_mobile.Replace(" ", "").Trim();
				Name = PickupScheduleRequest.destination_name;
				await SetPosition(Address1);
			} else {
				Name = string.Empty;
				Number = string.Empty;
				Address2 = string.Empty;

				IsAddress1ErrorVisible = IsAddress2ErrorVisible = IsNameErrorVisible = IsPhoneErrorVisible = false;
				CheckLoginValidation();


				Geolocation.GetLastKnownLocationAsync()
					  .ToObservable()
					  .Catch(Observable.Return(new Location()))
					  .SubscribeOn(RxApp.MainThreadScheduler)
					  .Subscribe(async location => {
						  var position = new Position(location.Latitude, location.Longitude);
						  Position = position;
						  await SetAddress(position);
					  });
			}
		}
		private async Task SelectContactCommandExecute()
		{
			try {
				ShowLoading();
				var userContact = await CommonHelper.GetContact();
				HideLoading();
				if (userContact != null) {
					Name = userContact?.DisplayName;
					if (userContact?.Phones != null && userContact?.Phones.Count > 0) {
						Number = userContact?.Phones[0].PhoneNumber.Trim();

					}
				}
			} catch (Exception ex) { }
		}
	}
}

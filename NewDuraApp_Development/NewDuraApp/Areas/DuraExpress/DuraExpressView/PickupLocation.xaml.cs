using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NewDuraApp.Helpers;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupLocation : ContentPage
	{
		//bool firstTime = true;
		ViewModelLocator viewModelLocator;
		private CompositeDisposable _disposable;
		public PickupLocation()
		{
			viewModelLocator = new ViewModelLocator();
			InitializeComponent();
			//App.Locator.PickupLocation.InitilizeData();
			//App.Locator.PickupLocation.MapPickupLocation = mapPickupLocation;

		}

		//async void mapPickupLocation_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
		//{
		//    var m = (Xamarin.Forms.Maps.Map)sender;
		//    if (m.VisibleRegion != null)
		//    {
		//        if (firstTime)
		//        {
		//            firstTime = false;
		//            return;
		//        }
		//        Location location = new Location(m.VisibleRegion.Center.Latitude, m.VisibleRegion.Center.Longitude);
		//        App.Locator.PickupLocation.PickupLocation = location;
		//        var PlacemarkAddress = await LocationHelpers.GetAddressBasedOnLatLong(location);
		//        App.Locator.PickupLocation.PlacemarkAddress = await LocationHelpers.GetAddressBasedOnLatLong(location);
		//        if (PlacemarkAddress != null)
		//        {
		//            App.Locator.PickupLocation.Address1 = $"{PlacemarkAddress?.FeatureName} {PlacemarkAddress?.SubAdminArea}";
		//            App.Locator.PickupLocation.Address2 = $"{PlacemarkAddress?.Locality} {PlacemarkAddress?.PostalCode} {PlacemarkAddress?.CountryName}";
		//        }
		//    }
		//}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			_disposable = new CompositeDisposable();
			MainThread.BeginInvokeOnMainThread(() => {
				this.WhenAnyValue(v => v.viewModelLocator.PickupLocation.Position)
			   .Subscribe((Position p) => MainThread.BeginInvokeOnMainThread(() => SetPin(p)))
			   .DisposeWith(_disposable);

				Observable.FromEventPattern<MapClickedEventArgs>(
					mc => mapPickupLocation.MapClicked += mc,
					mc => mapPickupLocation.MapClicked -= mc)
					.Subscribe(ev => viewModelLocator.PickupLocation.ExecuteSetAddress.Execute(ev.EventArgs.Position))
					.DisposeWith(_disposable);
			});
			//string address = App.Locator.HomePage.CommonLatLong.FullAddress;
			//Location location = (await Geocoding.GetLocationsAsync($"{address}")).FirstOrDefault();
			//App.Locator.HomePage.CommonLatLong.latitude = location.Latitude;
			//App.Locator.HomePage.CommonLatLong.longitude = location.Latitude;

		}


		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_disposable.Dispose();
		}

		private Unit SetPin(Position position)
		{
			Pin pin = new Pin {
				Label = "The Place",
				Address = $"{App.Locator.PickupLocation.Address1}, {App.Locator.PickupLocation.Address2}",
				Type = PinType.Place,
				Position = position,
			};

			var latDegrees = mapPickupLocation.VisibleRegion?.LatitudeDegrees ?? 0.01;
			var longDegrees = mapPickupLocation.VisibleRegion?.LongitudeDegrees ?? 0.01;
			mapPickupLocation.MoveToRegion(new MapSpan(position, latDegrees, longDegrees));
			mapPickupLocation?.Pins?.Clear();
			mapPickupLocation?.Pins?.Add(pin);
			return Unit.Default;
		}

		async void CustomEntry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
		{
			await App.Locator.PickupLocation.AddressReturnCommandExecute();
		}
	}
}
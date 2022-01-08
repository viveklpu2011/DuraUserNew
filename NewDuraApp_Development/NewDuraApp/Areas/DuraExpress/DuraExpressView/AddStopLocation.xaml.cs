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
	public partial class AddStopLocation : ContentPage
	{
		ViewModelLocator viewModelLocator;
		private CompositeDisposable _disposable;
		public AddStopLocation()
		{
			viewModelLocator = new ViewModelLocator();
			InitializeComponent();
			//App.Locator.AddStopLocation.InitilizeData();
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
		//        App.Locator.AddStopLocation.AddStopLocation = location;
		//        var PlacemarkAddress = await LocationHelpers.GetAddressBasedOnLatLong(location);
		//        App.Locator.AddStopLocation.PlacemarkAddress = await LocationHelpers.GetAddressBasedOnLatLong(location);
		//        if (PlacemarkAddress != null)
		//        {
		//            App.Locator.AddStopLocation.Address1 = $"{PlacemarkAddress?.FeatureName} {PlacemarkAddress?.SubAdminArea}";
		//            App.Locator.AddStopLocation.Address2 = $"{PlacemarkAddress?.Locality} {PlacemarkAddress?.PostalCode} {PlacemarkAddress?.CountryName}";
		//        }
		//    }
		//}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_disposable = new CompositeDisposable();

			// and then:
			Task.Run(() => {

				this.WhenAnyValue(v => v.viewModelLocator.AddStopLocation.Position)
					.Subscribe((Position p) => MainThread.BeginInvokeOnMainThread(() => SetPin(p)))
					.DisposeWith(_disposable);

				Observable.FromEventPattern<MapClickedEventArgs>(
					mc => mapPickupLocation.MapClicked += mc,
					mc => mapPickupLocation.MapClicked -= mc)
					.Subscribe(ev => viewModelLocator.AddStopLocation.ExecuteSetAddress.Execute(ev.EventArgs.Position))
					.DisposeWith(_disposable);
			});


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
				Address = $"{App.Locator.AddStopLocation.Address1}, {App.Locator.AddStopLocation.Address2}",
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
			await App.Locator.AddStopLocation.AddressReturnCommandExecute();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NewDuraApp.Areas.Profile.Menu.MyAddress.Views
{
	public partial class AddNewAddressPage : ContentPage
	{
		ViewModelLocator viewModelLocator;
		private CompositeDisposable _disposable;
		public AddNewAddressPage()
		{
			viewModelLocator = new ViewModelLocator();
			InitializeComponent();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			_disposable = new CompositeDisposable();

			this.WhenAnyValue(v => v.viewModelLocator.AddNewAddressPage.Position)
				.Subscribe((Position p) => MainThread.BeginInvokeOnMainThread(() => SetPin(p)))
				.DisposeWith(_disposable);

			Observable.FromEventPattern<MapClickedEventArgs>(
				mc => mapPickupLocation.MapClicked += mc,
				mc => mapPickupLocation.MapClicked -= mc)
				.Subscribe(ev => viewModelLocator.AddNewAddressPage.ExecuteSetAddress.Execute(ev.EventArgs.Position))
				.DisposeWith(_disposable);
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
				Address = $"{App.Locator.AddNewAddressPage.HomeAddress}",
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
		void pickertype_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (pickertype.SelectedIndex != -1) {
				App.Locator.AddNewAddressPage.AddressType = pickertype.Items[pickertype.SelectedIndex];
			}
		}

		void chkDefault_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
		{
			if (e.Value) {
				App.Locator.AddNewAddressPage.Default = "1";
			} else {
				App.Locator.AddNewAddressPage.Default = "0";
			}
		}

		async void CustomEntry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
		{
			await App.Locator.AddNewAddressPage.AddressReturnCommandExecute();
		}

		void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
		{
			try {
				pickertype.Focus();
			} catch (Exception ex) {

			}
		}
	}
}

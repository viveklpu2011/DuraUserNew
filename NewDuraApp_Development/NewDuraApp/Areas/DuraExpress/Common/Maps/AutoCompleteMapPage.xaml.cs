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

namespace NewDuraApp.Areas.DuraExpress.Common.Maps
{
	public partial class AutoCompleteMapPage : ContentPage
	{
		ViewModelLocator viewModelLocator;
		private CompositeDisposable _disposable;
		public AutoCompleteMapPage()
		{
			viewModelLocator = new ViewModelLocator();
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_disposable = new CompositeDisposable();
			txtsearch.Unfocus();
			this.WhenAnyValue(v => v.viewModelLocator.AutoCompleteMapPage.Position)
				.Subscribe((Position p) => MainThread.BeginInvokeOnMainThread(() => SetPin(p)))
				.DisposeWith(_disposable);

			Observable.FromEventPattern<MapClickedEventArgs>(
				mc => mapPopup.MapClicked += mc,
				mc => mapPopup.MapClicked -= mc)
				.Subscribe(ev => viewModelLocator.AutoCompleteMapPage.ExecuteSetAddress.Execute(ev.EventArgs.Position))
				.DisposeWith(_disposable);
		}
		private async void OnTextChanged(object sender, EventArgs eventArgs)
		{
			try {
				if (!viewModelLocator.AutoCompleteMapPage.IsReadOnlyText) {
					if (!string.IsNullOrWhiteSpace(viewModelLocator.AutoCompleteMapPage.AddressText)) {
						await viewModelLocator.AutoCompleteMapPage.GetPlacesPredictionsAsync();
					} else {
						viewModelLocator.AutoCompleteMapPage.IsVisibleListView = false;
					}
				}
			} catch (Exception ex) { }

		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_disposable.Dispose();
			App.Locator.AutoCompleteMapPage.Addresses.Clear();
			App.Locator.AutoCompleteMapPage.Addresses = null;
		}

		private Unit SetPin(Position position)
		{
			Pin pin = new Pin {
				Label = "The Place",
				Address = $"{App.Locator.AutoCompleteMapPage.AddressText}",
				Type = PinType.Place,
				Position = position,

			};

			var latDegrees = mapPopup.VisibleRegion?.LatitudeDegrees ?? 0.01;
			var longDegrees = mapPopup.VisibleRegion?.LongitudeDegrees ?? 0.01;
			mapPopup.MoveToRegion(new MapSpan(position, latDegrees, longDegrees));
			mapPopup?.Pins?.Clear();
			mapPopup?.Pins?.Add(pin);
			return Unit.Default;
		}
		async void CustomEntry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
		{
			await App.Locator.AutoCompleteMapPage.AddressReturnCommandExecute();
		}

		async void CustomEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(viewModelLocator.AutoCompleteMapPage.Address2)) {
				await viewModelLocator.AutoCompleteMapPage.GetCountryCodeAutocomplete(viewModelLocator.AutoCompleteMapPage.Address2);
			} else {
				viewModelLocator.AutoCompleteMapPage.IsVisibleCountryListView = false;
			}
		}        //protected override bool OnBackButtonPressed()

		//{
		//    return true;
		//}
	}
}

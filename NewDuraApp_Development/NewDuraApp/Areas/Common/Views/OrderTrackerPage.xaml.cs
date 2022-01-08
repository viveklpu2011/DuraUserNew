using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DuraApp.Core.Models.ResponseModels;
using NewDuraApp.Helpers;
using NewDuraApp.LocationTracker.Helper;
using NewDuraApp.LocationTracker.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NewDuraApp.Areas.Common.Views
{
	public partial class OrderTrackerPage : ContentPage
	{

		readonly GroundOverlay _overlay;
		Location location;
		IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();
		public OrderTrackerPage()
		{
			InitializeComponent();

			// AddMapStyle();
		}
		void AddMapStyle()
		{
			var assembly = typeof(OrderTrackerPage).GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream($"NewDuraApp.LocationTracker.MapStyle.json");
			string styleFile;
			using (var reader = new System.IO.StreamReader(stream)) {
				styleFile = reader.ReadToEnd();
			}

			map.MapStyle = MapStyle.FromJson(styleFile);

		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			TrackPath_Clicked();
		}
		async void TrackPath_Clicked()
		{

			var currentPosition = await LocationHelpers.getCurrentPosition();
			location = new Location(currentPosition);
			if (currentPosition == null && currentPosition.Latitude < 0 && currentPosition.Longitude < 0) {
				await Navigation.PopAsync();
				await DisplayAlert("Not found.", "User current location not found.", "");
			}

			var googleDirection = await googleMapsApi.GetDirections(App.Locator.OrderTrackerPage.GetOrderData.pickuplat, App.Locator.OrderTrackerPage.GetOrderData.pickuplon, Convert.ToString(currentPosition.Latitude), Convert.ToString(currentPosition.Longitude));
			if (googleDirection.Routes != null && googleDirection.Routes.Count > 0) {
				List<Position> posi = new List<Position>();
				if (googleDirection.Routes.FirstOrDefault().Legs != null
					&& googleDirection.Routes.FirstOrDefault().Legs.FirstOrDefault().Steps != null
					&& googleDirection.Routes.FirstOrDefault().Legs.FirstOrDefault().Steps.Count > 0) {
					Position position = new Position();
					foreach (var item in googleDirection.Routes.First().Legs.FirstOrDefault().Steps) {
						position = new Position(item.StartLocation.Lat, item.StartLocation.Lng);
						posi.Add(position);
					}
				}

				//var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));

				map.Polylines.Clear();

				var polyline = new Xamarin.Forms.GoogleMaps.Polyline();
				polyline.StrokeColor = Color.Black;
				polyline.StrokeWidth = 3;

				foreach (var p in posi) {
					polyline.Positions.Add(p);

				}
				try {
					map.Polylines.Add(polyline);
				} catch (Exception ex) { }
				map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(polyline.Positions[0].Latitude, polyline.Positions[0].Longitude), Xamarin.Forms.GoogleMaps.Distance.FromMiles(0.50f)));
				map.UiSettings.CompassEnabled = true;
				var pin = new Xamarin.Forms.GoogleMaps.Pin {

					Type = PinType.SearchResult,
					Position = new Xamarin.Forms.GoogleMaps.Position(polyline.Positions.First().Latitude, polyline.Positions.First().Longitude),
					Label = "Pin",
					Address = "Pin",
					Tag = "CirclePoint",
					Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("circleimgnew.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "circleimg.png", WidthRequest = 25, HeightRequest = 25 })

				};
				map.Pins.Add(pin);

				var positionIndex = 1;

				Device.StartTimer(TimeSpan.FromSeconds(.1), () => {
					if (posi.Count > positionIndex) {
						// var bearing = map.GroundOverlays.FirstOrDefault().Bearing;
						UpdatePostions(posi[positionIndex]);
						positionIndex++;
						return true;
					} else {
						App.Locator.OrderTrackerPage.LeftTime = "Driver reached";
						return false;
					}
				});
			} else {
				await App.Current.MainPage.DisplayAlert("", "No route found", "Ok");
			}

		}

		async void UpdatePostions(Xamarin.Forms.GoogleMaps.Position position)
		{
			if (map.Pins.Count == 1 && map.Polylines != null && map.Polylines?.Count > 1)
				return;

			var cPin = map.Pins.FirstOrDefault();

			if (cPin != null) {
				cPin.Position = new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude);
				cPin.Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("circleimgnew.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "circleimg.png", WidthRequest = 25, HeightRequest = 25 });
				await map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(
					new CameraPosition(new Position(cPin.Position.Latitude, cPin.Position.Longitude),
					17d, // bearing(rotation)
					0d)), // tilt
					TimeSpan.FromSeconds(1));
				//map.MoveToRegion(MapSpan.FromCenterAndRadius(cPin.Position, Distance.FromMeters(500)));
				var previousPosition = map.Polylines?.FirstOrDefault()?.Positions?.FirstOrDefault();
				map.Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
				var souce = $"{location.Latitude},{location.Longitude}";
				var dest = $"{position.Latitude},{position.Longitude}";
				await App.Locator.OrderTrackerPage.GetGoogleTime(souce, dest);
			} else {
				map.Polylines?.FirstOrDefault()?.Positions?.Clear();
			}
		}

		//public async void OnEnterAddressTapped(object sender, EventArgs e)
		//{
		//    await Navigation.PushAsync(new SearchPlacePage() { BindingContext = this.BindingContext }, false);

		//}

		//public void Handle_Stop_Clicked(object sender, EventArgs e)
		//{
		//    searchLayout.IsVisible = true;
		//    stopRouteButton.IsVisible = false;
		//    map.Polylines.Clear();
		//    map.Pins.Clear();
		//}

		////Center map in actual location 
		//protected override void OnAppearing()
		//{
		//    base.OnAppearing();
		//    map.GetActualLocationCommand.Execute(null);
		//}

		//void OnCalculate(System.Object sender, System.EventArgs e)
		//{
		//    searchLayout.IsVisible = false;
		//    stopRouteButton.IsVisible = true;
		//}

		//async void OnLocationError(System.Object sender, System.EventArgs e)
		//{
		//    await DisplayAlert("Error", "Unable to get actual location", "Ok");
		//}
	}
}

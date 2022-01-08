using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace NewDuraApp.Helpers
{
	public class LocationHelpers
	{


		public static async Task<Location> getCurrentPosition()
		{
			Location location1 = new Location();
			try {
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;
				var location = await locator.GetPositionAsync();
				if (location != null) {
					location1 = new Location(location.Latitude, location.Longitude);
				}
			} catch (FeatureNotSupportedException fnsEx) {
				//return null;
				// Handle not supported on device exception
			} catch (FeatureNotEnabledException fneEx) {
				//return null;
				// Handle not enabled on device exception
			} catch (PermissionException pEx) {
				//return null;
				// Handle permission exception
			} catch (Exception ex) {
				//return null;
				// Unable to get location
			}
			return location1;
		}
		public static async Task<Address> GetAddressBasedOnLatLong(Location location)
		{
			Address placemark = new Address();
			try {
				var locator = CrossGeolocator.Current;
				Plugin.Geolocator.Abstractions.Position pos = new Plugin.Geolocator.Abstractions.Position(location.Latitude, location.Longitude);
				var places = await locator.GetAddressesForPositionAsync(pos);

				placemark = places?.FirstOrDefault();
				if (placemark != null) {
					var geocodeAddress =
						$"AdminArea:       {placemark.AdminArea}\n" +
						$"CountryCode:     {placemark.CountryCode}\n" +
						$"CountryName:     {placemark.CountryName}\n" +
						$"FeatureName:     {placemark.FeatureName}\n" +
						$"Locality:        {placemark.Locality}\n" +
						$"PostalCode:      {placemark.PostalCode}\n" +
						$"SubAdminArea:    {placemark.SubAdminArea}\n" +
						$"SubLocality:     {placemark.SubLocality}\n" +
						$"SubThoroughfare: {placemark.SubThoroughfare}\n" +
						$"Thoroughfare:    {placemark.Thoroughfare}\n";

					Console.WriteLine(geocodeAddress);

				}
			} catch (FeatureNotSupportedException fnsEx) {
				// Feature not supported on device
			} catch (Exception ex) {
				// Handle exception that may have occurred in geocoding
			}
			return placemark;
		}
		public static async Task<Location> GetLatLongBasedOnAddress(string address)
		{
			Location location = new Location();
			try {
				//var locator = CrossGeolocator.Current;
				//var approximateLocations = await locator.GetPositionsForAddressAsync(address);
				//var position = approximateLocations.FirstOrDefault();

				location.Latitude = Helpers.CommonHelper.CurrentLat;
				location.Longitude = Helpers.CommonHelper.CurrentLong;

				if (location != null) {
					Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
				}
			} catch (FeatureNotSupportedException fnsEx) {
				// Feature not supported on device
			} catch (Exception ex) {
				// Handle exception that may have occurred in geocoding
			}
			return location;
		}
		public static async Task<Location> GetLatLongBasedOnAddressonHome(string address)
		{
			Location location = new Location();
			try {
				var locator = CrossGeolocator.Current;
				var approximateLocations = await locator.GetPositionsForAddressAsync(address);
				var position = approximateLocations.FirstOrDefault();

				location.Latitude = position.Latitude;
				location.Longitude = position.Longitude;
				Helpers.CommonHelper.CurrentLat = location.Latitude;
				Helpers.CommonHelper.CurrentLong = location.Longitude;

				if (location != null) {
					Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
				}
			} catch (FeatureNotSupportedException fnsEx) {
				// Feature not supported on device
			} catch (Exception ex) {
				// Handle exception that may have occurred in geocoding
			}
			return location;
		}
	}
}

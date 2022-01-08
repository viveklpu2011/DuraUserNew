using System;
using System.Threading.Tasks;
using NewDuraApp.LocationTracker.Models;

namespace NewDuraApp.LocationTracker.Services
{
    public interface IGoogleMapsApiService
    {
        Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        Task<GooglePlace> GetPlaceDetails(string placeId);
        Task<GoogleDirection> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
    }
}

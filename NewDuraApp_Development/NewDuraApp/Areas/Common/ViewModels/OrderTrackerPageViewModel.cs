using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.LocationTracker.Helper;
using NewDuraApp.LocationTracker.Models;
using NewDuraApp.LocationTracker.Services;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Command = Xamarin.Forms.Command;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class OrderTrackerPageViewModel : AppBaseViewModel
    {

        private INavigationService _navigationService;
        private IUserCoreService _userCoreService;

        public ICommand CalculateRouteCommand { get; set; }
        public ICommand UpdatePositionCommand { get; set; }

        public ICommand LoadRouteCommand { get; set; }
        public ICommand StopRouteCommand { get; set; }
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();

        public bool HasRouteRunning { get; set; }
        string _originLatitud;
        string _originLongitud;
        string _destinationLatitud;
        string _destinationLongitud;

        private string _leftTime;
        public string LeftTime
        {
            get { return _leftTime; }
            set
            {
                _leftTime = value;
                OnPropertyChanged(nameof(LeftTime));
            }
        }

        private bool _isShowLoading;
        public bool IsShowLoading
        {
            get { return _isShowLoading; }
            set
            {
                _isShowLoading = value;
                OnPropertyChanged(nameof(IsShowLoading));
            }
        }

        GooglePlaceAutoCompletePrediction _placeSelected;
        public GooglePlaceAutoCompletePrediction PlaceSelected
        {
            get
            {
                return _placeSelected;
            }
            set
            {
                _placeSelected = value;
                if (_placeSelected != null)
                    GetPlaceDetailCommand.Execute(_placeSelected);
            }
        }

        private GetOrderDetailsModel _getOrderData;
        public GetOrderDetailsModel GetOrderData
        {
            get { return _getOrderData; }
            set
            {
                _getOrderData = value;
                OnPropertyChanged(nameof(GetOrderData));
            }
        }

        public ICommand FocusOriginCommand { get; set; }
        public ICommand GetPlacesCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> RecentPlaces { get; set; } = new ObservableCollection<GooglePlaceAutoCompletePrediction>();
        public bool ShowRecentPlaces { get; set; }
        bool _isPickupFocused = true;

        string _pickupText;
        public string PickupText
        {
            get
            {
                return _pickupText;
            }
            set
            {
                _pickupText = value;
                if (!string.IsNullOrEmpty(_pickupText))
                {
                    _isPickupFocused = true;
                    GetPlacesCommand.Execute(_pickupText);
                }
            }
        }

        string _originText;
        public string OriginText
        {
            get
            {
                return _originText;
            }
            set
            {
                _originText = value;
                if (!string.IsNullOrEmpty(_originText))
                {
                    _isPickupFocused = false;
                    GetPlacesCommand.Execute(_originText);
                }
            }
        }

        public ICommand GetLocationNameCommand { get; set; }
        public bool IsRouteNotRunning
        {
            get
            {
                return !HasRouteRunning;
            }
        }

        public OrderTrackerPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GetOrderData = App.Locator.OrderDetails2.GetOrderData;
            //LoadRouteCommand = new Command(async () => await LoadRoute());
            //StopRouteCommand = new Command(StopRoute);
            //GetPlacesCommand = new Xamarin.Forms.Command<string>(async (param) => await GetPlacesByName(param));
            //GetPlaceDetailCommand = new Xamarin.Forms.Command<GooglePlaceAutoCompletePrediction>(async (param) => await GetPlacesDetail(param));
            //GetLocationNameCommand = new Xamarin.Forms.Command<Position>(async (param) => await GetLocationName(param));
        }
        //public async Task LoadRoute()
        //{
        //    var positionIndex = 1;
        //    var googleDirection = await googleMapsApi.GetDirections(_originLatitud, _originLongitud, _destinationLatitud, _destinationLongitud);
        //    if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
        //    {
        //        var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
        //        CalculateRouteCommand.Execute(positions);

        //        HasRouteRunning = true;

        //        //Location tracking simulation
        //        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        //        {
        //            if (positions.Count > positionIndex && HasRouteRunning)
        //            {
        //                UpdatePositionCommand.Execute(positions[positionIndex]);
        //                positionIndex++;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        });
        //    }
        //    else
        //    {
        //        await App.Current.MainPage.DisplayAlert(":(", "No route found", "Ok");
        //    }

        //}
        //public void StopRoute()
        //{
        //    HasRouteRunning = false;
        //}

        //public async Task GetPlacesByName(string placeText)
        //{
        //    var places = await googleMapsApi.GetPlaces(placeText);
        //    var placeResult = places.AutoCompletePlaces;
        //    if (placeResult != null && placeResult.Count > 0)
        //    {
        //        Places = new ObservableCollection<GooglePlaceAutoCompletePrediction>(placeResult);
        //    }

        //    ShowRecentPlaces = (placeResult == null || placeResult.Count == 0);
        //}

        //public async Task GetPlacesDetail(GooglePlaceAutoCompletePrediction placeA)
        //{
        //    var place = await googleMapsApi.GetPlaceDetails(placeA.PlaceId);
        //    if (place != null)
        //    {
        //        if (_isPickupFocused)
        //        {
        //            PickupText = place.Name;
        //            _originLatitud = $"{place.Latitude}";
        //            _originLongitud = $"{place.Longitude}";
        //            _isPickupFocused = false;
        //            FocusOriginCommand.Execute(null);
        //        }
        //        else
        //        {
        //            _destinationLatitud = $"{place.Latitude}";
        //            _destinationLongitud = $"{place.Longitude}";

        //            RecentPlaces.Add(placeA);

        //            if (_originLatitud == _destinationLatitud && _originLongitud == _destinationLongitud)
        //            {
        //                await App.Current.MainPage.DisplayAlert("Error", "Origin route should be different than destination route", "Ok");
        //            }
        //            else
        //            {
        //                LoadRouteCommand.Execute(null);
        //                await App.Current.MainPage.Navigation.PopAsync(false);
        //                CleanFields();
        //            }

        //        }
        //    }
        //}

        //void CleanFields()
        //{
        //    PickupText = OriginText = string.Empty;
        //    ShowRecentPlaces = true;
        //    PlaceSelected = null;
        //}

        ////Get place 
        //public async Task GetLocationName(Position position)
        //{
        //    try
        //    {
        //        var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
        //        var placemark = placemarks?.FirstOrDefault();
        //        if (placemark != null)
        //        {
        //            PickupText = placemark.FeatureName;
        //        }
        //        else
        //        {
        //            PickupText = string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        internal async Task InitializedData()
        {
            //_originLatitud = "30.707";
            //_originLongitud = "76.708";
            //_destinationLatitud = "30.732999";
            //_destinationLongitud = "76.771163";
        }

        public async Task GetGoogleTime(string source, string destination)
        {
            if (CheckConnection())
            {
                IsShowLoading = true;
                try
                {
                    var result = await _userCoreService.GetGoogleDirectionTime(source, destination);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == "OK")
                    {
                        if (result?.Data?.rows.Count > 0 && result?.Data?.rows != null)
                        {
                            foreach (var item in result?.Data?.rows)
                            {
                                if (item.elements != null && item.elements.Count > 0)
                                {
                                    foreach (var elementitem in item.elements)
                                    {
                                        LeftTime = elementitem.duration.text;
                                        if (elementitem.duration.text == "1 min")
                                        {
                                            LeftTime = AppResources.Driver_arrived;
                                        }
                                    }
                                }
                            }
                            IsShowLoading = false;
                        }
                    }
                    else
                    {
                        ShowToast(AppResources.ServerError);
                    }
                }
                catch (Exception ex)
                {
                    //ShowToast(CommonMessages.ServerError);
                }
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

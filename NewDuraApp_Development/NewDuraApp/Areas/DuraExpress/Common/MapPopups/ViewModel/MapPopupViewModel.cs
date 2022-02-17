using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NewDuraApp.Areas.DuraExpress.Common.MapPopups.ViewModel
{
    public class MapPopupViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;

        private string _pageType;
        public string PageType
        {
            get { return _pageType; }
            set
            {
                _pageType = value;
                OnPropertyChanged(nameof(PageType));
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

        public Xamarin.Forms.Command ExecuteSetPosition { get; set; }
        public Xamarin.Forms.Command<Position> ExecuteSetAddress { get; set; }
        public IAsyncCommand AddressReturnCommand { get; set; }

        private string _address123;
        public string Address123
        {
            get { return _address123; }
            set
            {
                _address123 = value;

                OnPropertyChanged(nameof(Address123));
            }
        }

        private string _address2;
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                OnPropertyChanged(nameof(Address2));
            }
        }

        public MapPopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            AddressReturnCommand = new AsyncCommand(AddressReturnCommandExecute, allowsMultipleExecutions: false);
            ExecuteSetAddress = new Xamarin.Forms.Command<Position>(async (position) => await SetAddress(position));
            ExecuteSetPosition = new Xamarin.Forms.Command(async () => await SetPosition(this.Address123, this.Address2));
        }

        public async Task AddressReturnCommandExecute()
        {
            await SetPosition(this.Address123, this.Address2);
        }

        private async Task SetAddress(Position p)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
            this.Address123 = $"{addrs.SubLocality} {addrs.SubAdminArea}";
            this.Address2 = $"{addrs.PostalCode} {addrs.Locality} {addrs.CountryName}";
            Position = p;
        }

        private async Task SetPosition(string street, string city)
        {
            var location = (await Geocoding.GetLocationsAsync($"{street}, {city}")).FirstOrDefault();
            if (location == null) return;
            Position = new Position(location.Latitude, location.Longitude);
        }

        internal async Task InitilizeData(string pagetype = "")
        {
            PageType = pagetype;
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
}

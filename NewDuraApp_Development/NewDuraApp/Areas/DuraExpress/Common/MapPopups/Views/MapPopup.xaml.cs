using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NewDuraApp.ViewModels;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NewDuraApp.Areas.DuraExpress.Common.MapPopups.Views
{
    public partial class MapPopup : PopupPage
    {
        ViewModelLocator viewModelLocator;
        private CompositeDisposable _disposable;
        public MapPopup()
        {
            viewModelLocator = new ViewModelLocator();
            InitializeComponent();
            //App.Locator.WhereTo.MapWhereToLocation = mapPickupLocation;
            //App.Locator.MapPopup.InitilizeData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _disposable = new CompositeDisposable();

            this.WhenAnyValue(v => v.viewModelLocator.MapPopup.Position)
                .Subscribe((Position p) => MainThread.BeginInvokeOnMainThread(() => SetPin(p)))
                .DisposeWith(_disposable);

            Observable.FromEventPattern<MapClickedEventArgs>(
                mc => mapPopup.MapClicked += mc,
                mc => mapPopup.MapClicked -= mc)
                .Subscribe(ev => viewModelLocator.MapPopup.ExecuteSetAddress.Execute(ev.EventArgs.Position))
                .DisposeWith(_disposable);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _disposable.Dispose();
        }

        private Unit SetPin(Position position)
        {
            Pin pin = new Pin
            {
                Label = "The Place",
                Address = $"{App.Locator.MapPopup.Address123}, {App.Locator.MapPopup.Address2}",
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
            await App.Locator.MapPopup.AddressReturnCommandExecute();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

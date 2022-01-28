using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Command = Xamarin.Forms.Command;

namespace NewDuraApp.Areas.DuraExpress.DuraExpressViewModel
{
    public class PickupScheduleViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand DoneCommand { get; set; }
        private DateTime _datePick;
        private DateTime _datePickLater;
        private IAuthenticationService _authService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand TimeTouchCommand { get; set; }
        public bool ishowtoast;
        private bool _isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private bool _isVisibleLaterView;
        public bool IsVisibleLaterView
        {
            get { return _isVisibleLaterView; }
            set
            {
                _isVisibleLaterView = value;
                OnPropertyChanged(nameof(IsVisibleLaterView));
            }
        }

        private DuraAddressCommonModel _duraAddressCommon;
        private PickupScheduleRequestModel _pickupScheduleRequest;
        public PickupScheduleRequestModel PickupScheduleRequest
        {
            get { return _pickupScheduleRequest; }
            set { _pickupScheduleRequest = value; OnPropertyChanged(nameof(PickupScheduleRequest)); }
        }

        public DuraAddressCommonModel DuraAddressCommon
        {
            get { return _duraAddressCommon; }
            set { _duraAddressCommon = value; OnPropertyChanged(nameof(DuraAddressCommon)); }
        }

        public DateTime DatePick
        {
            get { return _datePick; }
            set
            {
                _datePick = value;
                if (DatePick.Date <= DateTime.Now.Date && _pickTime < DateTime.Now.TimeOfDay && ishowtoast)
                {
                    ShowToast(AppResources.time_should_not_be_less_than_current_time);
                    IsButtonEnabled = false;
                }
                else
                {
                    IsButtonEnabled = true;
                }
                OnPropertyChanged();
            }
        }

        public DateTime DatePickLater
        {
            get { return _datePickLater; }
            set { _datePickLater = value; OnPropertyChanged(nameof(DatePickLater)); }
        }

        private TimeSpan _pickTime;
        public TimeSpan PickTime
        {
            get { return _pickTime; }
            set
            {
                _pickTime = value;
                if (DatePick.Date <= DateTime.Now.Date && _pickTime < DateTime.Now.TimeOfDay && ishowtoast)
                {
                    ShowToast(AppResources.time_should_not_be_less_than_current_time);
                    IsButtonEnabled = false;
                }
                else
                {
                    IsButtonEnabled = true;
                }
                this.OnPropertyChanged(nameof(PickTime));
            }
        }

        private bool _asapIsChecked;
        public bool AsapIsChecked
        {
            get { return _asapIsChecked; }
            set { _asapIsChecked = value; OnPropertyChanged(); }
        }

        private bool _laterIsCheck;
        public bool LaterIsCheck
        {
            get { return _laterIsCheck; }
            set { _laterIsCheck = value; OnPropertyChanged(); }
        }
        private bool _isAsaspSelected;

        public bool IsAsaspSelected
        {
            get { return _isAsaspSelected; }
            set { _isAsaspSelected = value; OnPropertyChanged(); }
        }

        private DateTime _minDate = DateTime.Now;
        private DateTime _minDateLater = DateTime.Now;
        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value; OnPropertyChanged(); }
        }

        public DateTime MinDateLater
        {
            get { return _minDateLater; }
            set { _minDateLater = value; OnPropertyChanged(nameof(MinDateLater)); }
        }

        public ICommand rdcheck => new Command(() =>
        {
            IsVisibleLaterView = false;
            AsapIsChecked = true;
            LaterIsCheck = false;
            IsButtonEnabled = true;
        });

        public ICommand LaterSelected => new Command(() =>
        {
            IsVisibleLaterView = true;
            AsapIsChecked = false;
            LaterIsCheck = true;
            IsButtonEnabled = true;
            DatePick = DateTime.Now;
            PickTime = DateTime.Now.TimeOfDay;
            if (PickTime < DateTime.Now.TimeOfDay)
            {
                IsButtonEnabled = false;
            }
            else
            {
                IsButtonEnabled = true;
            }
        });

        public PickupScheduleViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            DoneCommand = new AsyncCommand(DoneCommandExecute, allowsMultipleExecutions: false);
            AsapIsChecked = true;
            ishowtoast = false;
            MinDate = DateTime.Today;
            PickTime = DateTime.Now.TimeOfDay;
            TimeTouchCommand = new AsyncCommand(TimeTouchCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task TimeTouchCommandExecute()
        {

        }

        private async Task DoneCommandExecute()
        {
            string _type = string.Empty;
            string _date = string.Empty;
            if (AsapIsChecked)
            {
                _type = "ASAP";
                _date = $"{DateTime.Now.Date.ToString("yyyy-MM-dd")} {DateTime.Now.ToString("HH:mm")}";

                App.Locator.DuraExpress.PickupScheduleLocTextVisible = true;
                App.Locator.DuraExpress.PickupScheduleLocText = $"ASAP- Date: {DateTime.Now.Date.ToString("ddd, MMM d, yyyy")}, Time: {DateTime.Now.TimeOfDay.Hours}:{DateTime.Now.TimeOfDay.Minutes}";

                App.Locator.TrackOrder.PickupScheduleLocTextVisible = true;
                App.Locator.TrackOrder.PickupScheduleLocText = $"ASAP- Date: {DateTime.Now.Date.ToString("ddd, MMM d, yyyy")}, Time: {DateTime.Now.TimeOfDay.Hours}:{DateTime.Now.TimeOfDay.Minutes}";
            }
            else
            {
                _type = "Later";
                _date = $"{DatePick.Date.ToString("yyyy-MM-dd")} {PickTime.Hours}:{PickTime.Minutes}";

                App.Locator.DuraExpress.PickupScheduleLocTextVisible = true;
                App.Locator.DuraExpress.PickupScheduleLocText = $"Later- Date: {DatePick.Date.ToString("ddd, MMM d, yyyy")}, Time: {PickTime.Hours}:{PickTime.Minutes}";

                App.Locator.TrackOrder.PickupScheduleLocTextVisible = true;
                App.Locator.TrackOrder.PickupScheduleLocText = $"Later- Date: {DatePick.Date.ToString("ddd, MMM d, yyyy")}, Time: {PickTime.Hours}:{PickTime.Minutes}";
            }
            DuraAddressCommon = new DuraAddressCommonModel();
            DuraAddressCommon.PickupType = _type;
            DuraAddressCommon.PickupDate = _date;
            PickupScheduleRequest = new PickupScheduleRequestModel();
            PickupScheduleRequest.type = DuraAddressCommon.PickupType;
            PickupScheduleRequest.pdate = DuraAddressCommon.PickupDate;
            PickupScheduleRequest.IsAvailablePickupSchedule = true;
            App.Locator.DuraExpress.PickupScheduleRequest = PickupScheduleRequest;
            await App.Locator.TrackOrder.InitilizeData();
            await _navigationService.ClosePopupsAsync();
        }

        internal async Task InitilizeData()
        {
            ishowtoast = true;
        }

    }
}

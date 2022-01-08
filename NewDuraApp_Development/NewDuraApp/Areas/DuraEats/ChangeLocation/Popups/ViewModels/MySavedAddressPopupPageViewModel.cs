using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.ChangeLocation.Popups.ViewModels
{
    public class MySavedAddressPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand ClosePopup { get; set; }
        private ObservableCollection<AddressModel> _addressList;
        public IAsyncCommand<AddressModel> AddressDetails { get; set; }
        public ObservableCollection<AddressModel> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; OnPropertyChanged(nameof(AddressList)); }
        }
        public MySavedAddressPopupPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ClosePopup = new AsyncCommand(ClosePopUpcommand);
            InitilizeData();
        }
        private async Task ClosePopUpcommand()
        {
            await _navigationService.ClosePopupsAsync();
        }
        public async Task InitilizeData()
        {
            AddressList = new ObservableCollection<AddressModel>
            {
                new AddressModel()
                {
                   AddressId=0,
                   Block="1976 M Reyes St Bangka",
                   City="Makati City",
                   ContactNumber="+63 7898547856",
                   ContactPerson="John Smith",
                   HouseNo="1976 Capt. M. Reyes, Makati, Metro Manila Phillipines",
                   HouseType="Home",
                   AddressIcon=FontIcons.FontIconsClass.Home
                },
                new AddressModel()
                {
                   AddressId=0,
                   Block="1976 M Reyes St Bangka",
                   City="Makati City",
                   ContactNumber="+63 7898547856",
                   ContactPerson="John Smith",
                   HouseNo="1976 Capt. M. Reyes, Makati, Metro Manila Phillipines",
                   HouseType="Work",
                   AddressIcon=FontIcons.FontIconsClass.BriefcaseVariant
                },
            };
        }
    }
}

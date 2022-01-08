using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.Services.LocationService;
using NewDuraApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace NewDuraApp.Areas.Common.Permissions.ViewModels
{
    public class PermissionPageViewModel : AppBaseViewModel
    {
        public IAsyncCommand<PermissionInfo> GrandpermissionCommand { get; set; }
        INavigationService _navigationService;
        public ILocationService locationService => DependencyService.Get<ILocationService>();
        public IPlatformSpecificLocationService platFormLocationService => DependencyService.Get<IPlatformSpecificLocationService>();

        private ObservableCollection<PermissionInfo> _permissionsList;
        public ObservableCollection<PermissionInfo> PermissionsList
        {
            get { return _permissionsList; }
            set
            {
                _permissionsList = value;
                OnPropertyChanged(nameof(PermissionsList));
            }
        }

        private int _caroselPosition;
        public int CaroselPosition
        {
            get { return _caroselPosition; }
            set
            {
                _caroselPosition = value;
                OnPropertyChanged(nameof(CaroselPosition));
            }
        }

        public PermissionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GrandpermissionCommand = new AsyncCommand<PermissionInfo>(GrandpermissionCommandExecute);
            //GetCountryCode();
        }

        private async Task GrandpermissionCommandExecute(PermissionInfo arg)
        {
            if (arg != null)
            {
                if (CaroselPosition == 2)
                {
                    var statusLocationService = platFormLocationService.IsLocationServiceEnabled();
                    if (!statusLocationService)
                    {
                        ShowSettingToast(AppResources.Location_service_not_enabled, AppResources.Settings);
                        return;
                    }
                }
                arg.IsGranted = await CheckAndRequestPermissionAsync(arg.Permission) == PermissionStatus.Granted;

                if (arg.IsGranted && CaroselPosition == 0)
                {
                    CaroselPosition = 1;
                }
                else if (arg.IsGranted && CaroselPosition == 1)
                {
                    CaroselPosition = 2;
                }
                else if (arg.IsGranted && CaroselPosition == 2)
                {
                    CaroselPosition = 3;
                }
                else if (arg.IsGranted && CaroselPosition == 3)
                {
                    SettingsExtension.IsPermissionEnabled = true;
                    if (_navigationService.GetCurrentPageViewModel() != typeof(LoginPageViewModels))
                    {
                        await App.Locator.LoginPage.GetAllLocation();
                        await _navigationService.NavigateToAsync<LoginPageViewModels>();
                    }
                }
            }
        }

        internal async Task InitilizeData()
        {
            await LoadPermissions();
        }
        async Task LoadPermissions()
        {
            PermissionsList = new ObservableCollection<PermissionInfo>()
            {
                {
                    await CreatePermission(new Camera(), "camera", AppResources.Camera_Permission, AppResources.Need_to_access_your_camera_so_that_you_can_upload_your_profile_picture_and_other_documents)
                },
                {
                    await CreatePermission(new ContactsRead(), AppResources.contacts, AppResources.Contact_Permission,AppResources.We_need_to_access_your_phone_book_in_order_to_save_address_with_contact)
                },
                {
                    await CreatePermission(new LocationWhenInUse(), AppResources.locations, AppResources.Location_Permission, AppResources.We_need_to_find_where_you_are_in_order_to_better_facilitate_you)
                },
                {
                    await CreatePermission(new StorageRead(), "gallery", AppResources.Storage_Permission, AppResources.Need_to_access_your_gallery_so_that_you_can_easily_upload_your_documents)
                }
            };
        }

        async Task<PermissionInfo> CreatePermission(BasePermission permission, string icon, string name, string description)
        {
            return new PermissionInfo()
            {
                Icon = icon,
                Permission = permission,
                Name = name,
                Description = description,
                IsGranted = await permission.CheckStatusAsync() == PermissionStatus.Granted
            };
        }

        async Task<PermissionStatus> CheckAndRequestPermissionAsync(BasePermission permission)
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }
            return status;
        }
    }
}

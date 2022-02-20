using System;
using System.IO;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class PersonalDetailsViewModel : AppBaseViewModel
    {
        public IAsyncCommand GoToReferralCmd { get; set; }
        public IAsyncCommand PickImageCommand { get; set; }
        private SignupRequestModel _signupRequestModelVM;
        INavigationService _navigationService;
        private bool _isFirstNameErrorVisible;
        private bool _isLastNameErrorVisible;
        private bool _isDoneButtonEnabled;

        private byte[] _userImage;
        public byte[] UserImage
        {
            get { return _userImage; }
            set { _userImage = value; OnPropertyChanged(nameof(UserImage)); }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }

        public bool IsFirstNameErrorVisible
        {
            get { return _isFirstNameErrorVisible; }
            set { _isFirstNameErrorVisible = value; OnPropertyChanged(nameof(IsFirstNameErrorVisible)); }
        }

        public bool IsLastNameErrorVisible
        {
            get { return _isLastNameErrorVisible; }
            set { _isLastNameErrorVisible = value; OnPropertyChanged(nameof(IsLastNameErrorVisible)); }
        }

        private bool _isProfilePic;
        public bool IsProfilePic
        {
            get { return _isProfilePic; }
            set { _isProfilePic = value; OnPropertyChanged(nameof(IsProfilePic)); }
        }

        public bool IsDoneButtonEnabled
        {
            get { return _isDoneButtonEnabled; }
            set { _isDoneButtonEnabled = value; OnPropertyChanged(nameof(IsDoneButtonEnabled)); }
        }

        public SignupRequestModel SignupRequestModelVM
        {
            set
            {
                _signupRequestModelVM = value;
                OnPropertyChanged(nameof(SignupRequestModelVM));
            }
            get => _signupRequestModelVM;
        }

        public PersonalDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToReferralCmd = new AsyncCommand(GoToReferralCmdExecute, allowsMultipleExecutions: false);
            PickImageCommand = new AsyncCommand(PickImage, allowsMultipleExecutions: false);
            IsProfilePic = false;
            ProfileImage = ImageHelper.GetImageNameFromResource("user_pic_placeholder.png");
        }

        private async Task GoToReferralCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(ReferralCodeViewModel))
            {
                if (IsProfilePic == false)
                {
                    ShowAlert(AppResources.Please_select_profile_pic, "Profile", AppResources.Ok);
                    return;
                }
                SignupRequestModelVM = new SignupRequestModel();
                SignupRequestModelVM = App.Locator.SignUpPage.SignupRequestModelVM;
                SignupRequestModelVM.first_name = FirstName;
                SignupRequestModelVM.last_name = LastName;
                SignupRequestModelVM.userImage = UserImage;
                SignupRequestModelVM.login_type = "normal";
                await _navigationService.NavigateToAsync<ReferralCodeViewModel>();
                FirstName = LastName = string.Empty;
            }
        }

        private string _profilepic;
        public string Profilepic
        {
            get { return _profilepic; }
            set { _profilepic = value; OnPropertyChanged(nameof(Profilepic)); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                if (!string.IsNullOrEmpty(_firstName))
                {
                    if (RegexUtilities.AlphanumericNameValidation(_firstName))
                        IsFirstNameErrorVisible = false;
                    else
                        IsFirstNameErrorVisible = true;
                    CheckValidation();
                }
                else
                {
                    IsFirstNameErrorVisible = false;
                }
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                if (!string.IsNullOrEmpty(_lastName))
                {
                    if (RegexUtilities.AlphanumericNameValidation(_lastName))
                        IsLastNameErrorVisible = false;
                    else
                        IsLastNameErrorVisible = true;
                    CheckValidation();
                }
                else
                {
                    IsLastNameErrorVisible = false;
                }
                OnPropertyChanged(nameof(LastName));
            }
        }

        private bool _firstNameNotValid;
        public bool FirstNameNotValid
        {
            get => _firstNameNotValid;
            set
            {
                _firstNameNotValid = value;
                OnPropertyChanged(nameof(FirstNameNotValid));
            }
        }

        private bool _lasttNameNotValid;
        public bool LastNameNotValid
        {
            get => _lasttNameNotValid;
            set
            {
                _lasttNameNotValid = value;
                OnPropertyChanged(nameof(LastNameNotValid));
            }
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                IsDoneButtonEnabled = false;
                return false;
            }
            else if (IsFirstNameErrorVisible || IsLastNameErrorVisible)
            {
                IsDoneButtonEnabled = false;
                return false;
            }
            else
            {
                IsDoneButtonEnabled = true;
                return true;
            }
        }

        private async Task PickImage()
        {
            var res = await ShowCameraPopup();
            if (res != null)
            {
                if (res.Item1 == ProfilePicSelectionType.Camera)
                {
                    try
                    {
                        var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                        if (status != PermissionStatus.Granted)
                        {
                            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                            {
                                ShowToast("Camera Permission Required");
                            }
                            status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                        }
                        if (status == PermissionStatus.Granted)
                        {
                            try
                            {
                                await CrossMedia.Current.Initialize();
                                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                                {
                                    ShowAlert("No Camera", ":( No camera avaialble.", "OK");
                                    return;
                                }
                                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                {
                                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                                    Directory = "Dura",
                                    CompressionQuality = 92,
                                    Name = "test.jpg"
                                });
                                if (file == null)
                                    return;

                                ProfileImage = ImageSource.FromStream(() =>
                                {
                                    var stream = file.GetStream();
                                    return stream;
                                });
                                UserImage = ImageHelper.ReadToEnd(file.GetStream());
                                IsProfilePic = true;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (status != PermissionStatus.Unknown)
                        {
                            status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (res.Item1 == ProfilePicSelectionType.Gallery)
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        ShowAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                        return;
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                            CompressionQuality = 82
                        });
                        await Task.Delay(1000);
                        if (file == null)
                            return;
                        ProfileImage = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        UserImage = ImageHelper.ReadToEnd(file.GetStream());
                        IsProfilePic = true;
                    });
                }
            }
        }

        internal async Task InitializedData()
        {
            CheckValidation();
            string defaultImage = "user_pic_placeholder.png";
            UserImage = await ImageHelper.GetStreamFormResource(defaultImage);
            ProfileImage = ImageSource.FromStream(() => new MemoryStream(UserImage));
        }
    }
}

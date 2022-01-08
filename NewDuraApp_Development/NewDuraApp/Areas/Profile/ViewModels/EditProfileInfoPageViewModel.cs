using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Profile.PopupViews.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Plugin.Media;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.ViewModels
{
    public class EditProfileInfoPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        private IAuthenticationService _authenticationService;
        public IAsyncCommand NavigateToOTP { get; set; }
        public IAsyncCommand PickImageCommand { get; set; }
        public IAsyncCommand UpdateUserNameCommand { get; set; }
        public IAsyncCommand SaveLocationCommand { get; set; }
        private string _titleText = AppResources.Edit_Info;
        private List<LocationDataResponse> locList;
        private ObservableCollection<NewLocationDataResponse> _allLocationList;
        public ObservableCollection<NewLocationDataResponse> AllLocationList
        {
            get { return _allLocationList; }
            set
            {
                try
                {
                    SetProperty(ref _allLocationList, value);
                }
                catch (Exception ex) { }
            }
        }
        private ObservableCollection<NewLocationDataResponse> _allLocationListCode;
        public ObservableCollection<NewLocationDataResponse> AllLocationListCode
        {
            get { return _allLocationListCode; }
            set
            {
                try
                {
                    SetProperty(ref _allLocationListCode, value);
                }
                catch (Exception ex) { }
            }
        }


        private string _buttonTextOTP = AppResources.Send_OTP;
        public string ButtonTextOTP
        {
            get { return _buttonTextOTP; }
            set
            {
                _buttonTextOTP = value;
                OnPropertyChanged(nameof(ButtonTextOTP));
            }
        }

        private ObservableCollection<NewLocationDataResponse> _countriesList;
        public ObservableCollection<NewLocationDataResponse> CountriesList
        {
            get { return _countriesList; }
            set
            {
                _countriesList = value;
                this.OnPropertyChanged(nameof(CountriesList));
            }
        }
        private bool _isLocationSaveButtonEnabled;
        public bool IsLocationSaveButtonEnabled
        {
            get { return _isLocationSaveButtonEnabled; }
            set
            {
                _isLocationSaveButtonEnabled = value;
                OnPropertyChanged(nameof(IsLocationSaveButtonEnabled));
            }
        }

        private NewLocationDataResponse _selectedLocation;
        public NewLocationDataResponse SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                if (_selectedLocation != null)
                {
                    if (!RegexUtilities.EmptyString(_selectedLocation.country_name))
                        IsLocationSaveButtonEnabled = false;
                    else
                        IsLocationSaveButtonEnabled = true;
                }
                this.OnPropertyChanged(nameof(SelectedLocation));
            }
        }
        private string _countriesTitle;
        private NewLocationDataResponse _selectedCountries;
        public NewLocationDataResponse SelectedCountries
        {
            get { return _selectedCountries; }
            set
            {
                _selectedCountries = value;
                this.OnPropertyChanged(nameof(SelectedCountries));
                this.OnPropertyChanged(nameof(CountriesTitle));
                CountriesTitle = $"+{_selectedCountries.country_code}";
            }
        }

        public string CountriesTitle
        {
            get { return _countriesTitle; }
            set
            {
                _countriesTitle = value;
                this.OnPropertyChanged("CountriesTitle");
            }
        }
        private GetProfileDetailsModel _profileDetails;
        public GetProfileDetailsModel ProfileDetails
        {
            get { return _profileDetails; }
            set
            {
                _profileDetails = value;
                OnPropertyChanged(nameof(ProfileDetails));
            }
        }

        private byte[] _productImage;
        public byte[] ProductImage
        {
            get { return _productImage; }
            set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
        }
        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
        }
        private bool _isVisileEditNameStack = true, _isVisibleEditPhoneStack, _isVisibleEditCityStack;
        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value; OnPropertyChanged("TitleText"); }
        }
        public bool IsVisileEditNameStack
        {
            get { return _isVisileEditNameStack; }
            set { _isVisileEditNameStack = value; OnPropertyChanged("IsVisileEditNameStack"); }
        }
        public bool IsVisibleEditPhoneStack
        {
            get { return _isVisibleEditPhoneStack; }
            set { _isVisibleEditPhoneStack = value; OnPropertyChanged("IsVisibleEditPhoneStack"); }
        }
        public bool IsVisibleEditCityStack
        {
            get { return _isVisibleEditCityStack; }
            set { _isVisibleEditCityStack = value; OnPropertyChanged("IsVisibleEditCityStack"); }
        }

        private string _stopWatchSeconds;

        public EditProfileInfoPageViewModel(INavigationService navigationService, IUserCoreService userCoreService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            _authenticationService = authenticationService;
            NavigateToOTP = new AsyncCommand(NavigateToOTPPage, allowsMultipleExecutions: false);
            PickImageCommand = new AsyncCommand(PickImage, allowsMultipleExecutions: false);
            UpdateUserNameCommand = new AsyncCommand(UpdateUserNameCommandExecute, allowsMultipleExecutions: false);
            SaveLocationCommand = new AsyncCommand(SaveLocationCommandExecute, allowsMultipleExecutions: false);
            //GetCountryCode();
        }

        private async Task SaveLocationCommandExecute()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    var form = new MultipartFormDataContent();


                    form.Add(new StringContent(Convert.ToString(SelectedLocation.id)), "country_id");
                    form.Add(new StringContent(Convert.ToString(SettingsExtension.UserId)), "user_id");


                    var result = await TryWithErrorAsync(_userCoreService.UpdateUserCountry(form, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        await App.Locator.MyProfile.InitilizeData();
                        await App.Locator.ProfilePage.InitilizeData(ProfileDetails);
                        await _navigationService.NavigateBackAsync();
                        ShowToast(AppResources.Location_updated_successfully);

                    }
                    else
                    {
                        ShowAlert(AppResources.Location_not_updated_Dura_Server_down);
                    }
                }
                catch (Exception ex)
                {
                    //ShowToast(CommonMessages.ServerError);
                }
                //if (locList != null && locList.Count > 0)
                //{
                //    AllLocationList = new ObservableCollection<NewLocationDataResponse>(locList);
                //}
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        private async Task PickImage()
        {
            var res = await ShowCameraPopup();
            //var ress = ret.Item1;
            //var res = await ShowCameraActionSheet();
            if (res != null)
            {
                if (res.Item1 == ProfilePicSelectionType.Camera)
                {
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
                    if (file != null)
                    {
                        ProfileImage = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImage = ImageHelper.ReadToEnd(file.GetStream());
                    }
                    else
                    {
                        ProductImage = string.IsNullOrEmpty(ProfileDetails?.profile_image) ? await ImageHelper.GetStreamFormResource("user_pic_placeholder.png") : await ImageHelper.GetImageFromUrl(ProfileDetails?.profile_image);
                        ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
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

                        if (file != null)
                        {
                            ProfileImage = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                return stream;
                            });
                            try
                            {
                                ProductImage = ImageHelper.ReadToEnd(file.GetStream());
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            ProductImage = string.IsNullOrEmpty(ProfileDetails?.profile_image) ? await ImageHelper.GetStreamFormResource("user_pic_placeholder.png") : await ImageHelper.GetImageFromUrl(ProfileDetails?.profile_image);
                            ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
                        }
                    });


                }

            }


        }
        private async Task NavigateToOTPPage()
        {
            if (ButtonTextOTP != "Save Phone")
            {
                await SendOTP();
            }
            else
            {
                await UpdatePhone();
            }

        }
        private async Task SendOTP()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {

                    OTPRequestModel oTPRequestModel = new OTPRequestModel
                    {
                        country_code = CountriesTitle,
                        phone = ProfileDetails?.phone
                    };
                    var result = await TryWithErrorAsync(_authenticationService.SendOTP(oTPRequestModel), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        //ShowToast($"Your OTP is {result?.Data?.otp}");
                        App.Locator.CurrentUser.SendWay = SendInvite.OTP.ToString();
                        App.UserPhone = $"{CountriesTitle}{ProfileDetails?.phone}";
                        OTPPopupPageViewModel oTPPopupPageViewModel = new OTPPopupPageViewModel(_navigationService, _authenticationService, _userCoreService);
                        await _navigationService.NavigateToPopupAsync<OTPPopupPageViewModel>(true, oTPPopupPageViewModel);

                        //App.Locator.OTPPopupPage.PhoneNumber = ProfileDetails?.phone;
                        await App.Locator.OTPPopupPage.InitilizeData();
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                    ShowToast(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
        private async Task UpdatePhone()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    UpdatePhoneRequestModel updatePhoneRequestModel = new UpdatePhoneRequestModel
                    {
                        phone = ProfileDetails?.phone,
                        user_id = SettingsExtension.UserId,
                        country_code = CountriesTitle
                    };
                    var result = await TryWithErrorAsync(_userCoreService.UpdatePhone(updatePhoneRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        ShowToast(AppResources.Phone_number_updated_successfully);
                        await App.Locator.MyProfile.InitilizeData();
                        await App.Locator.ProfilePage.InitilizeData(ProfileDetails);
                        await _navigationService.NavigateBackAsync();
                    }
                    else
                    {
                        ShowToast(AppResources.Please_recheck_your_phone_number_It_is_already_present_in_dura_server);
                        //ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                    //ShowToast(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }
        internal async Task InitilizeData(GetProfileDetailsModel args = null)
        {
            ShowLoading();
            if (args != null)
            {
                ProfileDetails = args;
                ProductImage = string.IsNullOrEmpty(ProfileDetails?.profile_image) ? await ImageHelper.GetStreamFormResource("user_pic_placeholder.png") : await ImageHelper.GetImageFromUrl(ProfileDetails?.profile_image);
                ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));

                if (App.Locator.HomePage.AllLocationList == null)
                    await App.Locator.HomePage.GetAllLocation();


                CountriesList = App.Locator.HomePage.AllLocationList;
                AllLocationList = App.Locator.HomePage.AllLocationList;
                AllLocationListCode = App.Locator.HomePage.AllLocationListCode;


                //var DistinctItems = App.Locator.HomePage.AllLocationList.GroupBy(x => x.country_code).Select(y => y.First());


                if (IsVisibleEditCityStack)
                {
                    SelectedLocation = GetSelectedCountryByCountryId(args.country_id);
                }
                else
                {
                    ButtonTextOTP = AppResources.Send_OTP;
                    try
                    {
                        var countryCode = Convert.ToInt32(ProfileDetails?.country_code.Remove(0, 1));
                        //SelectedCountries = GetCountryBasedOnCountryCode(countryCode);
                        CountriesTitle = $"+{SelectedCountries?.country_code}";
                    }
                    catch (Exception ex) { HideLoading(); }
                }
            }
            HideLoading();
        }
        private NewLocationDataResponse GetCountryBasedOnCountryCode(int countrycode)
        {
            NewLocationDataResponse countries = new NewLocationDataResponse();

            if (countrycode > 0)
            {
                if (CountriesList != null && CountriesList.Count > 0)
                {
                    countries = CountriesList.Where(x => x.country_code == countrycode).FirstOrDefault();

                }
            }
            return countries;
        }
        //public async Task GetCountryCode()
        //{
        //    ShowLoading();
        //    try
        //    {
        //        string jsonFileName = "countriescode.json";

        //        CountriesList = new ObservableCollection<Countries>();
        //        var assembly = typeof(EditProfileInfoPage).GetTypeInfo().Assembly;
        //        Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.CommonData.{jsonFileName}");
        //        using (var reader = new System.IO.StreamReader(stream))
        //        {
        //            var jsonString = reader.ReadToEnd();
        //            var countries = JsonConvert.DeserializeObject<ObservableCollection<Countries>>(jsonString);
        //            foreach (var item in countries)
        //            {
        //                CountriesList.Add(item);
        //            }
        //        }

        //        var country_name = await TrackingService.GetUserCountryNameAsync();
        //        GetCountryBasedOnCurrentCountry(country_name);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    HideLoading();
        //}
        //private Countries GetCountryBasedOnCurrentCountry(string countryname)
        //{
        //    Countries countries = new Countries();

        //    if (!string.IsNullOrEmpty(countryname))
        //    {
        //        if (CountriesList != null && CountriesList.Count > 0)
        //        {
        //            countries = CountriesList.Where(x => x.name == countryname).FirstOrDefault();
        //            SelectedCountries = countries;
        //            CountriesTitle = SelectedCountries.dial_code;
        //        }
        //    }
        //    return countries;
        //}
        private NewLocationDataResponse GetSelectedCountryByCountryId(long counrryid)
        {
            NewLocationDataResponse locationDataResponse = new NewLocationDataResponse();

            if (counrryid > 0)
            {
                if (CountriesList != null && CountriesList.Count > 0)
                {
                    locationDataResponse = CountriesList.Where(x => x.id == counrryid).FirstOrDefault();

                }
            }
            return locationDataResponse;
        }

        public void SetStackVisibility(string classId)
        {
            ShowLoading();
            if (classId == "name")
            {
                IsVisileEditNameStack = true;
                IsVisibleEditPhoneStack = false;
                IsVisibleEditCityStack = false;
                TitleText = AppResources.Edit_Info;
            }
            else if (classId == "phone")
            {
                IsVisileEditNameStack = false;
                IsVisibleEditPhoneStack = true;
                IsVisibleEditCityStack = false;
                TitleText = AppResources.Edit_Info;
            }
            else
            {
                IsVisileEditNameStack = false;
                IsVisibleEditPhoneStack = false;
                IsVisibleEditCityStack = true;
                TitleText = AppResources.Edit_Location;
            }
            HideLoading();
        }

        private async Task UpdateUserNameCommandExecute()
        {
            if (CheckConnection())
            {
                if (ProfileDetails != null)
                {
                    if (string.IsNullOrEmpty(ProfileDetails?.name))
                    {
                        ShowToast(AppResources.Please_enter_user_full_name);
                        return;
                    }
                    else if (!Regex.Match(ProfileDetails?.name, @"^[a-zA-Z]+[\s|-]?[a-zA-Z]+[\s|-]?[a-zA-Z ]+$").Success)
                    {
                        ShowToast(AppResources.Please_enter_valid_user_name);
                        return;
                    }
                }
                ShowLoading();
                try
                {
                    var form = new MultipartFormDataContent();

                    form.Add(new ByteArrayContent(ProductImage), "photo", $"ProductImage_{CommonHelper.GenerateRandomId(5)}.jpg");
                    form.Add(new StringContent(ProfileDetails?.name.Trim()), "name");
                    form.Add(new StringContent(Convert.ToString(SettingsExtension.UserId)), "user_id");

                    var result = await TryWithErrorAsync(_userCoreService.UpdateUserName(form, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        await App.Locator.MyProfile.InitilizeData();
                        await App.Locator.ProfilePage.InitilizeData(ProfileDetails);
                        await _navigationService.NavigateBackAsync();
                        ShowToast(AppResources.Your_UserName_updated_successfully);
                    }
                    else
                    {
                        ShowAlert(AppResources.Error_in_updating_records);
                    }
                }
                catch (Exception ex)
                {
                    // ShowAlert(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);

        }

        //public async Task GetAllLocation()
        //{
        //    if (CheckConnection())
        //    {
        //        ShowLoading();
        //        try
        //        {
        //            locList = new List<LocationDataResponse>();
        //            var result = await TryWithErrorAsync(_authenticationService.GetAllLocations(), true, false);
        //            if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
        //            {
        //                foreach (var item in result?.Data?.data)
        //                {
        //                    locList.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                ShowAlert(result?.Data?.message);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowToast(CommonMessages.ServerError);
        //        }
        //        if (locList != null && locList.Count > 0)
        //        {
        //            AllLocationList = new ObservableCollection<LocationDataResponse>(locList);
        //        }
        //        HideLoading();
        //    }
        //    else
        //        ShowToast(CommonMessages.NoInternet);
        //}
    }
}

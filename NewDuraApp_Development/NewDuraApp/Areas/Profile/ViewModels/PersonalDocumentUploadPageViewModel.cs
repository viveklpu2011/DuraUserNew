using System;
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.ViewModels
{
    public class PersonalDocumentUploadPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand NavigateBackToProfileHome { get; set; }
        private IUserCoreService _userCoreService;
        public IAsyncCommand PickImageIDFrontCommand { get; set; }
        public IAsyncCommand PickImageIDBackCommand { get; set; }
        public IAsyncCommand PickImageDrivingFrontCommand { get; set; }
        public IAsyncCommand PickImageDrivingBackCommand { get; set; }

        public IAsyncCommand DeleteImageIDFrontCommand { get; set; }
        public IAsyncCommand DeleteImageIDBackCommand { get; set; }
        public IAsyncCommand DeleteImageDrivingFrontCommand { get; set; }
        public IAsyncCommand DeleteImageDrivingBackCommand { get; set; }

        #region Id Images
        private byte[] _productImageIDFront;
        public byte[] ProductImageIDFront
        {
            get { return _productImageIDFront; }
            set { _productImageIDFront = value; OnPropertyChanged(nameof(ProductImageIDFront)); }
        }
        private ImageSource _profileImageIDFront;
        public ImageSource ProfileImageIDFront
        {
            get { return _profileImageIDFront; }
            set { _profileImageIDFront = value; OnPropertyChanged(nameof(ProfileImageIDFront)); }
        }

        private byte[] _productImageIDBack;
        public byte[] ProductImageIDBack
        {
            get { return _productImageIDBack; }
            set { _productImageIDBack = value; OnPropertyChanged(nameof(ProductImageIDBack)); }
        }
        private ImageSource _profileImageIDBack;
        public ImageSource ProfileImageIDBack
        {
            get { return _profileImageIDBack; }
            set { _profileImageIDBack = value; OnPropertyChanged(nameof(ProfileImageIDBack)); }
        }

        #endregion

        #region Driving Images
        private byte[] _productImageDrivingFront;
        public byte[] ProductImageDrivingFront
        {
            get { return _productImageDrivingFront; }
            set { _productImageDrivingFront = value; OnPropertyChanged(nameof(ProductImageDrivingFront)); }
        }
        private ImageSource _profileImageDrivingFront;
        public ImageSource ProfileImageDrivingFront
        {
            get { return _profileImageDrivingFront; }
            set { _profileImageDrivingFront = value; OnPropertyChanged(nameof(ProfileImageDrivingFront)); }
        }

        private byte[] _productImageDrivingBack;
        public byte[] ProductImageDrivingBack
        {
            get { return _productImageDrivingBack; }
            set { _productImageDrivingBack = value; OnPropertyChanged(nameof(ProductImageDrivingBack)); }
        }
        private ImageSource _profileImageDrivingBack;
        public ImageSource ProfileImageDrivingBack
        {
            get { return _profileImageDrivingBack; }
            set { _profileImageDrivingBack = value; OnPropertyChanged(nameof(ProfileImageDrivingBack)); }
        }

        #endregion

        private bool _isIdFrontImageDeleteVisible = true;
        public bool IsIdFrontImageDeleteVisible
        {
            get { return _isIdFrontImageDeleteVisible; }
            set
            {
                _isIdFrontImageDeleteVisible = value;
                OnPropertyChanged(nameof(IsIdFrontImageDeleteVisible));
            }
        }
        private bool _isIdBackImageDeleteVisible = true;
        public bool IsIdBackImageDeleteVisible
        {
            get { return _isIdBackImageDeleteVisible; }
            set
            {
                _isIdBackImageDeleteVisible = value;
                OnPropertyChanged(nameof(IsIdBackImageDeleteVisible));
            }
        }

        private bool _isDrivingFrontImageDeleteVisible = true;
        public bool IsDrivingFrontImageDeleteVisible
        {
            get { return _isDrivingFrontImageDeleteVisible; }
            set
            {
                _isDrivingFrontImageDeleteVisible = value;
                OnPropertyChanged(nameof(IsDrivingFrontImageDeleteVisible));
            }
        }
        private bool _isDrivingBackImageDeleteVisible = true;
        public bool IsDrivingBackImageDeleteVisible
        {
            get { return _isDrivingBackImageDeleteVisible; }
            set
            {
                _isDrivingBackImageDeleteVisible = value;
                OnPropertyChanged(nameof(IsDrivingBackImageDeleteVisible));
            }
        }

        public PersonalDocumentUploadPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            PickImageIDFrontCommand = new AsyncCommand(PickImageIDFrontCommandExecute, allowsMultipleExecutions: false);
            PickImageIDBackCommand = new AsyncCommand(PickImageIDBackCommandExecute, allowsMultipleExecutions: false);
            PickImageDrivingFrontCommand = new AsyncCommand(PickImageDrivingFrontCommandExecute, allowsMultipleExecutions: false);
            PickImageDrivingBackCommand = new AsyncCommand(PickImageDrivingBackCommandExecute, allowsMultipleExecutions: false);

            DeleteImageIDFrontCommand = new AsyncCommand(DeleteImageIDFrontCommandExecute, allowsMultipleExecutions: false);
            DeleteImageIDBackCommand = new AsyncCommand(DeleteImageIDBackCommandExecute, allowsMultipleExecutions: false);
            DeleteImageDrivingFrontCommand = new AsyncCommand(DeleteImageDrivingFrontCommandExecute, allowsMultipleExecutions: false);
            DeleteImageDrivingBackCommand = new AsyncCommand(DeleteImageDrivingBackCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task DeleteImageDrivingBackCommandExecute()
        {
            await DeletePersonalDocument(2, "backimage");
        }

        private async Task DeleteImageDrivingFrontCommandExecute()
        {
            await DeletePersonalDocument(2, "frontimage");
        }

        private async Task DeleteImageIDBackCommandExecute()
        {
            await DeletePersonalDocument(1, "backimage");
        }

        private async Task DeleteImageIDFrontCommandExecute()
        {
            await DeletePersonalDocument(1, "frontimage");
        }

        private async Task PickImageDrivingBackCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Driving;
            documentTypeModel.documentType = DocumentType.Driving;
            documentTypeModel.propertyname = "backimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageDrivingFrontCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Driving;
            documentTypeModel.documentType = DocumentType.Driving;
            documentTypeModel.propertyname = "frontimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageIDBackCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.ID;
            documentTypeModel.documentType = DocumentType.ID;
            documentTypeModel.propertyname = "backimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageIDFrontCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.ID;
            documentTypeModel.documentType = DocumentType.ID;
            documentTypeModel.propertyname = "frontimage";
            await PickImage(documentTypeModel);
        }

        internal async Task InitilizeData()
        {
            await GetPersonalDocument();
        }

        private async Task PickImage(DocumentTypeModel documentTypeModel)
        {
            //var res = await ShowCameraActionSheet();
            var res = await ShowCameraPopup();
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

                    await SetImageAndUpload(documentTypeModel, file);
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
                        await SetImageAndUpload(documentTypeModel, file);
                    });

                }
            }

            //var result = await ShowConfirmationAlert("Please select below one", "Edit Photo", "Pick from gallery", "Pick from camera");
            //if (result) {

            //} else {

            //}
        }

        private async Task SetImageAndUpload(DocumentTypeModel documentTypeModel, MediaFile file)
        {
            if (documentTypeModel != null)
            {
                if (documentTypeModel.documentType == DocumentType.ID &&
                    documentTypeModel.documentIdType == DocumentIdType.ID &&
                    documentTypeModel.propertyname == "frontimage")
                {
                    if (file != null)
                    {
                        IsIdFrontImageDeleteVisible = false;
                        ProfileImageIDFront = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImageIDFront = ImageHelper.ReadToEnd(file.GetStream());

                        await AddMoreDetailsCommandExecute(ProductImageIDFront, "frontimage", DocumentIdType.ID);
                    }
                    else
                    {
                        IsIdFrontImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.ID &&
                    documentTypeModel.documentIdType == DocumentIdType.ID &&
                    documentTypeModel.propertyname == "backimage")
                {

                    if (file != null)
                    {
                        IsIdBackImageDeleteVisible = false;
                        ProfileImageIDBack = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImageIDBack = ImageHelper.ReadToEnd(file.GetStream());

                        await AddMoreDetailsCommandExecute(ProductImageIDBack, "backimage", DocumentIdType.ID);
                    }
                    else
                    {
                        IsIdBackImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.Driving &&
                    documentTypeModel.documentIdType == DocumentIdType.Driving &&
                    documentTypeModel.propertyname == "frontimage")
                {


                    if (file != null)
                    {
                        IsDrivingFrontImageDeleteVisible = false;
                        ProfileImageDrivingFront = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImageDrivingFront = ImageHelper.ReadToEnd(file.GetStream());
                        await AddMoreDetailsCommandExecute(ProductImageDrivingFront, "frontimage", DocumentIdType.Driving);
                    }
                    else
                    {
                        IsDrivingFrontImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.Driving &&
                    documentTypeModel.documentIdType == DocumentIdType.Driving &&
                    documentTypeModel.propertyname == "backimage")
                {

                    if (file != null)
                    {
                        IsDrivingBackImageDeleteVisible = false;
                        ProfileImageDrivingBack = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            return stream;
                        });
                        ProductImageDrivingBack = ImageHelper.ReadToEnd(file.GetStream());
                        await AddMoreDetailsCommandExecute(ProductImageDrivingBack, "backimage", DocumentIdType.Driving);
                    }
                    else
                    {
                        IsDrivingBackImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }
                }
            }

        }

        private async Task AddMoreDetailsCommandExecute(byte[] data, string type, DocumentIdType documentid)
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    var form = new MultipartFormDataContent();
                    int id = (int)documentid;
                    form.Add(new ByteArrayContent(data), type, $"DocumentImage_{type}{CommonHelper.GenerateRandomId(5)}.jpg");
                    form.Add(new StringContent(Convert.ToString(id)), "document_id");
                    form.Add(new StringContent(Convert.ToString(SettingsExtension.UserId)), "user_id");


                    var result = await TryWithErrorAsync(_userCoreService.UploadDocument(form, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (id == 1)
                        {
                            ShowToast($"ID Card {type} uploaded");
                        }
                        else if (id == 2)
                        {
                            ShowToast($"Driving Id {type} uploaded");
                        }
                    }
                    //ShowAlert(result?.Data?.message, result?.Data?.message);
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

        private async Task GetPersonalDocument()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel()
                    {
                        user_id = SettingsExtension.UserId
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetPersonalDocument(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.dataidcard != null)
                        {
                            var item = result?.Data?.dataidcard;
                            if (!string.IsNullOrEmpty(item?.frontimage))
                            {
                                ShowLoading();
                                var checkImageData = await ImageHelper.GetImageFromUrl(item?.frontimage);
                                if (checkImageData == null)
                                {
                                    IsIdFrontImageDeleteVisible = true;
                                }
                                else
                                {
                                    IsIdFrontImageDeleteVisible = false;
                                    ProductImageIDFront = checkImageData;
                                    ProfileImageIDFront = item?.frontimage;
                                }
                                HideLoading();
                            }
                            else
                            {
                                IsIdFrontImageDeleteVisible = true;
                            }

                            if (!string.IsNullOrEmpty(item?.backimage))
                            {
                                ShowLoading();
                                var checkImageData = await ImageHelper.GetImageFromUrl(item?.backimage);
                                if (checkImageData == null)
                                {
                                    IsIdBackImageDeleteVisible = true;
                                }
                                else
                                {
                                    IsIdBackImageDeleteVisible = false;
                                    ProductImageIDBack = checkImageData;
                                    ProfileImageIDBack = item?.backimage;
                                }
                                HideLoading();
                            }
                            else
                            {
                                IsIdBackImageDeleteVisible = true;
                            }
                        }
                        else
                        {
                            IsIdFrontImageDeleteVisible = true;
                            IsIdBackImageDeleteVisible = true;
                        }
                        if (result?.Data?.datadriver != null)
                        {
                            var item = result?.Data?.datadriver;
                            if (!string.IsNullOrEmpty(item?.frontimage))
                            {
                                ShowLoading();
                                var checkImageData = await ImageHelper.GetImageFromUrl(item?.frontimage);
                                if (checkImageData == null)
                                {
                                    IsDrivingFrontImageDeleteVisible = true;
                                }
                                else
                                {
                                    IsDrivingFrontImageDeleteVisible = false;
                                    ProductImageDrivingFront = checkImageData;
                                    ProfileImageDrivingFront = item?.frontimage;
                                }
                                HideLoading();
                            }
                            else
                            {
                                IsDrivingFrontImageDeleteVisible = true;
                            }

                            if (!string.IsNullOrEmpty(item?.backimage))
                            {
                                ShowLoading();
                                var checkImageData = await ImageHelper.GetImageFromUrl(item?.backimage);
                                if (checkImageData == null)
                                {
                                    IsDrivingBackImageDeleteVisible = true;
                                }
                                else
                                {
                                    IsDrivingBackImageDeleteVisible = false;
                                    ProductImageDrivingBack = checkImageData;
                                    ProfileImageDrivingBack = item?.backimage;
                                }
                                HideLoading();

                            }
                            else
                            {
                                IsDrivingBackImageDeleteVisible = true;
                            }
                        }
                        else
                        {
                            IsDrivingFrontImageDeleteVisible = true;
                            IsDrivingBackImageDeleteVisible = true;
                        }
                        //if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        //{
                        //    foreach (var item in result?.Data?.data)
                        //    {
                        //        if (item?.documentname == "id card")
                        //        {
                        //            if (!string.IsNullOrEmpty(item?.frontimage))
                        //            {
                        //                ShowLoading();
                        //                var checkImageData = await ImageHelper.GetImageFromUrl(item?.frontimage);
                        //                if (checkImageData == null)
                        //                {
                        //                    IsIdFrontImageDeleteVisible = true;
                        //                }
                        //                else
                        //                {
                        //                    IsIdFrontImageDeleteVisible = false;
                        //                    ProductImageIDFront = checkImageData;
                        //                    ProfileImageIDFront = ImageSource.FromStream(() => new MemoryStream(ProductImageIDFront));
                        //                }
                        //                HideLoading();
                        //            }
                        //            else
                        //            {
                        //                IsIdFrontImageDeleteVisible = true;
                        //            }

                        //            if (!string.IsNullOrEmpty(item?.backimage))
                        //            {
                        //                ShowLoading();
                        //                var checkImageData = await ImageHelper.GetImageFromUrl(item?.backimage);
                        //                if (checkImageData == null)
                        //                {
                        //                    IsIdBackImageDeleteVisible = true;
                        //                }
                        //                else
                        //                {
                        //                    IsIdBackImageDeleteVisible = false;
                        //                    ProductImageIDBack = checkImageData;
                        //                    ProfileImageIDBack = ImageSource.FromStream(() => new MemoryStream(ProductImageIDBack));
                        //                }
                        //                HideLoading();
                        //            }
                        //            else
                        //            {
                        //                IsIdBackImageDeleteVisible = true;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            IsDrivingFrontImageDeleteVisible = true;
                        //            IsDrivingBackImageDeleteVisible = true;
                        //        }

                        //        if (item?.documentname == "driver's license")
                        //        {
                        //            if (!string.IsNullOrEmpty(item?.frontimage))
                        //            {
                        //                ShowLoading();
                        //                var checkImageData = await ImageHelper.GetImageFromUrl(item?.frontimage);
                        //                if (checkImageData == null)
                        //                {
                        //                    IsDrivingFrontImageDeleteVisible = true;
                        //                }
                        //                else
                        //                {
                        //                    IsDrivingFrontImageDeleteVisible = false;
                        //                    ProductImageDrivingFront = checkImageData;
                        //                    ProfileImageDrivingFront = ImageSource.FromStream(() => new MemoryStream(ProductImageDrivingFront));
                        //                }
                        //                HideLoading();
                        //            }
                        //            else
                        //            {
                        //                IsDrivingFrontImageDeleteVisible = true;
                        //            }

                        //            if (!string.IsNullOrEmpty(item?.backimage))
                        //            {
                        //                ShowLoading();
                        //                var checkImageData = await ImageHelper.GetImageFromUrl(item?.backimage);
                        //                if (checkImageData == null)
                        //                {
                        //                    IsDrivingBackImageDeleteVisible = true;
                        //                }
                        //                else
                        //                {
                        //                    IsDrivingBackImageDeleteVisible = false;
                        //                    ProductImageDrivingBack = checkImageData;
                        //                    ProfileImageDrivingBack = ImageSource.FromStream(() => new MemoryStream(ProductImageDrivingBack));
                        //                }
                        //                HideLoading();

                        //            }
                        //            else
                        //            {
                        //                IsDrivingBackImageDeleteVisible = true;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            IsIdFrontImageDeleteVisible = true;
                        //            IsIdBackImageDeleteVisible = true;
                        //        }
                        //    }
                        //}
                    }
                    //ShowAlert(result?.Data?.message, result?.Data?.message);
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

        private async Task DeletePersonalDocument(int id, string type)
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    DeleteDocumentRequestModel deleteDocumentRequestModel = new DeleteDocumentRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                        id = id,
                        type = type
                    };
                    var result = await TryWithErrorAsync(_userCoreService.DeleteDocument(deleteDocumentRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200 && result?.Data?.data == 1)
                    {
                        string msg = string.Empty;
                        if (id == 1 && type == "frontimage")
                        {
                            msg = AppResources.Id_card_front_image_removed;
                            IsIdFrontImageDeleteVisible = true;
                        }
                        else if (id == 1 && type == "backimage")
                        {
                            msg = AppResources.Id_card_back_image_removed;
                            IsIdBackImageDeleteVisible = true;
                        }
                        if (id == 2 && type == "frontimage")
                        {
                            msg = AppResources.Driving_license_front_image_removed;
                            IsIdFrontImageDeleteVisible = true;
                        }
                        else if (id == 2 && type == "backimage")
                        {
                            msg = AppResources.Driving_license_back_image_removed;
                            IsIdBackImageDeleteVisible = true;
                        }
                        ShowToast(msg);
                        await GetPersonalDocument();
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
    }
}

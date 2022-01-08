using System;
using System.IO;
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
    public class BusinessDocumentUploadPageViewModel : AppBaseViewModel
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

        public BusinessDocumentUploadPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
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
            await DeletePersonalDocument(4, "backimage");
        }

        private async Task DeleteImageDrivingFrontCommandExecute()
        {
            await DeletePersonalDocument(4, "frontimage");
        }

        private async Task DeleteImageIDBackCommandExecute()
        {
            await DeletePersonalDocument(3, "backimage");
        }

        private async Task DeleteImageIDFrontCommandExecute()
        {
            await DeletePersonalDocument(3, "frontimage");
        }

        private async Task PickImageDrivingBackCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Business2;
            documentTypeModel.documentType = DocumentType.Business2;
            documentTypeModel.propertyname = "backimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageDrivingFrontCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Business2;
            documentTypeModel.documentType = DocumentType.Business2;
            documentTypeModel.propertyname = "frontimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageIDBackCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Business1;
            documentTypeModel.documentType = DocumentType.Business1;
            documentTypeModel.propertyname = "backimage";
            await PickImage(documentTypeModel);
        }

        private async Task PickImageIDFrontCommandExecute()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.documentIdType = DocumentIdType.Business1;
            documentTypeModel.documentType = DocumentType.Business1;
            documentTypeModel.propertyname = "frontimage";
            await PickImage(documentTypeModel);
        }

        internal async Task InitilizeData()
        {
            await GetBusinessDocument();
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

            //	var result = await ShowConfirmationAlert("Please select below one", "Edit Photo", "Pick from gallery", "Pick from camera");
            //if (result) {

            //} else {

            //}
        }

        private async Task SetImageAndUpload(DocumentTypeModel documentTypeModel, MediaFile file)
        {
            if (documentTypeModel != null)
            {
                if (documentTypeModel.documentType == DocumentType.Business1 &&
                    documentTypeModel.documentIdType == DocumentIdType.Business1 &&
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

                        await AddMoreDetailsCommandExecute(ProductImageIDFront, "frontimage", DocumentIdType.Business1);
                    }
                    else
                    {
                        IsIdFrontImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.Business1 &&
                    documentTypeModel.documentIdType == DocumentIdType.Business1 &&
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

                        await AddMoreDetailsCommandExecute(ProductImageIDBack, "backimage", DocumentIdType.Business1);
                    }
                    else
                    {
                        IsIdBackImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.Business2 &&
                    documentTypeModel.documentIdType == DocumentIdType.Business2 &&
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
                        await AddMoreDetailsCommandExecute(ProductImageDrivingFront, "frontimage", DocumentIdType.Business2);
                    }
                    else
                    {
                        IsDrivingFrontImageDeleteVisible = true;
                        ShowToast(AppResources.error_on_uploading_image_try_again);
                    }

                }
                else if (documentTypeModel.documentType == DocumentType.Business2 &&
                    documentTypeModel.documentIdType == DocumentIdType.Business2 &&
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
                        await AddMoreDetailsCommandExecute(ProductImageDrivingBack, "backimage", DocumentIdType.Business2);
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
                        if (id == 3)
                        {
                            ShowToast($"Document 1 {type} uploaded");
                        }
                        else if (id == 4)
                        {
                            ShowToast($"Document 2 {type} uploaded");
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

        private async Task GetBusinessDocument()
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
                    var result = await TryWithErrorAsync(_userCoreService.GetBusinessDocument(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.dataone != null)
                        {
                            var item = result?.Data?.dataone;
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
                                    ProfileImageIDFront = ImageSource.FromStream(() => new MemoryStream(ProductImageIDFront));
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
                                    ProfileImageIDBack = ImageSource.FromStream(() => new MemoryStream(ProductImageIDBack));
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
                        if (result?.Data?.datatwo != null)
                        {
                            var item = result?.Data?.datatwo;
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
                                    ProfileImageDrivingFront = ImageSource.FromStream(() => new MemoryStream(ProductImageDrivingFront));
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
                                    ProfileImageDrivingBack = ImageSource.FromStream(() => new MemoryStream(ProductImageDrivingBack));
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
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        string msg = string.Empty;
                        if (id == 3 && type == "frontimage")
                        {
                            msg = AppResources.Document_1_front_image_removed;
                            IsIdFrontImageDeleteVisible = true;
                        }
                        else if (id == 3 && type == "backimage")
                        {
                            msg = AppResources.Document_1_back_image_removed;
                            IsIdBackImageDeleteVisible = true;
                        }
                        if (id == 4 && type == "frontimage")
                        {
                            msg = AppResources.Document_2_front_image_removed;
                            IsIdFrontImageDeleteVisible = true;
                        }
                        else if (id == 4 && type == "backimage")
                        {
                            msg = AppResources.Document_2_back_image_removed;
                            IsIdBackImageDeleteVisible = true;
                        }
                        //ShowToast(msg);
                        await GetBusinessDocument();
                    }

                }
                catch (Exception ex)
                {
                    //ShowAlert(CommonMessages.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

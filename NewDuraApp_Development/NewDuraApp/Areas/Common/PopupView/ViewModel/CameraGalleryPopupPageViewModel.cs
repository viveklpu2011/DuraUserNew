using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Common.PopupView.ViewModel
{
    public class CameraGalleryPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public Tuple<ProfilePicSelectionType, string> ReturnValue;
        private ObservableCollection<UploadImageTypeModel> _uploadImageTypeList;
        public ObservableCollection<UploadImageTypeModel> UploadImageTypeList
        {
            get { return _uploadImageTypeList; }
            set
            {
                _uploadImageTypeList = value;
                OnPropertyChanged(nameof(UploadImageTypeList));
            }
        }

        public IAsyncCommand<UploadImageTypeModel> GetSelectedOptionCommand { get; set; }
        public IAsyncCommand CancelCommand { get; set; }
        public CameraGalleryPopupPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GetSelectedOptionCommand = new AsyncCommand<UploadImageTypeModel>(GetSelectedOptionCommandExecute);
            CancelCommand = new AsyncCommand(CancelCommandExecute);
        }

        private async Task CancelCommandExecute()
        {
            await _navigationService.ClosePopupsAsync();

        }
        private async Task ClosePopUp(ProfilePicSelectionType outputType, string inputValue)
        {
            ReturnValue = new Tuple<ProfilePicSelectionType, string>(outputType, inputValue);
            await _navigationService.ClosePopupsAsync();
        }

        private async Task GetSelectedOptionCommandExecute(UploadImageTypeModel arg)
        {
            if (arg != null)
            {
                if (arg.UploadType == ProfilePicSelectionType.Camera.ToString())
                {
                    await ClosePopUp(ProfilePicSelectionType.Camera, "");
                }
                else
                {
                    await ClosePopUp(ProfilePicSelectionType.Gallery, "");
                }
            }


        }

        public async Task InitilizeData()
        {
            UploadImageTypeList = new ObservableCollection<UploadImageTypeModel>()
            {
                new UploadImageTypeModel()
                {
                    Id=1,
                    ImageName="cameraupload.png",
                    UploadType=ProfilePicSelectionType.Camera.ToString()
                },
                new UploadImageTypeModel()
                {
                    Id=1,
                    ImageName="galleryupload.png",
                    UploadType=ProfilePicSelectionType.Gallery.ToString()
                }
            };
        }
    }
}

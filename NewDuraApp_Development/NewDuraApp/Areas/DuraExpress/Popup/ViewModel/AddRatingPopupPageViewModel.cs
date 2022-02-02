using System;
using System.IO;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace NewDuraApp.Areas.DuraExpress.Popup.ViewModel
{
    public class AddRatingPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand RateCommand { get; set; }
        public IAsyncCommand CloseCommand { get; set; }
        private GetOrderDetailsModel _getOrderData;
        public GetOrderDetailsModel GetOrderData
        {
            get { return _getOrderData; }
            set
            {
                _getOrderData = value;
                OnPropertyChanged(nameof(GetOrderData));
            }
        }

        private bool _isVisibleRating;
        public bool IsVisibleRatingError
        {
            get { return _isVisibleRating; }
            set
            {
                _isVisibleRating = value;
                OnPropertyChanged(nameof(IsVisibleRatingError));
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

        private int _selectedRating;
        public int SelectedRating
        {
            get { return _selectedRating; }
            set
            {
                _selectedRating = value;
                if (_selectedRating > 0)
                {
                    IsVisibleRatingError = false;
                }
                OnPropertyChanged(nameof(SelectedRating));
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }

        public AddRatingPopupPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            RateCommand = new AsyncCommand(RateCommandExecute, allowsMultipleExecutions: false);
            CloseCommand = new AsyncCommand(CloseCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task CloseCommandExecute()
        {
            await _navigationService.ClosePopupsAsync();
        }

        private async Task RateCommandExecute()
        {
            if (SelectedRating <= 0)
            {
                IsVisibleRatingError = true;
                return;
            }
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    SubmitRatingRequestModel submitRatingRequest = new SubmitRatingRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                        driver_id = GetOrderData.driver_id,
                        order_id = GetOrderData?.order_no,
                        rating = SelectedRating,
                        remarks = Remarks,
                        service_type = "Dura Drive"
                    };
                    var result = await TryWithErrorAsync(_userCoreService.SubmitRating(submitRatingRequest, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        await _navigationService.ClosePopupsAsync();
                        ShowToast(AppResources.Thanks_for_provide_the_rating);
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert(AppResources.ServerError);
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        internal async Task InitilizeData(GetOrderDetailsModel getDriverDetailsModel = null)
        {
            if (getDriverDetailsModel != null)
            {
                GetOrderData = getDriverDetailsModel;
                ProductImage = string.IsNullOrEmpty(GetOrderData?.driverphoto) ? await ImageHelper.GetStreamFormResource("user_pic_placeholder.png") : await ImageHelper.GetImageFromUrl(GetOrderData?.driverphoto);
                ProfileImage = ImageSource.FromStream(() => new MemoryStream(ProductImage));
            }
        }
    }
}

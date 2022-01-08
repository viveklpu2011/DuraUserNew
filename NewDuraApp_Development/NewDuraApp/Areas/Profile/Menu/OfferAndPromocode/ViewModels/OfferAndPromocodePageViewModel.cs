using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels
{
    public class OfferAndPromocodePageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IAuthenticationService _authService;
        private IUserCoreService _userCoreService;
        private ObservableCollection<PromoCodeListModel> _promoCodeList;
        public IAsyncCommand<PromoCodeListModel> PromoCodeDetails { get; set; }
        public IAsyncCommand ApplyPromoCodeCommand { get; set; }
        private bool _isVisibleEntryFrame;
        private string _promoCode;
        public string PromoCode
        {
            get { return _promoCode; }
            set { _promoCode = value; OnPropertyChanged(nameof(PromoCode)); }
        }

        private string _offersText;
        public string OffersText
        {
            get { return _offersText; }
            set
            {
                _offersText = value;
                OnPropertyChanged(nameof(OffersText));
            }
        }

        public bool IsVisibleEntryFrame
        {
            get { return _isVisibleEntryFrame; }
            set { _isVisibleEntryFrame = value; OnPropertyChanged(nameof(IsVisibleEntryFrame)); }
        }
        public ObservableCollection<PromoCodeListModel> PromoCodeList
        {
            get { return _promoCodeList; }
            set { _promoCodeList = value; OnPropertyChanged(nameof(PromoCodeList)); }
        }
        public OfferAndPromocodePageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            PromoCodeDetails = new AsyncCommand<PromoCodeListModel>(ProCodeDetailsCommand);
            ApplyPromoCodeCommand = new AsyncCommand(ApplyPromoCodeCommandExecute);
        }

        private async Task ApplyPromoCodeCommandExecute()
        {
            if (string.IsNullOrEmpty(PromoCode))
            {
                ShowToast(AppResources.Please_enter_promo_code_to_get_offer);
                return;
            }
            if (PromoCodeList == null || PromoCodeList.Count <= 0)
            {
                ShowToast(AppResources.No_promo_code_offer_available_for_you);
                return;
            }
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    var promodetails = PromoCodeList.Where(x => x.name == PromoCode && x.is_used == false).FirstOrDefault();
                    if (promodetails != null)
                    {
                        VerifyPromoCodeData verifyPromoCodeData = new VerifyPromoCodeData
                        {
                            application = promodetails?.application,
                            area = promodetails?.area,
                            created_at = promodetails?.created_at,
                            discount = promodetails?.discount,
                            id = promodetails.id,
                            limitperuser = promodetails?.limitperuser,
                            maxdiscount = promodetails?.maxdiscount,
                            merchant = promodetails?.merchant,
                            name = promodetails?.name,
                            promocodelimit = promodetails?.promocodelimit,
                            promotype = promodetails?.promotype,
                            valid_date = promodetails?.valid_date
                        };
                        App.Locator.AddMoreDetails.VerifyPromoCode = verifyPromoCodeData;
                        await App.Locator.AddMoreDetails.InitilizeData();
                        await _navigationService.NavigateBackAsync();
                    }
                    else
                    {
                        ShowToast("Wrong/used promo code");
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

        private async Task ProCodeDetailsCommand(PromoCodeListModel arg)
        {
            if (arg != null)
            {
                if (IsVisibleEntryFrame)
                {
                    VerifyPromoCodeData verifyPromoCodeData = new VerifyPromoCodeData
                    {
                        application = arg?.application,
                        area = arg?.area,
                        created_at = arg?.created_at,
                        discount = arg?.discount,
                        id = arg.id,
                        limitperuser = arg?.limitperuser,
                        maxdiscount = arg?.maxdiscount,
                        merchant = arg?.merchant,
                        name = arg?.name,
                        promocodelimit = arg?.promocodelimit,
                        promotype = arg?.promotype,
                        valid_date = arg?.valid_date
                    };
                    App.Locator.AddMoreDetails.VerifyPromoCode = verifyPromoCodeData;
                    await App.Locator.AddMoreDetails.InitilizeData();
                    await _navigationService.NavigateBackAsync();
                }

                if (App.Locator.CurrentUser.SendWay == SendInvite.PROMO.ToString())
                {
                    if (!arg.is_used)
                    {
                        VerifyPromoCodeData verifyPromoCodeData = new VerifyPromoCodeData
                        {
                            application = arg?.application,
                            area = arg?.area,
                            created_at = arg?.created_at,
                            discount = arg?.discount,
                            id = arg.id,
                            limitperuser = arg?.limitperuser,
                            maxdiscount = arg?.maxdiscount,
                            merchant = arg?.merchant,
                            name = arg?.name,
                            promocodelimit = arg?.promocodelimit,
                            promotype = arg?.promotype,
                            valid_date = arg?.valid_date
                        };
                        App.Locator.CurrentUser.SendWay = "";
                        App.Locator.AddMoreDetails.VerifyPromoCode = verifyPromoCodeData;
                        ShowToast(AppResources.This_promo_will_be_applied_for_your_next_order);
                        await _navigationService.NavigateBackAsync();
                    }
                }
            }

        }

        public async Task InitilizeData()
        {
            if (App.IsVisiblePromoEntry)
            {
                IsVisibleEntryFrame = true;
            }
            else
            {
                IsVisibleEntryFrame = false;
            }
            await GetAllPromocode();
        }
        private async Task GetAllPromocode()
        {

            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    CommonUserIdRequestModel commonUserIdRequest = new CommonUserIdRequestModel
                    {
                        user_id = SettingsExtension.UserId
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetPromoCodeList(commonUserIdRequest, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
                    {
                        PromoCodeList = new ObservableCollection<PromoCodeListModel>();
                        if (result?.Data?.data.Count > 0)
                        {
                            OffersText = AppResources.Available_offers;
                            foreach (var item in result?.Data?.data)
                            {
                                PromoCodeList.Add(item);
                            }
                        }
                        else
                        {
                            OffersText = AppResources.No_offers_available;
                        }

                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                    //ShowAlert(result.Data.message);
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
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels
{
    public class TopupAmuntPopupPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand<WalletAmountModel> AmountDetails { get; set; }
        public IAsyncCommand ClosePopup { get; set; }
        public IAsyncCommand TopupWalletCommand { get; set; }

        private bool _isEnabledButton;
        public bool IsEnabledButton
        {
            get { return _isEnabledButton; }
            set { _isEnabledButton = value; OnPropertyChanged(nameof(IsEnabledButton)); }
        }

        private WalletAmountModel _walletAmount;
        public WalletAmountModel WalletAmount
        {
            get { return _walletAmount; }
            set { _walletAmount = value; OnPropertyChanged(nameof(WalletAmount)); }
        }

        private ObservableCollection<WalletAmountModel> _topUpAmountModelList;
        public ObservableCollection<WalletAmountModel> TopUpAmountModelList
        {
            get { return _topUpAmountModelList; }
            set { _topUpAmountModelList = value; OnPropertyChanged(nameof(TopUpAmountModelList)); }
        }

        private ObservableCollection<WalletAmountModel> _selectedTopUpAmountModelList;
        public ObservableCollection<WalletAmountModel> SelectedTopUpAmountModelList
        {
            get { return _selectedTopUpAmountModelList; }
            set { _selectedTopUpAmountModelList = value; OnPropertyChanged(nameof(SelectedTopUpAmountModelList)); }
        }

        public TopupAmuntPopupPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            AmountDetails = new AsyncCommand<WalletAmountModel>(AmountDetailsPage);
            ClosePopup = new AsyncCommand(ClosePopupPage);
            TopupWalletCommand = new AsyncCommand(TopupWalletCommandPage);
            InitilizeData();
        }

        private async Task TopupWalletCommandPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddMoneyPageViewModel))
            {
                await App.Locator.AddMoneyPage.InitilizeData();
                await _navigationService.NavigateToAsync<AddMoneyPageViewModel>();
                await _navigationService.ClosePopupsAsync();
            }
        }

        private async Task ClosePopupPage()
        {
            await _navigationService.ClosePopupsAsync();
        }

        private async Task AmountDetailsPage(WalletAmountModel arg)
        {
            if (SelectedTopUpAmountModelList != null && SelectedTopUpAmountModelList.Count > 0)
            {
                #region commented code
                //var serialisedData = arg;
                //var index = TopUpAmountModelList.IndexOf(TopUpAmountModelList.Where(x => x.id == serialisedData.id).FirstOrDefault());

                //var background = "";
                //var border = "";
                //if (serialisedData.Backgrondcolor == "#ffffff")
                //{
                //    background = "#DDDAFF";
                //    border = "#211E66";
                //}
                //else
                //{
                //    background = "#ffffff";
                //    border = "#828282";
                //}
                //TopUpAmountModelList[index] = new WalletAmountModel
                //{
                //    amount = serialisedData.amount,
                //    Backgrondcolor = background,
                //    Bordercolor = border,
                //    created_at = serialisedData.created_at,
                //    id = serialisedData.id,
                //    status = serialisedData.status,
                //    updated_at = serialisedData.updated_at
                //};

                //TopUpAmountModelList = new ObservableCollection<WalletAmountModel>();
                #endregion
                var list = new List<WalletAmountModel>();
                foreach (var item in SelectedTopUpAmountModelList)
                {
                    if (item.id == arg.id)
                    {
                        item.Backgrondcolor = "#DDDAFF";
                        item.Bordercolor = "#211E66";
                    }
                    else
                    {
                        item.Backgrondcolor = "#ffffff";
                        item.Bordercolor = "#828282";
                    }
                    list.Add(item);
                }
                TopUpAmountModelList = new ObservableCollection<WalletAmountModel>(list);
                IsEnabledButton = true;
                WalletAmount = new WalletAmountModel();
                WalletAmount = arg;
                App.Locator.AddMoneyPage.WalletAmount = arg;
            }
        }

        public async Task InitilizeData()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    var result = await TryWithErrorAsync(_userCoreService.GetWalletAmountList(SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.data != null)
                    {
                        TopUpAmountModelList = new ObservableCollection<WalletAmountModel>();
                        var walletAmountModel = new WalletAmountModel();
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            foreach (var item in result?.Data?.data)
                            {
                                walletAmountModel = new WalletAmountModel();
                                walletAmountModel.amount = item.amount;
                                walletAmountModel.Backgrondcolor = "#ffffff";
                                walletAmountModel.Bordercolor = "#828282";
                                walletAmountModel.created_at = item.created_at;
                                walletAmountModel.id = item.id;
                                walletAmountModel.status = item.status;
                                walletAmountModel.updated_at = item.updated_at;
                                TopUpAmountModelList.Add(walletAmountModel);
                            }
                            SelectedTopUpAmountModelList = TopUpAmountModelList;
                        }
                    }
                    else
                    {
                        ShowAlert(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }
    }
}

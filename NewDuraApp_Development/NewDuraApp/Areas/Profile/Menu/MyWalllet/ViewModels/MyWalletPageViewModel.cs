using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels
{
	public class MyWalletPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private ObservableCollection<Amount> _myWalletList;
		public IAsyncCommand<ProfileMenuModel> MenuDetails { get; set; }
		public IAsyncCommand NavigateToSelectAmount { get; set; }
		private IUserCoreService _userCoreService;
		public ObservableCollection<Amount> MyWalletList {
			get { return _myWalletList; }
			set { _myWalletList = value; OnPropertyChanged(nameof(MyWalletList)); }
		}
		private GetSaveWalletAmountResponseModel _getSaveWalletAmount;
		public GetSaveWalletAmountResponseModel GetSaveWalletAmount {
			get { return _getSaveWalletAmount; }
			set { _getSaveWalletAmount = value; OnPropertyChanged(nameof(GetSaveWalletAmount)); }
		}
		public MyWalletPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			NavigateToSelectAmount = new AsyncCommand(NavigateToSelectAmountPage);
		}

		private async Task NavigateToSelectAmountPage()
		{
			// await App.Locator.TopupAmuntPopupPage.InitilizeData();
			App.Locator.CurrentUser.SendWay = SendInvite.MENU.ToString();
			TopupAmuntPopupPageViewModel topupAmuntPopupPageViewModel = new TopupAmuntPopupPageViewModel(_navigationService, _userCoreService);
			await _navigationService.NavigateToPopupAsync<TopupAmuntPopupPageViewModel>(true, topupAmuntPopupPageViewModel);
		}

		public async Task InitilizeData()
		{

			await GetWalletAmount();
			await GetWalletTransactionList();
		}

		private async Task GetWalletAmount()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					GetSaveWalletAmountRequestModel getSaveWalletAmountRequestModel = new GetSaveWalletAmountRequestModel {
						user_id = SettingsExtension.UserId,
						amount = 0
					};
					var result = await TryWithErrorAsync(_userCoreService.SaveWalletAmount(getSaveWalletAmountRequestModel, SettingsExtension.Token), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						GetSaveWalletAmount = result?.Data;
					} else {
						ShowAlert(result?.Data?.message);
					}
					//ShowAlert(result.Data.message);
				} catch (Exception ex) {
					//ShowToast(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);

		}
		private async Task GetWalletTransactionList()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel {
						user_id = SettingsExtension.UserId,
					};
					var result = await TryWithErrorAsync(_userCoreService.GetWallet(commonUserIdRequestModel, SettingsExtension.Token), true, false);
					if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200) {
						if (result?.Data?.data != null && result?.Data?.data.Count > 0) {

							var transactionList = new List<Amount>();
							foreach (var item in result?.Data?.data) {
								transactionList.Add(item);
							}
							//var sortedList = transactionList.OrderByDescending(x => x.TransactionDate).ToList();
							MyWalletList = new ObservableCollection<Amount>(transactionList);
						}
					} else {
						ShowAlert(result?.Data?.message);
					}
					//ShowAlert(result.Data.message);
				} catch (Exception ex) {
					//ShowToast(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);

		}
	}
}

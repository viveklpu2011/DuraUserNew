using System;
using System.Collections.ObjectModel;
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

namespace NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels
{
	public class MyAddressPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private IUserCoreService _userCoreService;

		private GetAddressModel _addressData;
		public GetAddressModel AddressData {
			get { return _addressData; }
			set {
				_addressData = value;
				OnPropertyChanged(nameof(AddressData));
			}
		}
		public IAsyncCommand<GetAddressModel> AddressDetails { get; set; }
		public IAsyncCommand<GetAddressModel> DeleteAddressDetails { get; set; }
		public IAsyncCommand AddNewAddress { get; set; }
		private ObservableCollection<GetAddressModel> _addressList;
		public ObservableCollection<GetAddressModel> AddressList {
			get { return _addressList; }
			set { _addressList = value; OnPropertyChanged(nameof(AddressList)); }
		}
		public MyAddressPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			AddressDetails = new AsyncCommand<GetAddressModel>(AddressDetailsCommand);
			DeleteAddressDetails = new AsyncCommand<GetAddressModel>(DeleteAddressDetailsCommand);
			AddNewAddress = new AsyncCommand(AddNewAddressCommand);
		}

		private async Task DeleteAddressDetailsCommand(GetAddressModel arg)
		{
			if (arg != null) {
				if (CheckConnection()) {
					ShowLoading();
					try {
						DeleteAddressRequestModel deleteAddressRequestModel = new DeleteAddressRequestModel() {
							user_id = SettingsExtension.UserId,
							addressid = arg.id
						};
						var result = await TryWithErrorAsync(_userCoreService.DeleteAddress(deleteAddressRequestModel, SettingsExtension.Token), true, false);
						if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
							AddressList.RemoveAt(AddressList.IndexOf(arg));
							ShowToast(AppResources.Address_removed);
							await App.Locator.HomePage.GetAddressList();
						} else {
							ShowToast(AppResources.Error_on_removing_address);
						}
						//ShowAlert(result?.Data?.message, result?.Data?.message);
					} catch (Exception ex) {
						// ShowAlert(CommonMessages.ServerError);
					}
					HideLoading();
				} else
					ShowToast(AppResources.NoInternet);


			}
		}

		private async Task AddNewAddressCommand()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(AddNewAddressPageViewModel)) {
				await _navigationService.NavigateToAsync<AddNewAddressPageViewModel>();
				await App.Locator.AddNewAddressPage.InitilizeData();
			}
		}

		private async Task AddressDetailsCommand(GetAddressModel arg)
		{
			if (arg != null) {
				AddressData = arg;
				if (_navigationService.GetCurrentPageViewModel() != typeof(AddNewAddressPageViewModel)) {
					await _navigationService.NavigateToAsync<AddNewAddressPageViewModel>();
					await App.Locator.AddNewAddressPage.InitilizeData(AddressData);
				}
			}
		}

		public async Task InitilizeData()
		{
			AddressList = App.Locator.HomePage.AddressList;
		}

		private async Task GetAddressList()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel() {
						user_id = SettingsExtension.UserId,
					};
					var result = await TryWithErrorAsync(_userCoreService.GetAddress(commonUserIdRequestModel, SettingsExtension.Token), true, false);
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						if (result?.Data?.data != null && result?.Data?.data.Count > 0) {
							AddressList = new ObservableCollection<GetAddressModel>();
							foreach (var item in result?.Data?.data) {
								AddressList.Add(item);
							}
						}
					}
					//ShowAlert(result?.Data?.message, result?.Data?.message);
				} catch (Exception ex) {
					// ShowAlert(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(AppResources.NoInternet);


		}
	}
}

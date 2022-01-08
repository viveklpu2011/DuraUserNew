using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.Common.PopupView.View;
using NewDuraApp.Areas.Common.PopupView.ViewModel;
using NewDuraApp.Areas.Common.Views;
using NewDuraApp.Areas.DuraEats.DuraEatsChatHelp.ViewModel;
using NewDuraApp.Areas.Profile.Menu.MyAddress.ViewModels;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.ViewModels;
using NewDuraApp.Areas.Profile.Menu.OfferAndPromocode.ViewModels;
using NewDuraApp.Areas.Profile.Menu.ReferFriend.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Reviews.ViewModels;
using NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels;
using NewDuraApp.Areas.Profile.Menu.Settings.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.ViewModels
{

	public class MyProfileViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private IUserCoreService _userCoreService;
		private ObservableCollection<ProfileMenuModel> _profileMenuModelList;
		public IAsyncCommand<ProfileMenuModel> MenuDetails { get; set; }
		public IAsyncCommand NavigateToViewProfile { get; set; }
		public IAsyncCommand LogoutCommand { get; set; }
		public IAsyncCommand AvailableBalanceCommand { get; set; }
		public IAsyncCommand ReviewCommand { get; set; }
		public ObservableCollection<ProfileMenuModel> ProfileMenuModelList {
			get { return _profileMenuModelList; }
			set { _profileMenuModelList = value; OnPropertyChanged(nameof(ProfileMenuModelList)); }
		}
		private bool _isRefreshing;
		public bool IsRefreshing {
			get { return _isRefreshing; }
			set {
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}
		public IAsyncCommand RefreshCommand { get; set; }
		private byte[] _productImage;
		public byte[] ProductImage {
			get { return _productImage; }
			set { _productImage = value; OnPropertyChanged(nameof(ProductImage)); }
		}
		private ImageSource _profileImage;
		public ImageSource ProfileImage {
			get { return _profileImage; }
			set { _profileImage = value; OnPropertyChanged(nameof(ProfileImage)); }
		}
		private GetProfileDetailsModel _profileDetails;
		public GetProfileDetailsModel ProfileDetails {
			get { return _profileDetails; }
			set {
				_profileDetails = value;
				OnPropertyChanged(nameof(ProfileDetails));
			}
		}
		private string _appVersion;
		public string AppVersion {
			get { return _appVersion; }
			set {
				_appVersion = value;
				OnPropertyChanged(nameof(AppVersion));
			}
		}
		public MyProfileViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			AppVersion = VersionTracking.CurrentVersion;
			MenuDetails = new AsyncCommand<ProfileMenuModel>(NavigateToMenuDetailsPage, allowsMultipleExecutions: false);
			NavigateToViewProfile = new AsyncCommand(NavigateToViewProfilePage, allowsMultipleExecutions: false);
			LogoutCommand = new AsyncCommand(LogoutCommandExecute, allowsMultipleExecutions: false);
			AvailableBalanceCommand = new AsyncCommand(AvailableBalanceCommandExecute, allowsMultipleExecutions: false);
			ReviewCommand = new AsyncCommand(ReviewCommandExecute, allowsMultipleExecutions: false);
			RefreshCommand = new AsyncCommand(RefreshCommandExecute, allowsMultipleExecutions: false);
		}
		public async Task RefreshCommandExecute()
		{
			IsRefreshing = true;
			await InitilizeData();
			IsRefreshing = false;
		}
		private async Task ReviewCommandExecute()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(MyReviewsPageViewModel)) {

				await _navigationService.NavigateToAsync<MyReviewsPageViewModel>();
				await App.Locator.MyReviewsPage.InitilizeData();
			}
		}

		private async Task AvailableBalanceCommandExecute()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(MyWalletPageViewModel)) {
				await _navigationService.NavigateToAsync<MyWalletPageViewModel>();
				//await App.Locator.MyWalletPage.InitilizeData();
			}
		}

		private async Task LogoutCommandExecute()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(LogoutPopupPageViewModel)) {
				await PopupNavigation.Instance.PushAsync(new LogoutPopupPage());
			}

			//var result = await ShowConfirmationAlert("Are you sure you want to logout?", "Question");
			//if (result)
			//{
			//    App.Current.MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
			//    await App.Locator.LoginPage.InitilizeData();

			//    App.Locator.PickupSchedule.IsVisibleLaterView = false;
			//    App.Locator.PickupSchedule.AsapIsChecked = true;
			//    App.Locator.PickupSchedule.LaterIsCheck = false;
			//    App.Locator.PickupSchedule.IsButtonEnabled = true;
			//    App.Locator.PickupSchedule.DuraAddressCommon = null;
			//    App.Locator.PickupSchedule.PickupScheduleRequest = null;
			//    App.Locator.PickupLocation.PickupScheduleRequest = null;
			//    App.Locator.WhereTo.PickupScheduleRequest = null;
			//    App.Locator.AddStopLocation.PickupScheduleRequest = null;
			//    App.Locator.DuraExpress.PickupLocationText = string.Empty;
			//    App.Locator.DuraExpress.PickupLocationTextVisible = false;
			//    SettingsExtension.PickupAddress = string.Empty;
			//    App.Locator.DuraExpress.PickupScheduleLocText = string.Empty;
			//    App.Locator.DuraExpress.PickupScheduleLocTextVisible = false;
			//    App.Locator.DuraExpress.PickupWhereToText = string.Empty;
			//    App.Locator.DuraExpress.PickupWhereToTextVisible = false;
			//    App.Locator.AddMoreDetails.VehicleListSelectedData = null;
			//    App.Locator.SelectVehicle.VehicleListSelectedData = null;
			//    SettingsExtension.IsLoggedIn = false;
			//    App.Locator.PickupLocation.Address1 = string.Empty;
			//    App.Locator.PickupLocation.Address2 = string.Empty;
			//    App.Locator.WhereTo.Address1 = string.Empty;
			//    App.Locator.WhereTo.Address2 = string.Empty;
			//    App.Locator.AddStopLocation.Address1 = string.Empty;
			//    App.Locator.AddStopLocation.Address2 = string.Empty;


			//}

		}

		private async Task NavigateToViewProfilePage()
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(ProfilePageViewModel)) {
				await App.Locator.ProfilePage.InitilizeData(ProfileDetails);
				await _navigationService.NavigateToAsync<ProfilePageViewModel>();
			}
		}

		private async Task NavigateToMenuDetailsPage(ProfileMenuModel arg)
		{
			if (arg != null) {
				if (arg.MenuId == 0) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(SavedCardsListPageViewModel)) {
						await App.Locator.SavedCardsListPage.InitilizeData();
						await _navigationService.NavigateToAsync<SavedCardsListPageViewModel>();
					}
				} else if (arg.MenuId == 1) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(MyAddressPageViewModel)) {
						await App.Locator.MyAddressPage.InitilizeData();
						await _navigationService.NavigateToAsync<MyAddressPageViewModel>();
					}
				} else if (arg.MenuId == 2) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(OfferAndPromocodePageViewModel)) {
						App.Locator.CurrentUser.SendWay = SendInvite.PROMO.ToString();
						await App.Locator.OfferAndPromocodePage.InitilizeData();
						await _navigationService.NavigateToAsync<OfferAndPromocodePageViewModel>();
					}
				} else if (arg.MenuId == 3) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(ReferAFriendPageViewModel)) {
						await App.Locator.ReferAFriendPage.InitilizeData();
						await _navigationService.NavigateToAsync<ReferAFriendPageViewModel>();
					}
				} else if (arg.MenuId == 4) {
					//ShowAlert(AppResources.We_are_Working_on_it_It_is_in_Progress, "DuraDrive", AppResources.Ok);
					await _navigationService.NavigateToAsync<ChatViewModel>();
				}
				  //        await _navigationService.NavigateToAsync<MyReviewsPageViewModel>();
				  //        await App.Locator.MyReviewsPage.InitilizeData();
				  //    }
				  //}

				  //else if (arg.MenuId == 6)
				  //{
				  //    await BeARiderMethod();
				  //}
				  else if (arg.MenuId == 5) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(DuraEatsHelpPageViewModel)) {
						await App.Locator.DuraEatsHelpPage.InitilizeData();
						await _navigationService.NavigateToAsync<DuraEatsHelpPageViewModel>();
					}
				} else if (arg.MenuId == 6) {
					if (_navigationService.GetCurrentPageViewModel() != typeof(SettingsPageViewModel)) {
						await App.Locator.SettingsPage.InitilizeData();
						await _navigationService.NavigateToAsync<SettingsPageViewModel>();
					}
				} else if (arg.MenuId == 10) {
					var result = await ShowConfirmationAlert(AppResources.Are_you_sure_you_want_to_logout, AppResources.Logout);
					if (result) {
						App.Current.MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
						await App.Locator.LoginPage.InitilizeData();

						App.Locator.PickupSchedule.IsVisibleLaterView = false;
						App.Locator.PickupSchedule.AsapIsChecked = true;
						App.Locator.PickupSchedule.LaterIsCheck = false;
						App.Locator.PickupSchedule.IsButtonEnabled = true;
						App.Locator.PickupSchedule.DuraAddressCommon = null;
						App.Locator.PickupSchedule.PickupScheduleRequest = null;
						App.Locator.PickupLocation.PickupScheduleRequest = null;
						App.Locator.WhereTo.PickupScheduleRequest = null;
						App.Locator.AddStopLocation.PickupScheduleRequest = null;
						App.Locator.DuraExpress.PickupLocationText = string.Empty;
						App.Locator.DuraExpress.PickupLocationTextVisible = false;
						SettingsExtension.PickupAddress = string.Empty;
						App.Locator.DuraExpress.PickupScheduleLocText = string.Empty;
						App.Locator.DuraExpress.PickupScheduleLocTextVisible = false;
						App.Locator.DuraExpress.PickupWhereToText = string.Empty;
						App.Locator.DuraExpress.PickupWhereToTextVisible = false;
						App.Locator.AddMoreDetails.VehicleListSelectedData = null;
						App.Locator.SelectVehicle.VehicleListSelectedData = null;
						SettingsExtension.IsLoggedIn = false;
						App.Locator.PickupLocation.Address1 = string.Empty;
						App.Locator.PickupLocation.Address2 = string.Empty;
						App.Locator.WhereTo.Address1 = string.Empty;
						App.Locator.WhereTo.Address2 = string.Empty;
						App.Locator.AddStopLocation.Address1 = string.Empty;
						App.Locator.AddStopLocation.Address2 = string.Empty;


					}

					// await _navigationService.NavigateToAsync<LoginPageViewModels>();


				}
			}
		}

		public async Task InitilizeData()
		{
			ProfileMenuModelList = new ObservableCollection<ProfileMenuModel>
			{
				new ProfileMenuModel
				{
				   MenuId=0,
				   ImageName=ImageHelper.GetImageNameFromResource("saved_cards.png"),
				  MenuName=AppResources.Saved_Cards,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=true
				},
				new ProfileMenuModel
				{
				   MenuId=1,
				   ImageName=ImageHelper.GetImageNameFromResource("saved_address.png"),
				   MenuName=AppResources.Saved_Addresses,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=true
				},
				new ProfileMenuModel
				{
				   MenuId=2,
				   ImageName=ImageHelper.GetImageNameFromResource("offers_promo.png"),
				   MenuName=AppResources.Offers_Promo,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=true
				},
				new ProfileMenuModel
				{
				   MenuId=3,
				   ImageName=ImageHelper.GetImageNameFromResource("rafer_a_friend.png"),
				   MenuName=AppResources.Refer_a_Friend,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=false
				},
                //new ProfileMenuModel
                //{
                //   MenuId=6,
                //   ImageName=ImageHelper.GetImageNameFromResource("be_a_rider.png"),
                //   MenuName=!string.IsNullOrEmpty(SettingsExtension.BeARiderString)?SettingsExtension.BeARiderString:"Be a rider",
                //   StackThickness=new Thickness(0,6,0,10),
                //   IsMenuEnabled=false
                //},
                new ProfileMenuModel
				{
				   MenuId=4,
				   ImageName=ImageHelper.GetImageNameFromResource("support_chat.png"),
				  MenuName=AppResources.Support_Chat,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=false
				},
				new ProfileMenuModel
				{
				   MenuId=5,
				   ImageName=ImageHelper.GetImageNameFromResource("help_center.png"),
				   MenuName=AppResources.Help_Center,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=false
				},
				new ProfileMenuModel
				{
				   MenuId=6,
				   ImageName=ImageHelper.GetImageNameFromResource("settings.png"),
					MenuName=AppResources.Settings,
				   StackThickness=new Thickness(0,6,0,10),
				   IsMenuEnabled=true
				},

			};
			await GetProfileDetails();
		}

		public async Task GetProfileDetails()
		{
			if (CheckConnection()) {
				try {
					CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel() {
						user_id = SettingsExtension.UserId
					};
					ShowLoading();
					var result = await TryWithErrorAsync(_userCoreService.GetProfileDetails(commonUserIdRequestModel, SettingsExtension.Token), true, false);
					HideLoading();
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						if (result?.Data?.data != null) {
							ProfileDetails = result?.Data?.data;
							App.Locator.ProfilePage.ProfileDetails = ProfileDetails;
							App.Locator.ProfilePage.GoogleLink = ProfileDetails.google;
							App.Locator.ProfilePage.FacebookLink = ProfileDetails.facebook;

						}
					} else if (result?.ResultType == ResultType.Unauthorized) {
						await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
					}
				} catch (Exception ex) {
				}
			} else
				ShowToast(AppResources.NoInternet);

		}
		private async Task BeARiderMethod()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel() {
						user_id = SettingsExtension.UserId
					};
					var result = await TryWithErrorAsync(_userCoreService.BeARider(commonUserIdRequestModel, SettingsExtension.Token), true, false);
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						var listItem = ProfileMenuModelList.Where(i => i.MenuId == 6).First();
						var index = ProfileMenuModelList.IndexOf(listItem);
						var newCustomListItem = new ProfileMenuModel {
							ImageName = ImageHelper.GetImageNameFromResource("be_a_rider.png"),
							MenuId = 6,
							MenuName = AppResources.You_are_now_rider_pending_approval,
							StackThickness = new Thickness(0, 6, 0, 10)
						};
						SettingsExtension.BeARiderString = AppResources.You_are_now_rider_pending_approval;
						if (index != -1)
							ProfileMenuModelList[index] = newCustomListItem;

						//ShowToast(result?.Data?.message);
					} else if (result?.ResultType == ResultType.Unauthorized) {
						//IsBackEnabled = true;
						await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
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

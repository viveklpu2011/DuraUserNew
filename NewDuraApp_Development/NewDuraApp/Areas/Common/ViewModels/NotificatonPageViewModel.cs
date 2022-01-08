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
using NewDuraApp.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Common.ViewModels
{
	public class NotificatonPageViewModel : AppBaseViewModel
	{
		private ObservableCollection<NotificatonData> _notificationsList;
		private INavigationService _navigationService;
		private IUserCoreService _userCoreService;

		private bool _isMarkedAsReadEnabled;
		public bool IsMarkedAsReadEnabled {
			get { return _isMarkedAsReadEnabled; }
			set {
				_isMarkedAsReadEnabled = value;
				OnPropertyChanged(nameof(IsMarkedAsReadEnabled));
			}
		}

		public IAsyncCommand MarkasReadCommand { get; set; }
		public IAsyncCommand<object> NotificationCommand { get; set; }
		public ObservableCollection<NotificatonData> NotificationsList {
			get { return _notificationsList; }
			set { _notificationsList = value; OnPropertyChanged(); }
		}
		public NotificatonPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			MarkasReadCommand = new AsyncCommand(AllReadNotification);
			NotificationCommand = new AsyncCommand<object>(OneByOneReadNotification);
		}

		internal async Task InitializedData()
		{
			await LoadAllNotificationData();
		}
		public async Task LoadAllNotificationData()
		{
			if (CheckConnection()) {

				try {
					NotificatonRequestModel NotificationRequestMdl = new NotificatonRequestModel() {
						user_id = SettingsExtension.UserId
					};
					ShowLoading();
					var result = await TryWithErrorAsync(_userCoreService.GetNotification(NotificationRequestMdl, SettingsExtension.Token), true, false);
					HideLoading();
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						if (result?.Data?.data != null && result?.Data?.data.Count > 0) {
							IsMarkedAsReadEnabled = true;
							NotificationsList = new ObservableCollection<NotificatonData>(result.Data.data);
							//NotificationsList = new ObservableCollection<NotificatonData>(NotificationsList.Reverse());
						} else {
							IsMarkedAsReadEnabled = false;
						}
					} else if (result?.ResultType == ResultType.Unauthorized) {
						await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
					}
					//ShowAlert(result?.Data?.message, result?.Data?.message);
				} catch (Exception ex) {
					ShowAlert(AppResources.ServerError);
				}
				HideLoading();
			} else
				ShowToast(AppResources.NoInternet);
		}

		private async Task AllReadNotification()
		{
			if (CheckConnection()) {

				try {
					ReadNotificationRequestModel ReadNotificationRequestMdl = new ReadNotificationRequestModel() {
						user_id = SettingsExtension.UserId,
						notification_id = 1,
						is_read = "all"
					};
					ShowLoading();
					var result = await TryWithErrorAsync(_userCoreService.ReadNotification(ReadNotificationRequestMdl, SettingsExtension.Token), true, false);
					HideLoading();
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						await LoadAllNotificationData();
					} else if (result?.ResultType == ResultType.Unauthorized) {
						await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
					}
				} catch (Exception ex) {
					ShowAlert(CommonMessages.ServerError);
				}

			} else
				ShowToast(CommonMessages.NoInternet);
		}
		private async Task OneByOneReadNotification(object obj)
		{
			if (CheckConnection()) {

				try {
					NotificatonData NotificatonModel = obj as NotificatonData;
					ReadNotificationRequestModel ReadNotificationRequestMdl = new ReadNotificationRequestModel() {
						user_id = SettingsExtension.UserId,
						notification_id = NotificatonModel.id
					};
					ShowLoading();
					var result = await TryWithErrorAsync(_userCoreService.ReadNotification(ReadNotificationRequestMdl, SettingsExtension.Token), true, false);
					HideLoading();
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						await LoadAllNotificationData();
						//var list = new List<NotificatonData>();
						//if (NotificationsList!=null && NotificationsList.Count>0)
						//{
						//    foreach (var item in NotificationsList)
						//    {
						//        if (item.id == NotificatonModel.id)
						//        {
						//            item.Backgrondcolor = "#DDDAFF";
						//        }
						//        else
						//        {
						//            item.Backgrondcolor = "#ffffff";
						//        }
						//        list.Add(item);
						//    }
						//    TopUpAmountModelList = new ObservableCollection<WalletAmountModel>(list);
						//}

					} else if (result?.ResultType == ResultType.Unauthorized) {
						await LogoutHelper.LogoutOnTokenExpire("Token expire, you will be redirected to login");
					}
				} catch (Exception ex) {
					ShowAlert(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);
		}
	}
}
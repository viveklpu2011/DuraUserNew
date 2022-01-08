using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using DuraApp.Core.Helpers;
using DuraApp.Core.Models.RequestModels;
using NewDuraApp.Areas.Common.Views;
using Xamarin.Forms;

namespace NewDuraApp.Helpers
{
	public class LogoutHelper
	{

		static IUserDialogs _userDialogs => UserDialogs.Instance;
		public static async Task LogoutOnTokenExpire(string msg)
		{
			try {
				_userDialogs.Alert(msg);

				using (Acr.UserDialogs.UserDialogs.Instance.Loading("Please wait")) {
					App.Current.MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
					await App.Locator.LoginPage.InitilizeData();
					await App.Locator.LoginPage.GetAllLocation();
					App.Locator.AddStopLocation.StopAddressList.Clear();
					App.Locator.AddStopLocation.stopid = 0;
					App.Locator.AddStopLocation.StopAddressList = new ObservableCollection<PickupScheduleRequestStopModel>();
					App.Locator.TrackOrder.StopAddressList.Clear();


					SettingsExtension.IsLoggedIn = false;
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
					App.Locator.PickupLocation.Address2 = string.Empty;
					App.Locator.WhereTo.Address2 = string.Empty;
					App.Locator.AddStopLocation.Address2 = string.Empty;
				}
			} catch (Exception ex) {
				var val = ex.Message;

			}




		}
	}
}

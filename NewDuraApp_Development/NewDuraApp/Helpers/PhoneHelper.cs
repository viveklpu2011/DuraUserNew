using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NewDuraApp.Helpers
{
	public class PhoneHelper
	{
		public static void PlacePhoneCall(string number)
		{
			try {
				PhoneDialer.Open(number);
			} catch (ArgumentNullException anEx) {
				// Number was null or white space
			} catch (FeatureNotSupportedException ex) {
				// Phone Dialer is not supported on this device.
			} catch (Exception ex) {
				// Other error has occurred.
			}
		}

		public static async Task SendSms(string messageText, string recipient)
		{
			try {
				var message = new SmsMessage(messageText, new[] { recipient });
				await Sms.ComposeAsync(message);
			} catch (FeatureNotSupportedException ex) {
				// Sms is not supported on this device.
			} catch (Exception ex) {
				// Other error has occurred.
			}
		}
	}
}

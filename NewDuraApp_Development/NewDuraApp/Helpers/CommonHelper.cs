using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NewDuraApp.Helpers
{
	public class CommonHelper
	{
		public static double CurrentLat { get; internal set; }
		public static double CurrentLong { get; internal set; }
		public static double CurrentLatPick { get; internal set; }
		public static double CurrentLongPick { get; internal set; }
		public static double CurrentLatDrop { get; internal set; }
		public static double CurrentLongDrop { get; internal set; }
		public static double CurrentLatStop { get; internal set; }
		public static double CurrentLongStop { get; internal set; }

		public static string GenerateRandomId(int length)
		{
			char[] stringChars = new char[length];
			byte[] randomBytes = new byte[length];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
				rng.GetBytes(randomBytes);
			}

			string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

			for (int i = 0; i < stringChars.Length; i++) {
				stringChars[i] = chars[randomBytes[i] % chars.Length];
			}

			return new string(stringChars);
		}

		public static async Task<Contact> GetContact()
		{
			Contact contact = new Contact();
			try {
				contact = await Contacts.PickContactAsync();

				if (contact == null)
					return null;

				var id = contact.Id;
				var namePrefix = contact.NamePrefix;
				var givenName = contact.GivenName;
				var middleName = contact.MiddleName;
				var familyName = contact.FamilyName;
				var nameSuffix = contact.NameSuffix;
				var displayName = contact.DisplayName;
				var phones = contact.Phones; // List of phone numbers
				var emails = contact.Emails; // List of email addresses
			} catch (Exception ex) {
				// Handle exception here.
			}
			return contact;
		}
	}
}

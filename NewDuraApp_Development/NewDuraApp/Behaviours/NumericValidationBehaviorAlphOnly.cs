using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace NewDuraApp.Behaviours
{
	public class NumericValidationBehaviorAlphOnly : Behavior<Entry>
	{
		protected override void OnAttachedTo(Entry entry)
		{
			entry.TextChanged += OnEntryTextChanged;
			base.OnAttachedTo(entry);
		}

		protected override void OnDetachingFrom(Entry entry)
		{
			entry.TextChanged -= OnEntryTextChanged;
			base.OnDetachingFrom(entry);
		}

		private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
		{

			if (!string.IsNullOrWhiteSpace(args.NewTextValue)) {
				bool isValid = false;
				Regex regex = new Regex(@"^[a-zA-Z ]+$");

				if (regex.IsMatch(args.NewTextValue)) {
					isValid = true;
				}


				//bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers
				//if (isValid || args.NewTextValue.Contains("+")) {
				//	isValid = true;
				//}


				((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
			}
		}
	}
}

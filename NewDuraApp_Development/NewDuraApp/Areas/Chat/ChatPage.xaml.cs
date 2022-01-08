using Xamarin.Forms;

namespace NewDuraApp.Areas
{
	public partial class ChatPage : ContentPage
	{
		public ChatPage()
		{
			InitializeComponent();
			webView.Source = "https://duradriver.com/duradrive/chatmaster/#!/login";
		}
	}
}

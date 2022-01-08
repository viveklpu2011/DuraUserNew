using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.NoConnection.ViewModel
{
	public class NoInternetPopupPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		public IAsyncCommand ButtonCommand { get; set; }
		public IAsyncCommand CloseInternetPopupCommand { get; set; }

		private bool _isButtonEnabled;
		public bool IsButtonEnabled {
			get { return _isButtonEnabled; }
			set {
				_isButtonEnabled = value;
				OnPropertyChanged(nameof(IsButtonEnabled));
			}
		}

		private string _buttonText = AppResources.Try_Again;
		public string ButtonText {
			get { return _buttonText; }
			set {
				_buttonText = value;
				OnPropertyChanged(nameof(ButtonText));
			}
		}

		private string _connectionText = AppResources.Not_Connected;
		public string ConnectionText {
			get { return _connectionText; }
			set {
				_connectionText = value;
				OnPropertyChanged(nameof(ConnectionText));
			}
		}

		private bool _isConnectedToInternet;
		public bool IsConnectedToInternet {
			get { return _isConnectedToInternet; }
			set {
				_isConnectedToInternet = value;
				OnPropertyChanged(nameof(IsConnectedToInternet));
			}
		}

		public NoInternetPopupPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			ButtonCommand = new AsyncCommand(ButtonCommandExecute);
			CloseInternetPopupCommand = new AsyncCommand(CloseInternetPopupCommandExecute);
		}

		private async Task CloseInternetPopupCommandExecute()
		{
			if (ConnectivityService.IsConnected()) {
				IsConnectedToInternet = true;
				ConnectionText = AppResources.Connected;
				IsButtonEnabled = false;
				await _navigationService.ClosePopupsAsync();
			} else {
				ShowToast(AppResources.Not_connected_to_internet);
			}
		}

		private async Task ButtonCommandExecute()
		{
			IsButtonEnabled = false;
			if (ButtonText == AppResources.Try_Again) {
				if (ConnectivityService.IsConnected()) {
					IsConnectedToInternet = true;
					ConnectionText = AppResources.Connected;
					IsButtonEnabled = false;
					await Task.Delay(1000);
					await _navigationService.ClosePopupsAsync();
				} else {
					IsButtonEnabled = true; 
					IsConnectedToInternet = false;
					ConnectionText = AppResources.Not_Connected;
				}
			}
		}

		public async Task InitilizeData()
		{
			ButtonText = AppResources.Try_Again;
			IsConnectedToInternet = false;
			ConnectionText = AppResources.Not_Connected;
			IsButtonEnabled = true;
		}

	}
}

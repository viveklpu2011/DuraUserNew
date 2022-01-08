using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.Essentials;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.About.ViewModels
{
	public class AboutPageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private string _appVersion;
		public string AppVersion {
			get { return _appVersion; }
			set {
				_appVersion = value;
				OnPropertyChanged(nameof(AppVersion));
			}
		}

		private string _aboutUsUrl;
		public string AboutUsUrl {
			get { return _aboutUsUrl; }
			set {
				_aboutUsUrl = value;
				OnPropertyChanged(nameof(AboutUsUrl));
			}
		}

		public AboutPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			AppVersion = VersionTracking.CurrentVersion;
		}
		public async Task InitilizeData()
		{
			//AboutUsUrl = AppConstant.AboutusUrl;
		}
	}
}

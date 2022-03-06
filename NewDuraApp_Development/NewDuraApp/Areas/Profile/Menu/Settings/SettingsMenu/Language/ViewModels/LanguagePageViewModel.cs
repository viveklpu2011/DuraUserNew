using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Plugin.Multilingual;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Profile.Menu.Settings.SettingsMenu.Language.ViewModels
{
	public class LanguagePageViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
        public IAsyncCommand SaveCommand { get; set; }
        public LanguagePageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
            SaveCommand = new AsyncCommand(SaveCommandExecute, allowsMultipleExecutions: false);
        }
		public async Task InitilizeData()
		{
			//ShowAlert("We are Working on it.It is in Progress.", "DuraDrive", "Ok");
		}
        public async Task SaveCommandExecute()
        {
            ShowLoading();
            ChangeLanguage(); 
            await App.Locator.HomePage.GetAllLocation();
            await App.Locator.HomePage.InitilizeData();
            await App.Locator.HomePage.CheckAccountIsVerified();
            await _navigationService.NavigateToAsync<AppShellViewModel>(); 
            HideLoading();
        } 

        private LanguageModel _selectedLanguage;
        public LanguageModel SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { _selectedLanguage = value; OnPropertyChanged(nameof(SelectedLanguage)); /*ChangeLanguage();*/ }

        } 
        public class LanguageModel
        {
            public string LanguageName { get; set; }
        }
        public ObservableCollection<LanguageModel> LanguageList { get; set; } = new ObservableCollection<LanguageModel>()
        {
            new LanguageModel { LanguageName = "English"},
            new LanguageModel { LanguageName = "Filipino"},
        };

        private async void ChangeLanguage()
        {
            if (SelectedLanguage.LanguageName == "English")
            {
                SettingsExtension.SelectedLanguage = false;
                CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("en");
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            }
            else
            {
                SettingsExtension.SelectedLanguage = true;
                CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("fil");
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            }
        }
    }
}

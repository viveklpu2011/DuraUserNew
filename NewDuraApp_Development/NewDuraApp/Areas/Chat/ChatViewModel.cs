using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas
{
	public class ChatViewModel : AppBaseViewModel
	{
		INavigationService _navigationService;
		private IUserCoreService _userCoreService;
		public ChatViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
		}
	}
}

using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Common.ViewModels;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Menu.ReferFriend.ViewModels
{
    public class ReferAFriendPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToTermsCmd { get; set; }
        public IAsyncCommand InviteCommand { get; set; }
        public IAsyncCommand SubmitReferralCodeCommand { get; set; }
        private IUserCoreService _userCoreService;

        private string _referralCode;
        public string ReferralCode
        {
            get { return _referralCode; }
            set
            {
                _referralCode = value;
                OnPropertyChanged(nameof(ReferralCode));
            }
        }

        private string _referralCodeDesc;
        public string ReferralCodeDesc
        {
            get { return _referralCodeDesc; }
            set
            {
                _referralCodeDesc = value;
                OnPropertyChanged(nameof(ReferralCodeDesc));
            }
        }

        public ReferAFriendPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToTermsCmd = new AsyncCommand(GoToTermsCmdExecute);
            InviteCommand = new AsyncCommand(InviteCommandExecute);
            SubmitReferralCodeCommand = new AsyncCommand(SubmitReferralCodeCommandExecute);
            ReferralCodeDesc = AppResources.Refer_a_friend_get_you_and_your_friend_off_on_food_ordering;
            ReferralCode = "2XdgrVU";
        }

        private async Task InviteCommandExecute()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "Hey Friend, \nGet 50% off on your dura express .Here’s a coupon for a free. Use Code while Registeration of app Here is the app link : https://Play.google.com/duradrive and code is " + ReferralCode,
                Title = AppResources.Dura_Drive_ReferralCode
            });
        }

        private async Task SubmitReferralCodeCommandExecute()
        {
            if (string.IsNullOrEmpty(ReferralCode))
            {
                ShowToast(AppResources.Please_enter_the_referral_code);
                return;
            }
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    ReferralCodeRequestModel referralCodeRequestModel = new ReferralCodeRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                        referal_code = ReferralCode
                    };
                    var result = await TryWithErrorAsync(_userCoreService.SubmitReferralCode(referralCodeRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        ReferralCode = string.Empty;
                        await _navigationService.NavigateBackAsync();
                    }
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }

        private async Task GoToTermsCmdExecute()
        {
            await Browser.OpenAsync(AppConstant.TermsServicesUrl, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#211E66"),
                PreferredControlColor = Color.White
            });
        }

        public async Task InitilizeData()
        {
            //ReferralCode = string.Empty;
        }
    }
}

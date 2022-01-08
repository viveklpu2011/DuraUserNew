using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class ApplyPromoCodePageViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public ApplyPromoCodePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async Task InitilizeData()
        {

        }
    }
}

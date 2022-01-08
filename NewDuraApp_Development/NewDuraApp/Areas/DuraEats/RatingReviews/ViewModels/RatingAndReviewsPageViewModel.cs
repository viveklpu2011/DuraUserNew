using System;
using System.Threading.Tasks;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels
{
    public class RatingAndReviewsPageViewModel:AppBaseViewModel
    {
        INavigationService _navigationService;
        public RatingAndReviewsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async Task InitilizeData()
        {

        }
    }
}

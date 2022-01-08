using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.Reviews.ViewModels
{
	public class MyReviewsPageViewModel : AppBaseViewModel
	{
		public IAsyncCommand<UserReviewsModel> ReveiewDetails { get; set; }
		INavigationService _navigationService;
		private IUserCoreService _userCoreService;
		private ObservableCollection<GetRatingModel> _userReviews;
		public ObservableCollection<GetRatingModel> UserReviews {
			get { return _userReviews; }
			set { _userReviews = value; OnPropertyChanged(nameof(UserReviews)); }
		}

		private GetRatingResponseModel _reviewData;
		public GetRatingResponseModel ReviewData {
			get { return _reviewData; }
			set {
				_reviewData = value;
				OnPropertyChanged(nameof(ReviewData));
			}
		}

		public MyReviewsPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			ReveiewDetails = new AsyncCommand<UserReviewsModel>(ReveiewDetailsPage);
		}

		private async Task ReveiewDetailsPage(UserReviewsModel arg)
		{
			if (_navigationService.GetCurrentPageViewModel() != typeof(RatingAndReviewsPageViewModel)) {
				await App.Locator.RatingAndReviewsPage.InitilizeData();
				await _navigationService.NavigateToAsync<RatingAndReviewsPageViewModel>();
			}
		}


		public async Task InitilizeData()
		{
			await GetReviews();
			//UserReviews = new ObservableCollection<UserReviewsModel>
			//{
			//    new UserReviewsModel()
			//    {
			//        Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
			//        NumberOfRatings=5,
			//        ReviewId=0,
			//        UserAddress="Makati Avenue 1200",
			//        UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
			//        UserLikesCount=100,
			//        UserName="John Doe",
			//        ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
			//        {
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            },
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            },
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            },
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            },
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            }
			//        }
			//    },
			//    new UserReviewsModel()
			//    {
			//        Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
			//        NumberOfRatings=5,
			//        ReviewId=1,
			//        UserAddress="Makati Avenue 1200",
			//        UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
			//        UserLikesCount=100,
			//        UserName="Jessica Johnson",
			//        ItemDetailsReviewsImageList=null
			//    },
			//    new UserReviewsModel()
			//    {
			//        Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
			//        NumberOfRatings=5,
			//        ReviewId=2,
			//        UserAddress="Makati Avenue 1200",
			//        UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
			//        UserLikesCount=100,
			//        UserName="John Doe",
			//        ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
			//        {
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            },
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            }
			//        }
			//    },
			//    new UserReviewsModel()
			//    {
			//        Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
			//        NumberOfRatings=5,
			//        ReviewId=3,
			//        UserAddress="Makati Avenue 1200",
			//        UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
			//        UserLikesCount=100,
			//        UserName="John Doe",
			//        ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
			//        {
			//            new ItemDetailsReviewsImageModel()
			//            {
			//                ReviewImageId=0,
			//                ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
			//            }
			//        }
			//    }
			//};
		}

		private async Task GetReviews()
		{
			if (CheckConnection()) {
				ShowLoading();
				try {
					CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel() {
						user_id = SettingsExtension.UserId,
					};
					var result = await TryWithErrorAsync(_userCoreService.GetUserRating(commonUserIdRequestModel, SettingsExtension.Token), true, false);
					if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
						ShowLoading();
						ReviewData = result?.Data;

						if (result?.Data?.data != null && result?.Data?.data.Count > 0) {
							List<GetRatingModel> lstReviews = new List<GetRatingModel>();
							GetRatingModel getRatingModel = new GetRatingModel();
							foreach (var item in result?.Data?.data) {
								getRatingModel = new GetRatingModel();
								getRatingModel.created_at = item.created_at;
								getRatingModel.driver_address = item.driver_address;
								getRatingModel.firstname = item.firstname;
								getRatingModel.lastname = item.lastname;
								getRatingModel.middlename = item.middlename;
								getRatingModel.rating = item.rating;
								getRatingModel.remarks = item.remarks;
								getRatingModel.user_firstname = item.user_firstname;
								getRatingModel.user_lastname = item.user_lastname;
								getRatingModel.profilephoto_url = item.profilephoto_url;
								//var img = string.IsNullOrEmpty(item?.profilephoto_url) ? await ImageHelper.GetStreamFormResource("camera.png") : await ImageHelper.GetImageFromUrl(item?.profilephoto_url);
								//getRatingModel.UserImage = ImageSource.FromStream(() => new MemoryStream(img));
								lstReviews.Add(getRatingModel);
							}
							if (lstReviews != null && lstReviews.Count > 0) {
								UserReviews = new ObservableCollection<GetRatingModel>(lstReviews);
							}
						}
						HideLoading();
					}
				} catch (Exception ex) {
					// ShowAlert(CommonMessages.ServerError);
				}
				HideLoading();
			} else
				ShowToast(CommonMessages.NoInternet);
		}
	}
}

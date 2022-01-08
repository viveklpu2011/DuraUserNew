using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.DuraEats.ItemDetails.Popups.ViewModels;
using NewDuraApp.Areas.DuraEats.MyCart.ViewModels;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.DuraEats.ItemDetails.ViewMdels
{
    public class DuraEatsItemDetailsPageViewModel:AppBaseViewModel
    {
        private ObservableCollection<DuraEatsItemDetailsFoodModel> _duraEatsFoodMenuList;
        private ObservableCollection<UserReviewsModel> _userReviews;
        private DuraEatsHomePageRestaurentModel _restaurentDetails;
        public IAsyncCommand SkipCommand { get; set; }
        public IAsyncCommand GoToMyCartCmd { get; set; }
        public IAsyncCommand<DuraEatsItemDetailsFoodModel> FoodDetails { get; set; }
        INavigationService _navigationService;
        public DuraEatsHomePageRestaurentModel RestaurentDetails
        {
            get { return _restaurentDetails; }
            set { _restaurentDetails = value; OnPropertyChanged(nameof(RestaurentDetails)); }
        }
        public ObservableCollection<DuraEatsItemDetailsFoodModel> DuraEatsFoodMenuList
        {
            get { return _duraEatsFoodMenuList; }
            set { _duraEatsFoodMenuList = value; OnPropertyChanged(nameof(DuraEatsFoodMenuList)); }
        }
        public ObservableCollection<UserReviewsModel> UserReviews
        {
            get { return _userReviews; }
            set { _userReviews = value; OnPropertyChanged(nameof(UserReviews)); }
        }
        public DuraEatsItemDetailsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToMyCartCmd = new AsyncCommand(GoToMyCartCmdExecute);
            //FoodDetails = new AsyncCommand<DuraEatsItemDetailsFoodModel>(GetFoodDetails);
        }

        private async Task GoToMyCartCmdExecute()
        {
            if(_navigationService.GetCurrentPageViewModel()!= typeof(MyCartPageViewModel))
            {
                await _navigationService.NavigateToAsync<MyCartPageViewModel>();
            }
        }

        public async Task GetFoodDetails(DuraEatsItemDetailsFoodList arg)
        {
          
                App.Locator.ItemsDetailAddCartPopup.DuraEatsFoodMenuDetails = arg;
                await App.Locator.ItemsDetailAddCartPopup.InitilizeData();
                ItemsDetailAddCartPopupViewModel CartPopupp = new ItemsDetailAddCartPopupViewModel(_navigationService);
                await _navigationService.NavigateToPopupAsync<ItemsDetailAddCartPopupViewModel>(true, CartPopupp);

        }

        public async Task InitilizeData()
        {
            var data = RestaurentDetails;

            DuraEatsFoodMenuList = new ObservableCollection<DuraEatsItemDetailsFoodModel>
            {
                new DuraEatsItemDetailsFoodModel
                {
                   MenuHeadingId=0,
                   MenuHeading="Recommended",
                   FoodList=new List<DuraEatsItemDetailsFoodList>
                   {
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=0
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=1
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=2
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=3
                       },
                   }
                },
                new DuraEatsItemDetailsFoodModel
                {
                   MenuHeadingId=1,
                   MenuHeading="Pizza",
                   FoodList=new List<DuraEatsItemDetailsFoodList>
                   {
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=0
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=1
                       }
                   }
                },
                new DuraEatsItemDetailsFoodModel
                {
                   MenuHeadingId=2,
                   MenuHeading="Snacks",
                   FoodList=new List<DuraEatsItemDetailsFoodList>
                   {
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=0
                       }
                   }
                },
                new DuraEatsItemDetailsFoodModel
                {
                   MenuHeadingId=3,
                   MenuHeading="Lunch",
                   FoodList=new List<DuraEatsItemDetailsFoodList>
                   {
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=0
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=1
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=2
                       },
                       new DuraEatsItemDetailsFoodList
                       {
                           FoodImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                           FoodDetails="Vegetable mixed in Mayonnaise Spread. Fresh Aloo Tikki used.",
                           FoodName="Double Cheese Veg Club Burger",
                           FoodPrice="₱20",
                           MenuId=3
                       },
                   }
                },
            };

            UserReviews = new ObservableCollection<UserReviewsModel>
            {
                new UserReviewsModel()
                {
                    Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
                    NumberOfRatings=5,
                    ReviewId=0,
                    UserAddress="Makati Avenue 1200",
                    UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
                    UserLikesCount=100,
                    UserName="John Doe",
                    ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
                    {
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        },
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        },
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        },
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        },
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        }
                    }
                },
                new UserReviewsModel()
                {
                    Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
                    NumberOfRatings=5,
                    ReviewId=1,
                    UserAddress="Makati Avenue 1200",
                    UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
                    UserLikesCount=100,
                    UserName="Jessica Johnson",
                    ItemDetailsReviewsImageList=null
                },
                new UserReviewsModel()
                {
                    Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
                    NumberOfRatings=5,
                    ReviewId=2,
                    UserAddress="Makati Avenue 1200",
                    UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
                    UserLikesCount=100,
                    UserName="John Doe",
                    ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
                    {
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        },
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        }
                    }
                },
                new UserReviewsModel()
                {
                    Description="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat.",
                    NumberOfRatings=5,
                    ReviewId=3,
                    UserAddress="Makati Avenue 1200",
                    UserImage=ImageHelper.GetImageNameFromResource("t_shirt.png"),
                    UserLikesCount=100,
                    UserName="John Doe",
                    ItemDetailsReviewsImageList=new List<ItemDetailsReviewsImageModel>
                    {
                        new ItemDetailsReviewsImageModel()
                        {
                            ReviewImageId=0,
                            ReviewImage=ImageHelper.GetImageNameFromResource("food_one.png"),
                        }
                    }
                }
            };
        }
    }
}

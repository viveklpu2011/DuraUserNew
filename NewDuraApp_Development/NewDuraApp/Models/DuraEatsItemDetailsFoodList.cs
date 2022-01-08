using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
    public class DuraEatsItemDetailsFoodModel
    {
        public long MenuHeadingId { get; set; }
        public string MenuHeading { get; set; }
        public List<DuraEatsItemDetailsFoodList> FoodList { get; set; }
    }
    public class DuraEatsItemDetailsFoodList
    {
        public long MenuId { get; set; }
        public ImageSource FoodImage { get; set; }
        public string FoodName { get; set; }
        public string FoodDetails { get; set; }
        public string FoodPrice { get; set; }
    }

    public class ItemDetailsReviewsImageModel
    {
        public long ReviewImageId { get; set; }
        public ImageSource ReviewImage { get; set; }
    }
    public class UserReviewsModel
    {
        public long ReviewId { get; set; }
        public string UserName { get; set; }
        public ImageSource UserImage { get; set; }
        public int NumberOfRatings { get; set; }
        public long UserLikesCount { get; set; }
        public string Description { get; set; }
        public string UserAddress { get; set; }
        public List<ItemDetailsReviewsImageModel> ItemDetailsReviewsImageList { get; set; }
    }
}

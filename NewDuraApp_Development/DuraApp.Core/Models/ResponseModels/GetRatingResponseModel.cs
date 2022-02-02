using System;
using System.Collections.Generic;
using System.Linq;
using DuraApp.Core.Models.Common;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetRatingResponseModel : CommonResponseModel
    {
        public List<GetRatingModel> data { get; set; }
        public double avg_stars { get; set; }
        public int TotalPositiveRating
        {
            get
            {
                if (data != null && data.Count > 0)
                {
                    return data.Where(x => x.Rating > 2).Count();
                }
                else
                {
                    return 0;
                }
            }
        }
        public int TotalNegativeRating
        {
            get
            {
                if (data != null && data.Count > 0)
                {
                    return data.Where(x => x.Rating <= 2).Count();
                }
                else
                {
                    return 0;
                }
            }
        }
        public int TotalReviews
        {
            get
            {
                if (data != null && data.Count > 0)
                {
                    return data.Where(x => x.remarks != string.Empty).Count();
                }
                else
                {
                    return 0;
                }
            }
        }
        public int TotalStars
        {
            get
            {
                if (data != null && data.Count > 0)
                {
                    return ((data.Sum(item => item.Rating)) / data.Count);
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class GetRatingModel
    {
        public string remarks { get; set; }
        public string rating { get; set; }
        public string created_at { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string driver_address { get; set; }
        public string profilephoto_url { get; set; }
        public int Rating
        {
            get
            {
                if (!string.IsNullOrEmpty(rating))
                {
                    return Convert.ToInt32(rating);
                }
                else
                {
                    return 0;
                }
            }
        }
        public ImageSource UserImage { get; set; }
        public byte[] ProductImage { get; set; }
        public string FullName
        {
            get
            {
                return $"{firstname} {lastname}";
            }
        }
    }
}

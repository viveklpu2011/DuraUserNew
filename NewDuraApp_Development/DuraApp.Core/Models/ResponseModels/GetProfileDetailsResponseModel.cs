using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetProfileDetailsModel
    {
        public int id { get; set; }
        public string is_verified { get; set; }
        public string profile_image { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string amount { get; set; }
        public int review { get; set; }
        public string currency { get; set; }
        public string facebook { get; set; }
        public string google { get; set; }
        public string status { get; set; }
        public long country_id { get; set; }
        public string country_code { get; set; }
        public string Facebook
        {
            get
            {
                if (!string.IsNullOrEmpty(facebook))
                {
                    return facebook == "0" ? "" : facebook;
                }
                else
                {
                    return "";
                }
            }
            set { }
        }
        public string Google
        {
            get
            {
                if (!string.IsNullOrEmpty(google))
                {
                    return google == "0" ? "" : google;
                }
                else
                {
                    return "";
                }
            }
            set { }
        }
    }

    public class GetProfileDetailsResponseModel : CommonResponseModel
    {
        public GetProfileDetailsModel data { get; set; }
    }
}

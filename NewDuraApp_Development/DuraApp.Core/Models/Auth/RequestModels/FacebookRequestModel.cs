using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Models.Auth.RequestModels
{
    public class FacebookRequestModel
    {
        public string email { get; set; } 
        public string area { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string login_type { get; set; }
        public string facebook_id { get; set; }
        public string google_id { get; set; }
        public string referral_code { get; set; }

    }
}
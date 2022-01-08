using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Models.Auth.ResponseModel
{
    public class FacebookResponseModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string profile_image { get; set; }
        public FBData data { get; set; }
    }
    public class FbHeaders
    {
    }

    public class FbOriginal
    {
        public string token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class FBData
    {
        public FbHeaders headers { get; set; }
        public FbOriginal original { get; set; }
        public object exception { get; set; }
    }

}
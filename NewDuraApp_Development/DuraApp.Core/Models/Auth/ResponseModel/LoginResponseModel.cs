using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.Auth.ResponseModel
{
    public class LoginResponseModel : CommonResponseModel
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string profile_image { get; set; }
        public Data data { get; set; }
        public string area { get; set; }
    }

    public class Headers
    {
    }

    public class Original
    {
        public string token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
    public class Data
    {
        public Headers headers { get; set; }
        public Original original { get; set; }
        public object exception { get; set; }
    }

}

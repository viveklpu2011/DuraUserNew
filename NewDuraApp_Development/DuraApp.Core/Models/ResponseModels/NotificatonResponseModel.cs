using DuraApp.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class NotificatonResponseModel : CommonResponseModel
    {
        public List<NotificatonData> data { get; set; }
    }
    public class NotificatonData
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public object image { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string login_type { get; set; }
        public int is_read { get; set; }
        public string reason { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public Color NotificationColor
        {
            get
            {
                if (is_read == 0)
                {
                    return Color.FromHex("#f1eee6");
                }
                else
                {
                    return Color.White;
                }
            }
        }

    }
}
using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class CardListResponseModel : CommonResponseModel
    {
        public List<CardListModel> data { get; set; }
    }

    public class CardListModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string expirydate { get; set; }
        public string securitycode { get; set; }
        public long user_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}

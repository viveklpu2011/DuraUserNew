using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class VerifyPromoCodeResponseModel : CommonResponseModel
    {
        public VerifyPromoCodeData data { get; set; }
    }

    public class VerifyPromoCodeData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string area { get; set; }
        public string merchant { get; set; }
        public string promotype { get; set; }
        public string discount { get; set; }
        public string application { get; set; }
        public string promocodelimit { get; set; }
        public string limitperuser { get; set; }
        public string maxdiscount { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string valid_date { get; set; }
        public bool is_used { get; set; }
    }
}

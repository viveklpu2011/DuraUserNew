using System;
using System.Collections.Generic;
using System.Globalization;
using DuraApp.Core.Models.Common;
using Xamarin.Forms;

namespace DuraApp.Core.Models.ResponseModels
{
    public class PromoCodeListResponse : CommonResponseModel
    {
        public List<PromoCodeListModel> data { get; set; }
    }
    public class PromoCodeListModel
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
        public string valid_date { get; set; }
        public bool is_used { get; set; }
        public string IsPromoUsedText
        {
            get
            {
                if (is_used)
                {
                    return "Used";
                }
                else
                {
                    return "Apply";
                }
            }
        }
        public Color IsPromoUsedColor
        {
            get
            {
                if (is_used)
                {
                    return Color.Gray;
                }
                else
                {
                    return Color.FromHex("#211E66");
                }
            }
        }
        public DateTime PromoValidDate
        {
            get
            {
                return DateTime.Parse(valid_date);
            }
        }
    }
}

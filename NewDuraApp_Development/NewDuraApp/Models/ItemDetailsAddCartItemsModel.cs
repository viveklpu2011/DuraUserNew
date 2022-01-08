using System;
using System.Collections.Generic;

namespace NewDuraApp.Models
{
    public class ItemDetailsAddCartAddOnsModel
    {
        public int AddOnsItemId { get; set; }
        public string AddOnsItemHeading { get; set; }
        public List<AddOnsItems> AddOnsItemList  { get; set; }
    }
    public class AddOnsItems
    {
        public int AddOnsId { get; set; }
        public string AddOnsItemName { get; set; }
        public string AddOnsPrice { get; set; }
    }
}

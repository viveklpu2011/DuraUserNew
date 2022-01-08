using System;
namespace NewDuraApp.GCash.Models
{
    public class Redirect
    {
        public string success { get; set; }
        public string failed { get; set; }
        public string checkout_url { get; set; }
    }

    public class Address
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }

    public class Billing
    {
        public Address address { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class AttributesSourceMaster
    {
        public int amount { get; set; }
        public Billing billing { get; set; }
        public string currency { get; set; }
        public bool livemode { get; set; }
        public Redirect redirect { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
    }

    public class DataSourceMaster
    {
        public string id { get; set; }
        public string type { get; set; }
        public AttributesSourceMaster attributes { get; set; }
    }

    public class CreateSourceMaster
    {
        public DataSourceMaster data { get; set; }
    }
}

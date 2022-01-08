using System;
namespace NewDuraApp.GCash.Models
{
    public class Source
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Attributes
    {
        public int amount { get; set; }
        public Source source { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string statement_descriptor { get; set; }
    }

    public class Data
    {
        public Attributes attributes { get; set; }
    }

    public class CreatePaymentMaster
    {
        public Data data { get; set; }
    }
}

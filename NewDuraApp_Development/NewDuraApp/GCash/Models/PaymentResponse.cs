using System;
using System.Collections.Generic;

namespace NewDuraApp.GCash.Models
{
    public class AttributesPayment
    {
        public object access_url { get; set; }
        public long amount { get; set; }
        public string balance_transaction_id { get; set; }
        public object billing { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public bool disputed { get; set; }
        public object external_reference_number { get; set; }
        public int fee { get; set; }
        public bool livemode { get; set; }
        public long net_amount { get; set; }
        public string origin { get; set; }
        public object payment_intent_id { get; set; }
        public object payout { get; set; }
        public Source source { get; set; }
        public string statement_descriptor { get; set; }
        public string status { get; set; }
        public object tax_amount { get; set; }
        public List<object> refunds { get; set; }
        public List<object> taxes { get; set; }
        public int available_at { get; set; }
        public int created_at { get; set; }
        public int paid_at { get; set; }
        public int updated_at { get; set; }
    }

    public class DataPayment
    {
        public string id { get; set; }
        public string type { get; set; }
        public AttributesPayment attributes { get; set; }
    }

    public class PaymentResponse
    {
        public DataPayment data { get; set; }
    }

}

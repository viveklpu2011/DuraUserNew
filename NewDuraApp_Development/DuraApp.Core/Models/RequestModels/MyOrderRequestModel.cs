using System;
namespace DuraApp.Core.Models.RequestModels
{
	public class MyOrderRequestModel : OrderDetailsRequestModel
	{
		public int status { get; set; }
		public int page_id { get; set; }
		public int totalcount { get; set; }
	}
	public class MyOrderRequestModel1
	{
		public long user_id { get; set; }
		public int status { get; set; }
		public int page_id { get; set; }
		public int totalcount { get; set; }
	}
}

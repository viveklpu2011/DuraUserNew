using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Models.RequestModels
{
	public class ReadNotificationRequestModel : Common.CommonResponseModel
	{
		public int user_id { get; set; }
		public int notification_id { get; set; }
		public string is_read { get; set; }
	}
}

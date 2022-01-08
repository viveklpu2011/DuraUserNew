using System;
namespace NewDuraApp.Models
{
	public class NetworkAuthData
	{
		public string phone { get; set; }
		public string login_type { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Logo { get; set; }
		public string Picture { get; set; }
		public string Background { get; set; }
		public string Foreground { get; set; }
		public string Email { get; internal set; }
	}
}

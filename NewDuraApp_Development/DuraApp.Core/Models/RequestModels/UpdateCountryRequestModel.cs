using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class UpdateCountryRequestModel : CommonUserIdRequestModel
    {
        public long country_id { get; set; }
    }
}

using System;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class ReferCodeResponseModel : CommonResponseModel
    {
        public ReferCodeModel data { get; set; }
    }

    public class ReferCodeModel
    {
        public long id { get; set; }
        public long refid { get; set; }
        public string recode { get; set; }
        public long issued { get; set; }
        public long usedbyuserid { get; set; }
        public string createddatetime { get; set; }
    }
}

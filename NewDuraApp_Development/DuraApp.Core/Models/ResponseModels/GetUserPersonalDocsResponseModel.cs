using System;
using System.Collections.Generic;
using DuraApp.Core.Models.Common;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GetUserPersonalDocsResponseModel : CommonResponseModel
    {
        public Dataidcard dataidcard { get; set; }
        public Dataidcard datadriver { get; set; }
    }
    public class GetUserBusinessDocsResponseModel : CommonResponseModel
    {
        public Dataidcard dataone { get; set; }
        public Dataidcard datatwo { get; set; }
    }
    public class Dataidcard
    {
        public string documentname { get; set; }
        public string frontimage { get; set; }
        public string backimage { get; set; }
        public int status { get; set; }
    }

}

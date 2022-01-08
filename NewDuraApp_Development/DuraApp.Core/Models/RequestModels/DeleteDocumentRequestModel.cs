using System;
namespace DuraApp.Core.Models.RequestModels
{
    public class DeleteDocumentRequestModel
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public string type { get; set; }
    }
}

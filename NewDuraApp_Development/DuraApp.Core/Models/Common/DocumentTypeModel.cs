using System;
using DuraApp.Core.Helpers.Enums;

namespace DuraApp.Core.Models.Common
{
    public class DocumentTypeModel
    {
        public string propertyname { get; set; }
        public DocumentType documentType { get; set; }
        public DocumentIdType documentIdType { get; set; }
    }
}

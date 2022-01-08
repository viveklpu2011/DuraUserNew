using System;
namespace NewDuraApp.Models
{
    public class AddressModel
    {
        public long AddressId { get; set; }
        public string HouseNo { get; set; }
        public string Block { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string HouseType { get; set; }
        public string BlockAddress { get { return $"{Block} {City}"; } }
        public string AddressIcon { get; set; }
    }
}

using System;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
    public class SavedCardsModel
    {
        public long CardId { get; set; }
        public string CardHolderName { get; set; }
        public string CardSecurityCode { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public ImageSource CardType { get; set; }
    }
}

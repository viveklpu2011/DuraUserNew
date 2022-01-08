using System;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
    public class DuraEatsHomePageRestaurentModel
    {
        public int Id { get; set; }
        public ImageSource RestarentImage { get; set; }
        public string RestaurentName { get; set; }
        public string RestaurentLocation { get; set; }
        public string RestaurentDistance { get; set; }
    }
}

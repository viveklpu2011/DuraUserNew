using System;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
    public class ProfileMenuModel
    {
        public int MenuId { get; set; }
        public ImageSource ImageName { get; set; }
        public string MenuName { get; set; }
        public Thickness StackThickness { get; set; }
        public bool IsMenuEnabled { get; set; }
    }
}

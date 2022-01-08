using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
   public class DuraShopModel: AppBaseViewModel
    {
        public string DayName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public ImageSource BannerImage { get; set; }

        public ImageSource ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public ImageSource NewProductImage { get; set; }
        public string NewProductName { get; set; }
        public string NewProductPrice { get; set; }

        private Color _selectedColor;
        public Color ColorSelected
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value; OnPropertyChanged();
            }
        }
        private Color _frameColorSelected;
        public Color FrameColorSelected
        {
            get => _frameColorSelected;
            set
            {
                _frameColorSelected = value; OnPropertyChanged();
            }
        }
        private Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value; OnPropertyChanged();
            }
        }
        private Color _textColor1;
        public Color TextColor1
        {
            get => _textColor1;
            set
            {
                _textColor1 = value; OnPropertyChanged();
            }
        }
    }
}

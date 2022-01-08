using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NewDuraApp.Models
{
  public  class ProductItemModel : AppBaseViewModel
    {
        public ImageSource ProductImage { get; set; }
        public string SizeText { get; set; }
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

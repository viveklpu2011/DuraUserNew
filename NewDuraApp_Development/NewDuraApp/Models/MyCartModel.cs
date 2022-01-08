using NewDuraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewDuraApp.Models
{
   public class MyCartModel: AppBaseViewModel
    {
        public string ProductDescription { get; set; }


        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(); }
        }

        //public ICommand IncrementCmd => new Command(Increment);
        //private void Increment(object obj)
        //{
        //    Quantity++;
        //}
        //public ICommand DecrementCmd => new Command(Decrement);
        //private void Decrement(object obj)
        //{
        //    if (Quantity > 0)
        //    {
        //        Quantity--;
        //    }
        //}
    }
}

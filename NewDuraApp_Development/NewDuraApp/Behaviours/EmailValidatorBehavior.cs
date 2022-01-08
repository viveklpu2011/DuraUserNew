using System;
using System.Text.RegularExpressions;
using DuraApp.Core.Helpers;
using Xamarin.Forms;

namespace NewDuraApp.Behaviours
{
    public class EmailValidatorBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty IsEmailValidProperty =
          BindableProperty.Create(propertyName: nameof(IsEmailValid),
              returnType: typeof(bool),
              declaringType: typeof(EmailValidatorBehavior),
              defaultValue: false);

        public bool IsEmailValid
        {
            get
            {
                return (bool)GetValue(IsEmailValidProperty);
            }
            set
            {
                SetValue(IsEmailValidProperty, value);
            }
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsEmailValid = false;
            IsEmailValid = (Regex.IsMatch(e.NewTextValue, AppConstant.EmailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            //IsEmailValid = IsValid;
           ((Entry)sender).TextColor = IsEmailValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}

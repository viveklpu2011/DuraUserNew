using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NewDuraApp.Controls;
using NewDuraApp.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessTimePicker), typeof(BorderlessTimePickerRenderer))]
namespace NewDuraApp.Droid.Renderers
{
    public class BorderlessTimePickerRenderer : TimePickerRenderer
    {
        public static void Init() { }
        protected Xamarin.Forms.TimePicker _element;
        public BorderlessTimePickerRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            ////Disposing
            //if (e.OldElement != null)
            //{
            //    _element = null;
            //}

            ////Creating
            //if (e.NewElement != null)
            //{
            //    _element = e.NewElement;
            //}

            if (e.OldElement == null)
            {

                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;

                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
        //protected override TimePickerDialog CreateTimePickerDialog(int hours, int minutes)
        //{
        //    var d = new TimePickerDialog(Context, () => {

        //    },hours,minutes,false);

        //    var dialog = new TimePickerDialog(Context, (o, e) =>
        //    {
        //        _element.Time = new TimeSpan();
        //        ((IElementController)_element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        //    }, year, month, day);

        //    dialog.SetButton((int)DialogButtonType.Positive, Context.Resources.GetString(global::Android.Resource.String.Ok), OnOk);
        //    dialog.SetButton((int)DialogButtonType.Negative, Context.Resources.GetString(global::Android.Resource.String.Cancel), OnCancel);

        //    return dialog;
        //}
    }
}
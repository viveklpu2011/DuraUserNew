using Android.Content;
using Android.Content.Res;
using NewDuraApp.Controls;
using NewDuraApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomRadioBtton), typeof(CustomRadioButtonRenderer))]
namespace NewDuraApp.Droid.Renderers
{
    public class CustomRadioButtonRenderer : RadioButtonRenderer
    {
        public CustomRadioButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.RadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ButtonTintList = ColorStateList.ValueOf(Android.Graphics.Color.Rgb(33, 30, 102));
            }
        }
    }
}

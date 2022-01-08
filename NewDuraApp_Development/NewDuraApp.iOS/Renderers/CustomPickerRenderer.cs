using NewDuraApp.Controls;
using NewDuraApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace NewDuraApp.iOS.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var element = (CustomPicker)this.Element;

            if (this.Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
            {
                var image = $"{element.Image}.png";
                var downarrow = UIImage.FromBundle(element.Image);
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.BackgroundColor = element.BackgroundColor.ToUIColor();
                Control.BorderStyle = UITextBorderStyle.None;
                Control.RightView = new UIImageView(ImaageResizeExtension.MaxResizeImage(downarrow, 20, 20));
            }
        }
    }
}

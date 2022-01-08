using System;
using System.ComponentModel;
using NewDuraApp.Controls;
using NewDuraApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace NewDuraApp.iOS.Renderers
{
    public class BorderlessEditorRenderer : EditorRenderer
    {
        public static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control!=null)
            {
                Control.Layer.BorderWidth = 0;
            }
            
        }
    }
}

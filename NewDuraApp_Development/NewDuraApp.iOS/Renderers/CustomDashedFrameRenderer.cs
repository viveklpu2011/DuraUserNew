using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using NewDuraApp.Controls;
using NewDuraApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDashedFrame), typeof(CustomDashedFrameRenderer))]
namespace NewDuraApp.iOS.Renderers
{
    public class CustomDashedFrameRenderer : FrameRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CustomDashedFrame customFrame = Element as CustomDashedFrame;
            CAShapeLayer viewBorder = new CAShapeLayer();
            viewBorder.StrokeColor = UIColor.FromRGB(255,0,0).CGColor;
            viewBorder.StrokeStart = 0;
            viewBorder.StrokeEnd = 1;
            viewBorder.BorderColor = Color.Transparent.ToCGColor();
            viewBorder.FillColor = null;
            viewBorder.LineDashPattern = new NSNumber[] { new NSNumber(5), new NSNumber(2) };
            
            viewBorder.CornerRadius = customFrame.CornerRadius;
            viewBorder.MasksToBounds = true;
            viewBorder.Frame = NativeView.Bounds;
            viewBorder.Path = UIBezierPath.FromRect(NativeView.Bounds).CGPath;

            var path = UIBezierPath.FromRoundedRect(this.Bounds, UIRectCorner.TopLeft |
                                                                 UIRectCorner.BottomLeft |
                                                                 UIRectCorner.TopRight |
                                                                 UIRectCorner.BottomRight,
                                                                 new CGSize(customFrame.CornerRadius,customFrame.CornerRadius));
            
            viewBorder.Path = path.CGPath;
            Layer.AddSublayer(viewBorder);

            // If you don't want the shadow effect
            Element.HasShadow = false;
    }
}


}

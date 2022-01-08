

using NewDuraApp.Controls;
using NewDuraApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MySearchBar), typeof(MySearchBar_iOS))]
namespace NewDuraApp.iOS.Renderers
{
    public class MySearchBar_iOS : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var searchbar = (UISearchBar)Control;

            if (e.NewElement != null)
            {
                searchbar.TintColor = UIColor.Black;
                searchbar.BarTintColor = UIColor.Clear;
                searchbar.Layer.CornerRadius = 8;
                searchbar.Layer.BorderWidth = 0;
                searchbar.Layer.BorderColor = UIColor.Clear.CGColor;
                searchbar.SearchBarStyle = UISearchBarStyle.Minimal;
               
                searchbar.SetShowsCancelButton(false, false);
                searchbar.ShowsCancelButton = false;

                searchbar.TextChanged += delegate
                {
                    searchbar.ShowsCancelButton = false;
                };
            }

        }
    }
}

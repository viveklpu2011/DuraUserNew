using System;

using Android.Widget;
using Android.Text;
using G = Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NewDuraApp.Controls;
using NewDuraApp.Droid.Renderers;
using Android.Content;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(MySearchBar), typeof(MySearchBar_Droid))]

namespace NewDuraApp.Droid.Renderers
{
    public class MySearchBar_Droid : SearchBarRenderer
    {
        public MySearchBar_Droid(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            if (Control!=null)
            {
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);

                // Get native control (background set in shared code, but can use SetBackgroundColor here)
                SearchView searchView = (base.Control as SearchView);
                searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

                // Access search textview within control
                int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
                EditText textView = (searchView.FindViewById(textViewId) as EditText);

                // Set custom colors
                //textView.SetBackgroundColor(G.Color.Rgb(225, 225, 225));
                textView.SetTextColor(G.Color.Rgb(32, 32, 32));
                textView.SetHintTextColor(G.Color.Rgb(128, 128, 128));
                textView.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
                // Customize frame color
                int frameId = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
                Android.Views.View frameView = (searchView.FindViewById(frameId) as Android.Views.View);
            }
          
           // frameView.SetBackgroundColor(G.Color.Rgb(96, 96, 96));
        }
    }
}

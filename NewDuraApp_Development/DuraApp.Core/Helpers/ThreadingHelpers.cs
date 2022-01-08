using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DuraApp.Core.Helpers
{
    public static class ThreadingHelpers
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (Device.RuntimePlatform == "Test")
            {
                action?.Invoke();
            }
            else
                Device.BeginInvokeOnMainThread(action);
        }
    }
}

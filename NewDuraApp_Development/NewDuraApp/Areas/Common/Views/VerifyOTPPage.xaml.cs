using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Common.Views
{
    public partial class VerifyOTPPage : ContentPage
    {
        private CancellationTokenSource cancellation;
        bool shouldRun;
        public VerifyOTPPage()
        {
            InitializeComponent();
            ResendOTPlbl.TextColor = Color.FromHex("#D72625");
        }
        void VerifyEntryText_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (sender != null)
            {
                var value = e.NewTextValue;
                var selectedEntry = (Entry)sender;
                switch (selectedEntry.AutomationId)
                {
                    case "1":
                        if (!string.IsNullOrEmpty(value))
                        {
                            // Pin1.Unfocus();
                            Pin2.Focus();
                            //Pin3.Unfocus();
                            //Pin4.Unfocus();
                        }
                        break;
                    case "2":
                        if (!string.IsNullOrEmpty(value))
                        {
                            //Pin1.Unfocus();
                            // Pin2.Unfocus();
                            Pin3.Focus();
                            //Pin4.Unfocus();
                        }
                        break;
                    case "3":
                        if (!string.IsNullOrEmpty(value))
                        {
                            // Pin1.Unfocus();
                            // Pin2.Unfocus();
                            // Pin3.Unfocus();
                            Pin4.Focus();
                        }
                        break;
                    case "4":
                        if (!string.IsNullOrEmpty(value))
                        {
                            Pin1.Unfocus();
                            Pin2.Unfocus();
                            Pin3.Unfocus();
                            Pin4.Unfocus();
                            //  NewPin1.Focus();
                        }
                        break;
                    case "5":
                        if (string.IsNullOrEmpty(value))
                        {

                            Pin3.Focus();
                            //  NewPin1.Focus();
                        }
                        break;
                }

            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.Locator.VerifyOTPPage.StartTimerEvent += async () =>
            {
                App.Locator.VerifyOTPPage.IsEnable = false;
                TimeSpan time = new TimeSpan(0, 2, 0);
                TimeSpan subtractTime = TimeSpan.FromSeconds(1);
                CancellationTokenSource cts = this.cancellation = new CancellationTokenSource();
                Device.BeginInvokeOnMainThread(() =>
                {

                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {

                        if (this.cancellation != null)
                            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
                        // Do something
                        if (time.Hours == 0 && time.Minutes == 0 && time.Seconds == 0)
                        {
                            lblTimer.Text = time.ToString();
                            ResendOTPlbl.TextColor = Color.FromHex("#D72625");
                            if (this.cancellation != null)
                                Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
                            shouldRun = false;
                            App.Locator.VerifyOTPPage.IsEnable = true;
                            //App.Locator.StartTimerPage.SendMessagesOnAlarm();
                            return shouldRun;
                        }
                        else
                        {
                            ResendOTPlbl.TextColor = Color.FromHex("#ff9999");
                            time = time - subtractTime;
                            lblTimer.Text = time.ToString();
                            shouldRun = true;
                            return shouldRun;
                        }
                    });

                });
                //App.Locator.VerifyOTPPage.IsEnable = true;
            };



        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Locator.VerifyOTPPage.OTP1 = App.Locator.VerifyOTPPage.OTP2 = App.Locator.VerifyOTPPage.OTP3 = App.Locator.VerifyOTPPage.OTP4 = string.Empty;
            if (this.cancellation != null)
                Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
            shouldRun = false;
        }
    }
}

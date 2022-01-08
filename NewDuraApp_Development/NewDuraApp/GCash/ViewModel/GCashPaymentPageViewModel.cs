using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels;
using NewDuraApp.GCash.Helpers;
using NewDuraApp.GCash.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;

namespace NewDuraApp.GCash.ViewModel
{
    public class GCashPaymentPageViewModel : AppBaseViewModel
    {
        private INavigationService _navigationService;
        private IUserCoreService _userCoreService;

        HttpClient client;
        CreateSourceMaster ResponseDataSource;
        public IAsyncCommand ButtonTryAgainCommand { get; set; }
        public IAsyncCommand BackCommand { get; set; }
        private WalletAmountModel _walletAmount;
        private GetSaveWalletAmountResponseModel _getSaveWalletAmount;
        public GetSaveWalletAmountResponseModel GetSaveWalletAmount
        {
            get { return _getSaveWalletAmount; }
            set { _getSaveWalletAmount = value; OnPropertyChanged(nameof(GetSaveWalletAmount)); }
        }

        private bool _isBackEnabled;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set
            {
                _isBackEnabled = value;
                OnPropertyChanged(nameof(IsBackEnabled));
            }
        }

        private bool _isAppearing;
        public bool IsAppearing
        {
            get { return _isAppearing; }
            set
            {
                _isAppearing = value;
                OnPropertyChanged(nameof(IsAppearing));
            }
        }

        private string _paymentText;
        public string PaymentText
        {
            get { return _paymentText; }
            set
            {
                _paymentText = value;
                OnPropertyChanged(nameof(PaymentText));
            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        private bool _isVisibleLoader;
        public bool IsVisibleLoader
        {
            get { return _isVisibleLoader; }
            set
            {
                _isVisibleLoader = value;
                OnPropertyChanged(nameof(IsVisibleLoader));
            }
        }

        public WalletAmountModel WalletAmount
        {
            get { return _walletAmount; }
            set { _walletAmount = value; OnPropertyChanged(nameof(WalletAmount)); }
        }

        private string _animationFileName = "payment_initiating.json";
        public string AnimationFileName
        {
            get { return _animationFileName; }
            set
            {
                _animationFileName = value;
                OnPropertyChanged(nameof(AnimationFileName));
            }
        }

        public GCashPaymentPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            ButtonTryAgainCommand = new AsyncCommand(ButtonTryAgainCommandExecute);
            BackCommand = new AsyncCommand(BackCommandExecute);
        }

        private async Task BackCommandExecute()
        {
            var result = ShowConfirmationAlert("Do you want to cancel the transaction");
            if (result.Result)
            {
                await _navigationService.NavigateBackAsync();
            }
        }

        private async Task ButtonTryAgainCommandExecute()
        {

            await GcashInitiatePayment();
        }

        internal async Task InitilizeData()
        {
            client = new HttpClient();
            ResponseDataSource = new CreateSourceMaster();
            //PaymentText = "Initiating Payment";
            //IsVisibleLoader = true;
            //ButtonText = "Please wait";
            WalletAmount = App.Locator.AddMoneyPage.WalletAmount;
            await GcashInitiatePayment();
        }

        public async Task GcashInitiatePayment()
        {
            if (CheckConnection())
            {
                AnimationFileName = "payment_initiating.json";
                PaymentText = "Initiating Payment";
                IsVisibleLoader = true;
                ButtonText = "Please wait";
                var currencyAmt = (Convert.ToInt32(WalletAmount?.amount) * 100);
                var phonenumber = SettingsExtension.Phone;
                var authData = string.Format("{0}:{1}", GCashHelper.Public_Key, "");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                CreateSourceMaster createSourceMaster = new CreateSourceMaster();
                Redirect redirectObj = new Redirect { failed = GCashHelper.GCashFailedUrl, success = GCashHelper.GCashSuccessUrl };
                Address addressObj = new Address { line1 = "", line2 = "", city = "", state = "", country = "PH", postal_code = "" };
                Billing billingObj = new Billing { name = SettingsExtension.UserFullName, address = addressObj, phone = SettingsExtension.Phone, email = SettingsExtension.UserEmail };
                AttributesSourceMaster attribute = new AttributesSourceMaster { amount = currencyAmt, billing = billingObj, currency = "PHP", redirect = redirectObj, type = "gcash" };
                DataSourceMaster data = new DataSourceMaster { attributes = attribute };
                createSourceMaster.data = data;
                var jsonData = JsonConvert.SerializeObject(createSourceMaster);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var WebAPIUrl = GCashHelper.GCashPaymentUrlSource; // Set your REST API URL here.
                var uri = new Uri(WebAPIUrl);
                try
                {
                    var response = await client.PostAsync(uri, content);
                    await Task.Delay(3000);
                    if (response.IsSuccessStatusCode)
                    {
                        await Task.Delay(3000);
                        PaymentText = "Payment Initiated";
                        ButtonText = "Payment Initiated";
                        IsVisibleLoader = false;
                        var contentResult = await response.Content.ReadAsStringAsync();
                        ResponseDataSource = JsonConvert.DeserializeObject<CreateSourceMaster>(contentResult);
                        var confirmation = await ShowConfirmationAlert("Do you want to process a payment", "Payment Initiated");
                        if (confirmation)
                        {
                            AnimationFileName = "payment_processing.json";
                            PaymentText = "Payment Processing";
                            ButtonText = "Payment Processing";
                            IsVisibleLoader = true;
                            IsAppearing = true;

                            if (_navigationService.GetCurrentPageViewModel() != typeof(PaymentPageWebviewViewModel))
                            {
                                await _navigationService.NavigateToAsync<PaymentPageWebviewViewModel>();
                                await App.Locator.PaymentPageWebview.InitilizeData(ResponseDataSource.data.attributes.redirect.checkout_url);
                            }
                        }
                        else
                        {
                            IsBackEnabled = true;
                            AnimationFileName = "payment_failed.json";
                            IsVisibleLoader = false;
                            PaymentText = "Payment Cancelled";
                            ButtonText = "Try again";
                        }
                    }
                    else
                    {
                        IsBackEnabled = true;
                        AnimationFileName = "payment_failed.json";
                        IsVisibleLoader = false;
                        PaymentText = "Payment Initiating Failed";
                        ButtonText = "Try again";
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }
        public async Task GcashMakePayment()
        {
            if (CheckConnection())
            {
                string contentResult = string.Empty;
                var authData = string.Format("{0}:{1}", GCashHelper.Secret_Key, "");
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

                CreatePaymentMaster createPaymentMaster = new CreatePaymentMaster();
                Data data = new Data();
                Attributes attributes = new Attributes();
                Source source = new Source();

                if (ResponseDataSource.data.id != string.Empty && ResponseDataSource.data.type != string.Empty)
                {
                    source.id = ResponseDataSource.data.id;
                    source.type = ResponseDataSource.data.type;
                }
                else
                {
                    IsBackEnabled = true;
                    ShowAlert("Please Rehit to Pay Button to request payment", "Error Gcash");
                    return;
                }

                attributes.amount = (Convert.ToInt32(WalletAmount?.amount) * 100);
                attributes.currency = "PHP";
                attributes.description = "Dura Drive Wallet Recharge";
                attributes.statement_descriptor = "Dura";
                attributes.source = source;
                data.attributes = attributes;
                createPaymentMaster.data = data;
                var jsonData = JsonConvert.SerializeObject(createPaymentMaster);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var WebAPIUrl = GCashHelper.GCashPaymentUrl; // Set your REST API URL here.
                var uri = new Uri(WebAPIUrl);
                try
                {
                    var response = await client.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        AnimationFileName = "payment_success.json";
                        IsVisibleLoader = false;
                        PaymentText = "Payment Done";
                        ButtonText = "Payment Done";
                        contentResult = await response.Content.ReadAsStringAsync();
                        var ResponseData = JsonConvert.DeserializeObject<PaymentResponse>(contentResult);
                        if (ResponseData != null && ResponseData?.data != null && ResponseData?.data?.attributes != null)
                        {
                            await NavigateToRehargeSuccessfullPage(ResponseData.data.attributes.amount, ResponseData.data?.attributes?.balance_transaction_id);
                        }

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        IsBackEnabled = true;
                        AnimationFileName = "payment_failed.json";
                        IsVisibleLoader = false;
                        PaymentText = "Payment Failed";
                        ButtonText = "Try again";
                        var ResponseDataError = JsonConvert.DeserializeObject<ErrorClass>(contentResult);
                        ShowAlert("Your Payment is not approved by Gcash.if it is deducted it will revert back within 1 hour. More Details ... " + ResponseDataError?.errors[0].detail, "Error Gcash");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    IsBackEnabled = true;
                    AnimationFileName = "payment_failed.json";
                    ButtonText = "Try again";
                    IsVisibleLoader = false;
                }
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }

        private async Task NavigateToRehargeSuccessfullPage(long amount, string transactionid)
        {
            if (CheckConnection())
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading("Adding amount to your wallet."))
                {
                    try
                    {
                        GetSaveWalletAmountRequestModel getSaveWalletAmountRequestModel = new GetSaveWalletAmountRequestModel
                        {
                            user_id = SettingsExtension.UserId,
                            amount = (Convert.ToDouble(amount) / 100),
                            transactionid = transactionid,
                            transactiontype = "gcash"
                        };
                        var result = await TryWithErrorAsync(_userCoreService.SaveWalletAmount(getSaveWalletAmountRequestModel, SettingsExtension.Token), true, false);
                        if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                        {
                            GetSaveWalletAmount = result?.Data;
                            App.Locator.RechargeSuccessfullPopupPage.GetSaveWalletAmount = GetSaveWalletAmount;

                            RechargeSuccessfullPopupPageViewModel rechargeSuccessfullPopupPageViewModel = new RechargeSuccessfullPopupPageViewModel(_navigationService);
                            await _navigationService.NavigateToPopupAsync<RechargeSuccessfullPopupPageViewModel>(true, rechargeSuccessfullPopupPageViewModel);


                        }
                        else
                        {
                            ShowAlert(result?.Data?.message);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowToast(CommonMessages.ServerError);
                    }
                }

            }
            else
                ShowToast(CommonMessages.NoInternet);

        }
    }
}

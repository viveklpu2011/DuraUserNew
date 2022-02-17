using System;
using System.Collections.ObjectModel;
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
using NewDuraApp.GCash.Helpers;
using NewDuraApp.GCash.Models;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Profile.Menu.MyWalllet.Popups.ViewModels
{
    public class RechargeToppupPopupPageViewModel : AppBaseViewModel
    {
        HttpClient client;
        CreateSourceMaster ResponseDataSource;
        INavigationService _navigationService;
        public IAsyncCommand NavigateToRehargeSuccessfull { get; set; }
        private IUserCoreService _userCoreService;

        private GetSaveWalletAmountResponseModel _getSaveWalletAmount;
        public GetSaveWalletAmountResponseModel GetSaveWalletAmount
        {
            get { return _getSaveWalletAmount; }
            set { _getSaveWalletAmount = value; OnPropertyChanged(nameof(GetSaveWalletAmount)); }
        }

        private WalletAmountModel _walletAmount;
        public WalletAmountModel WalletAmount
        {
            get { return _walletAmount; }
            set { _walletAmount = value; OnPropertyChanged(nameof(WalletAmount)); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        public RechargeToppupPopupPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            NavigateToRehargeSuccessfull = new AsyncCommand(NavigateToRehargeSuccessfullPage);
            InitilizeData();
        }

        public async Task InitilizeData()
        {
            client = new HttpClient();
            ResponseDataSource = new CreateSourceMaster();
            WalletAmount = App.Locator.AddMoneyPage.WalletAmount;
            Phone = SettingsExtension.Phone;
        }

        private async Task NavigateToRehargeSuccessfullPage()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    GetSaveWalletAmountRequestModel getSaveWalletAmountRequestModel = new GetSaveWalletAmountRequestModel
                    {
                        user_id = SettingsExtension.UserId,
                        amount = Convert.ToDouble(WalletAmount?.amount)
                    };
                    var result = await TryWithErrorAsync(_userCoreService.SaveWalletAmount(getSaveWalletAmountRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result?.Data?.status == 200)
                    {
                        GetSaveWalletAmount = result?.Data;
                        App.Locator.RechargeSuccessfullPopupPage.GetSaveWalletAmount = GetSaveWalletAmount;
                        await _navigationService.ClosePopupsAsync();
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
                HideLoading();
            }
            else
                ShowToast(CommonMessages.NoInternet);
        }

        private async Task GcashInitiatePayment()
        {
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
                if (response.IsSuccessStatusCode)
                {
                    var contentResult = await response.Content.ReadAsStringAsync();
                    ResponseDataSource = JsonConvert.DeserializeObject<CreateSourceMaster>(contentResult);
                    ShowAlert($"Response From API {ResponseDataSource.data.id}");
                    await Browser.OpenAsync(ResponseDataSource.data.attributes.redirect.checkout_url, new BrowserLaunchOptions
                    {
                        LaunchMode = BrowserLaunchMode.SystemPreferred,
                        TitleMode = BrowserTitleMode.Show,
                        PreferredToolbarColor = Color.AliceBlue,
                        PreferredControlColor = Color.Violet
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task GcashMakePayment()
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
                    contentResult = await response.Content.ReadAsStringAsync();
                    var ResponseData = JsonConvert.DeserializeObject<PaymentResponse>(contentResult);
                    ShowAlert("Response From API - Payment DONE Successfully", ResponseData.data.id);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var ResponseDataError = JsonConvert.DeserializeObject<ErrorClass>(contentResult);
                    ShowAlert("Your Payment is not approved by Gcash.if it is deducted it will revert back within 1 hour. More Details ... " + ResponseDataError?.errors[0].detail, "Error Gcash");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}

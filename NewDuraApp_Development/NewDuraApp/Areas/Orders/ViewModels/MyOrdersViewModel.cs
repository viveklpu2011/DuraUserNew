using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Areas.DuraEats.RatingReviews.ViewModels;
using NewDuraApp.Areas.DuraExpress.DuraExpressViewModel;
using NewDuraApp.Areas.DuraExpress.Popup.View;
using NewDuraApp.Areas.DuraShop.ViewModel;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewDuraApp.Areas.Orders.ViewModels
{
    public class MyOrdersViewModel : AppBaseViewModel
    {
        int PageSize = 4;
        public int PageNo;
        static bool isSamePageNumber = true;
        INavigationService _navigationService;
        public bool _isLock;
        public bool _isAllItemLoaded;
        List<MyOrderResponse> lstOrderList = new List<MyOrderResponse>();
        public IAsyncCommand<MyOrderResponse> TrackLocationCommand { get; set; }
        public IAsyncCommand GoToRateCmd { get; set; }
        public IAsyncCommand<MyOrdersModel> GoToTrackOrderCmd { get; set; }
        public IAsyncCommand GoToEatsRateCmd { get; set; }
        public IAsyncCommand<MyOrderResponse> TrackOrderDetailsCommand { get; set; }
        public IAsyncCommand<MyOrderResponse> CancelOrderCommand { get; set; }
        public IAsyncCommand<MyOrderResponse> RateAndReviewCommand { get; set; }
        private IUserCoreService _userCoreService;
        private ObservableCollection<MyOrderResponse> _dureExpressOrderList;
        private GetpickupDetailsModel _getpickupData;
        public GetpickupDetailsModel GetPickupData
        {
            get { return _getpickupData; }
            set
            {
                _getpickupData = value;
                OnPropertyChanged(nameof(GetPickupData));
            }
        }

        private int _itemTreshold;
        public int ItemTreshold
        {
            get { return _itemTreshold; }
            set { SetProperty(ref _itemTreshold, value); }
        }

        private long _pickUpId;
        public long PickUpId
        {
            get { return _pickUpId; }
            set
            {
                _pickUpId = value;
                OnPropertyChanged(nameof(PickUpId));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private bool _isEmpty = false;
        public bool IsEmpty
        {
            get => _isEmpty;
            set { _isEmpty = value; OnPropertyChanged(); }
        }

        private bool _isOnLoad;
        public bool IsOnLoad
        {
            get { return _isOnLoad; }
            set
            {
                _isOnLoad = value;
                OnPropertyChanged(nameof(IsOnLoad));
            }
        }

        private bool _isNoRecord = true;
        public bool IsNoRecord
        {
            get { return _isNoRecord; }
            set
            {
                _isNoRecord = value;
                OnPropertyChanged(nameof(IsNoRecord));
            }
        }

        private bool _isLoadingIndicator;
        public bool IsLoadingIndicator
        {
            get { return _isLoadingIndicator; }
            set
            {
                _isLoadingIndicator = value;
                OnPropertyChanged(nameof(IsLoadingIndicator));
            }
        }

        public ObservableCollection<MyOrderResponse> DuraExpressOrderList
        {
            get { return _dureExpressOrderList; }
            set { _dureExpressOrderList = value; OnPropertyChanged(nameof(DuraExpressOrderList)); }
        }

        private ObservableCollection<MyOrdersModel> _orderList;
        public ObservableCollection<MyOrdersModel> OrderList
        {
            get { return _orderList; }
            set { _orderList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MyOrdersModel> _completedOrderList;
        public ObservableCollection<MyOrdersModel> CompletedOrderList
        {
            get { return _completedOrderList; }
            set { _completedOrderList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MyOrdersModel> _duraEatsOrderList;
        public ObservableCollection<MyOrdersModel> DuraEatsOrderList
        {
            get { return _duraEatsOrderList; }
            set { _duraEatsOrderList = value; OnPropertyChanged(); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private OrderStatusType orderStatusType;
        public OrderStatusType OrderStatus
        {
            get { return orderStatusType; }
            set
            {
                orderStatusType = value;
                OnPropertyChanged(nameof(OrderStatus));
            }
        }

        public IAsyncCommand RemainThresholdReachCommand { get; set; }
        public MyOrdersViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            PageNo = 1;
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            GoToRateCmd = new AsyncCommand(GoToRateCmdExecute);
            GoToTrackOrderCmd = new AsyncCommand<MyOrdersModel>(GoToTrackOrderCmdExecute, allowsMultipleExecutions: false);
            GoToEatsRateCmd = new AsyncCommand(GoToEatsRateCmdExecute, allowsMultipleExecutions: false);
            TrackOrderDetailsCommand = new AsyncCommand<MyOrderResponse>(TrackOrderDetailsCommandExecute, allowsMultipleExecutions: false);
            RateAndReviewCommand = new AsyncCommand<MyOrderResponse>(RateAndReviewCommandExecute, allowsMultipleExecutions: false);
            RemainThresholdReachCommand = new AsyncCommand(RemainThresholdReachCommandExecute, allowsMultipleExecutions: false);
            OrderStatusList = GetOrderStatusList();
            TrackLocationCommand = new AsyncCommand<MyOrderResponse>(TrackLocationCommandute, allowsMultipleExecutions: false);
            CancelOrderCommand = new AsyncCommand<MyOrderResponse>(CancelOrderCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task CancelOrderCommandExecute(MyOrderResponse arg)
        {
            PickUpId = arg.pickup_id;
            await App.Locator.CancelRideReasonPopup.InitilizeData();
            await PopupNavigation.Instance.PushAsync(new CancelRideReasonPopup());
        }

        private async Task TrackLocationCommandute(MyOrderResponse arg)
        {
            if (arg != null && !string.IsNullOrEmpty(arg?.driverlocationlink))
            {
                await Browser.OpenAsync(arg.driverlocationlink, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#211E66"),
                    PreferredControlColor = Color.White
                });
            }
            else
            {
                ShowToast(AppResources.No_driver_location_link_found);
            }
        }

        private async Task RateAndReviewCommandExecute(MyOrderResponse arg)
        {
            try
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetails2ViewModel))
                {
                    GetPickupData = new GetpickupDetailsModel();
                    GetPickupData.pickup_id = arg.pickup_id;
                    GetPickupData.status = arg.status;
                    await App.Locator.OrderDetails2.InitilizeData(GetPickupData, false);
                    await Shell.Current.GoToAsync("OrderDetails2");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task TrackOrderDetailsCommandExecute(MyOrderResponse arg)
        {
            try
            {
                if (_navigationService.GetCurrentPageViewModel() != typeof(OrderDetails2ViewModel))
                {
                    GetPickupData = new GetpickupDetailsModel();
                    GetPickupData.pickup_id = arg.pickup_id;
                    await App.Locator.OrderDetails2.InitilizeData(GetPickupData, false);
                    await Shell.Current.GoToAsync("OrderDetails2");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private ObservableCollection<MyOrdersModel> GetDuraEatsOrderList()
        {
            return new ObservableCollection<MyOrdersModel>()
            {
                new MyOrdersModel()
                {
                    OrderAddress="Makati Avenue 1200"
                },
                new MyOrdersModel()
                {
                    OrderAddress="Makati Avenue 1200"
                },
            };
        }

        private ObservableCollection<MyOrdersModel> GetCompletedOrderList()
        {
            return new ObservableCollection<MyOrdersModel>()
           {
               new MyOrdersModel()
               {
                   CompleteOrderNumber ="#1566565656544",
                   CompleteOrderDate=" July 25 2020 at 05:30 pm",
                   CompletedOrderAddress1="1976 Capt. M. Reyes, Makati, Metro Manila Phillipnes"
               },
                new MyOrdersModel()
               {
                   CompleteOrderNumber ="#1566565656544",
                   CompleteOrderDate=" July 25 2020 at 05:30 pm",
                   CompletedOrderAddress1="1976 Capt. M. Reyes, Makati, Metro Manila Phillipnes"
               },
                new MyOrdersModel()
               {
                   CompleteOrderNumber ="#1566565656544",
                   CompleteOrderDate=" July 25 2020 at 05:30 pm",
                   CompletedOrderAddress1="1976 Capt. M. Reyes, Makati, Metro Manila Phillipnes"
               },
           };
        }

        private async Task GoToEatsRateCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(RatingAndReviewsPageViewModel))
            {
                await _navigationService.NavigateToAsync<RatingAndReviewsPageViewModel>();
            }
        }

        private async Task GoToRateCmdExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(RateandReviewViewModel))
            {
                await _navigationService.NavigateToAsync<RateandReviewViewModel>();
            }
        }

        private async Task GoToTrackOrderCmdExecute(MyOrdersModel myOrdersModel)
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(TrackOrderShopViewModel))
            {
                await _navigationService.NavigateToAsync<TrackOrderShopViewModel>();
                MessagingCenter.Send<MyOrdersViewModel>(this, "FromMyOrderShop");
            }
        }

        private ObservableCollection<MyOrdersModel> GetOrderList()
        {
            return new ObservableCollection<MyOrdersModel>()
            {
                new MyOrdersModel()
                {
                    ProductName="Levi's",
                    ProductDescription="Men Red T Shirt",
                    Size="M",
                    Price="₱16",
                    OrderNumber="156656566544",
                    OrderOn="July 25 2020 at 05:30 Pm",
                    OrderStatus="156656566544"
                },
                new MyOrdersModel()
                {
                    ProductName="Levi's",
                    ProductDescription="Men Red T Shirt",
                    Size="M",
                    Price="₱16",
                    OrderNumber="156656566544",
                    OrderOn="July 25 2020 at 05:30 Pm",
                    OrderStatus="156656566544"
                },
                new MyOrdersModel()
                {
                    ProductName="Levi's",
                    ProductDescription="Men Red T Shirt",
                    Size="M",
                    Price="₱16",
                    OrderNumber="156656566544",
                    OrderOn="July 25 2020 at 05:30 Pm",
                    OrderStatus="156656566544"
                },
            };
        }

        private ObservableCollection<OrderStatusModel> _orderStatusList;
        public ObservableCollection<OrderStatusModel> OrderStatusList
        {
            get { return _orderStatusList; }
            set { _orderStatusList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<OrderStatusModel> GetOrderStatusList()
        {
            return new ObservableCollection<OrderStatusModel>()
            {
                new OrderStatusModel()
                {
                    ChooseOrderStatus=OrderStatusType.Pending.ToString(),
                    OrderId=OrderStatusType.Pending
                } ,
                new OrderStatusModel()
                {
                    ChooseOrderStatus=OrderStatusType.Ongoing.ToString(),
                    OrderId=OrderStatusType.Ongoing
                },
                new OrderStatusModel()
                {
                   ChooseOrderStatus= OrderStatusType.Completed.ToString(),
                    OrderId=OrderStatusType.Completed
                },
                new OrderStatusModel()
                {
                    ChooseOrderStatus=OrderStatusType.Cancelled.ToString(),
                    OrderId=OrderStatusType.Cancelled
                },
            };
        }

        private bool _selectedOrderStatus;
        public bool SelectedOrderStatus
        {
            get { return _selectedOrderStatus; }
            set { _selectedOrderStatus = value; OnPropertyChanged(); }
        }

        public async Task InitilizeDataMyOrder()
        {
            PageNo = 1;
            OrderStatus = OrderStatusType.Pending;
            await GetDuraExpressOrderNew(OrderStatus);
        }

        private async Task RemainThresholdReachCommandExecute()
        {
            if (DuraExpressOrderList.Count > 0)
            {
                if (isSamePageNumber)
                {
                    isSamePageNumber = false;
                    PageNo = PageNo + 1;
                    await GetDuraExpressOrder(OrderStatus, PageNo);
                }
            }
        }

        public async Task GetDuraExpressOrder(OrderStatusType status, int pageno)
        {
            if (IsLoadingIndicator)
                return;
            IsLoadingIndicator = true;
            if (CheckConnection())
            {
                try
                {
                    int id = (int)status;
                    var form = new MultipartFormDataContent();
                    form.Add(new StringContent(Convert.ToString(SettingsExtension.UserId)), "user_id");
                    form.Add(new StringContent(Convert.ToString(id)), "status");
                    form.Add(new StringContent(Convert.ToString(pageno)), "page_id");
                    form.Add(new StringContent(Convert.ToString(PageSize)), "totalcount");
                    var listfoOrder = new List<MyOrderResponse>();
                    var result = await TryWithErrorAsync(_userCoreService.GetMyOrder(form, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            if (result?.Data?.data.Count > 0)
                            {
                                foreach (var student in result?.Data?.data)
                                {
                                    listfoOrder.Add(student);
                                }
                                if (listfoOrder != null && listfoOrder.Count > 0)
                                {
                                    foreach (var item in listfoOrder)
                                    {
                                        DuraExpressOrderList.Add(item);
                                    }
                                    isSamePageNumber = true;
                                }
                            }
                        }
                        if (DuraExpressOrderList.Count() == 0)
                        {
                            ItemTreshold = -1;
                            return;
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        IsNoRecord = false;
                        IsEmpty = true;
                        IsNoRecord = false;
                        DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                        lstOrderList = new List<MyOrderResponse>();
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else if (result?.ResultType == ResultType.Ok)
                    {
                        if (DuraExpressOrderList == null)
                        {
                            DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                            lstOrderList = new List<MyOrderResponse>();
                        }
                        ShowToast(AppResources.No_more_record_found);
                    }
                    else
                    {
                        IsEmpty = true;
                        IsNoRecord = false;
                        if (DuraExpressOrderList == null)
                        {
                            DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                            lstOrderList = new List<MyOrderResponse>();
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsOnLoad = false;
                    IsEmpty = true;
                }
                finally
                {
                    IsLoadingIndicator = false;
                }
            }
            else
            {
                IsOnLoad = false;
                ShowToast(AppResources.NoInternet);
            }
        }

        public async Task GetDuraExpressOrderNew(OrderStatusType status)
        {
            if (IsLoadingIndicator)
                return;
            ShowLoading();
            IsLoadingIndicator = true;
            if (CheckConnection())
            {
                try
                {
                    int id = (int)status;
                    DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                    lstOrderList = new List<MyOrderResponse>();
                    ItemTreshold = 1;
                    PageNo = 1;
                    lstOrderList.Clear();
                    DuraExpressOrderList.Clear();
                    var form = new MultipartFormDataContent();
                    form.Add(new StringContent(Convert.ToString(SettingsExtension.UserId)), "user_id");
                    form.Add(new StringContent(Convert.ToString(id)), "status");
                    form.Add(new StringContent(Convert.ToString(PageNo)), "page_id");
                    form.Add(new StringContent(Convert.ToString(4)), "totalcount");
                    var result = await TryWithErrorAsync(_userCoreService.GetMyOrder(form, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            if (result?.Data?.data.Count > 0)
                            {
                                IsOnLoad = false;
                                foreach (var orderData in result?.Data?.data)
                                {
                                    DuraExpressOrderList.Add(orderData);
                                }
                                isSamePageNumber = true;
                            }
                        }
                    }
                    else if (result?.ResultType == ResultType.Unauthorized)
                    {
                        IsEmpty = true;
                        IsNoRecord = false;
                        DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                        lstOrderList = new List<MyOrderResponse>();
                        await LogoutHelper.LogoutOnTokenExpire(AppResources.Token_expired);
                    }
                    else
                    {
                        IsOnLoad = false;
                        IsEmpty = true;
                        IsNoRecord = false;
                        DuraExpressOrderList = new ObservableCollection<MyOrderResponse>(lstOrderList);
                        lstOrderList = new List<MyOrderResponse>();
                        ShowToast(result?.Data?.message);
                    }
                }
                catch (Exception ex)
                {
                    IsOnLoad = false;
                    IsEmpty = true;
                }
                finally
                {
                    IsLoadingIndicator = false;
                    HideLoading();
                }
            }
            else
            {
                IsOnLoad = false;
                HideLoading();
                ShowToast(AppResources.NoInternet);
            }
        }
    }
}
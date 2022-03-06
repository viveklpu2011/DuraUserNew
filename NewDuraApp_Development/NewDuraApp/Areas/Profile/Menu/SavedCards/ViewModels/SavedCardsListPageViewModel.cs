using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using NewDuraApp.Helpers;
using NewDuraApp.Models;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels
{
    public class SavedCardsListPageViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        private IUserCoreService _userCoreService;
        public IAsyncCommand<CardListModel> CardsDetails { get; set; }
        public IAsyncCommand<CardListModel> DeleteCardCommand { get; set; }
        public IAsyncCommand NavigateToAddNewCard { get; set; }
        private double _defaultCollectionVieHeight = 100;
        private double _collectionViewHeight = 0;

        private ObservableCollection<CardListModel> _savedCardsModelList;
        public ObservableCollection<CardListModel> SavedCardsModelList
        {
            get { return _savedCardsModelList; }
            set { _savedCardsModelList = value; OnPropertyChanged(nameof(SavedCardsModelList)); }
        }
        public double CollectionViewHeight
        {
            get { return _collectionViewHeight; }
            set { _collectionViewHeight = value; OnPropertyChanged(nameof(CollectionViewHeight)); }
        }

        private CardListModel _cardData;
        public CardListModel CardData
        {
            get { return _cardData; }
            set
            {
                _cardData = value;
                OnPropertyChanged(nameof(CardData));
            }
        }

        public SavedCardsListPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
        {
            _navigationService = navigationService;
            _userCoreService = userCoreService;
            CardsDetails = new AsyncCommand<CardListModel>(NavigateToCardDetailsPage, allowsMultipleExecutions: false);
            NavigateToAddNewCard = new AsyncCommand(NavigateToAddNewCardPage, allowsMultipleExecutions: false);
            DeleteCardCommand = new AsyncCommand<CardListModel>(DeleteCardCommandExecute, allowsMultipleExecutions: false);
        }

        private async Task DeleteCardCommandExecute(CardListModel arg)
        {
            if (arg != null)
            {
                if (CheckConnection())
                {
                    ShowLoading();
                    try
                    {
                        DeleteCardRequestModel deleteCardRequestModel = new DeleteCardRequestModel()
                        {
                            user_id = SettingsExtension.UserId,
                            id = arg.id
                        };
                        var result = await TryWithErrorAsync(_userCoreService.DeleteCard(deleteCardRequestModel, SettingsExtension.Token), true, false);
                        if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                        {
                            SavedCardsModelList.RemoveAt(SavedCardsModelList.IndexOf(arg));
                            ShowToast(AppResources.Card_Removed);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    HideLoading();
                }
                else
                    ShowToast(AppResources.NoInternet);
            }
        }

        private async Task NavigateToCardDetailsPage(CardListModel arg)
        {
            if (arg != null)
            {
                CardData = arg;
                if (_navigationService.GetCurrentPageViewModel() != typeof(AddNewCardPageViewModel))
                {
                    await _navigationService.NavigateToAsync<AddNewCardPageViewModel>();
                    await App.Locator.AddNewCardPage.InitilizeData(CardData);
                }
            }
        }

        private async Task NavigateToAddNewCardPage()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(AddNewCardPageViewModel))
            {
                await _navigationService.NavigateToAsync<AddNewCardPageViewModel>();
                await App.Locator.AddNewCardPage.InitilizeData();
            }
        }

        public async Task InitilizeData()
        {
            if (CheckConnection())
            {
                ShowLoading();
                try
                {
                    CommonUserIdRequestModel commonUserIdRequestModel = new CommonUserIdRequestModel()
                    {
                        user_id = SettingsExtension.UserId,
                    };
                    var result = await TryWithErrorAsync(_userCoreService.GetCard(commonUserIdRequestModel, SettingsExtension.Token), true, false);
                    if (result?.ResultType == ResultType.Ok && result.Data.status == 200)
                    {
                        if (result?.Data?.data != null && result?.Data?.data.Count > 0)
                        {
                            SavedCardsModelList = new ObservableCollection<CardListModel>();
                            foreach (var item in result?.Data?.data)
                            {
                                SavedCardsModelList.Add(item);
                            }
                            CollectionViewHeight = _defaultCollectionVieHeight * SavedCardsModelList.Count;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                HideLoading();
            }
            else
                ShowToast(AppResources.NoInternet);
        }
    }
}

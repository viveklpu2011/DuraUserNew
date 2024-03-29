﻿using System.Threading.Tasks;
using NewDuraApp.Areas.DuraShop.ViewModel;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;
using Xamarin.CommunityToolkit.ObjectModel;

namespace NewDuraApp.Areas.Common.ViewModels
{
    public class SearchScreenViewModel : AppBaseViewModel
    {
        INavigationService _navigationService;
        public IAsyncCommand GoToDuraShop { get; set; }
        public SearchScreenViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToDuraShop = new AsyncCommand(GoToDuraShopExecute, allowsMultipleExecutions: false);
        }

        private async Task GoToDuraShopExecute()
        {
            if (_navigationService.GetCurrentPageViewModel() != typeof(DuraShopViewModel))
            {
                await _navigationService.NavigateToAsync<DuraShopViewModel>();
            }
        }
    }
}

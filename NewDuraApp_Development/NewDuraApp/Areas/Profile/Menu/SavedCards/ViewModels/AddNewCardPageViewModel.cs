using System;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Services.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using NewDuraApp.Resources;
using NewDuraApp.Services.Interfaces;
using NewDuraApp.ViewModels;

namespace NewDuraApp.Areas.Profile.Menu.SavedCards.ViewModels
{
	public class AddNewCardPageViewModel : AppBaseViewModel
	{
		public IAsyncCommand AddUpdateSaveCardCommand { get; set; }
		private IUserCoreService _userCoreService;
		INavigationService _navigationService;
		private CardListModel _cardData;
		public CardListModel CardData {
			get { return _cardData; }
			set {
				_cardData = value;
				OnPropertyChanged(nameof(CardData));
			}
		}
		private string _headerText =AppResources.Add_New_Card;
		public string HeaderText {
			get { return _headerText; }
			set {
				_headerText = value;
				OnPropertyChanged(nameof(HeaderText));
			}
		}

		private string _buttonText =AppResources.Add;
		public string ButtonText {
			get { return _buttonText; }
			set {
				_buttonText = value;
				OnPropertyChanged(nameof(ButtonText));
			}
		}

		private string _cardHolderName;
		public string CardHolderName {
			get { return _cardHolderName; }
			set {
				_cardHolderName = value;
				if (!string.IsNullOrEmpty(_cardHolderName)) {
					if (RegexUtilities.AlphanumericNameValidation(_cardHolderName)) {
						IsCardHoilderNameErrorVisible = false;
					} else {
						IsCardHoilderNameErrorVisible = true;
					}

				} else {
					IsCardHoilderNameErrorVisible = false;
				}
				CheckCardValidation();
				OnPropertyChanged(nameof(CardHolderName));
			}
		}

		private string _expiryDate;
		public string ExpiryDate {
			get { return _expiryDate; }
			set {
				_expiryDate = value;
				if (!string.IsNullOrEmpty(_expiryDate)) {
					if (RegexUtilities.EmptyString(_expiryDate)) {
						IsExpiryDateErrorVisible = false;
					} else {
						IsExpiryDateErrorVisible = true;
					}

				} else {
					IsExpiryDateErrorVisible = false;
				}
				CheckCardValidation();
				OnPropertyChanged(nameof(ExpiryDate));
			}
		}

		private string _cardNumber;
		public string CardNumber {
			get { return _cardNumber; }
			set {
				_cardNumber = value;
				if (!string.IsNullOrEmpty(_cardNumber)) {
					if (RegexUtilities.EmptyString(_cardNumber) && _cardNumber.Length >= 13) {
						IsCardNumberErrorVisible = false;
					} else {
						IsCardNumberErrorVisible = true;
					}
				} else {
					IsCardNumberErrorVisible = false;
				}

				CheckCardValidation();
				OnPropertyChanged(nameof(CardNumber));
			}
		}

		private string _securityCode;
		public string SecurityCode {
			get { return _securityCode; }
			set {
				_securityCode = value;
				if (!string.IsNullOrEmpty(_securityCode)) {
					if (RegexUtilities.EmptyString(_securityCode)) {
						IsSecurityCodeErrorVisible = false;
					} else {
						IsSecurityCodeErrorVisible = true;
					}
				} else {
					IsSecurityCodeErrorVisible = false;
				}

				CheckCardValidation();
				OnPropertyChanged(nameof(SecurityCode));
			}
		}

		private bool _isCardHolderNameErrorVisible;
		public bool IsCardHoilderNameErrorVisible {
			get { return _isCardHolderNameErrorVisible; }
			set {
				_isCardHolderNameErrorVisible = value;
				OnPropertyChanged(nameof(IsCardHoilderNameErrorVisible));
			}
		}

		private bool _isCardNumberErrorVisible;
		public bool IsCardNumberErrorVisible {
			get { return _isCardNumberErrorVisible; }
			set {
				_isCardNumberErrorVisible = value;
				OnPropertyChanged(nameof(IsCardNumberErrorVisible));
			}
		}

		private bool _isExpiryDateErrorVisible;
		public bool IsExpiryDateErrorVisible {
			get { return _isExpiryDateErrorVisible; }
			set {
				_isExpiryDateErrorVisible = value;
				OnPropertyChanged(nameof(IsExpiryDateErrorVisible));
			}
		}

		private bool _isSecurityCodeErrorVisible;
		public bool IsSecurityCodeErrorVisible {
			get { return _isSecurityCodeErrorVisible; }
			set {
				_isSecurityCodeErrorVisible = value;
				OnPropertyChanged(nameof(IsSecurityCodeErrorVisible));
			}
		}

		private bool _isButtonEnabled;
		public bool IsButtonEnabled {
			get { return _isButtonEnabled; }
			set {
				_isButtonEnabled = value;
				OnPropertyChanged(nameof(IsButtonEnabled));
			}
		}

		public AddNewCardPageViewModel(INavigationService navigationService, IUserCoreService userCoreService)
		{
			_navigationService = navigationService;
			_userCoreService = userCoreService;
			AddUpdateSaveCardCommand = new AsyncCommand(AddUpdateSaveCardCommandExecute);
		}

		private async Task AddUpdateSaveCardCommandExecute()
		{
			if (ButtonText == "Update") {
				if (CheckConnection()) {
					ShowLoading();
					try {
						UpdateCardRequestModel updateCardRequestModel = new UpdateCardRequestModel() {
							user_id = SettingsExtension.UserId,
							expirydate = ExpiryDate,
							name = CardHolderName,
							number = CardNumber,
							securitycode = SecurityCode,
							id = CardData.id
						};
						var result = await TryWithErrorAsync(_userCoreService.UpdateCard(updateCardRequestModel, SettingsExtension.Token), true, false);
						if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
							await _navigationService.NavigateBackAsync();
							await App.Locator.SavedCardsListPage.InitilizeData();
							ShowToast(AppResources.Card_Updated);
						}
						//ShowAlert(result?.Data?.message, result?.Data?.message);
					} catch (Exception ex) {
						// ShowAlert(CommonMessages.ServerError);
					}
					HideLoading();
				} else
					ShowToast(AppResources.NoInternet);

			} else {
				if (CheckConnection()) {
					ShowLoading();
					try {
						AddCardRequestModel addCardRequestModel = new AddCardRequestModel() {
							user_id = SettingsExtension.UserId,
							expirydate = ExpiryDate,
							name = CardHolderName,
							number = CardNumber,
							securitycode = SecurityCode
						};
						var result = await TryWithErrorAsync(_userCoreService.AddCard(addCardRequestModel, SettingsExtension.Token), true, false);
						if (result?.ResultType == ResultType.Ok && result.Data.status == 200) {
							await _navigationService.NavigateBackAsync();
							await App.Locator.SavedCardsListPage.InitilizeData();
							ShowToast(AppResources.New_Card_Added);
						}
						//ShowAlert(result?.Data?.message, result?.Data?.message);
					} catch (Exception ex) {
						// ShowAlert(CommonMessages.ServerError);
					}
					HideLoading();
				} else
					ShowToast(AppResources.NoInternet);

			}
		}

		public async Task InitilizeData(CardListModel cardListModel = null)
		{
			if (cardListModel != null) {
				CardData = cardListModel;
				ButtonText = "Update";
				HeaderText = "Update Card";
				CardHolderName = cardListModel?.name;
				CardNumber = cardListModel?.number;
				ExpiryDate = cardListModel?.expirydate;
				SecurityCode = cardListModel?.securitycode;
			} else {
				ButtonText =AppResources.Add;
				HeaderText = AppResources.Add_New_Card;

				CardNumber = CardHolderName = SecurityCode = ExpiryDate = string.Empty;
				IsCardHoilderNameErrorVisible = IsCardNumberErrorVisible = IsExpiryDateErrorVisible = IsSecurityCodeErrorVisible = false;
				CheckCardValidation();
			}
		}

		private bool CheckCardValidation()
		{
			if (IsCardHoilderNameErrorVisible ||
				IsCardNumberErrorVisible ||
				IsExpiryDateErrorVisible ||
				IsSecurityCodeErrorVisible) {
				IsButtonEnabled = false;
				return false;
			} else if (string.IsNullOrEmpty(CardHolderName) ||
				  string.IsNullOrEmpty(CardNumber) ||
				  string.IsNullOrEmpty(SecurityCode) ||
				  string.IsNullOrEmpty(ExpiryDate)) {
				IsButtonEnabled = false;
				return false;
			} else {
				IsButtonEnabled = true;
				return true;
			}
		}
	}
}

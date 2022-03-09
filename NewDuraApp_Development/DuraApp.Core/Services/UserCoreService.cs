using System;
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Models.Result;
using DuraApp.Core.Services;
using DuraApp.Core.Services.Interfaces;
using Newtonsoft.Json;

namespace ShriekSecurity.Core.Services
{
    public class UserCoreService : IUserCoreService
    {
        private readonly HttpService _httpService;
        public UserCoreService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Result<CommonResponseModel>> AddAddress(AddAddressRequestModel request, string token)
        {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(Convert.ToString(request.user_id)), "user_id");
            form.Add(new StringContent(Convert.ToString(request.address1)), "address1");
            form.Add(new StringContent(Convert.ToString(request.address2)), "address2");
            form.Add(new StringContent(Convert.ToString(request.contactperson)), "contactperson");
            form.Add(new StringContent(Convert.ToString(request.mobile)), "mobile");
            form.Add(new StringContent(Convert.ToString(request.isdefault)), "isdefault");
            form.Add(new StringContent(Convert.ToString(request.latitude)), "latitude");
            form.Add(new StringContent(Convert.ToString(request.longitude)), "longitude");
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.AddAddress, form, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> AddCard(AddCardRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.AddCard, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<PickupScheduleResponseModel>> AddMoreDetailsPickup(MultipartFormDataContent request, string token)
        {
            var response = await _httpService.PostJsonAsync<PickupScheduleResponseModel>(Urls.BASE_URL + Urls.SAVE_ADD_MORE_DETAILS, request, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> DeleteAddress(DeleteAddressRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.DeleteAddress, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> DeleteCard(DeleteCardRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.DeleteCard, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<DeleteDocumentResponseModel>> DeleteDocument(DeleteDocumentRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<DeleteDocumentResponseModel>(Urls.BASE_URL + Urls.DeleteDocument, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<FindDriverResponseModel>> FindDriver(FindDriverRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<FindDriverResponseModel>(Urls.BASE_URL + Urls.Find_Driver, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetAddressResponseModel>> GetAddress(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetAddressResponseModel>(Urls.BASE_URL + Urls.GetAddress, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CardListResponseModel>> GetCard(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CardListResponseModel>(Urls.BASE_URL + Urls.GetCard, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetDriverDetailsResponseModel>> GetDriverDetails(GetDriverDetailsRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetDriverDetailsResponseModel>(Urls.BASE_URL + Urls.GetDriverDetails, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetOrderDetailsResponseModel>> GetOrderDetails(OrderDetailsRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetOrderDetailsResponseModel>(Urls.BASE_URL + Urls.GetOrderDetails, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetUserPersonalDocsResponseModel>> GetPersonalDocument(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetUserPersonalDocsResponseModel>(Urls.BASE_URL + Urls.GetPersonalDocuments, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetUserBusinessDocsResponseModel>> GetBusinessDocument(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetUserBusinessDocsResponseModel>(Urls.BASE_URL + Urls.GetBusinessDocuments, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetpickupDetailsResponseModel>> GetPickupDetails(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetpickupDetailsResponseModel>(Urls.BASE_URL + Urls.GetPickupDetails, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            Console.Write($"Response is :{response}");
            return response;
        }

        public async Task<Result<GetProfileDetailsResponseModel>> GetProfileDetails(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetProfileDetailsResponseModel>(Urls.BASE_URL + Urls.GetProfileDetails, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<PromoCodeListResponse>> GetPromoCodeList(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<PromoCodeListResponse>(Urls.BASE_URL + Urls.PROMO_LIST, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<VehicleListResponseModel>> GetVehicleList(VehicleListRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<VehicleListResponseModel>(Urls.BASE_URL + Urls.GET_VEHICLE_LIST, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<WalletAmountResponseModel>> GetWalletAmountList(string token)
        {
            var response = await _httpService.GetJsonAsync<WalletAmountResponseModel>(Urls.BASE_URL + Urls.GET_WALLET_AMOUNT_LIST, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> PaymentMode(PaymentModeRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.PaymentMode, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<PickupScheduleResponseModel>> PickupSchedule(PickupScheduleReqModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<PickupScheduleResponseModel>(Urls.BASE_URL + Urls.SCHEDULE_PICKUP, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<ReferCodeResponseModel>> ReferCode(ReferCodeRequestModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<ReferCodeResponseModel>(Urls.BASE_URL + Urls.ReferCode, jsonRequest);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetSaveWalletAmountResponseModel>> SaveWalletAmount(GetSaveWalletAmountRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetSaveWalletAmountResponseModel>(Urls.BASE_URL + Urls.Get_Save_Wallet_Amount, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<TipPriceResponseModel>> TipPrice(string token)
        {
            var response = await _httpService.GetJsonAsync<TipPriceResponseModel>(Urls.BASE_URL + Urls.Tip_Price, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdateAddress(UpdateAddressRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateAddress, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdateCard(UpdateCardRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateCard, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdateSocialLink(UpdateSocialLinkRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateSocialLink, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdateUserName(MultipartFormDataContent request, string token)
        {
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateUserName, request, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UploadDocument(MultipartFormDataContent request, string token)
        {
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UploadDocument, request, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<VerifyPromoCodeResponseModel>> VerifyPromoCode(VerifyPromoCodeRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<VerifyPromoCodeResponseModel>(Urls.BASE_URL + Urls.VERIFY_PROMO, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> ChangePassword(ChangePasswordRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UPDATE_USER_PASSWORD, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<MyOrderResponseModel>> GetMyOrder(MultipartFormDataContent request, string token)
        {
            //var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<MyOrderResponseModel>(Urls.BASE_URL + Urls.GetMyOrders, request, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<NotificatonResponseModel>> GetNotification(NotificatonRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<NotificatonResponseModel>(Urls.BASE_URL + Urls.GetNotification, jsonRequest, token);
            return response;
        }

        public async Task<Result<GetWalletAmountResponseModel>> GetWallet(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetWalletAmountResponseModel>(Urls.BASE_URL + Urls.GetWalletHistory, jsonRequest, token);

            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> ReadNotification(ReadNotificationRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.ReadNotification, jsonRequest, token);
            return response;
        }

        public async Task<Result<CommonResponseModel>> CancelRide(CancelRideRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.CancelRide, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdateUserCountry(MultipartFormDataContent request, string token)
        {
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateCountry, request, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> BeARider(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.BeARider, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UpdatePhone(UpdatePhoneRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdatePhone, jsonRequest, token);

            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> SubmitRating(SubmitRatingRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.PostRating, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GetRatingResponseModel>> GetUserRating(CommonUserIdRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<GetRatingResponseModel>(Urls.BASE_URL + Urls.GetRating, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> SubmitReferralCode(ReferralCodeRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.SubmitReferral, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<GoogleTimeResponseModel>> GetGoogleDirectionTime(string source, string destination)
        {
            var url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + source + "&destinations=" + destination + "&key=" + AppConstant.GooglePlaceKey + "";
            var response = await _httpService.GetJsonAsync<GoogleTimeResponseModel>(url);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> VerifyPhoneNumber(VerifyPhoneRequestModel request, string token)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<CommonResponseModel>(Urls.BASE_URL + Urls.UpdateVerifiedPhone, jsonRequest, token);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }
    }
}
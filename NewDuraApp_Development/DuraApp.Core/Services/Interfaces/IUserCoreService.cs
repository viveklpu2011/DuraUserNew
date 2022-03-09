using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.RequestModels;
using DuraApp.Core.Models.ResponseModels;
using DuraApp.Core.Models.Result;

namespace DuraApp.Core.Services.Interfaces
{
    public interface IUserCoreService
    {
        Task<Result<PromoCodeListResponse>> GetPromoCodeList(CommonUserIdRequestModel request, string token);
        Task<Result<VerifyPromoCodeResponseModel>> VerifyPromoCode(VerifyPromoCodeRequestModel request, string token);
        Task<Result<PickupScheduleResponseModel>> PickupSchedule(PickupScheduleReqModel request, string token);
        Task<Result<VehicleListResponseModel>> GetVehicleList(VehicleListRequestModel request, string token);
        Task<Result<PickupScheduleResponseModel>> AddMoreDetailsPickup(MultipartFormDataContent request, string token);
        Task<Result<WalletAmountResponseModel>> GetWalletAmountList(string token);
        Task<Result<GetSaveWalletAmountResponseModel>> SaveWalletAmount(GetSaveWalletAmountRequestModel request, string token);
        Task<Result<FindDriverResponseModel>> FindDriver(FindDriverRequestModel request, string token);
        Task<Result<TipPriceResponseModel>> TipPrice(string token);
        Task<Result<CommonResponseModel>> PaymentMode(PaymentModeRequestModel request, string token);
        Task<Result<GetDriverDetailsResponseModel>> GetDriverDetails(GetDriverDetailsRequestModel request, string token);
        Task<Result<GetpickupDetailsResponseModel>> GetPickupDetails(CommonUserIdRequestModel request, string token);
        Task<Result<GetOrderDetailsResponseModel>> GetOrderDetails(OrderDetailsRequestModel request, string token);
        Task<Result<GetProfileDetailsResponseModel>> GetProfileDetails(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdateSocialLink(UpdateSocialLinkRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdateUserName(MultipartFormDataContent request, string token);
        Task<Result<CommonResponseModel>> AddAddress(AddAddressRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdateAddress(UpdateAddressRequestModel request, string token);
        Task<Result<CommonResponseModel>> DeleteAddress(DeleteAddressRequestModel request, string token);
        Task<Result<GetAddressResponseModel>> GetAddress(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> AddCard(AddCardRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdateCard(UpdateCardRequestModel request, string token);
        Task<Result<CommonResponseModel>> DeleteCard(DeleteCardRequestModel request, string token);
        Task<Result<CardListResponseModel>> GetCard(CommonUserIdRequestModel request, string token);
        Task<Result<ReferCodeResponseModel>> ReferCode(ReferCodeRequestModel request);
        Task<Result<CommonResponseModel>> UploadDocument(MultipartFormDataContent request, string token);
        Task<Result<GetUserPersonalDocsResponseModel>> GetPersonalDocument(CommonUserIdRequestModel request, string token);
        Task<Result<DeleteDocumentResponseModel>> DeleteDocument(DeleteDocumentRequestModel request, string token);
        Task<Result<GetUserBusinessDocsResponseModel>> GetBusinessDocument(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> ChangePassword(ChangePasswordRequestModel request, string token);
        Task<Result<MyOrderResponseModel>> GetMyOrder(MultipartFormDataContent request, string token);
        Task<Result<NotificatonResponseModel>> GetNotification(NotificatonRequestModel request, string token);
        Task<Result<CommonResponseModel>> ReadNotification(ReadNotificationRequestModel request, string token);
        Task<Result<GetWalletAmountResponseModel>> GetWallet(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> CancelRide(CancelRideRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdateUserCountry(MultipartFormDataContent request, string token);
        Task<Result<CommonResponseModel>> BeARider(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> UpdatePhone(UpdatePhoneRequestModel request, string token);
        Task<Result<CommonResponseModel>> SubmitRating(SubmitRatingRequestModel request, string token);
        Task<Result<GetRatingResponseModel>> GetUserRating(CommonUserIdRequestModel request, string token);
        Task<Result<CommonResponseModel>> SubmitReferralCode(ReferralCodeRequestModel request, string token);
        Task<Result<GoogleTimeResponseModel>> GetGoogleDirectionTime(string source, string destination);
        Task<Result<CommonResponseModel>> VerifyPhoneNumber(VerifyPhoneRequestModel request, string token);
    }
}
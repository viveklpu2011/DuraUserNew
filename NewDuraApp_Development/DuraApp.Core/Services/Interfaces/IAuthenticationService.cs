
using System.Net.Http;
using System.Threading.Tasks;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.Result;

namespace DuraApp.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AllLocationResponseModel>> GetAllLocations();
        Task<Result<GetAllLocationResponseModel>> GetAllLocationsNew();
        Task<Result<OTPResponseModel>> SendOTP(OTPRequestModel request);
        Task<Result<VerifyOTPResponseModel>> VerifyOTP(VerifyOTPRequestModel request);
        Task<Result<CommonResponseModel>> UserSignup(MultipartFormDataContent request);
        Task<Result<LoginResponseModel>> Login(LoginRequestModel request);
        Task<Result<ResetPasswordResponseModel>> ResetPassword(ResetPasswordRequestModel request);
        Task<Result<FacebookResponseModel>> FbLogin(MultipartFormDataContent request);
    }
}

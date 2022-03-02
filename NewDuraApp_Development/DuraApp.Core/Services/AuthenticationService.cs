using DuraApp.Core.Helpers;
using DuraApp.Core.Helpers.Enums;
using DuraApp.Core.Models.Auth.RequestModels;
using DuraApp.Core.Models.Auth.ResponseModel;
using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.Google;
using DuraApp.Core.Models.Result;
using DuraApp.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DuraApp.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpService _httpService;
        public AuthenticationService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Result<AllLocationResponseModel>> GetAllLocations()
        {
            var response = await _httpService.GetJsonAsync<AllLocationResponseModel>(Urls.BASE_URL + Urls.GET_ALL_LOCATION);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }
        public async Task<Result<GoogleLocation>> GetAddressList(string lat, string log, string mapkey)
        {
            var response = await _httpService.GetJsonAsync<GoogleLocation>("https://maps.googleapis.com/maps/api/geocode/json?latlng="+lat+","+log+ "&key="+mapkey);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }
        public async Task<Result<GetAllLocationResponseModel>> GetAllLocationsNew()
        {
            var response = await _httpService.GetJsonAsync<GetAllLocationResponseModel>(Urls.BASE_URL + Urls.GET_ALL_LOCATION);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }
        public async Task<Result<LoginResponseModel>> Login(LoginRequestModel request)
        {
            //var jsonRequest = JsonConvert.SerializeObject(request);

            var form = new MultipartFormDataContent();
            form.Add(new StringContent(Convert.ToString(request.email)), "email");
            form.Add(new StringContent(Convert.ToString(request.password)), "password");
            form.Add(new StringContent(Convert.ToString(request.phone)), "phone");
            form.Add(new StringContent(Convert.ToString(request.country_code)), "country_code");
            //form.Add(new StringContent(Convert.ToString(request.country_id)), "country_id");
            form.Add(new StringContent(Convert.ToString(request.device_id)), "device_id");
            form.Add(new StringContent(Convert.ToString(request.device_type)), "device_type");

            var response = await _httpService.PostJsonAsync<LoginResponseModel>(Urls.BASE_URL + Urls.LOGIN, form);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<ResetPasswordResponseModel>> ResetPassword(ResetPasswordRequestModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<ResetPasswordResponseModel>(Urls.BASE_URL + Urls.RESET_PASSWORD, jsonRequest);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<OTPResponseModel>> SendOTP(OTPRequestModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<OTPResponseModel>(Urls.BASE_URL + Urls.SEND_OTP, jsonRequest);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<CommonResponseModel>> UserSignup(MultipartFormDataContent request)
        {
            var response = await _httpService.PostJsonAsyncWithoutToken<CommonResponseModel>(Urls.BASE_URL + Urls.REGISTER, request);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }


        public async Task<Result<FacebookResponseModel>> FbLogin(MultipartFormDataContent request)
        {
            var response = await _httpService.PostJsonAsyncWithoutToken<FacebookResponseModel>(Urls.BASE_URL + Urls.REGISTER, request);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        public async Task<Result<VerifyOTPResponseModel>> VerifyOTP(VerifyOTPRequestModel request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var response = await _httpService.PostJsonAsync<VerifyOTPResponseModel>(Urls.BASE_URL + Urls.VERIFY_OTP, jsonRequest);
            if (response?.ResultType == ResultType.Ok)
            {
                if (response?.Data != null)
                {
                    //App.Locator.CurrentUser.AppointmentData = response?.Data;
                }
            }
            return response;
        }

        //public async Task<Result<BaseApiResponseModel>> ForgotPassword(ForgotPasswordRequestModel request)
        //{
        //    var jsonRequest = JsonConvert.SerializeObject(request);
        //    var response = await _httpService.PostJsonAsync<BaseApiResponseModel>(Urls.BASE_URL + Urls.FORGOT_URL, jsonRequest);
        //    if (response?.ResultType == ResultType.Ok)
        //    {
        //        if (response?.Data != null)
        //        {
        //            //App.Locator.CurrentUser.AppointmentData = response?.Data;
        //        }
        //    }
        //    return response;
        //}

        //public async Task<Result<GetServiceLevelResponseModel>> GetAllServicesLevel()
        //{
        //    var response = await _httpService.GetJsonAsync<GetServiceLevelResponseModel>(Urls.BASE_URL + Urls.GET_SERVICE_LEVEL_URL);
        //    if (response?.ResultType == ResultType.Ok)
        //    {
        //        if (response?.Data != null)
        //        {
        //            //App.Locator.CurrentUser.AppointmentData = response?.Data;
        //        }
        //    }
        //    return response;
        //}

    }
}
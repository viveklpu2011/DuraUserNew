using DuraApp.Core.Models.Common;
using DuraApp.Core.Models.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DuraApp.Core.Services
{
    public class HttpService
    {
        protected readonly HttpClient _client;
        public HttpService()
        {
            //client.Timeout = new TimeSpan(0, 0, 60);
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            _client = new HttpClient(httpClientHandler);
        }
        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, string json)
        {
            await SetTokenAsync();
            //await SetHeaderAsync();
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);

        }
        public virtual async Task<Result<T>> PostJsonAsyncWithoutToken<T>(string url, MultipartFormDataContent content)
        {
            await SetTokenAsync();

            var result = await _client.PostAsync(url, content);
            return await HandleResponseAsync<T>(result);

        }
        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, string json, string token)
        {
            //await SetTokenAsync();
            await SetHeaderAsync(token);
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            Console.WriteLine($"Error Response is-------------------{result}");
            return await HandleResponseAsync<T>(result);

        }

        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, MultipartFormDataContent content, string token)
        {
            await SetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                await SetHeaderAsync(token);

            }
            var result = await _client.PostAsync(url, content);
            return await HandleResponseAsync<T>(result);

        }
        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, MultipartFormDataContent content)
        {
            await SetTokenAsync();
            var result = await _client.PostAsync(url, content);
            return await HandleResponseAsync<T>(result);

        }

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, Dictionary<string, string> dictionary)
        {
            await SetTokenAsync();
            HttpContent content = new FormUrlEncodedContent(dictionary);
            var result = await _client.PostAsync(url, content);
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostAnonymousJsonAsync<T>(string url, Dictionary<string, string> dictionary)
        {
            _client.DefaultRequestHeaders.Clear();
            HttpContent content = new FormUrlEncodedContent(dictionary);
            var result = await _client.PostAsync(url, content);
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostAnonymousJsonAsync<T>(string url, string json)
        {
            _client.DefaultRequestHeaders.Clear();
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP POST request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task PostJsonAsync(string url, string json)
        {
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, ByteArrayContent imageContent)
        {
            await SetTokenAsync();
            var result = await _client.PostAsync(url, imageContent);
            return await HandleResponseAsync<T>(result);

        }

        /// <summary>
        /// Makes an HTTP GET Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> GetJsonAsync<T>(string url)
        {
            //await SetTokenAsync();
            await SetHeaderAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.GetAsync(url);
            return await HandleResponseAsync<T>(result);
        }
        public virtual async Task<Result<T>> GetJsonAsync<T>(string url, string token)
        {
            //await SetTokenAsync();
            await SetHeaderAsync(token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.GetAsync(url);
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP GET Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> GetAnonymousJsonAsync<T>(string url)
        {
            _client.DefaultRequestHeaders.Clear();
            await SetHeaderAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.GetAsync(url);
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP PUT request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PutJsonAsync<T>(string url, string json)
        {
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP PUT request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task PutJsonAsync(string url, string json)
        {
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public virtual async Task<Result<T>> SendJsonAsync<T>(HttpRequestMessage httpRequestMessage)
        {
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.SendAsync(httpRequestMessage);
            return await HandleResponseAsync<T>(result);
        }

        /// <summary>
        /// Makes an HTTP DELETE request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> DeleteAsync<T>(string url)
        {
            await SetTokenAsync();
            var result = await _client.DeleteAsync(url);
            return await HandleResponseAsync<T>(result);

        }
        private async Task SetHeaderAsync(string token)
        {
            _client.DefaultRequestHeaders.Clear();
            //  _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "ck_2a86b3ecd81d79007dd998da3f97f5e08e11cec1", "cs_4175240a3f37a75796b20327b63ae5119d2e2fdf"))));
            // _client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer ", token);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            //_client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("ck_2a86b3ecd81d79007dd998da3f97f5e08e11cec1", "cs_4175240a3f37a75796b20327b63ae5119d2e2fdf");
        }
        /// <summary>
        /// Handles the response from an HTTP request and parses the data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="useSessionExpirationMessage">
        /// Used to override the default session expired result. This is used for working around
        /// that authentication end points return Unauthorized for wrong password (should be BadRequest), and that the token doesn't have any expiration info,
        /// so the only way we know it expires is through an unauthorized response.
        /// </param>
        /// <returns></returns>
        protected virtual async Task<Result<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            // get the content of the response and handle mapping the json data to the given type.
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Api Response : {content}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture });
                    return new SuccessResult<T>(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return new UnexpectedResult<T>();
                }
            }
            else
            {
                // if unauthorized, pass that data back to sign the user out. This should eventually be handled by the base view model request handling
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //var errorData = JsonConvert.DeserializeObject<BaseResponse>(content);
                    return new UnauthorizedResult<T>("Please recheck your email/password , not matching with duradrive database.");
                }

                // error, but no content? wing it.
                if (string.IsNullOrEmpty(content))
                {
                    return new UnexpectedResult<T>();
                }

                // otherwise, start mapping errors.
                try
                {
                    // try to use the full error
                    var errorData = JsonConvert.DeserializeObject<BaseAPIResponseEntity>(content);
                    if (errorData?.ModelState != null)
                    {
                        return new InvalidResult<T>(String.Join(System.Environment.NewLine, errorData.ModelState.SelectMany(kv => kv.Value)) ?? errorData.error_description ?? errorData.Message ?? string.Empty);
                    }
                    return new InvalidResult<T>(errorData.error_description ?? errorData.Message ?? string.Empty);
                }
                catch
                {
                    Console.WriteLine(content);
                    return new UnexpectedResult<T>();
                }
            }
        }
        private async Task SetTokenAsync()
        {
            _client.DefaultRequestHeaders.Clear();
            try
            {
                //var LoggedUserToken = SettingsExtension.AccessToken;
                //if (!string.IsNullOrEmpty(LoggedUserToken.access_token))
                //{
                //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(LoggedUserToken.access_type, LoggedUserToken.access_token);
                //}
            }
            catch
            {

            }
        }
        private async Task SetHeaderAsync()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "ck_2a86b3ecd81d79007dd998da3f97f5e08e11cec1", "cs_4175240a3f37a75796b20327b63ae5119d2e2fdf"))));

            //_client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("ck_2a86b3ecd81d79007dd998da3f97f5e08e11cec1", "cs_4175240a3f37a75796b20327b63ae5119d2e2fdf");
        }
        private void SetDefaultHeader()
        {
            _client.DefaultRequestHeaders.Clear();
            //_client.DefaultRequestHeaders.Add("API_KEY", "AnyPrivateKey"); // This will be need if we have some private api key
        }
    }
}

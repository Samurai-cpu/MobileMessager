using CG.Models;
using CG.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CG.Services
{

    public static class JsonUtils
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToJson<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }

    //singleton


    // использовать как синглтон
    public class AuthRequest
    {
        private HttpClient httpClient;
        private UserProvider userProvider;

        public AuthRequest ()
        {
            httpClient = HttpClientProvider.GetHttpClient();
           
        }

        private async Task<HttpResponseMessage> MakeSimpleHttpRequestAsync(string uri, HttpMethod httpMethod, object body)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = new Uri("BaseUrl" + uri);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                var jsonBody = new StringContent(json, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = jsonBody;
            }

            var result = await httpClient.SendAsync(httpRequestMessage);

            return result;
        }

        public async Task<T> MakeHttpRequestAsync<T>(string url, HttpMethod httpMethod, object body = null, int countUnauthorizedRequest = 0)
        {
            var httpResponseMessage = await MakeSimpleHttpRequestAsync(url, httpMethod, body);
         

            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (countUnauthorizedRequest < 3)
                {
                   // var authResult = await GetAuthTokenAsync();
                    return await MakeHttpRequestAsync<T>(url, httpMethod, body, ++countUnauthorizedRequest);
                }
                throw new Exception("User is unauthorized");
            }
            if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                if (countUnauthorizedRequest < 3)
                {
                  //  var authResult = await GetAuthTokenAsync(true);
                    return await MakeHttpRequestAsync<T>(url, httpMethod, body, ++countUnauthorizedRequest);
                }
            }

            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(response);
        }

        private async Task<HttpResponseMessage> MakeSimpleFormHttpRequestAsync(string url, Dictionary<string, string> values)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;

            var form = new MultipartFormDataContent();

            foreach (KeyValuePair<string, string> value in values)
                form.Add(new StringContent(value.Value), value.Key);

            httpRequestMessage.RequestUri = new Uri(httpClient.BaseAddress + url);
            httpRequestMessage.Content = form;

            var response = await httpClient.SendAsync(httpRequestMessage);
            return response;
        }

        public async Task<T> MakeFormHttpRequestAsync<T>(string url, Dictionary<string, string> values, int countUnauthorizedRequest = 0)
        {
            var httpResponseMessage = await MakeSimpleFormHttpRequestAsync(url, values);
            //await HttpErrorHandle(httpResponseMessage);
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (countUnauthorizedRequest < 3)
                {
                    //var token = await GetAuthTokenAsync();

                    return await MakeFormHttpRequestAsync<T>(url, values);
                }

                throw new Exception("User is anautorized");
            }

            var stream = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(stream);

            return result;
        }

        
        /// <summary>
        /// при запуске приложения
        /// раз в 30 минут
        /// </summary>
        /// <returns></returns>
        public async Task<AuthResult> MakeAuthTokenAsync()
        {
            var userProvider = new UserProvider("");
            
            // берешь acesstoken и проверяешь валиден ли он отправляя гет по ссылке 

            string refresh = userProvider.GetItem().RefreshToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refresh);
            var authResponseMessage = await httpClient.GetAsync(Config.ApiUrl+ Config.AuthUserInfoUrl);
            if (authResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var authStream = await authResponseMessage.Content.ReadAsStringAsync();
                var authInfo = JsonConvert.DeserializeObject<AuthUserInfo>(authStream);

                return new AuthResult()
                {
                    //Заменить после того как перемстишь дырявого юзера
                    AccessToken = userProvider.GetItem().AccessToken,
                    RefreshToken = refresh,
                    Roles = authInfo.Roles,
                    UserName = authInfo.Login

                };
            }
            var jsonObj = new
            {
                refreshToken = refresh
            };

            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Post;
            httpRequest.RequestUri = new Uri(Config.ApiUrl+Config.UpdateRefreshTokenUrl);

            var json = JsonConvert.SerializeObject(jsonObj);
            var jsonBody = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequest.Content = jsonBody;


            var updateResult = await httpClient.SendAsync(httpRequest);
            if (updateResult.StatusCode == HttpStatusCode.OK)
            {
                var res = JsonConvert.DeserializeObject<AuthResult>(updateResult.Content.ToJson());

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.RefreshToken);
                var userToUpadate = userProvider.GetItem();

                userToUpadate.RefreshToken = res.RefreshToken;
                userToUpadate.AccessToken = res.AccessToken;

                userProvider.UpdateItem(userToUpadate.Id);

            }
            else if (updateResult.StatusCode == HttpStatusCode.Unauthorized)
            {
               // redirect
            }
            else
            {
                //handler errosr
            }


            return new AuthResult();
 
        }
    }

    public class RegistrationService
    {
        private readonly AuthRequest authRequest = new AuthRequest();

        public async Task RegisterAsync(string userName, string phone, string password, string email)
        {
            var objToSend = new
            {
                isOnline = true,
                isApiWork = "false",
                isQuery = 123
            };

           
        }
    }
}

using CG.Dal;
using CG.Models;
using CG.Providers;
using CG.Providers.Base;
using CG.Services.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class AuthRequest : BaseApiCall
    {

        private readonly IRsaCryptService rsaCryptService;
        private readonly IAesCipher aes;
        private HttpClient httpClient;
        private UserProvider userProvider;

        public AuthRequest()
        {
            httpClient = HttpClientProvider.GetHttpClient();
            rsaCryptService = new RsaCryptService();
            aes = new AesCryptService();
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
                    var authResult = await MakeAuthTokenAsync();
                    return await MakeHttpRequestAsync<T>(url, httpMethod, body, ++countUnauthorizedRequest);
                }
                throw new Exception("User is unauthorized");
            }
            if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                if (countUnauthorizedRequest < 3)
                {
                    var authResult = await MakeAuthTokenAsync(true);
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
                    var token = await MakeAuthTokenAsync();
                    return await MakeFormHttpRequestAsync<T>(url, values);
                }

                throw new Exception("User is anautorized");
            }

            var stream = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(stream);

            return result;
        }


        public async Task<AuthResult> MakeAuthTokenAsync(bool isNeedRefresh = false)
        {


            string refresh = ContextProvider.Database.GetItem().RefreshToken;

            if (!isNeedRefresh)
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refresh);
                var authResponseMessage = await httpClient.GetAsync(Config.ApiUrl + Config.AuthUserInfoUrl);
                if (authResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var authStream = await authResponseMessage.Content.ReadAsStringAsync();
                    var authInfo = JsonConvert.DeserializeObject<AuthUserInfo>(authStream);

                    return new AuthResult()
                    {
                        //Заменить после того как перемстишь дырявого юзера
                        AccessToken = ContextProvider.Database.GetItem().AccessToken,
                        RefreshToken = refresh,
                        Roles = authInfo.Roles,
                        UserName = authInfo.Login

                    };
                }
            }
            var jsonObj = new
            {
                refreshToken = refresh
            };

            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Post;
            httpRequest.RequestUri = new Uri(Config.ApiUrl + Config.UpdateRefreshTokenUrl);

            var json = JsonConvert.SerializeObject(jsonObj);
            var jsonBody = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequest.Content = jsonBody;


            var updateResult = await httpClient.SendAsync(httpRequest);
            if (updateResult.StatusCode == HttpStatusCode.OK)
            {
                var res = JsonConvert.DeserializeObject<AuthResult>(updateResult.Content.ToJson());

                httpClient = HttpClientProvider.RenewAcessToken(res.AccessToken);
                var userToUpadate = ContextProvider.Database.GetItem();

                userToUpadate.RefreshToken = res.RefreshToken;
                userToUpadate.AccessToken = res.AccessToken;

                ContextProvider.Database.UpdateItem(userToUpadate.Id);
            }


            return new AuthResult();
        }
        
        public async Task CreateSessionAsync()
        {

                var rsaPair = rsaCryptService.GenerateKeys();
                var strongKeys = ProviderFactory.StrongKeyProvider.GetItem();

                if (strongKeys == null)
                {
                    var firstSessionRequestModel = new CreateMessengerSessionRequest()
                    {
                        PublicKey = rsaPair.publicKey
                    };

                    string jsonRequest = firstSessionRequestModel.ToJson();

                    var firstSessionResponse = await GetStringFromHttpResultAsync(Config.CreateFirstSessionUrl, HttpMethod.Post, jsonRequest);
                    var response = JsonConvert.DeserializeObject<CreateFirstMessangerSessionResponse>(firstSessionResponse);

                    byte[] decryptedAesKey = rsaCryptService.Decrypt(rsaPair.privateKey, response.CryptedAes).FromUrlSafeBase64(); //на этом этапе имеем расшифрованый ключ aes для работы

                    ProviderFactory.StrongKeyProvider.SaveItem(new StrongKey()
                    {
                        Cypher = decryptedAesKey
                    });

                    await MakeAuthTokenAsync(true);

                    rsaPair = rsaCryptService.GenerateKeys();
                    string cryptedPublicKey = await aes.Crypt(rsaPair.publicKey);

                    var sessionRequestModel = new CreateMessengerSessionRequest()
                    {
                        PublicKey = cryptedPublicKey
                    };

                    jsonRequest = sessionRequestModel.ToJson();
                    var sessionRequest = BuildRequestMessage(Config.CreateSessionUrl, HttpMethod.Post, jsonRequest);

                    var sessionResponseMessage = await httpClient.SendAsync(sessionRequest);
                    sessionResponseMessage.EnsureSuccessStatusCode();

                    var sessionResponse = JsonConvert.DeserializeObject<CreateMessangerSessionResponse>(await sessionResponseMessage.Content.ReadAsStringAsync());

                    string decryptedServerPublicKey = await aes.Decrypt(sessionResponse.ServerPublicKey);
                    string decryptedSessionId = await aes.Decrypt(sessionResponse.SessionId);

                    ProviderFactory.SessionProvider.SaveItem(new Session()
                    {
                        ClientPublicKey = rsaPair.publicKey,
                        ClientPrivateKey = rsaPair.privateKey,
                        ServerPublicKey = decryptedServerPublicKey,
                        Created = DateTime.Now,
                        SessionId = decryptedSessionId
                    });
                }
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


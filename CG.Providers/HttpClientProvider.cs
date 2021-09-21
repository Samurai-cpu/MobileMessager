using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CG.Providers
{
    public class HttpClientProvider
    {
        private static HttpClient httpClient;

        public static HttpClient RenewAcessToken(string acessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", acessToken);
            return httpClient;
        }

        public static HttpClient GetHttpClient()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Config.ApiUrl);
            }
            
            return httpClient;
        }
    }
}

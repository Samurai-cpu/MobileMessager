using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CG.Providers
{
    public class HttpClientProvider
    {
        public static HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(Config.ApiUrl);
            var userProvider = new UserProvider("");

            string savedToken = userProvider.GetItem().AccessToken;

            if (!string.IsNullOrEmpty(savedToken))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            return httpClient;
        }
    }
}

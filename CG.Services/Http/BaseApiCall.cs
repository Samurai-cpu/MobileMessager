using CG.Providers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CG.Services.Http
{
	public class BaseApiCall
	{
		protected HttpClient httpClient;

		public BaseApiCall()
		{
			httpClient = HttpClientProvider.GetHttpClient();
			httpClient.BaseAddress = new Uri(Config.ApiUrl);
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

		}

		protected HttpRequestMessage BuildRequestMessage(string uri, HttpMethod method, string content = null)
		{
			var httpRequestMessage = new HttpRequestMessage();

			httpRequestMessage.RequestUri = new Uri(Config.ApiUrl + uri);
			httpRequestMessage.Method = method;

			if (!string.IsNullOrEmpty(content))
			{
				StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
				httpRequestMessage.Content = httpContent;
			}

			return httpRequestMessage;
		}


		protected async Task<string> GetStringFromHttpResultAsync(string uri, HttpMethod method, string content = null)
		{
			var request = BuildRequestMessage(uri, method, content);
			var response = await httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsStringAsync();

			return result;
		}
	}
}

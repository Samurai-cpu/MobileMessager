using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Models
{
    public class AuthResult
    {
		[JsonProperty("userName")]
		public string UserName { get; set; }
		[JsonProperty("roles")]
		public List<string> Roles { get; set; }
		[JsonProperty("accessToken")]
		public string AccessToken { get; set; }
		[JsonProperty("refreshToken")]
		public string RefreshToken { get; set; }
	}
}

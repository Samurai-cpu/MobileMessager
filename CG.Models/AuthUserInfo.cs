using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CG.Models
{
    public class AuthUserInfo
    {
		[JsonProperty("login")]
		public string Login { get; set; }
		[JsonProperty("roles")]
		public List<string> Roles { get; set; }
		[JsonProperty("isPhoneConfirmed")]
		public bool IsPhoneConfirmed { get; set; }
		[JsonProperty("isEmailConfirmed")]
		public bool IsEmailConfirmed { get; set; }
	}
}

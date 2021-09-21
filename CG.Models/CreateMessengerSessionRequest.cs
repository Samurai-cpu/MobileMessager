using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Models
{
	public class CreateMessengerSessionRequest
	{
		[JsonProperty("PublicKey")]
		public string PublicKey { get; set; } // публичный ключ для клиента
	}
}

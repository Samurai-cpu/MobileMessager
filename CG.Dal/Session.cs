using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CG.Dal
{
	[Table("Sessions")]
	public class Session
    {
		[PrimaryKey, AutoIncrement, Column("_id")]
		public long Id { get; set; }
		public string SessionId { get; set; }
		public string ClientPublicKey { get; set; }
		public string ClientPrivateKey { get; set; }
		public string ServerPublicKey { get; set; }
		public DateTime Created { get; set; }
	}
}

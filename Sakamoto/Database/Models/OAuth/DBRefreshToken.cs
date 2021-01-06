using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.OAuth
{
	[Table("oauth_refresh_token")]
	public class DBRefreshToken
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		[Column("access_code_id")]
		public string AccessCode { get; set; }
		[Column("revoked")]
		public bool Revoked { get; set; }
		[Column("created_at")]
		public int CreatedAt { get; set; }
		[Column("expires_at")]
		public int ExpiresAt { get; set; }
	}
}

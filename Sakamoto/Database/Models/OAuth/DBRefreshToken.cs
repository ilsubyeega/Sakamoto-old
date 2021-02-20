using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sakamoto.Database.Models.OAuth
{
	[Table("oauth_refresh_token")]
	public class DBRefreshToken
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		[Column("access_token_id")]
		public string AccessToken { get; set; }
		[Column("revoked")]
		public bool Revoked { get; set; }
		[Column("created_at")]
		public long? CreatedAt { get; set; }
		[Column("expires_at")]
		public long? ExpiresAt { get; set; }
	}
}

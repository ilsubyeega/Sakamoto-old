using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sakamoto.Database.Models.OAuth
{
	[Table("oauth_personal_access_token")]
	public class DBPersonalAccessToken
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		[Column("client_id")]
		public long ClientId { get; set; }
		[Column("scopes")]
		public string Scopes { get; set; }
		[Column("revoked")]
		public bool Revoked { get; set; }
		[Column("created_at")]
		public long? CreatedAt { get; set; }
		[Column("expires_at")]
		public long? ExpiresAt { get; set; }
	}
}

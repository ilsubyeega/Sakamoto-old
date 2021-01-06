using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sakamoto.Database.Models
{
	[Table("users")]
	public class DBUser
	{
		[Key]
		[Column("user_id")]
		public uint Id { get; set; }
		[Column("user_type")]
		public byte UserType { get; set; }
		[Column("username")]
		public string UserName { get; set; }
		[Column("user_email")]
		public string Email { get; set; }
		[Column("user_password")]
		public string Password { get; set; }


		[Column("user_permissions")]
		public uint UserPermissions { get; set; }
		[Column("user_debuff")]
		public uint UserDebuff { get; set; }


		[Column("user_supporter_level")]
		public byte SupporterLevel { get; set; }
		[Column("has_supported")]
		public bool HasSupported { get; set; }


		[Column("user_ip")]
		public string LastIP { get; set; }
		[Column("user_lastvisit")]
		public long LastVisit { get; set; }
		[Column("user_regdate")]
		public long RegisterationDate { get; set; }
		[Column("user_pwweak")]
		public bool IsPasswordWeak { get; set; }
		[Column("user_last_confirm_key")]
		public string LastConfirmKey { get; set; }


		[Column("user_lang")]
		public byte Language { get; set; }
		[Column("user_country")]
		public string Country { get; set; }
		[Column("user_timezone")]
		public decimal Timezone { get; set; }
		[Column("user_playstyle")]
		public uint PlayStyle { get; set; }
		[Column("user_playmode")]
		public byte PlayMode { get; set; }
		[Column("user_color")]
		public byte PageColor { get; set; }
		[Column("osu_id")]
		public uint OsuId { get; set; }


		[Column("hidden_online")]
		public bool HiddenOnline { get; set; }
		[Column("ignore_pm")]
		public bool IgnorePM { get; set; }


		[Column("group_id")]
		public uint GroupID { get; set; }


		[Column("c_locations")]
		public string CustomLocations { get; set; }
		[Column("c_interest")]
		public string CustomInterest { get; set; }
		[Column("c_roles")]
		public string CustomRoles { get; set; }
		[Column("c_twitter")]
		public string CustomTwitter { get; set; }
		[Column("c_discord")]
		public string CustomDiscord { get; set; }
		[Column("c_skype")]
		public string CustomSkype { get; set; }
		[Column("c_website")]
		public string CustomWebsite { get; set; }
	}
}

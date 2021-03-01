using Sakamoto.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Chat
{
	[Table("chat_channels_pm")]
	public class DBChannelPM
	{
		[Column("channel_id")]
		[Key]
		public int ChannelId { get; set; }
		[Column("user_id_1")]
		public int UserId1 { get; set; }
		[Column("user_id_2")]
		public int UserId2 { get; set; }
		public string GetIconUrl(int user)
			=> $"https://keesu.512.kr/avatar/{user}";
	}
}

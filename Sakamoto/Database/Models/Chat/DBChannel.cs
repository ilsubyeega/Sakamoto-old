using Sakamoto.Api;
using Sakamoto.Enums.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Chat
{
	[Table("chat_channels")]
	public class DBChannel
	{
		[Column("channel_id")]
		[Key]
		public int ChannelId { get; set; }
		[Column("name")]
		public string Name { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("creation_time")]
		public long CreationTime { get; set; }
		[Column("type")]
		public int Type { get; set; }
		[Column("allowed_groups")]
		public string AllowedGroups { get; set; }
		[Column("moderated")]
		public bool Morderated { get; set; }
		public string GetIconUrl(int? user = null)
		{
			switch ((ChannelType)Type)
			{
				default:
					return $"https://keesu.512.kr/chat/icon/{ChannelId}";
				case ChannelType.MULTIPLAYER:
					return "https://keesu.512.kr/chat/icon/mp_default.png";
				case ChannelType.SPECTATOR:
					return $"https://keesu.512.kr/avatar/{user ?? 0}";
				case ChannelType.PM:
					return $"https://keesu.512.kr/avatar/{user ?? 0}";
			}
		}
	}
}

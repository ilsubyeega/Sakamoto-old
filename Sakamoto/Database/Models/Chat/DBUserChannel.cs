using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Chat
{
	[Table("chat_user_channels")]
	public class DBUserChannel
	{
		[Column("id")]
		[Key]
		public int Id { get; set; }
		[Column("user_id")]
		public int UserId { get; set; }
		[Column("channel_id")]
		public int ChannelId { get; set; }
		[Column("hidden")]
		public bool IsHidden { get; set; }
		[Column("last_read_id")]
		public int LastReadId { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Chat
{
	[Table("chat_messages")]
	public class DBMessage
	{
		[Column("message_id")]
		[Key]
		public int MessageId { get; set; }
		[Column("user_id")]
		public int UserId { get; set; }
		[Column("channel_id")]
		public int ChannelId { get; set; }
		[Column("content")]
		public string Content { get; set; }
		[Column("is_action")]
		public bool IsAction { get; set; }
		[Column("timestamp")]
		public long Timestamp { get; set; }
	}
}

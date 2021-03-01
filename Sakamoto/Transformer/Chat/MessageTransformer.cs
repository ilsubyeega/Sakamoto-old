using Sakamoto.Api.Chat;
using Sakamoto.Database.Models;
using Sakamoto.Database.Models.Chat;
using Sakamoto.Transformer.ResponseTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Transformer.Chat
{
	public static class MessageTransformer
	{
		public static JsonMessage ToJsonMessage(this DBMessage message)
			=> ToJsonMessage(message.MessageId, message.UserId, message.ChannelId, message.Timestamp, message.Content, message.IsAction);
		private static JsonMessage ToJsonMessage(int message_id, int sender_id, int channel_id, long timestamp, string content, bool is_action)
		{
			return new JsonMessage
			{
				MessageId = message_id,
				SenderId = sender_id,
				ChannelId = channel_id,
				Timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToString("o"),
				Content = content,
				IsAction = is_action
			};
		}
		public static JsonMessage IncludeSender(this JsonMessage message, DBUser db)
		{
			message.Sender = UserTransformer.ToUserCompact(db);
			return message;
		}
	}
}

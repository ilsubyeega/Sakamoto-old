using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api.Chat
{
	public class JsonMessage
	{
		[JsonProperty("message_id")]
		public int MessageId;
		[JsonProperty("sender_id")]
		public int SenderId;
		[JsonProperty("channel_id")]
		public int ChannelId;
		[JsonProperty("timestamp")]
		public string Timestamp;
		[JsonProperty("content")]
		public string Content;
		[JsonProperty("is_action")]
		public bool IsAction;
		[JsonProperty("sender", NullValueHandling = NullValueHandling.Ignore)]
		public JsonUserCompact Sender = null;
	}
}

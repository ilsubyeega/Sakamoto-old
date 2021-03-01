using Newtonsoft.Json;
using Sakamoto.Enums.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api.Chat
{
	public class JsonChannel
	{
		[JsonProperty("channel_id")]
		public int ChannelId;
		[JsonProperty("name")]
		public string Name;
		[JsonProperty("description")]
		public string Description;
		[JsonProperty("icon")]
		public string Icon;
		[JsonProperty("type")]
		public ChannelType Type;
		[JsonProperty("first_message_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? FirstMessageId = null;
		[JsonProperty("last_read_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? LastReadId = null;
		[JsonProperty("last_message_id", NullValueHandling = NullValueHandling.Ignore)]
		public int? LastMessageId = null;
		[JsonProperty("recent_messages", NullValueHandling = NullValueHandling.Ignore)]
		public JsonMessage[] RecentMessages = null;
		[JsonProperty("moderated")]
		public bool IsModerated;
		[JsonProperty("users")]
		public int[] Users = new int[] { };
	}
}

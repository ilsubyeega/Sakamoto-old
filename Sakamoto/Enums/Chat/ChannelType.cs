using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Chat
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ChannelType
	{
		UNKNOWN,
		PUBLIC,
		PRIVATE,
		MULTIPLAYER,
		SPECTATOR,
		TEMPORARY,
		PM,
		GROUP
	}
}

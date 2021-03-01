using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Beatmap
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BeatmapStatus
	{
		none = -3,
		graveyard = -2,
		wip = -1,
		pending = 0,
		ranked = 1,
		approved = 2,
		qualified = 3,
		loved = 4
	}
}

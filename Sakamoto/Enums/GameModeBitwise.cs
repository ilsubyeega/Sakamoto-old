using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	// This enum exists for gamemode array.
	// For example: 15 = osu, taiko, fruit, mania.
	[JsonConverter(typeof(StringEnumConverter))]
	[Flags]
	public enum GameModeBitwise
	{
		none = 0, // unknown
		osu = 1 << 0,
		taiko = 1 << 1,
		fruit = 1 << 2,
		mania = 1 << 3
	}
}

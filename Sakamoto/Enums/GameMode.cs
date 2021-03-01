using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GameMode
	{
		osu = 0,
		taiko = 1,
		fruits = 2,
		mania = 3
	}
}

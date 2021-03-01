using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Beatmap
{
	// osu-web/databases/seeds/ModelSeeders/MiscSeeder.php
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BeatmapGenre
	{
		Any = 0,
		Unspecified = 1,
		[EnumMember(Value = "Video Game")]
		VideoGame = 2,
		Anime = 3,
		Rock = 4,
		Pop = 5,
		Other = 6,
		Novelty = 7,
		[EnumMember(Value = "Hip Hop")]
		HipHop = 9,
		Electronic = 10,
		Metal = 11,
		Classical = 12,
		Folk = 13,
		Jazz = 14
	}
}

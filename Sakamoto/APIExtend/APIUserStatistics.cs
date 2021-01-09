using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.APIExtend
{
	public class APIUserStatistics : osu.Game.Users.UserStatistics
	{
		[JsonProperty("pp_rank")]
		public int PPRank = 0;
		[JsonProperty("is_ranked")]
		public bool IsRanked = true;

	}
}

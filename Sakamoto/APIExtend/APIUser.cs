using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.APIExtend
{
	public partial class APIUser : osu.Game.Users.User
	{
		[JsonProperty("account_history")]
		public Empty[] AccountHistory = new Empty[0];
		[JsonProperty("active_tournament_banner")]
		public Empty[] ActiveTournamentBanner = new Empty[0];
		[JsonProperty("comments_count")]
		public int comments_count = 0;
		[JsonProperty("country_code")]
		public string CountryCode { get => Country.FlagName; set => Country.FlagName = value; }
		[JsonProperty("default_group")]
		public string DefaultGroup = "default";
		[JsonProperty("groups")]
		public Empty[] Groups = new Empty[0];
		[JsonProperty("has_supported")]
		public bool HasSupported = false;
		[JsonProperty("mapping_follower_count")]
		public int MappingFollwerCount = 0;
		[JsonProperty("max_blocks")]
		public int MaxBlocks = 25;
		[JsonProperty("max_friends")]
		public int MaxFriends = 250;
		[JsonProperty("page")]
		public UserPage Page = new UserPage();
		[JsonProperty("rank_history")]
		public new RankHistoryData RankHistoryData = null;
		[JsonProperty("rankHistory")]
		public new RankHistoryData RankHistoryDataFallback = null;
		[JsonProperty("scores_best_count")]
		public int ScoreBestCount = 0;
		[JsonProperty("scores_recent_count")]
		public int ScoreRecentCount = 0;
		[JsonProperty("title_url")]
		public string TitleUrl = null;
		[JsonProperty("playstyle")]
		public string[] PlayStyle = null;
		public class UserPage
		{
			[JsonProperty("html")]
			public string Html = "";
			[JsonProperty("raw")]
			public string Raw = "";
		}
		public void SetRankHistoryData(RankHistoryData data)
		{
			RankHistoryData = data;
			
		}
	}
}

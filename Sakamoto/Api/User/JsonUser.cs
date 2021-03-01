using Newtonsoft.Json;
using Sakamoto.Enums;

namespace Sakamoto.Api
{
	public class JsonUserCompact
	{
		[JsonProperty("avatar_url")]
		public string AvatarUrl;
		[JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
		public JsonCountry Country;
		[JsonProperty("country_code")]
		public string CountryCode;
		[JsonProperty("cover", NullValueHandling = NullValueHandling.Ignore)]
		public JsonCover Cover;
		[JsonProperty("current_mode_rank", NullValueHandling = NullValueHandling.Ignore)]
		public int? CurrentModeRank;
		[JsonProperty("default_group")]
		public string DefaultGroup;
		[JsonProperty("follower_count", NullValueHandling = NullValueHandling.Ignore)]
		public int? FollowerCount;
		[JsonProperty("groups", NullValueHandling = NullValueHandling.Ignore)]
		public JsonGroup[] Groups;
		[JsonProperty("id")]
		public int Id;
		[JsonProperty("is_active")]
		public bool IsActive = false;
		[JsonProperty("is_restricted")]
		public bool IsRestricted = false;
		[JsonProperty("is_silenced")]
		public bool IsSilenced = false;
		[JsonProperty("is_deleted")]
		public bool IsDeleted = false;
		[JsonProperty("is_online")]
		public bool IsOnline = false;
		[JsonProperty("is_supporter")]
		public bool IsSupporter = false;
		[JsonProperty("last_visit")]
		public string LastVisit = null;
		[JsonProperty("pm_friends_only")]
		public bool PmFriendsOnly = false;
		[JsonProperty("profile_colour")]
		public string ProfileColour = null;
		[JsonProperty("support_level", NullValueHandling = NullValueHandling.Ignore)]
		public int? SupporterLevel;
		[JsonProperty("username")]
		public string Username;
		[JsonProperty("playmoode", NullValueHandling = NullValueHandling.Ignore)]
		public GameMode? PlayMode;
	}
	public class JsonUser : JsonUserCompact
	{
		[JsonProperty("comments_count")]
		public int CommentCount;
		[JsonProperty("cover_url")]
		public string CoverUrl;

		[JsonProperty("discord")]
		public string Discord;
		[JsonProperty("has_supported")]
		public bool HasSupported;
		[JsonProperty("interests")]
		public string Interests;
		[JsonProperty("join_date")]
		public string JoinDate;
		[JsonProperty("kudosu")]
		public JsonKudosuInfo KudosuInfo;
		[JsonProperty("location")]
		public string Location;
		[JsonProperty("max_blocks")]
		public int MaxBlocks;
		[JsonProperty("max_friends")]
		public int MaxFriends;
		[JsonProperty("occupation")]
		public string Occupation = null;
		[JsonProperty("playstyle")]
		public string[] PlayStyle;
		[JsonProperty("post_count")]
		public int PostCount;
		[JsonProperty("profile_order")]
		public string[] ProfileOrder;
		[JsonProperty("skype")]
		public string Skype;
		[JsonProperty("title")]
		public string Title;
		[JsonProperty("title_url")]
		public string TitleUrl;
		[JsonProperty("twitter")]
		public string Twitter;
		[JsonProperty("website")]
		public string Website;

		[JsonProperty("favourite_beatmapset_count")]
		public int FavouriteBeatmapsetCount = 0;
		[JsonProperty("loved_beatmapset_count")]
		public int LovedBeatmapsetCount = 0;
		[JsonProperty("ranked_and_approved_beatmapset_count")]
		public int RankedApprovedMapsetCount = 0;
		[JsonProperty("unranked_beatmapset_count")]
		public int PendingMapsetCount = 0;
		[JsonProperty("graveyard_beatmapset_count")]
		public int GraveyardMapsetCount = 0;
		[JsonProperty("mapping_follower_count")]
		public int MapFollowers = 0;

		/*[JsonProperty("")]
		[JsonProperty("monthly_playcounts")]
		[JsonProperty("page")]
		[JsonProperty("previous_usernames")]
		[JsonProperty("replays_watched_counts")]
		[JsonProperty("score_best_count")]
		[JsonProperty("score_first_count")]
		[JsonProperty("score_recent_count")]
		
		[JsonProperty("statistics_rulesets")]
		[JsonProperty("support_level")]
		[JsonProperty("unread_pm_count")]
		[JsonProperty("rankHistory")]
		public JsonRankHistory RankHistoryFallback;*/
		[JsonProperty("statistics")]
		public JsonUserStatistics Statistics;
		[JsonProperty("rank_history")]
		public JsonRankHistory RankHistory;
	}


}

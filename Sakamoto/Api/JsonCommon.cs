using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Sakamoto.Api
{
	public class JsonCountry
	{
		[JsonProperty(@"code")]
		public string FlagName;
		[JsonProperty(@"display", NullValueHandling = NullValueHandling.Ignore)]
		public string Display;
		[JsonProperty(@"name")]
		public string FullName;

	}
	public class JsonCover
	{
		[JsonProperty("custom_url", NullValueHandling = NullValueHandling.Ignore)]
		public string CustomUrl;
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id;
		[JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url;
	}
	public class JsonGroup
	{
		[JsonProperty("colour")]
		public string Color;
		[JsonProperty("id")]
		public int Id;
		[JsonProperty("identifer")]
		public string Identifer;
		[JsonProperty("is_probationary")]
		public bool IsProbationary;
		[JsonProperty("name")]
		public string Name;
		[JsonProperty("playmodes", NullValueHandling = NullValueHandling.Ignore)]
		public object[] PlayModes;
		[JsonProperty("short_name")]
		public string ShortName;
	}
	
	public class JsonBeatmapFailTimesArray
	{
		[JsonProperty("exit")]
		public int[] Exits;
		[JsonProperty("fail")]
		public int[] Fails;
	}
	
	
	public class JsonBeatmapsetCovers
	{
		[JsonProperty("card")]
		public string Card;
		[JsonProperty("cover")]
		public string Cover;
		[JsonProperty("list")]
		public string List;
		[JsonProperty("slimcover")]
		public string SlimCover;
	}
	public class JsonDownloadAvaility
	{
		[JsonProperty("download_disabled")]
		public bool DownloadDisabled = false;
		[JsonProperty("more_information")]
		public string MoreInformation = null;
	}
	public class JsonNominationSummaryMeta
	{
		[JsonProperty("current")]
		public int Current = 0;
		[JsonProperty("required")]
		public int Required = 2;
	}
	public class JsonKudosuInfo
	{
		[JsonProperty("total")]
		public int Total;
		[JsonProperty("available")]
		public int Available;
	}
	public class JsonAchievement
	{
		[JsonProperty("achieved_at")]
		public string AchievedAt;

		[JsonProperty("achievement_id")]
		public int ID;
	}
	public class JsonRankHistory
	{
		[JsonProperty("mode")]
		public string Mode;
		[JsonProperty("data")]
		public int[] Data;
	}
	public class JsonBeatmapCovers
	{
		[JsonProperty("card")]
		public string Card;
		[JsonProperty("card@2x")]
		public string Card2x;
		[JsonProperty("cover")]
		public string Cover;
		[JsonProperty("cover@2x")]
		public string Cover2x;
		[JsonProperty("list")]
		public string List;
		[JsonProperty("list@2x")]
		public string List2x;
		[JsonProperty("slimcover")]
		public string SlimCover;
		[JsonProperty("slimcover@2x")]
		public string SlimCover2x;
		public JsonBeatmapCovers SetValue(int beatmapset_id)
		{
			var randomnumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
			Card = coverUrl(beatmapset_id, "card", randomnumber);
			Card2x = coverUrl(beatmapset_id, "card@2x", randomnumber);
			Cover = coverUrl(beatmapset_id, "cover", randomnumber);
			Cover2x = coverUrl(beatmapset_id, "cover@2x", randomnumber);
			List = coverUrl(beatmapset_id, "list", randomnumber);
			List2x = coverUrl(beatmapset_id, "list@2x", randomnumber);
			SlimCover = coverUrl(beatmapset_id, "slimcover", randomnumber);
			SlimCover2x = coverUrl(beatmapset_id, "slimcover@2x", randomnumber);
			return this;
		}
		private static string coverUrl(int id, string key, string query) => $"https://assets.ppy.sh/beatmaps/{id}/covers/{key}.jpg{(query == null ? "" : "?"+query)}";
	}
	public class JsonHype
	{
		[JsonProperty("current")]
		public int Current = 0;
		[JsonProperty("required")]
		public int Required = 5;
	}
	public class JsonCurrentUserAttributes
	{
		[JsonProperty("can_delete")]
		public bool CanDelete;
		[JsonProperty("can_edit_metadata")]
		public bool CanEditMetadata;
		[JsonProperty("can_hype")]
		public bool CanHype;
		[JsonProperty("can_remove_from_loved")]
		public bool CanRemoveFromLoved;
		[JsonProperty("is_watching")]
		public bool IsWatching;
		[JsonProperty("new_hype_time")]
		public int NewHypeTime;
		[JsonProperty("nomination_modes")]
		public int NominationModes; // no idea
		[JsonProperty("remaining_hype")]
		public int RemainingHype;
	}
	public class JsonIdName
	{
		[JsonProperty("id")]
		public int? Id;
		[JsonProperty("name")]
		public string Name;
	}
	public class JsonDescription
	{
		[JsonProperty("description")]
		public string Text;
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
	public enum GameMode
	{
		[EnumMember(Value = "osu")]
		osu,
		[EnumMember(Value = "taiko")]
		taiko,
		[EnumMember(Value = "fruits")]
		fruits,
		[EnumMember(Value = "mania")]
		mania
	}
	public class JsonBeatmapFailTimesArray
	{
		[JsonProperty("exit")]
		public int[] Exits;
		[JsonProperty("fail")]
		public int[] Fails;
	}
	public class JsonGenre
	{
		[JsonProperty("id")]
		public int? Id;
		[JsonProperty("name")]
		public string Name;
	}
	public class JsonLanguage
	{
		[JsonProperty("id")]
		public int? Id;
		[JsonProperty("name")]
		public string Name;
	}
	public enum Rank
	{
		[EnumMember(Value = "A")]
		A,
		[EnumMember(Value = "B")]
		B,
		[EnumMember(Value = "C")]
		C,
		[EnumMember(Value = "D")]
		D,
		[EnumMember(Value = "S")]
		S,
		[EnumMember(Value = "SH")]
		SH,
		[EnumMember(Value = "X")]
		X,
		[EnumMember(Value = "XH")]
		XH,
	}
	public enum BeatmapStatus
	{
		[EnumMember(Value = "graveyard")]
		Graveyard,
		[EnumMember(Value = "wip")]
		WIP,
		[EnumMember(Value = "pending")]
		Pending,
		[EnumMember(Value = "ranked")]
		Ranked,
		[EnumMember(Value = "approved")]
		Approved,
		[EnumMember(Value = "qualified")]
		Qualified,
		[EnumMember(Value = "loved")]
		Loved,
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
		public bool DownloadDisabled;
		[JsonProperty("more_information")]
		public string MoreInformation;
	}
	public class JsonNominationSummaryMeta
	{
		[JsonProperty("current")]
		public string Current;
		[JsonProperty("required")]
		public string Required;
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
}

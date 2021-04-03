using Newtonsoft.Json;
using osu.Game.Rulesets.Scoring;
using Sakamoto.Enums;
using System.Collections.Generic;

namespace Sakamoto.Api
{
	public class JsonScore
	{
		[JsonProperty("id")]
		public int Id = 0;
		[JsonProperty("best_id")]
		public int BestId = 0;
		[JsonProperty("user_id")]
		public int Userid = -1;
		[JsonProperty("accuracy")]
		public float Accuracy;
		[JsonProperty("mods")]
		public string[] Mods;
		[JsonProperty("score")]
		public long Score;
		[JsonProperty("max_combo")]
		public int MaxCombo;
		[JsonProperty("perfect")]
		public bool Perfect;
		[JsonProperty("statistics")]
		public Dictionary<HitResult, int> Statistics;
		[JsonProperty("pp")]
		public double Pp;
		[JsonProperty("rank")]
		public Rank Rank;
		[JsonProperty("created_at")]
		public string CreatedAt;
		[JsonProperty("mode")]
		public string Mode = "osu";
		[JsonProperty("mode_int")]
		public int ModeInt = 0;
		[JsonProperty("replay")]
		public bool Replay = false;
		[JsonProperty("user")]
		public JsonUserCompact User;

		[JsonProperty("beatmap", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmap Beatmap = null;
		[JsonProperty("beatmapset", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmapSet BeatmapSet = null;
		/*
		[JsonProperty("rank_country")]
		[JsonProperty("rank_global")]
		[JsonProperty("weight")]
		[JsonProperty("match")]
		*/
	}
	public class JsonUserScore
	{
		[JsonProperty("position")]
		public int Position;
		[JsonProperty("score")]
		public JsonScore Score;
	}
	public class JsonBeatmapScores
	{
		[JsonProperty("scores")]
		public JsonScore[] Scores;
		[JsonProperty("userScore", NullValueHandling = NullValueHandling.Ignore)]
		public JsonUserScore UserScores;
	}
}

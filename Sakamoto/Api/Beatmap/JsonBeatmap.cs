using Newtonsoft.Json;
using Sakamoto.Enums;
using Sakamoto.Enums.Beatmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api
{
	public class JsonBeatmapCompact
	{
		[JsonProperty("difficulty_rating")]
		public float DifficultyRating;
		[JsonProperty("id")]
		public int Id;
		[JsonProperty("mode")]
		public GameMode Mode;
		[JsonProperty("status")]
		public BeatmapStatus Status;
		[JsonProperty("total_length")]
		public int TotalLength;
		[JsonProperty("version")]
		public string Version;
		[JsonProperty("ranked")]
		public int Ranked;
		[JsonProperty("beatmapset", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmapSetCompact BeatmapSet;
		[JsonProperty("checksum", NullValueHandling = NullValueHandling.Ignore)]
		public string CheckSum;
		[JsonProperty("failtimes", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmapFailTimesArray FailTimes;
		[JsonProperty("max_combo", NullValueHandling = NullValueHandling.Ignore)]
		public int? MaxCombo;
	}
	public class JsonBeatmap : JsonBeatmapCompact
	{
		[JsonProperty("accuracy")]
		public float Accuracy;
		[JsonProperty("ar")]
		public float AR;
		[JsonProperty("beatmapset_id")]
		public int BeatmapSetId;
		[JsonProperty("bpm")]
		public float BPM;
		[JsonProperty("convert")]
		public bool Convert;
		[JsonProperty("count_circles")]
		public int CountCircles;
		[JsonProperty("count_sliders")]
		public int CountSliders;
		[JsonProperty("count_spinners")]
		public int CountSpinners;
		[JsonProperty("cs")]
		public float CircleSize;
		[JsonProperty("deleted_at")]
		public string DeletedAt;
		[JsonProperty("drain")]
		public float Drain;
		[JsonProperty("hit_length")]
		public int HitLength;
		[JsonProperty("is_scoreable")]
		public bool IsScoreable;
		[JsonProperty("last_updated")]
		public string LastUpdated;
		[JsonProperty("mode_int")]
		public int ModeInt;
		[JsonProperty("passcount")]
		public int PassCount;
		[JsonProperty("playcount")]
		public int PlayCount;
		[JsonProperty("url")]
		public string Url;
	}
}

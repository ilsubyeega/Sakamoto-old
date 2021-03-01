using Newtonsoft.Json;

namespace Sakamoto.Api
{
	public class JsonUserStatistics
	{
		[JsonProperty("level")]
		public JsonLevel Level;
		[JsonProperty("global_rank")]
		public int GlobalRank;
		[JsonProperty("pp")]
		public int PP;
		[JsonProperty("pp_rank")]
		public int PPRank;
		[JsonProperty("ranked_score")]
		public long RankedScore;
		[JsonProperty("hit_accuracy")]
		public double HitAccuracy;
		[JsonProperty("play_count")]
		public int PlayCount;
		[JsonProperty("play_time")]
		public int PlayTime; // TotalSecondsPlayed
		[JsonProperty("total_score")]
		public int TotalScore;
		[JsonProperty("total_hits")]
		public int TotalHits;
		[JsonProperty("maximum_combo")]
		public int MaximumCombo;
		[JsonProperty("replays_watched_by_others")]
		public int ReaplysWatchedByOthers;
		[JsonProperty("is_ranked")]
		public bool IsRanked;
		[JsonProperty("grade_counts")]
		public JsonGradeCounts Grades;
	}
	public class JsonLevel
	{
		[JsonProperty("current")]
		public int Current;
		[JsonProperty("progress")]
		public int Progress;
	}
	public class JsonGradeCounts
	{
		[JsonProperty("ss")]
		public int SS = 0;
		[JsonProperty("SSH")]
		public int SSH = 0;
		[JsonProperty("S")]
		public int S = 0;
		[JsonProperty("SH")]
		public int SH = 0;
		[JsonProperty("A")]
		public int A = 0;
	}
}

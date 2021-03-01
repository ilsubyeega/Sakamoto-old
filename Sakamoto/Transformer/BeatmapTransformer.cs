using Sakamoto.Api;
using Sakamoto.Database.Models;
using Sakamoto.Database.Models.Beatmap;
using Sakamoto.Enums;
using Sakamoto.Enums.Beatmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Transformer
{
	public static class BeatmapTransformer
	{
		public static JsonBeatmapCompact ToJsonBeatmapCompact(this DBBeatmap beatmap)
		{
			var a = new JsonBeatmapCompact();
			MoveCompact(a, beatmap);
			return a;
		}
		private static void MoveCompact(JsonBeatmapCompact compact, DBBeatmap beatmap)
		{
			compact.DifficultyRating = beatmap.DiffRating;
			compact.Id = beatmap.BeatmapId;
			compact.Mode = (GameMode)beatmap.PlayMode;
			compact.Status = (BeatmapStatus)beatmap.Ranked;
			compact.TotalLength = beatmap.TotalLength;
			compact.Version = beatmap.DifficultyName;
			compact.MaxCombo = beatmap.MaxCombo;
		}
		public static JsonBeatmap ToJsonBeatmap(this DBBeatmap beatmap)
		{
			var a = new JsonBeatmap();
			MoveCompact(a, beatmap);
			Move(a, beatmap);
			return a;
		}
		private static void Move(JsonBeatmap a, DBBeatmap b)
		{
			a.Accuracy = b.DiffOverall;
			a.AR = b.DiffApproach;
			a.BeatmapSetId = b.BeatmapsetId;
			a.BPM = b.BPM;
			a.Convert = false;
			a.CountCircles = b.CountNormal;
			a.CountSliders = b.CountSlider;
			a.CountSpinners = b.CountSpinner;
			a.CircleSize = b.DiffSize;
			a.DeletedAt = null;
			a.Drain = b.DiffDrain;
			a.HitLength = b.HitLength;
			a.IsScoreable = (b.Ranked < 3);
			a.LastUpdated = DateTimeOffset.FromUnixTimeSeconds(b.UpdatedDate).ToString("o");
			a.ModeInt = b.PlayMode;
			a.PassCount = b.PlayCount;
			a.PlayCount = b.PlayCount;
			a.Ranked = b.Ranked;
			a.Url = $"https://keesu.512.kr/beatmap/{b.BeatmapId}";
		}
		public static void IncludeBeatmapSet(this JsonBeatmap beatmap, JsonBeatmapSetCompact beatmapset)
			=> beatmap.BeatmapSet = beatmapset;
		public static void IncludeFailTimes(this JsonBeatmap beatmap)
		{
			var fails = new JsonBeatmapFailTimesArray();
			fails.Exits = new int[] { };
			fails.Fails = new int[] { };
			beatmap.FailTimes = fails;
		}
	}
}

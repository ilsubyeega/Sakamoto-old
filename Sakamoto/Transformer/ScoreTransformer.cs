using Sakamoto.Api;
using Sakamoto.Database.Models.Score;
using Sakamoto.Database.Score;
using Sakamoto.Helper;
using Sakamoto.Util;
using System;
using System.Collections.Generic;

namespace Sakamoto.Transformer
{
	public static class ScoreTransformer
	{
		public static JsonScore ToJsonScore(this DBLegacyScore score, JsonUserCompact user = null, JsonBeatmap beatmap = null)
		{
			/*

			var u = user ?? new JsonUserCompact
			{
				AvatarUrl = "https://keesu.ilsubyeega.com/static/default_avatar.png",
				Country = new JsonCountry
				{
					FlagName = "KR",
					FullName = "South Korea"
				},
				CountryCode = "KR",
				DefaultGroup = "default",
				Id = -1,
				Username = score.Username
			};
			var mod = ModsUtil.GetMods(score.Mods);
			var smod = new List<string>();
			foreach (var a in mod)
				smod.Add(a.Acronym);
			var s = new JsonScore
			{
				Accuracy = score.Accuracy / 100,
				Mods = smod.ToArray(),
				Score = score.Score,
				MaxCombo = score.MaxCombo,
				Perfect = score.FullCombo,
				Statistics = new Dictionary<string, int>(),
				Pp = score.Pp,
				Rank = GradeUtil.CalculateOsu(score.Mods, score.Count50, score.Count100, score.Count300, score.CountMiss),
				CreatedAt = DateTimeOffset.FromUnixTimeSeconds(score.Time).ToString("o"),
				User = u,
				Beatmap = beatmap
			};

			s.Statistics.Add("count_geki", score.CountGeki);
			s.Statistics.Add("count_300", score.Count300);
			s.Statistics.Add("count_katu", score.CountKatu);
			s.Statistics.Add("count_100", score.Count100);
			s.Statistics.Add("count_50", score.Count50);
			s.Statistics.Add("count_miss", score.CountMiss);
			return s;*/

			return null;
		}
		public static JsonScore ToJsonScore(this DBSoloScore score, JsonUserCompact user, JsonBeatmap beatmap)
		{
			var s = new JsonScore
			{
				Accuracy = score.Accuracy.Value,
				//Mods = smod.ToArray(),
				Score = score.TotalScore.Value,
				MaxCombo = score.MaxCombo.Value,
				Perfect = score.Perfect,
				Statistics = score.Statistics,
				Pp = score.PP.Value,
				Rank = score.Rank,
				CreatedAt = score.EndedAt.Value.ToString("o"),
				User = user,
				Beatmap = beatmap
			};

			
			return s;
		}
	}
}

using Sakamoto.Api;
using Sakamoto.Database.Models.Legacy;
using Sakamoto.Helper;
using Sakamoto.Util;
using System;
using System.Collections.Generic;

namespace Sakamoto.Transformer
{
	public static class ScoreTransformer
	{
		public static JsonScore ToJsonScore(this DBLegacyScore score, JsonBeatmap beatmap = null)
		{
			

			var u = new JsonUserCompact
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
				Statistics = new JsonStatics
				{
					Count300 = score.Count300,
					Count100 = score.Count100,
					Count50 = score.Count50,
					CountGeki = score.CountGeki,
					CountKatu = score.CountKatu,
					CountMiss = score.CountMiss
				},
				Pp = score.Pp,
				Rank = GradeUtil.CalculateOsu(score.Mods, score.Count50, score.Count100, score.Count300, score.CountMiss),
				CreatedAt = DateTimeOffset.FromUnixTimeSeconds(score.Time).ToString("o"),
				User = u,
				Beatmap = beatmap
			};

			return s;
		}
	}
}

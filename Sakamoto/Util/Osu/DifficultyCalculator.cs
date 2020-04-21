using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch.Difficulty;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mania.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Difficulty;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko.Difficulty;
using osu.Game.Scoring;
using osu.Shared;
using Sakamoto.Objects.Osu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Sakamoto.Util.Osu
{
	public static class DifficultyCalculator
	{
		public static double Calculate(int beatmap_id, int modeid, int modsint)
		{
			string beatmap_path = Path.Combine(Common.path_beatmaps, $"{beatmap_id}.osu");
			if (!File.Exists(beatmap_path))
			{
				Console.WriteLine($"Beatmap {beatmap_id} not found.");
				return -1;
			}
			ProcessorWorkingBeatmap working = new ProcessorWorkingBeatmap(beatmap_path, beatmap_id);
			return Calculate(working, modeid, modsint);
		}
		public static double Calculate(ProcessorWorkingBeatmap working, int modeid, int modsint)
		{
			Ruleset ruleset = LegacyHelper.GetRulesetFromID(modeid);
			Mod[] mods = ruleset.ConvertFromLegacyMods((LegacyMods)modsint).ToArray();
			DifficultyAttributes attr = ruleset.CreateDifficultyCalculator(working).Calculate(mods);
			return attr.StarRating;
		}

	}
}

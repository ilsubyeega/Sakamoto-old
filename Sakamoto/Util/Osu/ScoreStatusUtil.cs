using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Objects;
using Sakamoto.Objects.Osu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Osu
{
	public static class ScoreStatusUtil
	{
		public static IBeatmap GetBeatmap(int beatmap_id, int modeid = 0, int modsint = 0)
		{
			string beatmap_path = Path.Combine(Common.path_beatmaps, $"{beatmap_id}.osu");
			if (!File.Exists(beatmap_path))
			{
				Console.WriteLine($"Beatmap {beatmap_id} not found.");
				return null;
			}
			Ruleset ruleset = LegacyHelper.GetRulesetFromID(modeid);
			Mod[] mods = ruleset.ConvertFromLegacyMods((LegacyMods)modsint).ToArray();
			ProcessorWorkingBeatmap working = new ProcessorWorkingBeatmap(beatmap_path, beatmap_id);
			return working.GetPlayableBeatmap(ruleset.RulesetInfo, mods);
		}

		// This is for osu! mode. Idk they are not working at another modes.
		public static int GetHitObjectsCount(IBeatmap beatmap)
		{
			return beatmap.HitObjects.Count;
		}
		public static int GetMaxCombo(IBeatmap beatmap)
		{
			return beatmap.HitObjects.Count + beatmap.HitObjects.OfType<Slider>().Sum(s => s.NestedHitObjects.Count - 1);
		}
	}
}

using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Shared;
using osuTK.Graphics.OpenGL;
using Sakamoto.Objects.Osu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Osu
{
	public class PerformanceCalculator
	{
		public static double Calculate(int beatmap_id, int modeid, int modsint, int maxcombo, Dictionary<HitResult, int> stats)
		{
			string beatmap_path = Path.Combine(Common.path_beatmaps, $"{beatmap_id}.osu");
			if (!File.Exists(beatmap_path))
			{
				Console.WriteLine($"Beatmap {beatmap_id} not found.");
				return -1;
			}
			Ruleset ruleset = LegacyHelper.GetRulesetFromID(modeid);
			Mod[] mods = ruleset.ConvertFromLegacyMods((LegacyMods)modsint).ToArray();
			foreach (Mod mod in mods)
				Console.WriteLine(mod.Name);
			ProcessorWorkingBeatmap working = new ProcessorWorkingBeatmap(beatmap_path, beatmap_id);
			Score score = new ProcessorScoreDecoder(working).Parse(new ScoreInfo
			{
				Ruleset = ruleset.RulesetInfo,
				MaxCombo = maxcombo,
				Mods = mods,
				Statistics = stats
			});
			return ruleset.CreatePerformanceCalculator(working, score.ScoreInfo).Calculate();
		}
		
	}
}

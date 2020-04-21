using osu.Game.Rulesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;

namespace Sakamoto.Util.Osu
{
	public static class LegacyHelper
	{
		// https://github.com/ppy/osu-tools/blob/master/PerformanceCalculator/LegacyHelper.cs
		public static Ruleset GetRulesetFromID(int id)
		{
			switch (id)
			{
				default:
					throw new ArgumentException("Invalid ruleset ID provided.");
				case 0:
					return new OsuRuleset();
				case 1:
					return new TaikoRuleset();
				case 2:
					return new CatchRuleset();
				case 3:
					return new ManiaRuleset();
			}
		}
	}
}

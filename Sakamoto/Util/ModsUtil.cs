using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sakamoto.Util
{
	public static class ModsUtil
	{
		public static Mod[] GetMods(int modint, Ruleset ruleset = null)
		{
			if (ruleset == null) ruleset = new OsuRuleset();
			return ruleset.ConvertFromLegacyMods((LegacyMods)modint).ToArray();
		}
		public static Mod[] GetMods(string[] modstring, Ruleset ruleset = null)
		{
			if (ruleset == null) ruleset = new OsuRuleset();
			List<Mod> mods = new List<Mod>();
			if (modstring == null) return mods.ToArray();

			List<Mod> availableMods = ruleset.GetAllMods().ToList();
			foreach (string s in modstring)
			{
				Mod newMod = availableMods.FirstOrDefault(m =>
				string.Equals(m.Acronym, s, StringComparison.CurrentCultureIgnoreCase)
				|| string.Equals(m.Name.Replace(" ", ""), s.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase));
				if (newMod == null)
					throw new ArgumentException($"Invalid mod provided: {s}");
				mods.Add(newMod);
			}

			return mods.ToArray();
		}
	}
}

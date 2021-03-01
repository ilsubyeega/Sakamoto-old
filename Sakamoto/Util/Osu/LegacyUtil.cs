using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Osu
{
	public class LegacyUtil
	{
        public static Ruleset GetRulesetFromLegacyID(int id)
        {
            switch (id)
            {
                case 0:
                    return new OsuRuleset();
                case 1:
                    return new TaikoRuleset();
                case 2:
                    return new CatchRuleset();
                case 3:
                    return new ManiaRuleset();
                default:
                    throw new ArgumentException("wrong ruleset Id");
            }
        }
    }
}

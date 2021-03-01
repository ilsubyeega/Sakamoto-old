using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Beatmap
{
	public enum BeatmapCategory
	{
        Any,

        [Description("Has Leaderboard")]
        Leaderboard,
        Ranked,
        Qualified,
        Loved,
        Favourites,

        [Description("Pending & WIP")]
        Pending,
        Graveyard,

        [Description("My Maps")]
        Mine,
    }
}

using Sakamoto.Objects.InGame;
using System.Linq;
using System.Runtime.Caching;

namespace Sakamoto.Cache
{
	public static class ScoreboardCache
	{
		public static Beatmap GetBeatmapById(int beatmap_id)
			=> (Beatmap)MemoryCache.Default
			.Where(a => a.Value is Beatmap &&
			(a.Value as Beatmap).beatmapid == beatmap_id)
			.FirstOrDefault().Value;
		public static Beatmap GetBeatmapBySetIdAndMd5(int beatmapset_id, string md5)
			=> (Beatmap)MemoryCache.Default
			.Where(a => a.Value is Beatmap &&
			(a.Value as Beatmap).beatmapsetid == beatmapset_id && (a.Value as Beatmap).file_md5 == md5)
			.FirstOrDefault().Value;

		public static Scoreboard GetScoreboard(int type, int beatmapset_id, string md_5)
		{
			Scoreboard s = new Scoreboard(null, null);
			return s;
		}
	}
}

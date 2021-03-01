using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Database
{
	public static class BeatmapJsonUtil
	{
		public static JsonBeatmapSetCompact[] BeatmapSetWithIdArray(int[] beatmap_ids, bool is_compact, MariaDBContext ctx)
		{
			var blist = new List<JsonBeatmapSetCompact>();
			foreach (var v in beatmap_ids)
			{
				var b = ctx.BeatmapSets.Where(a => a.BeatmapsetId == v);
				foreach (var v2 in b)
				{
					var bl = ctx.Beatmaps.Where(a => a.BeatmapsetId == v).OrderBy(a => a.PlayMode).ThenBy(a => a.DiffOverall);
					var v3list = new List<JsonBeatmapCompact>();
					foreach (var v3 in bl)
						v3list.Add(is_compact ? v3.ToJsonBeatmapCompact() : v3.ToJsonBeatmap());
					var bbjson = is_compact ? v2.ToJsonBeatmapSetCompact() : v2.ToJsonBeatmapSet();
					bbjson.Beatmaps = v3list.ToArray();
					blist.Add(bbjson);
				}
			}
			return blist.ToArray();
		}
	}
}

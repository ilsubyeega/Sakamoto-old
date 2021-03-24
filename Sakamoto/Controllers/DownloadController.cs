using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class DownloadController : SakamotoController
	{
		private readonly MariaDBContext _dbcontext;
		public DownloadController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmapsets/{beatmapset_id}/download")]
		public async Task<IActionResult> DownloadBeatmap(int beatmapset_id)
		{
			var beatmapset = _dbcontext.BeatmapSets.FirstOrDefault(a => a.BeatmapsetId == beatmapset_id);
			if (beatmapset == null) return StatusCode(404, "Not found on current beatmap database");

			var bt = Beatconnect.Fetch(beatmapset_id);
			if (bt == null) return StatusCode(404, "Beatconnect not found atm");
			var btupdated = bt.RankedDate == null 
				? ParseDate(bt.LastUpdated).ToUnixTimeSeconds() : ParseDate(bt.RankedDate).ToUnixTimeSeconds();

			bool shouldrefresh = false;
			// in case check beatmapset db from this side.
			if (beatmapset.UpdatedDate != btupdated)
			{
				var rs = await OsuApi.TryReqeust($"{OsuApi.API_ROOT}/beatmapsets/{beatmapset_id}");
				if (rs == null) throw new Exception("Could not fetch api: No Idea..");
				var rsval = JsonConvert.DeserializeObject<JsonBeatmapSet>(rs);
				var rsupdated = ParseDate(rsval.RankedDate == null ? rsval.LastUpdated : rsval.RankedDate).ToUnixTimeSeconds();
				if (rsupdated != beatmapset.UpdatedDate) // If database was outdated
				{
					Console.WriteLine($"Beatmapset {beatmapset_id} was outdated from here, updating");
					await BeatmapSeeder.SeedSet(beatmapset_id, true, _dbcontext);
					shouldrefresh = rsupdated != btupdated;
				} else // Its same; so Beatconnect was wrong.
				{
					Console.WriteLine($"DownloadController: {beatmapset_id} was outdated from Beatconnect. forcing parameters.");
					shouldrefresh = true;
				}
			}

			return Redirect($"https://beatconnect.io/b/{beatmapset_id}/{bt.UUID}/" + (shouldrefresh ? "?force=1" : ""));
		}
	}
}

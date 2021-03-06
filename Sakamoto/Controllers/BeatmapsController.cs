using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Beatmap;
using Sakamoto.Enums.Beatmap;
using Sakamoto.Helper;
using Sakamoto.Transformer;
using Sakamoto.Util;
using Sakamoto.Util.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class BeatmapsController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public BeatmapsController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmaps/{beatmapid}")]
		public async Task<IActionResult> Beatmap(int beatmapid)
		{
			var dbb = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == beatmapid);
			if (dbb==null) await BeatmapSeeder.SeedSetFromBeatmap(beatmapid, false, _dbcontext);

			var beatmap = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == dbb.BeatmapId);
			if (beatmap == null) return StatusCode(404, "Beatmap Not Found");

			return StatusCode(200, beatmap.ToJsonBeatmap());
		}

		[HttpGet("beatmapsets/{beatmapsetid}")]
		public async Task<IActionResult> BeatmapSet(int beatmapsetid)
		{
			var dbb = await _dbcontext.BeatmapSets.FirstOrDefaultAsync(a => a.BeatmapsetId == beatmapsetid);
			if (dbb == null) await BeatmapSeeder.SeedSet(beatmapsetid, false, _dbcontext);

			var beatmapset = await _dbcontext.BeatmapSets.FirstOrDefaultAsync(a => a.BeatmapsetId == dbb.BeatmapsetId);
			if (beatmapset == null) return StatusCode(404, "Beatmap Not Found");

			var bjson = beatmapset.ToJsonBeatmapSet();
			var blist = new List<JsonBeatmapCompact>();
			foreach (var b in _dbcontext.Beatmaps.Where(a => a.BeatmapsetId == beatmapset.BeatmapsetId))
				blist.Add(b.ToJsonBeatmap());
			bjson.Beatmaps = blist.ToArray();

			var userid = (int)HttpContext.Items["userId"];
			var fav = _dbcontext.MapFavourites.FirstOrDefault(a => a.UserId == userid && a.BeatmapsetId == beatmapsetid);
			bjson.HasFavourited = (fav != null);

			return StatusCode(200, bjson);
		}


		[HttpGet("beatmaps/lookup")]
		public async Task<IActionResult> BeatmapLookup(int id, string checksum, string filename)
		{
			var dbb = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == id);
			if (dbb == null) await BeatmapSeeder.SeedSetFromBeatmap(id, false, _dbcontext);

			var beatmap = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == id);
			if (beatmap == null) beatmap = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.Checksum == checksum);
			if (beatmap == null) return StatusCode(404, "Beatmap Not Found");
			var beatmapset = await _dbcontext.BeatmapSets.FirstOrDefaultAsync(a => a.BeatmapsetId == beatmap.BeatmapsetId);
			if (beatmapset == null) return StatusCode(404, "Beatmapset Not Found");

			var json = beatmap.ToJsonBeatmap();
			json.IncludeFailTimes();
			json.IncludeBeatmapSet(beatmapset.ToJsonBeatmapSet());

			return StatusCode(200, json);
		}

		[HttpGet("beatmapsets/lookup")]
		public async Task<IActionResult> SetLookup(int? beatmap_id)
		{
			if (beatmap_id == null) return StatusCode(400, "No parameters found.");
			var dbb = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == beatmap_id.Value);
			while (dbb == null)
			{
				var value = await OsuApi.FetchBeatmap(beatmap_id.Value);
				if (value == null) return StatusCode(501, "Beatmap Fetch Failed"); // no beatmap found..
				await BeatmapSeeder.SeedSet(value.BeatmapSetId, false, _dbcontext);
				dbb = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == beatmap_id.Value);
			}
			

			var beatmapset = await _dbcontext.BeatmapSets.FirstOrDefaultAsync(a => a.BeatmapsetId == dbb.BeatmapsetId);
			if (beatmapset == null) return StatusCode(404, "Beatmap Not Found");

			var bjson = beatmapset.ToJsonBeatmapSet();
			var blist = new List<JsonBeatmapCompact>();
			foreach (var b in _dbcontext.Beatmaps.Where(a => a.BeatmapsetId == beatmapset.BeatmapsetId))
				blist.Add(b.ToJsonBeatmap());
			bjson.Beatmaps = blist.ToArray();

			var userid = (int)HttpContext.Items["userId"];
			var fav = _dbcontext.MapFavourites.FirstOrDefault(a => a.UserId == userid && a.BeatmapsetId == beatmapset.BeatmapsetId);
			bjson.HasFavourited = fav != null;

			return StatusCode(200, bjson);
		}

		[HttpPost("beatmapsets/{beatmapset_id}/favourites")]
		public async Task<IActionResult> Favourites(int beatmapset_id, [FromForm] string action = null)
		{
			var userid = (int)HttpContext.Items["userId"];

			var beatmapset = _dbcontext.BeatmapSets.FirstOrDefault(a => a.BeatmapsetId == beatmapset_id);
			if (beatmapset == null)
				return StatusCode(404, "Beatmapset not found.");

			switch (action){
				default:
					return StatusCode(402, "Invalid Action");
				case "favourite":
					if (_dbcontext.MapFavourites.Any(a => a.BeatmapsetId == beatmapset_id && a.UserId == userid))
						return StatusCode(200, "Already favourited");
					var fav = new DBBeatmapsetFavourite
					{
						UserId = userid,
						BeatmapsetId = beatmapset_id,
						FavouritedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
					};
					beatmapset.FavouriteCount++;
					_dbcontext.MapFavourites.Add(fav);
					break;
				case "unfavourite":
					var mapfav = await _dbcontext.MapFavourites.FirstOrDefaultAsync(a => a.BeatmapsetId == beatmapset_id && a.UserId == userid);
					if (mapfav == null)
						return StatusCode(200, "Already not favourited");
					_dbcontext.MapFavourites.Remove(mapfav);
					beatmapset.FavouriteCount--;
					break;
			}
			await _dbcontext.SaveChangesAsync();
			return StatusCode(200, new
			{
				favourite_count = _dbcontext.MapFavourites.Count(a => a.BeatmapsetId == beatmapset_id)
			});
		}
	}
}

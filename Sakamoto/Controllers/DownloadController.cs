using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakamoto.Database;
using Sakamoto.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class DownloadController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public DownloadController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmapsets/{beatmapset_id}/download")]
		public async Task<IActionResult> DownloadBeatmap(int beatmapset_id)
		{
			var exists = _dbcontext.BeatmapSets.Any(a => a.BeatmapsetId == beatmapset_id);
			if (!exists) return StatusCode(404, "Not found on current beatmap database");

			var bt = Beatconnect.Fetch(beatmapset_id);
			return Redirect($"https://beatconnect.io/b/{beatmapset_id}/{bt.UUID}/");
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using osu.Shared;
using System;
using System.Threading.Tasks;


namespace Sakamoto.Controllers
{
	[Host("osu.ppy.sh", "o.512.kr")]
	public class OsuController : Controller
	{
		private MemoryCache _cache;
		

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok("Sakamoto (Bancho) \nosu!bancho reversing project");
		}
		public async Task<IActionResult> GetScores(
			[FromQuery(Name = "v")] int type,
			[FromQuery(Name = "c")] int md5,
			[FromQuery(Name = "f")] string fileName,
			[FromQuery(Name = "i")] int beatmapSetId,
			[FromQuery(Name = "m")] GameMode playMode,
			[FromQuery(Name = "mods")] Mods mods,
			[FromQuery(Name = "us")] string username,
			[FromQuery(Name = "ha")] string password,
			[FromQuery(Name = "v")] int scoreboard_type,
			[FromQuery(Name = "vv")] int scoreboard_version)
		{

			return Ok();
		}
	}
}
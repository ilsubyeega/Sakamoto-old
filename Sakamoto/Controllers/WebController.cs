using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using osu.Shared;
using Sakamoto.Cache;
using Sakamoto.Objects.InGame;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sakamoto.Controllers
{

	[Route("/web/")]
	public class WebController : Controller
	{
		[HttpGet("")]
		public IActionResult i()
		{
			return Ok("Pog");
		}
		[HttpGet("osu-osz2-getscores.php")]
		public async Task<IActionResult> GetScores(
			[FromQuery(Name = "v")] int type,
			[FromQuery(Name = "c")] string md5,
			[FromQuery(Name = "f")] string fileName,
			[FromQuery(Name = "i")] int beatmapSetId,
			[FromQuery(Name = "m")] GameMode playMode,
			[FromQuery(Name = "mods")] Mods mods,
			[FromQuery(Name = "us")] string username,
			[FromQuery(Name = "ha")] string password,
			[FromQuery(Name = "v")] int scoreboard_type,
			[FromQuery(Name = "vv")] int scoreboard_version)
		{
			Scoreboard s = ScoreboardCache.GetScoreboard((int)playMode, beatmapSetId, md5);
			return Ok(s.AppendToString());
		}
	}
}

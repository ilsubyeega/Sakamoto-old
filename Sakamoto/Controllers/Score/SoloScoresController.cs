using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Score;
using Sakamoto.Enums;
using Sakamoto.Helper;
using Sakamoto.Transformer;
using Sakamoto.Transformer.ResponseTransformer;
using Sakamoto.Util;
using Sakamoto.Util.Osu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers.Score
{
	[Route("api/v2/")]
	[ApiController]
	[Authorize]
	public class SoloScoresController : SakamotoController
	{
		private readonly MariaDBContext _dbcontext;
		public SoloScoresController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmaps/{beatmapid}/scores")] // TODO: move to solo endpoint
		public async Task<IActionResult> GetSoloLeaderboard(int beatmapid, string type = "global", string mode = "osu", [FromQuery(Name = "mods[]")] string[] mods = null)
		{

			// Seeder 
			if (!_dbcontext.Beatmaps.Any(a => a.BeatmapId == beatmapid)) await BeatmapSeeder.SeedSetFromBeatmap(beatmapid, false, _dbcontext);


			var gm = (GameMode)Enum.Parse(typeof(GameMode), mode, true);
			var rulesetid = (short)gm;

			var beatmap = _dbcontext.Beatmaps.Where(a => a.BeatmapId == beatmapid).FirstOrDefault();
			if (beatmap == null) return StatusCode(200, defaultBeatmapScores);

			var scorequery = _dbcontext.SoloScores.Where(a => a.BeatmapId == beatmapid && a.RulesetId == rulesetid && a.Passed == true);
			if (mods != null && mods.Length > 0)
				scorequery.Where(a => a.Mods.All(b => mods.Contains(b.Acronym)));

			var scores = scorequery
				.Include(a => a.User)
				//.GroupBy(a => new { a.UserId, a.BeatmapId, a.RulesetId })
				//.SelectMany(a => a.OrderByDescending(a => a.TotalScore).Take(1))
				.OrderByDescending(a => a.TotalScore)
				.ToArray();

			var beatmapjson = beatmap.ToJsonBeatmap();
			var scoresjson = scores.Select(a => a.ToJsonScore(a.User.ToUserCompact(), beatmapjson)).ToArray();

			var result = new JsonBeatmapScores
			{
				Scores = scoresjson.ToArray(),
				UserScores = null
			};
			if (scoresjson.Count() > 0)
			{
				var rs = scoresjson.FirstOrDefault(a => a.User.Username.ToLower() == _user.UserName.ToLower());
				if (rs != null)
				{
					result.UserScores = new JsonUserScore
					{
						Position = Array.IndexOf(scoresjson, rs) + 1,
						Score = rs
					};
				}
			}

			return StatusCode(200, result);
		}

		private readonly JsonBeatmapScores defaultBeatmapScores = new JsonBeatmapScores
		{
			Scores = new JsonScore[] { },
			UserScores = null
		};
	}
}

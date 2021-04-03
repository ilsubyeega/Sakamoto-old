using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Legacy;
using Sakamoto.Enums;
using Sakamoto.Helper;
using Sakamoto.Transformer;
using Sakamoto.Util;
using Sakamoto.Util.Osu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[Route("api/v2/")]
	[ApiController]
	[Authorize]
	public class ScoresController : SakamotoController
	{
		private readonly MariaDBContext _dbcontext;
		public ScoresController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmaps/{beatmapid}/scores")]
		public async Task<IActionResult> Scores(int beatmapid, string type = "global", string mode = "osu", [FromQuery(Name = "mods[]")] string[] mods = null)
		{

			// Seeder 
			var dbb = await _dbcontext.Beatmaps.FirstOrDefaultAsync(a => a.BeatmapId == beatmapid);
			if (dbb == null) await BeatmapSeeder.SeedSetFromBeatmap(beatmapid, false, _dbcontext);


			var gm = (GameMode)Enum.Parse(typeof(GameMode), mode, true);


			var beatmap = _dbcontext.Beatmaps.Where(a => a.BeatmapId == beatmapid).FirstOrDefault();
			var combo = beatmap?.MaxCombo ?? 0;
			var a = _dbcontext.LegacyScores.Where(a => a.BeatmapId == beatmapid && a.PlayMode == ((int?)gm ?? 0));
			
			var beatmapjson = beatmap == null ? null : beatmap.ToJsonBeatmap();
			var list = new List<JsonScore>();
			var offsets = CalculateListOffset((int)gm, combo, a.ToArray());
			foreach (var val in offsets)
				list.Add(val.ToJsonScore(beatmapjson));

			var result = new JsonBeatmapScores
			{
				Scores = list.ToArray(),
				UserScores = null
			};
			if (list.Count > 0)
			{
				var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Id == _user.Id);
				var rs = offsets.FirstOrDefault(a => a.Username.ToLower() == user.UserName.ToLower());
				if (rs != null)
				{
					var js = rs.ToJsonScore();
					js.Userid = js.User.Id = _user.Id;
					result.UserScores = new JsonUserScore
					{
						Position = Array.IndexOf(offsets, rs) + 1,
						Score = js
					};
				}
			}

			return StatusCode(200, result);
		}


		// Score simulate for ordering
		// todo remove this when score submission is implemented
		private DBLegacyScore[] CalculateListOffset(int gamemode, int maxcombo, DBLegacyScore[] scores)
		{
			var tuple = new List<(float, DBLegacyScore)>();
			var ruleset = LegacyUtil.GetRulesetFromLegacyID(gamemode);
			foreach (var s in scores)
				tuple.Add((CalculateOffset(ruleset, s.MaxCombo, (float)maxcombo / s.MaxCombo, s.Accuracy/100, s.Mods), s));
			var tuple2 = tuple.OrderByDescending((item) => item.Item1).ToArray();
			
			var tuple3 = new List<DBLegacyScore>();
			foreach (var s in tuple2)
				tuple3.Add(s.Item2);
				
			return tuple3.ToArray();
		}
		private float CalculateOffset(Ruleset ruleset, int maxcombo, float combo, float accuracy, long mods)
		{
			var legacymods = ruleset.ConvertFromLegacyMods((LegacyMods)mods);
			float multiplier = 1;
			foreach (var a in legacymods)
				multiplier = multiplier * (float)a.ScoreMultiplier;
			return accuracy * Math.Max(1, maxcombo) * (1 + Math.Max(0, (combo * maxcombo) - 1) * multiplier);
		}
	}
	
}

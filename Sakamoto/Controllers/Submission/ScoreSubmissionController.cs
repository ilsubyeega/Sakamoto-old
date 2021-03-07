using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using osu.Game.IO.Legacy;
using osu.Game.Rulesets.Scoring;
using Sakamoto.Database;
using Sakamoto.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers.Submission
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class ScoreSubmissionController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public ScoreSubmissionController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpPost("scores/_submit")]
		public async Task<IActionResult> SubmitScore
			(
			[FromForm(Name = "ruleset[id]")] int rulesetid,
			[FromForm(Name = "ruleset[short]")] string rulesetshort,
			[FromForm(Name = "beatmap[id]")] int beatmapid,
			[FromForm(Name = "beatmap[checksum]")] string beatmapchecksum,
			[FromForm(Name = "beatmapset[id]")] int beatmapsetid,
			[FromForm(Name = "score[accuracy]")] double scoreacc,
			[FromForm(Name = "score[mods]")] long scoremods,
			[FromForm(Name = "score[max_combo]")] int scoremaxcombo,
			[FromForm(Name = "score[date]")] long submissiondate,
			[FromForm(Name = "score[total]")] long totalscore,
			[FromForm(Name = "score[hitresults]")] IFormFile scorehitresults,
			[FromForm(Name = "score[replay]")] IFormFile replay
			)
		{
			if (scorehitresults == null || replay == null || scorehitresults.Length == 0 || replay.Length == 0)
				return StatusCode(401, "Hitresult or replay is null");

			byte[] replayarr = null;
			var hitresults = new Dictionary<HitResult, int>();

			using (var ms = new MemoryStream())
			{
				scorehitresults.CopyTo(ms);
				ms.Position = 0;
				using (var reader = new SerializationReader(ms))
					while (reader.BaseStream.Position != reader.BaseStream.Length)
					{
						var index = reader.ReadUInt16();
						var value = reader.ReadInt32();
						hitresults.Add((HitResult)index, value);
					}
			}

			if (hitresults.Count == 0) return StatusCode(401, "Hitresult should be not null.");

			using (var ms = new MemoryStream())
			{
				replay.CopyTo(ms);
				replayarr = ms.ToArray();
			}

			var beatmap = _dbcontext.Beatmaps.FirstOrDefault(a => a.BeatmapId == beatmapid);
			if (beatmap == null)
			{
				await BeatmapSeeder.SeedSetFromBeatmap(beatmapid, false, _dbcontext);
				beatmap = _dbcontext.Beatmaps.FirstOrDefault(a => a.BeatmapId == beatmapid);
			}


			var beatmapset = _dbcontext.BeatmapSets.FirstOrDefault(a => a.BeatmapsetId == beatmapsetid);
			if (beatmap == null || beatmapset == null) return StatusCode(200);
			Console.WriteLine($"[NEW SCORE] Ruleset: {rulesetshort} ({rulesetid})");
			Console.WriteLine($"Beatmap: {beatmapset.Artist} - {beatmapset.Title}");
			Console.WriteLine($"Total Score: {totalscore}");
			Console.WriteLine($"Accuracy: {scoreacc * 100}% | Max Combo: {scoremaxcombo}x");
			Console.WriteLine($"Length (Hitresults/Replay): {hitresults.Count}/{replayarr.Length}");
			foreach (var a in hitresults)
				Console.WriteLine($"[H] {a.Key} : {a.Value}");


			return StatusCode(200);
		}
	}
}

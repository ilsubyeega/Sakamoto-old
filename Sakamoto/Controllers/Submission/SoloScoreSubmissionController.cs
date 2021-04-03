using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using osu.Game.Online.Rooms;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Score;
using Sakamoto.Enums;
using Sakamoto.Helper;
using Sakamoto.Transformer.ResponseTransformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers.Submission
{
	[Route("api/v2/")]
	[ApiController]
	[Authorize]
	public class SoloScoreSubmissionController : SakamotoController
	{
		private readonly MariaDBContext _dbcontext;
		public SoloScoreSubmissionController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpPost("solo/{beatmapId}/scores")]
		public async Task<IActionResult> CreateScore(int beatmapId, 
			[FromForm(Name = "beatmap_hash")] string beatmapHash, 
			[FromForm(Name = "ruleset_id")] int rulesetId, 
			[FromForm(Name = "version_hash")] string versionHash)
		{
			// Seeder 
			if (!_dbcontext.Beatmaps.Any(a => a.BeatmapId == beatmapId)) await BeatmapSeeder.SeedSetFromBeatmap(beatmapId, false, _dbcontext);


			var beatmap = _dbcontext.Beatmaps.Where(a => a.BeatmapId == beatmapId).FirstOrDefault();
			if (beatmap == null) return StatusCode(404, "beatmap not found");

			if (beatmap.Checksum != beatmapHash)
				return StatusCode(403, "Beatmap checksum isnt corrent.");

			if (rulesetId < 0 || rulesetId > 3)
				return StatusCode(403, "Not allowed ruleset Id.");

			var score = new DBSoloScore
			{
				UserId = _user.Id,
				BeatmapId = beatmapId,
				BeatmapChecksum = beatmapHash,
				RulesetId = rulesetId,
				StartedAt = DateTimeOffset.UtcNow
			};

			_dbcontext.SoloScores.Add(score);
			await _dbcontext.SaveChangesAsync();
			
			return StatusCode(200, new
			{
				id = score.Id
			});
		}
		[HttpPut]
		[HttpPatch]
		[Route("solo/{beatmapId}/scores/{scoreId}")]
		public async Task<IActionResult> UpdateScore(int beatmapId, int scoreId, [FromBody] SoloRequestedScoreInfo scoreInfo)
		{
			// This time, we wont do seed beatmap since it should be has.
			if (!scoreInfo.Validate()) return StatusCode(422, "Requested ScoreInfo is invalid.");

			var beatmap = _dbcontext.Beatmaps.Where(a => a.BeatmapId == beatmapId).FirstOrDefault();
			if (beatmap == null) return StatusCode(404, "beatmap not found");

			var score = _dbcontext.SoloScores.Include(a => a.User).FirstOrDefault(a => a.Id == scoreId);
			if (score == null) return StatusCode(404, "Pending Score not found");

			if (score.EndedAt != null) return StatusCode(403, "Score is already submitted.");

			score.Mods = scoreInfo.Mods;
			score.Accuracy = scoreInfo.Accuracy.Value;
			score.EndedAt = DateTimeOffset.UtcNow;
			score.MaxCombo = scoreInfo.MaxCombo.Value;
			score.Passed = scoreInfo.Passed.Value;
			score.Rank = (int)scoreInfo.Rank.Value;
			score.Statistics = scoreInfo.Statistics;
			score.TotalScore = scoreInfo.TotalScore;

			// TODO: Calculate performance points.

			await _dbcontext.SaveChangesAsync();

			var result = new SoloResponseScoreInfo
			{
				Accuracy = score.Accuracy.Value,
				BeatmapId = score.BeatmapId,
				EndedAt = score.EndedAt.Value,
				ID = score.Id,
				MaxCombo = score.MaxCombo.Value,
				Mods = score.Mods,
				Passed = score.Passed,
				Rank = (Rank)score.Rank,
				RulesetId = score.RulesetId,
				StartedAt = score.StartedAt,
				Statistics = score.Statistics,
				TotalScore = score.TotalScore.Value,
				UserId = _user.Id
			};

			return StatusCode(200, result);
		}



		public class SoloRequestedScoreInfo
		{
			[JsonProperty("accuracy")]
			public float? Accuracy;
			[JsonProperty("max_combo")]
			public int? MaxCombo;
			[JsonProperty("apimods")]
			public JsonMod[] Mods;
			[JsonProperty("passed")]
			public bool? Passed;
			[JsonProperty("rank")]
			public Rank? Rank;
			[JsonProperty("statistics")]
			public Dictionary<HitResult, int> Statistics;
			[JsonProperty("total_score")]
			public int? TotalScore;

			public bool Validate()
			{
				return (
					this.Accuracy != null &&
					this.MaxCombo != null &&
					this.Mods != null &&
					this.Passed != null &&
					this.Rank != null &&
					this.Statistics != null &&
					this.TotalScore != null
				);
			}
		}
		public class SoloResponseScoreInfo
		{
			[JsonProperty("accuracy")]
			public float Accuracy;
			[JsonProperty("beatmap_id")]
			public int BeatmapId;
			[JsonProperty("ended_at")]
			public DateTimeOffset EndedAt;
			[JsonProperty("id")]
			public long ID;
			[JsonProperty("max_combo")]
			public int MaxCombo;
			[JsonProperty("mods")]
			public JsonMod[] Mods;
			[JsonProperty("passed")]
			public bool Passed;
			[JsonProperty("rank")]
			[JsonConverter(typeof(StringEnumConverter))]
			public Rank Rank;
			[JsonProperty("ruleset_id")]
			public int RulesetId;
			[JsonProperty("started_at")]
			public DateTimeOffset StartedAt;
			[JsonProperty("statistics")]
			public Dictionary<HitResult, int> Statistics;
			[JsonProperty("total_score")]
			public long TotalScore;
			[JsonProperty("user_id")]
			public int UserId;
		}
	}
}

using osu.Game.Online.API;
using osu.Game.Rulesets.Scoring;
using Sakamoto.Api;
using Sakamoto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Score
{
	[Table("solo_scores")]
	public class DBSoloScore
	{
		[Key]
		[Column("id")]
		public long Id { get; set; }
		[Column("user_id")]
		public int UserId { get; set; }
		[Column("beatmap_id")]
		public int BeatmapId { get; set; }
		[Column("beatmap_checksum")]
		public string BeatmapChecksum { get; set; }
		[Column("ruleset_id")]
		public int RulesetId { get; set; }
		[Column("rank")]
		public int Rank { get; set; }


		[Column("total_score")]
		public int? TotalScore { get; set; }
		[Column("accuracy")]
		public float? Accuracy { get; set; }
		[Column("pp")]
		public float? PP { get; set; }
		[Column("max_combo")]
		public int? MaxCombo { get; set; }
		[Column("perfect")]
		public bool Perfect { get; set; }
		[Column("mods", TypeName = "json")]
		public JsonMod[] Mods { get; set; }
		[Column("statistics", TypeName = "json")]
		public Dictionary<HitResult, int> Statistics { get; set; }
		[Column("started_at")]
		public DateTimeOffset StartedAt { get; set; }
		[Column("ended_at")]
		public DateTimeOffset? EndedAt { get; set; }
		[Column("passed")]
		public bool Passed { get; set; }
		[Column("unranked")]
		public bool Unranked { get; set; }

		public virtual DBUser User { get; set; }
	}
}

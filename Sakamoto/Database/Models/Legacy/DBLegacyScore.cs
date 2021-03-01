using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sakamoto.Database.Models.Legacy
{
	[Table("old_scores")]
	[Keyless]
	public class DBLegacyScore
	{
		[Column("userid")]
		public int UserId { get; set; }
		[Column("username")]
		public string Username { get; set; }
		[Column("beatmap_id")]
		public int BeatmapId { get; set; }
		[Column("beatmapset_id")]
		public int BeatmapsetId { get; set; }
		[Column("score")]
		public long Score { get; set; }
		[Column("max_combo")]
		public int MaxCombo { get; set; }
		[Column("full_combo")]
		public bool FullCombo { get; set; }
		[Column("mods")]
		public int Mods { get; set; }
		[Column("300_count")]
		public int Count300 { get; set; }
		[Column("100_count")]
		public int Count100 { get; set; }
		[Column("50_count")]
		public int Count50 { get; set; }
		[Column("katus_count")]
		public int CountKatu { get; set; }
		[Column("gekis_count")]
		public int CountGeki { get; set; }
		[Column("misses_count")]
		public int CountMiss { get; set; }
		[Column("time")]
		public int Time { get; set; }
		[Column("play_mode")]
		public byte PlayMode { get; set; }
		[Column("completed")]
		public byte Completed { get; set; }
		[Column("accuracy")]
		public float Accuracy { get; set; }
		[Column("pp")]
		public float Pp { get; set; }
	}
}
